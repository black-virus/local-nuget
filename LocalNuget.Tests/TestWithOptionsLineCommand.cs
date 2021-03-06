using System;
using LocalNuget.Core.Commands;

namespace LocalNuget.Tests
{
    internal class TestWithOptionsLineCommand : ILineCommandWithOptions<TestOptions>
    {

        public TestOptions Options { get; set; }
        public static bool Executed;

        public void Execute() {
            if (!Options.Verbose) throw new Exception();
            Executed = true;
        }

    }
}