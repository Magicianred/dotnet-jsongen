// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Templates.Compilation
{
    public partial class ReflectionAssemblyLoadContext : ICodeGenAssemblyLoadContext
    {
        public Assembly LoadFromName(AssemblyName AssemblyName) => Assembly.Load(AssemblyName);

        public Assembly LoadStream(Stream assembly, Stream symbols)
        {
            using (var ms = new MemoryStream())
            {
                assembly.CopyTo(ms);
                return Assembly.Load(ms.ToArray());
            }
        }

        public Assembly LoadFromPath(string path) => Assembly.LoadFrom(path);
    }

    public partial class DefaultAssemblyLoadContext : ICodeGenAssemblyLoadContext
    {
        private AssemblyLoadContext _context = AssemblyLoadContext.Default;

        public Assembly LoadFromName(AssemblyName AssemblyName)
            => _context.LoadFromAssemblyName(AssemblyName);

        public Assembly LoadStream(Stream assembly, Stream symbols)
            => _context.LoadFromStream(assembly, symbols);

        public Assembly LoadFromPath(string path)
            => _context.LoadFromAssemblyPath(path);
    }
}
