using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CA.Blocks.Images.DependencyInjection;
using CA.Blocks.Images.Resize;
using CA.Images.Commands;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace CA.Images
{
    class Program
    {
        private static ServiceProvider _serviceProvider;

        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }


        private static void SetupDependencyInjection()
        {
            //setup our DI
            var services  = new ServiceCollection()
                .AddCaBlocksImages()
                .AddLogging(builder => builder
                    .AddConsole()
                    .AddFilter(level => level >= LogLevel.Information));

            _serviceProvider = services.BuildServiceProvider();
        }


        static void Main(string[] args)
        {
            SetupDependencyInjection();
           
            var types = LoadVerbs();
            Parser.Default.ParseArguments(args, types)
                .WithParsed(Run)
                .WithNotParsed(HandleErrors);
        }

        private static void Run(object obj)
        {
            var logger = _serviceProvider.GetService<ILogger<Program>>();
            if (logger != null)
            {
                try
                {
                    logger.LogInformation($"Running {obj.GetType()}");
                    switch (obj)
                    {
                        case ResizeOptions r:
                            RunResize(r);
                            break;
                    }

                    //process CloneOptions
                    void RunResize(ResizeOptions opts)
                    {
                        var worker = _serviceProvider.GetService<IImageResizerLib>();
                        worker?.Execute(opts.ToImageResizerLibOptions());
                    }

                    // other verb actions here....

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            }
            // Do other worker here
        }

        private static void HandleErrors(IEnumerable<Error> obj)
        {
            var _logger = _serviceProvider.GetService<ILogger<Program>>();
        }
    }
}
