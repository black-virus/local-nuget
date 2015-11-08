// ReSharper disable UnusedMemberInSuper.Global
namespace LocalNuget.Commands
{
    public interface ICommandRunner
    {
        void Execute();
        CommandOptions Options { get; set; }
    }
}