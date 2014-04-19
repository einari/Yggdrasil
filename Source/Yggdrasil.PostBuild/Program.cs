using Mono.Cecil;
using System.Linq;

namespace Yggdrasil.PostBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\projects\yggdrasil\Source\Yggdrasil.Microframework.TestApp\bin\Debug\Yggdrasil.Microframework.TestApp.exe";
            var yggdrasilFile = @"C:\projects\yggdrasil\Source\Yggdrasil.Microframework.TestApp\bin\Debug\Yggdrasil.Microframework.dll";
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(file);
            var yggdrasilAssemblyDefinition = AssemblyDefinition.ReadAssembly(yggdrasilFile);

            var type = new TypeDefinition("Yggdrasil", "_TypeMetaData", TypeAttributes.Class | TypeAttributes.Public);
            
            // Collect all types across all assemblies references - except system and non local copied assemblies

            // Generate array of all types for type discovery

            // Generate array with configuration for all types

            var typeReference = assemblyDefinition.MainModule.GetType("System.String[]", true);


            //type.Module.Import(typeReference);

            //var typeReference = assemblyDefinition.MainModule.GetType("System.String", true);

            var fieldDefinition = new FieldDefinition("TypeDefinitions", FieldAttributes.Public, typeReference);

            type.Fields.Add(fieldDefinition);

            assemblyDefinition.MainModule.Types.Add(type);

            assemblyDefinition.Write(file+"_modified");
            
        }
    }
}
