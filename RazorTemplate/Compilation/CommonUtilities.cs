// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Templates.Compilation
{

    public static class CommonUtilities
    {
        public static CompilationResult GetAssemblyFromCompilation(
            ICodeGenAssemblyLoadContext loader,
            Microsoft.CodeAnalysis.Compilation compilation)
        {
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms, pdbStream: null);

                if (!result.Success)
                {
                    var formatter = new DiagnosticFormatter();
                    var errorMessages = result.Diagnostics
                                            .Where(IsError)
                                            .Select(d => formatter.Format(d));

                    return CompilationResult.FromErrorMessages(errorMessages);
                }

                ms.Seek(0, SeekOrigin.Begin);
                
                try
                {
                    return CompilationResult.FromAssembly(loader.LoadStream(ms, symbols: null));
                }
                catch (Exception ex)
                {
                    var v = ex;
                    while (v.InnerException != null)
                    {
                        
                        v = v.InnerException;
                    }

                    return CompilationResult.FromErrorMessages(ex.GetAllErrorMessages());
                }
            }
        }
        private static IEnumerable<string> GetAllErrorMessages(this Exception ex)
        {
            if (ex is default(Exception)) yield break;
            yield return ex?.Message;
            foreach(var msg in GetAllErrorMessages(ex.InnerException))
            {
                yield return msg;
            }
        }

        private static bool IsError(Diagnostic diagnostic) => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error;
    }
}
