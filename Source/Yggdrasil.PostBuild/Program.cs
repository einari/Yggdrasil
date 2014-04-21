using Mono.Cecil;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Yggdrasil.PostBuild
{
    class Program
    {
        static bool IsSystemAssembly(AssemblyNameReference reference)
        {
            if (reference.FullName.StartsWith("Microsoft") ||
                reference.FullName.StartsWith("System"))
            {
                return true;
            }
            return false;
        }


        static void Main(string[] args)
        {
            var targetDir = @"C:\projects\yggdrasil\Source\Yggdrasil.Microframework.TestApp\bin\Debug\";

            Directory.SetCurrentDirectory(targetDir);

            var file = "Yggdrasil.Microframework.TestApp.exe";
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(file);

            var typeMetaData = new TypeDefinition("Yggdrasil", "_TypeMetaData", TypeAttributes.Class | TypeAttributes.Public);
            var types = GetAllTypes(assemblyDefinition);
            var allTypesTypeReference = assemblyDefinition.MainModule.GetType("System.String[]", true);
            var allTypesFieldDefinition = new FieldDefinition("AllTypes", FieldAttributes.Public | FieldAttributes.Static, allTypesTypeReference);
            var typeNames = types.Select(t => t.FullName).ToArray();
            allTypesFieldDefinition.Constant = typeNames;
            allTypesFieldDefinition.InitialValue = System.Text.UTF8Encoding.UTF8.GetBytes("Hello world");
            
            typeMetaData.Fields.Add(allTypesFieldDefinition);
            



            // Collect all types across all assemblies references - except system and non local copied assemblies

            // Generate array of all types for type discovery

            // Generate array with configuration for all types

            var typeReference = assemblyDefinition.MainModule.GetType("System.String[]", true);


            //type.Module.Import(typeReference);

            //var typeReference = assemblyDefinition.MainModule.GetType("System.String", true);

            var fieldDefinition = new FieldDefinition("TypeDefinitions", FieldAttributes.Public, typeReference);

            typeMetaData.Fields.Add(fieldDefinition);

            assemblyDefinition.MainModule.Types.Add(typeMetaData);

            assemblyDefinition.Write(file+"_modified");
            
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
                    types.AddRange(referencedAssemblyDefinition.MainModule.Types);
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
