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

            
            var yggdrasilAssembly = AssemblyDefinition.ReadAssembly("Yggdrasil.dll");
            yggdrasilAssembly.MainModule.AssemblyReferences.Add(yggdrasilAssembly.Name);

            var typeInfoTypeDefinition = yggdrasilAssembly.MainModule.GetType("Yggdrasil.Types.TypeInfo", true).Resolve();
            var typeInfoTypeDefinitionConstructor = typeInfoTypeDefinition.Methods.Single(t => t.Name == ".ctor");
            var typeInfoTypeFieldDefinition = module.Import(typeInfoTypeDefinition.Fields.Single(t => t.Name == "Type"));
            var typeInfoNamespaceFieldDefinition = module.Import(typeInfoTypeDefinition.Fields.Single(t => t.Name == "Namespace"));
            var typeInfoConstructorsFieldDefinition = module.Import(typeInfoTypeDefinition.Fields.Single(t => t.Name == "Constructors"));
            var typeInfoHasSingletonAttributeFieldDefinition = module.Import(typeInfoTypeDefinition.Fields.Single(t => t.Name == "HasSingletonAttribute"));

            var constructorInfoTypeDefinition = yggdrasilAssembly.MainModule.GetType("Yggdrasil.Types.ConstructorInfo", true).Resolve();
            var constructorInfoTypeDefinitionConstructor = constructorInfoTypeDefinition.Methods.Single(t => t.Name == ".ctor");
            var constructorInfoParametersFieldDefinition = module.Import(constructorInfoTypeDefinition.Fields.Single(t => t.Name == "Parameters"));

            var constructorParameterTypeDefinition = yggdrasilAssembly.MainModule.GetType("Yggdrasil.Types.ConstructorParameter", true).Resolve();
            var constructorParameterTypeDefinitionConstructor = constructorParameterTypeDefinition.Methods.Single(t => t.Name == ".ctor");
            var constructorParameterTypeFieldDefinition = module.Import(constructorParameterTypeDefinition.Fields.Single(t => t.Name == "Type"));
            var constructorParameterNameFieldDefinition = module.Import(constructorParameterTypeDefinition.Fields.Single(t => t.Name == "Name"));

            var typeInfoArrayTypeReference = new ArrayType(module.Import(typeInfoTypeDefinition));
            var constructorInfoArrayTypeReference = new ArrayType(module.Import(constructorInfoTypeDefinition));
            var constructorParameterArrayTypeReference = new ArrayType(module.Import(constructorParameterTypeDefinition));

            var allTypesFieldDefinition = new FieldDefinition("AllTypes", FieldAttributes.Public | FieldAttributes.Static, typeInfoArrayTypeReference);
            typeMetaData.Fields.Add(allTypesFieldDefinition);
            
            constructor.Body.InitLocals = true;

            var typeTypeReference = assemblyDefinition.MainModule.GetType("System.Type", true);
            var typeTypeDefinition = typeTypeReference.Resolve();
            var getTypeFromHandleMethod = typeTypeDefinition.Methods.Single(m=>m.Name == "GetTypeFromHandle");


            var il = constructor.Body.GetILProcessor();

            constructor.Body.Variables.Add(new VariableDefinition("typeInfos",module.Import(typeInfoArrayTypeReference)));
            constructor.Body.Variables.Add(new VariableDefinition(module.Import(typeInfoTypeDefinition)));
            constructor.Body.Variables.Add(new VariableDefinition("constructorInfos", module.Import(constructorInfoArrayTypeReference)));
            constructor.Body.Variables.Add(new VariableDefinition(module.Import(constructorInfoTypeDefinition)));
            var parametersVariable = new VariableDefinition("parameters", module.Import(constructorParameterArrayTypeReference));
            constructor.Body.Variables.Add(parametersVariable);
            var parameterVariable = new VariableDefinition("parameter", module.Import(constructorParameterTypeDefinition));
            constructor.Body.Variables.Add(parameterVariable);
            il.Append(Instruction.Create(OpCodes.Nop));
            il.Append(Instruction.Create(OpCodes.Ldc_I4, types.Count()));
            il.Append(Instruction.Create(OpCodes.Newarr, module.Import(typeTypeReference)));
            il.Append(Instruction.Create(OpCodes.Stloc_0));


            var typeIndex = 0;
            foreach (var typeDefinition in types)
            {
                il.Append(Instruction.Create(OpCodes.Ldloc_0));
                il.Append(Instruction.Create(OpCodes.Ldc_I4, typeIndex));
                il.Append(Instruction.Create(OpCodes.Newobj, module.Import(typeInfoTypeDefinitionConstructor)));
                il.Append(Instruction.Create(OpCodes.Stloc_1));

                il.Append(Instruction.Create(OpCodes.Ldloc_1));
                il.Append(Instruction.Create(OpCodes.Ldtoken, module.Import(typeDefinition)));
                il.Append(Instruction.Create(OpCodes.Call, module.Import(getTypeFromHandleMethod)));
                il.Append(Instruction.Create(OpCodes.Stfld, typeInfoTypeFieldDefinition));

                il.Append(Instruction.Create(OpCodes.Ldloc_1));
                il.Append(Instruction.Create(OpCodes.Ldstr, typeDefinition.Namespace));
                il.Append(Instruction.Create(OpCodes.Stfld, typeInfoNamespaceFieldDefinition));

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

                il.Append(Instruction.Create(OpCodes.Ldloc_1));
                il.Append(Instruction.Create(OpCodes.Ldc_I4, hasSingleton ? 1 : 0));
                il.Append(Instruction.Create(OpCodes.Stfld, typeInfoHasSingletonAttributeFieldDefinition));

                var constructors = typeDefinition.Methods.Where(t => t.Name == ".ctor").ToArray();

                il.Append(Instruction.Create(OpCodes.Ldc_I4, constructors.Length));
                il.Append(Instruction.Create(OpCodes.Newarr, module.Import(constructorInfoTypeDefinition)));
                il.Append(Instruction.Create(OpCodes.Stloc_2));

                
                var constructorIndex = 0;
                foreach (var constructorMethodDefinition in constructors )
                {
                    il.Append(Instruction.Create(OpCodes.Ldloc_2));
                    il.Append(Instruction.Create(OpCodes.Ldc_I4, constructorIndex));
                    il.Append(Instruction.Create(OpCodes.Newobj, module.Import(constructorInfoTypeDefinitionConstructor)));
                    il.Append(Instruction.Create(OpCodes.Stloc_3));

                    il.Append(Instruction.Create(OpCodes.Ldc_I4, constructorMethodDefinition.Parameters.Count()));
                    il.Append(Instruction.Create(OpCodes.Newarr, module.Import(constructorParameterTypeDefinition)));
                    il.Append(Instruction.Create(OpCodes.Stloc_S, parametersVariable));

                    var parameterIndex = 0;
                    foreach (var parameter in constructorMethodDefinition.Parameters)
                    {
                        il.Append(Instruction.Create(OpCodes.Ldloc, parametersVariable));
                        il.Append(Instruction.Create(OpCodes.Ldc_I4, parameterIndex));

                        il.Append(Instruction.Create(OpCodes.Newobj, module.Import(constructorParameterTypeDefinitionConstructor)));
                        il.Append(Instruction.Create(OpCodes.Stloc_S, parameterVariable));

                        il.Append(Instruction.Create(OpCodes.Ldloc_S, parameterVariable));
                        il.Append(Instruction.Create(OpCodes.Ldstr, parameter.Name));
                        il.Append(Instruction.Create(OpCodes.Stfld, constructorParameterNameFieldDefinition));

                        il.Append(Instruction.Create(OpCodes.Ldloc_S, parameterVariable));
                        il.Append(Instruction.Create(OpCodes.Ldtoken, module.Import(parameter.ParameterType)));
                        il.Append(Instruction.Create(OpCodes.Call, module.Import(getTypeFromHandleMethod)));
                        il.Append(Instruction.Create(OpCodes.Stfld, constructorParameterTypeFieldDefinition));

                        il.Append(Instruction.Create(OpCodes.Ldloc, parameterVariable));
                        il.Append(Instruction.Create(OpCodes.Stelem_Ref));
                        parameterIndex++;
                    }

                    //il.Append(Instruction.Create(OpCodes.Ldloc_2));
                    //il.Append(Instruction.Create(OpCodes.Ldc_I4, constructorIndex));
                    //il.Append(Instruction.Create(OpCodes.Ldloc_S, parametersVariable));
                    
                    
                    //il.Append(Instruction.Create(OpCodes.Stfld, constructorInfoParametersFieldDefinition));

                    /*
                    */


                    il.Append(Instruction.Create(OpCodes.Ldloc_3));
                    il.Append(Instruction.Create(OpCodes.Stelem_Ref));
                    constructorIndex++;
                }

                //il.Append(Instruction.Create(OpCodes.Ldloc_1));
                //il.Append(Instruction.Create(OpCodes.Ldloc_2));
                //il.Append(Instruction.Create(OpCodes.Stsfld, typeInfoConstructorsFieldDefinition));



                il.Append(Instruction.Create(OpCodes.Ldloc_1));
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
            var getTypeFromHandleMethod = typeTypeDefinition.Methods.Single(m => m.Name == "GetTypeFromHandle");
            var module = assemblyDefinition.MainModule;
            module.Import(typeTypeDefinition);
            module.Import(getTypeFromHandleMethod);

            var typeIndex = 0;
            foreach (var typeDefinition in types)
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

    }
}
