using System;
//using NugetRunner.Commands.Commands.Rebuild;
//using CommandsDeclarations = NugetRunner.Commands.Commands;
using LocalNuget.Commands;
using LocalNuget.Commands.Add;
using LocalNuget.Settings;
using AutoMapper;
using LocalNuget.Commands.List;
using LocalNuget.Storage;

namespace LocalNuget
{
    class Program
    {

        static void RegisterCommands()
        {
            CommandsCollection.RegisterConstructorParameter<IStringSettingsReader, JSonSettingsReader>(true);
            CommandsCollection.RegisterConstructorParameter<ISettingsReader, WorkSettingsReader>(true);
            CommandsCollection.RegisterConstructorParameter<ISettings, NugetSettings>(true);
            CommandsCollection.RegisterConstructorParameter<IStorage, JsonFileStorage>();
            CommandsCollection.RegisterCommand<AddLocalNugetCommand>("add");
            CommandsCollection.RegisterCommand<ListNugetCommand>("list");
            //CommandsCollection.RegisterCommand<CommandsDeclarations.Rebuild.Command, CommandsDeclarations.Rebuild.Options>("rebuild");
        }

        static void RegisterMappings()
        {
            Mapper.AddProfile<MapperProfile>();
        }

        static void Main(string[] args)
        {
            RegisterMappings();
            RegisterCommands();

            if (args.Length > 0) ExecuteOneCommand(args);
            else ExecuteCoupleCommands();
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
            var cmd = CommandsCollection.GetRunner(cmdWithArgsInput.Item1);
            if (cmd.Options != null)
            {
                var parseState = CommandLine.Parser.Default.ParseArguments(cmdWithArgsInput.Item2, cmd.Options);
                if (!parseState)
                {
                    //Console.WriteLine(cmd.Options.GetUsage());
                    return;
                }
            }
            cmd.Execute();
        }

        private static Tuple<string, string[]> ResolveCommandWithArguments(string[] args)
        {
            var commandArgs = new string[args.Length - 1];
            Array.Copy(args, 1, commandArgs, 0, commandArgs.Length);
            return new Tuple<string, string[]>(args[0], commandArgs);
        }

    }
}
