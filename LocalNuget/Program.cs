using System;
using LocalNuget.Commands.Add;
using LocalNuget.Settings;
using AutoMapper;
using LocalNuget.Commands.List;
using LocalNuget.Core.Commands;
using LocalNuget.Core.Results;
using LocalNuget.Models;
using LocalNuget.Storage;
using Ninject;

namespace LocalNuget
{
    class Program
    {

        private static IKernel _kernel;

        private static void SetupIoC()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IStringSettingsReader>().To<JSonSettingsReader>().InSingletonScope();
            _kernel.Bind<ISettingsReader>().To<WorkSettingsReader>().InSingletonScope();
            _kernel.Bind<ISettings>().To<NugetSettings>().InSingletonScope();
            _kernel.Bind<IStorage>().To<JsonFileStorage>();
            _kernel.Bind<IOutputFormat>().To<TextOutputFormat>().InSingletonScope();

            // Result bus
            _kernel.Bind<IResultBus<PackageInfoModel>>().To<ConsoleResultBus<PackageInfoModel>>().InSingletonScope();
        }

        private static void RegisterMappings()
        {
            Mapper.AddProfile<Models.MapperProfile>();
            Mapper.AddProfile<Settings.MapperProfile>();
        }

        private static void Main(string[] args)
        {
            RegisterMappings();
            SetupIoC();

            if (args.Length > 0)
                ExecuteOneCommand(args);
            else
                ExecuteCoupleCommands();
        }

        private static void ExecuteCoupleCommands()
        {
            while (true)
            {
                Console.WriteLine("Type command to execute, or exit to close:");
                var readLine = Console.ReadLine();
                if (readLine == null) continue;
                var cmdWithArgs = readLine.Split(' ');
                if (cmdWithArgs[0] == "exit") break;
                ExecuteCommand(cmdWithArgs);
            }
        }

        private static void ExecuteOneCommand(string[] args)
        {
            ExecuteCommand(args);
            Console.ReadKey();
        }

        private static void ExecuteCommand(string[] args)
        {
            var cmdWithArgsInput = ResolveCommandWithArguments(args);
            ILineCommand command;
            object options = null;
            switch (cmdWithArgsInput.Item1)
            {
                case "add":
                    var addCommand = _kernel.Get<AddLocalNugetCommand>();
                    addCommand.Options = new AddLocalNugetOptions();
                    options = addCommand.Options;
                    command = addCommand;
                    break;
                case "list":
                    var listCommand = _kernel.Get<ListNugetCommand>();
                    command = listCommand;
                    break;
                default: throw new Exception("Command not found");
            }
            if (options != null)
            {
                var parseState = CommandLine.Parser.Default.ParseArguments(cmdWithArgsInput.Item2, options);
                if (!parseState)
                {
                    return;
                }
            }
            command.Execute();
        }

        private static Tuple<string, string[]> ResolveCommandWithArguments(string[] args)
        {
            var commandArgs = new string[args.Length - 1];
            Array.Copy(args, 1, commandArgs, 0, commandArgs.Length);
            return new Tuple<string, string[]>(args[0], commandArgs);
        }

    }
}
