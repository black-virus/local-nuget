using LocalNuget.Commands;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace LocalNuget.Tests
{
    [Trait("", "Commands collection tests")]
    public class CommandsCollectionTests
    {
        [Fact(DisplayName = "Run default command")]
        public void GetRunner() {
            CommandsCollection.RegisterCommand<DefaultTestLineCommand>("test");
            var runner = CommandsCollection.GetRunner("test");
            runner.Execute();
            Assert.True(DefaultTestLineCommand.Executed);
        }

        [Fact(DisplayName = "Run command with options")]
        public void GetRunnerWithOptions() {
            CommandsCollection.RegisterCommand<TestWithOptionsLineCommand>("test-opt");
            var runner = CommandsCollection.GetRunner("test-opt");
            var opt = runner.Options as TestOptions;
            Assert.NotNull(opt);
            // ReSharper disable once PossibleNullReferenceException
            opt.Verbose = true;
            runner.Execute();
            Assert.True(TestWithOptionsLineCommand.Executed);
        }

    }
}
