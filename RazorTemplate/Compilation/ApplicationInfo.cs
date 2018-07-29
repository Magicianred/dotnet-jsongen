// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Templates.Compilation
{
    public class ApplicationInfo : IApplicationInfo
    {
        public ApplicationInfo(string appName, string appBasePath)
            : this(appName, appBasePath, "Debug")
        {

        }

        public ApplicationInfo(string appName, string appBasePath, string appConfiguration)
        {
            ApplicationName = appName ?? throw new ArgumentNullException(nameof(appName));
            ApplicationBasePath = appBasePath ?? throw new ArgumentNullException(nameof(appBasePath));
            ApplicationConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(appConfiguration));
        }
        public string ApplicationBasePath
        {
            get; private set;
        }

        public string ApplicationName
        {
            get; private set;
        }

        public string ApplicationConfiguration
        {
            get; private set;
        }
    }
}
