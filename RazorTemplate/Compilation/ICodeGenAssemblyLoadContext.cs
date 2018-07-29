// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Reflection;

namespace Templates.Compilation
{
    public interface ICodeGenAssemblyLoadContext
    {
        Assembly LoadStream(Stream assembly, Stream symbols);
        Assembly LoadFromName(AssemblyName AssemblyName);
        Assembly LoadFromPath(string path);
    }
}
