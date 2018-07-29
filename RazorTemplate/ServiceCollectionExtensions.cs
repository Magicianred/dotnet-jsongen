using Templates;
using Templates.Compilation;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using System.IO;
using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectTemplating(this IServiceCollection services, IProjectContext context)
            => services.AddProjectTemplating<RazorConfiguration>(context);
        public static IServiceCollection AddProjectTemplating<TConfig>(this IServiceCollection services,IProjectContext context)
            where TConfig : RazorConfiguration
        {
            services.TryAddSingleton(context);
            services.TryAddSingleton<IApplicationInfo>(_ => new ApplicationInfo(context.AssemblyName, context.AssemblyFullPath, context.Configuration));
            services.TryAddSingleton(RazorProjectFileSystem.Create(context.ProjectFullPath));
            services.TryAddSingleton<RoslynCompilationService>();
            return services.AddRazorTemlpate<TConfig, RazorProjectFileSystem, RoslynCompilationService>(TemplatingHelper.Register);
        }

        public static IServiceCollection AddRazorTemlpate<TConfig, TFile, TCompilation>(this IServiceCollection services, Action<RazorProjectEngineBuilder> builder)
            where TConfig : RazorConfiguration
            where TFile : RazorProjectFileSystem
            where TCompilation : class, ICompilationService
        {
            services.AddOptions();
            services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Trace));
            services.TryAddSingleton<ICompilationService, TCompilation>();
            services.TryAddSingleton<ICodeGenAssemblyLoadContext, DefaultAssemblyLoadContext>();
            services.TryAddSingleton(p => RazorProjectEngine.Create(
                p.GetService<TConfig>() ?? RazorConfiguration.Default,
                p.GetService<TFile>() ?? RazorProjectFileSystem.Create(Directory.GetCurrentDirectory()),
                x => {TemplatingHelper.Register(x); builder(x); })
                );
            services.TryAddSingleton<ITemplateFactory, TemplateFactory>();
            return services;
        }
        public static IServiceCollection AddRazorTemlpate(this IServiceCollection services, Action<RazorProjectEngineBuilder> builder)
        {
            services.AddRazorTemlpate<RazorConfiguration, RazorProjectFileSystem, DefaultCompilationService>(builder);;
            return services;
        }
    }
}
