using System;
using System.Linq;
using System.Reflection;
using Ninject;
using LocalNuget.Core.Commands;

namespace LocalNuget.Commands
{
    public sealed class CommandsCollection : IDisposable
    {

        #region Fields

        private readonly IKernel kernel;
        private static readonly CommandsCollection Instance = new CommandsCollection();
        private const string KernelNameFormat = "Command-{0}";

        #endregion

        #region Constructors

        private CommandsCollection()
        {
            kernel = new StandardKernel();
        }

        #endregion

        #region API

        public static void RegisterCommand<TCmd>(string commandName)
            where TCmd : ILineCommand
        {
            Instance.Register<TCmd>(commandName);
        }

        public static void RegisterConstructorParameter<TBind, TTo>(bool useSingleton = false)
        where TTo : TBind
        {
            Instance.RegisterParameter<TBind, TTo>(useSingleton);
        }
        
        #endregion

        #region Methods
        
        private void Register<TCmd>(string commandName)
            where TCmd : ILineCommand
        {
            kernel.Bind<ILineCommand>().To<TCmd>().Named(string.Format(KernelNameFormat, commandName));
        }

        private void RegisterParameter<TBind, TTo>(bool useSingleton)
            where TTo : TBind
        {
            var bind = kernel.Bind<TBind>().To<TTo>();
            if (useSingleton)
                bind.InSingletonScope();
        }
        
        #endregion
        
        public static ICommandRunner GetRunner(string commandName)
        {
            return Instance.GetCommandRunner(commandName);
        }

        private ICommandRunner GetCommandRunner(string commandName)
        {
            var cmd = kernel.Get<ILineCommand>(String.Format(KernelNameFormat, commandName));
            var cmdType = cmd.GetType();
            var optionsInterface = cmdType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.Name.StartsWith("ILineCommandWithOptions"));
            CommandOptions setOptionsInstance = null;
            var optionsType = optionsInterface?.GetGenericArguments().FirstOrDefault();
            if (optionsType != null)
            {
                var optionsInstance = Activator.CreateInstance(optionsType) as CommandOptions;
                if (optionsInstance != null)
                {
                    var cmdOptions = cmdType.GetProperty("Options", BindingFlags.Public | BindingFlags.Instance);
                    if (null != cmdOptions && cmdOptions.CanWrite)
                    {
                        cmdOptions.SetValue(cmd, optionsInstance, null);
                        setOptionsInstance = optionsInstance;
                    }
                }
            }
            var result = new DefaultCommandRunner(cmd);
            if (setOptionsInstance != null)
                result.Options = setOptionsInstance;
            return result;
        }
        
        private class DefaultCommandRunner : ICommandRunner
        {
            private readonly ILineCommand command;

            public CommandOptions Options { get; set; }

            public DefaultCommandRunner(ILineCommand command)
            {
                this.command = command;
            }

            public void Execute()
            {
                command.Execute();
            }

        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                kernel.Dispose();
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
