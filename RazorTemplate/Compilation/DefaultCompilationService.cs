// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using System.Reflection.PortableExecutable;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

namespace Templates.Compilation
{
    public class DefaultCompilationService : ICompilationService
    {

        private readonly ICodeGenAssemblyLoadContext _loader;

        public DefaultCompilationService(
            ICodeGenAssemblyLoadContext loader) => _loader = loader ?? throw new ArgumentNullException(nameof(loader));

        public Templates.CompilationResult Compile(string content)
        {

            var syntaxTrees = new[] { CSharpSyntaxTree.ParseText(content) };

            var references = LoadDefaultMetadataReference();
            
            var assemblyName = Path.GetRandomFileName();
            var compilation = CSharpCompilation.Create(
                        assemblyName,
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                        syntaxTrees: syntaxTrees,
                        references: references);

            var result = CommonUtilities.GetAssemblyFromCompilation(_loader, compilation);
          
            return result.Success
                ? Templates.CompilationResult.Successful(string.Empty, result.Assembly.GetExportedTypes()
                                   .First())
                : Templates.CompilationResult.Failed(content, result.ErrorMessages);
        }

        private IEnumerable<Type> DefaultType
            => new Type[] {
                typeof(Task),
                typeof(object),
                typeof(Enumerable),
                GetType()
            };

        private IEnumerable<MetadataReference> LoadDefaultMetadataReference()
            => DefaultType
                .SelectMany(x => x.GetTypeInfo().Assembly.GetReferencedAssemblies())
                .Union(DependencyContext.Default.GetDefaultAssemblyNames())
                .Select(_loader.LoadFromName)
                .Union(DefaultType.Select(x => x.Assembly))
                .Distinct()
                .SelectMany(GetMetadataReference);

        public static IEnumerable<MetadataReference> GetMetadataReference(Assembly assembly)
        {
            var references = new List<MetadataReference>();
            AssemblyMetadata assemblyMetadata;
            try
            {
                using (var stream = File.OpenRead(assembly.Location))
                {
                    var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                    assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                    references.Add(assemblyMetadata.GetReference());
                }
            }
            catch (Exception ex)
                when (ex is NotSupportedException
                        || ex is ArgumentException
                        || ex is BadImageFormatException
                        || ex is IOException)
            {
                throw;
            }
            return references;
        }
    }
}
