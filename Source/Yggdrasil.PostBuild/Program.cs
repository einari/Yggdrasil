using Mono.Cecil;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Yggdrasil.PostBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            var targetDir = @"C:\projects\yggdrasil\Source\Yggdrasil.Microframework.TestApp\bin\Debug\";

            Directory.SetCurrentDirectory(targetDir);

            var file = "Yggdrasil.Microframework.TestApp.exe";
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(file);

            var typeMetaData = new TypeDefinition("Yggdrasil", "_TypeMetaData", TypeAttributes.Class | TypeAttributes.Public);
            assemblyDefinition.MainModule.Types.Add(typeMetaData);

            var types = GetAllTypes(assemblyDefinition);

            var constructor = GetStaticConstructor(assemblyDefinition, typeMetaData);
            AddAllTypes(assemblyDefinition, typeMetaData, types, constructor);
            AddTypeDefinitions(assemblyDefinition, typeMetaData, types, constructor);

            // Generate array with configuration for all types

            var il = constructor.Body.GetILProcessor();
            il.Append(Instruction.Create(OpCodes.Ret));

            

            assemblyDefinition.Write(file+"_modified");
            
        }

        private static MethodDefinition GetStaticConstructor(AssemblyDefinition assemblyDefinition, TypeDefinition typeMetaData)
        {
            var constructor = new MethodDefinition(
                ".ctor",
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                assemblyDefinition.MainModule.TypeSystem.Void);
            typeMetaData.Methods.Add(constructor);

            return constructor;
        }

        static bool IsSystemAssembly(AssemblyNameReference reference)
        {
            if (reference.FullName.StartsWith("mscorlib") ||
                reference.FullName.StartsWith("Microsoft") ||
                reference.FullName.StartsWith("System"))
            {
                return true;
            }
            return false;
        }

        static void AddTypeDefinitions(AssemblyDefinition assemblyDefinition, TypeDefinition typeMetaData, IEnumerable<TypeDefinition> types, MethodDefinition constructor)
        {
            var module = assemblyDefinition.MainModule;

            var typeInfoTypeDefinition = new TypeDefinition("Yggdrasil", "_TypeInfo", TypeAttributes.Class | TypeAttributes.Public);
            assemblyDefinition.MainModule.Types.Add(typeInfoTypeDefinition);

            var typeTypeReference = assemblyDefinition.MainModule.GetType("System.Type",true);
            var typeFieldDefinition = new FieldDefinition("Type", FieldAttributes.Public, typeTypeReference);
            typeInfoTypeDefinition.Fields.Add(typeFieldDefinition);

            var namespaceFieldDefinition = new FieldDefinition("Namespace", FieldAttributes.Public, module.TypeSystem.String);
            typeInfoTypeDefinition.Fields.Add(namespaceFieldDefinition);

            var isValueFieldDefinition = new FieldDefinition("IsValueType", FieldAttributes.Public, module.TypeSystem.Boolean);
            typeInfoTypeDefinition.Fields.Add(isValueFieldDefinition);

            var isInterfaceFieldDefinition = new FieldDefinition("IsInterface", FieldAttributes.Public, module.TypeSystem.Boolean);
            typeInfoTypeDefinition.Fields.Add(isInterfaceFieldDefinition);

            var ConstuctorCountFieldDefinition = new FieldDefinition("ConstructorCount", FieldAttributes.Public, module.TypeSystem.Int32);
            typeInfoTypeDefinition.Fields.Add(ConstuctorCountFieldDefinition);

            var hasConstructorFieldDefinition = new FieldDefinition("HasConstructor", FieldAttributes.Public, module.TypeSystem.Boolean);
            typeInfoTypeDefinition.Fields.Add(hasConstructorFieldDefinition);

            var hasDefaultConstructorFieldDefinition = new FieldDefinition("HasDefaultConstructor", FieldAttributes.Public, module.TypeSystem.Boolean);
            typeInfoTypeDefinition.Fields.Add(hasDefaultConstructorFieldDefinition);

            var hasSingletonAttributeFieldDefinition = new FieldDefinition("HasSingletonAttribute", FieldAttributes.Public, module.TypeSystem.Boolean);
            typeInfoTypeDefinition.Fields.Add(hasSingletonAttributeFieldDefinition);

            var typeInfoTypeDefinitionConstructor = new MethodDefinition(
                ".ctor",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                assemblyDefinition.MainModule.TypeSystem.Void);
            typeInfoTypeDefinition.Methods.Add(typeInfoTypeDefinitionConstructor);
            
            
            constructor.Body.InitLocals = true;

            var typeTypeDefinition = typeTypeReference.Resolve();
            var getTypeFromHandleMethod = typeTypeDefinition.Methods.Single(m=>m.Name == "GetTypeFromHandle");

            var typeInfoArrayTypeReference = new ArrayType(typeInfoTypeDefinition);

            var il = constructor.Body.GetILProcessor();

            constructor.Body.Variables.Add(new VariableDefinition("typeInfos",typeInfoArrayTypeReference));
            il.Append(Instruction.Create(OpCodes.Nop));
            il.Append(Instruction.Create(OpCodes.Ldc_I4, types.Count()));
            il.Append(Instruction.Create(OpCodes.Newarr, typeTypeReference));
            il.Append(Instruction.Create(OpCodes.Stloc_1));

            var typeIndex = 0;
            foreach (var typeDefinition in types)
            {
                il.Append(Instruction.Create(OpCodes.Ldloc_1));
                il.Append(Instruction.Create(OpCodes.Ldc_I4, typeIndex));
                il.Append(Instruction.Create(OpCodes.Newobj, typeInfoTypeDefinitionConstructor));
                il.Append(Instruction.Create(OpCodes.Stloc_0));
                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Ldtoken, module.Import(typeDefinition)));
                il.Append(Instruction.Create(OpCodes.Call, module.Import(getTypeFromHandleMethod)));
                il.Append(Instruction.Create(OpCodes.Stfld, typeFieldDefinition));

                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Ldstr, typeDefinition.Namespace));
                il.Append(Instruction.Create(OpCodes.Stfld, namespaceFieldDefinition));

                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Ldc_I4, typeDefinition.IsValueType ? 1 : 0));
                il.Append(Instruction.Create(OpCodes.Stfld, isValueFieldDefinition));

                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Ldc_I4, typeDefinition.IsInterface ? 1 : 0));
                il.Append(Instruction.Create(OpCodes.Stfld, isInterfaceFieldDefinition));

                var hasSingleton = false;
                if (typeDefinition.HasCustomAttributes)
                {
                    foreach (var customAttribute in typeDefinition.CustomAttributes)
                    {
                        if (customAttribute.AttributeType.FullName == "Yggdrasil.SingletonAttribute")
                        {
                            hasSingleton = true;
                            break;
                        }
                    }
                }

                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Ldc_I4, hasSingleton ? 1 : 0));
                il.Append(Instruction.Create(OpCodes.Stfld, hasSingletonAttributeFieldDefinition));

                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Stelem_Ref));
                typeIndex++;
            }

            il.Append(Instruction.Create(OpCodes.Ldloc_1));
            il.Append(Instruction.Create(OpCodes.Stsfld, typeFieldDefinition));
        }



        static void AddAllTypes(AssemblyDefinition assemblyDefinition, TypeDefinition typeMetaData, IEnumerable<TypeDefinition> types, MethodDefinition constructor)
        {
            var typeTypeReference = assemblyDefinition.MainModule.GetType("System.Type", true);

            var typeArrayTypeReference = assemblyDefinition.MainModule.GetType("System.Type[]", true);
            var allTypesFieldDefinition = new FieldDefinition("AllTypes", FieldAttributes.Public | FieldAttributes.Static, typeArrayTypeReference);
            typeMetaData.Fields.Add(allTypesFieldDefinition);

            constructor.Body.InitLocals = true;

            var il = constructor.Body.GetILProcessor();

            constructor.Body.Variables.Add(new VariableDefinition("types", typeArrayTypeReference));
            il.Append(Instruction.Create(OpCodes.Nop));
            il.Append(Instruction.Create(OpCodes.Ldc_I4, types.Count()));
            il.Append(Instruction.Create(OpCodes.Newarr, typeTypeReference));
            il.Append(Instruction.Create(OpCodes.Stloc_0));

            var typeTypeDefinition = typeTypeReference.Resolve();
            var getTypeFromHandleMethod = typeTypeDefinition.Methods.Single(m=>m.Name == "GetTypeFromHandle");
            var module = assemblyDefinition.MainModule;
            module.Import(typeTypeDefinition);
            module.Import(getTypeFromHandleMethod);

            var typeIndex = 0;
            foreach( var typeDefinition in types ) 
            {
                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Ldc_I4, typeIndex));
                il.Append(Instruction.Create(OpCodes.Ldtoken, module.Import(typeDefinition)));
                il.Append(Instruction.Create(OpCodes.Call, module.Import(getTypeFromHandleMethod)));
                il.Append(Instruction.Create(OpCodes.Stelem_Ref));

                typeIndex++;
            }

            il.Append(Instruction.Create(OpCodes.Ldloc_0));
            il.Append(Instruction.Create(OpCodes.Stsfld, allTypesFieldDefinition));

        }

        static IEnumerable<TypeDefinition> GetAllTypes(AssemblyDefinition assemblyDefinition)
        {
            var types = new List<TypeDefinition>();

            foreach (var assemblyReference in assemblyDefinition.MainModule.AssemblyReferences)
            {
                if (IsSystemAssembly(assemblyReference)) continue;

                try
                {
                    var referencedAssemblyDefinition = assemblyDefinition.MainModule.AssemblyResolver.Resolve(assemblyReference);
                    types.AddRange(referencedAssemblyDefinition.MainModule.Types.Where(t=>!t.FullName.Contains("<")));
                }
                catch
                {
                    continue;
                }
            }

            return types;
        }
    }
}
