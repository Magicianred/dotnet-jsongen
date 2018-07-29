using Templates.Compilation;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System.IO;

namespace Templates
{

    public static class Engine
    {
        public static TemplateFactoryResult Create(this ITemplateFactory engine, FileInfo file)
            => engine.Create(File.ReadAllText(file.FullName));
        public static ITemplateFactory Create(
            string basePath, 
            Action<string> logger,
            Action<RazorProjectEngineBuilder> builder)
            => new TemplateFactory(
                RazorProjectEngine.Create(
                    RazorConfiguration.Default,
                    RazorProjectFileSystem.Create(basePath),
                    x => { builder(x); TemplatingHelper.Register(x); }),
                new DefaultCompilationService(
                    new DefaultAssemblyLoadContext()),
                new ActionLogger(logger));

        internal class ActionLogger : ILogger<ITemplateFactory>
        {
            private readonly Action<string> _logger;

            public ActionLogger(Action<string> logger) => _logger = logger;
            public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

            public bool IsEnabled(LogLevel logLevel)
                => logLevel == LogLevel.None;
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                if (formatter == null)
                {
                    throw new ArgumentNullException(nameof(formatter));
                }

                var message = formatter(state, exception);
                _logger(message);
            }
        }
    }
}
