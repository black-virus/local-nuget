using LocalNuget.Core.Commands;

namespace LocalNuget.Tests
{
    internal class DefaultTestLineCommand : ILineCommand
    {

        public static bool Executed;

        public void Execute() {
            Executed = true;
        }
    }
}