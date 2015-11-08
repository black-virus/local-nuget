using LocalNuget.Commands.Add;
using LocalNuget.Commands.List;
using LocalNuget.Models;
using LocalNuget.Tests.Fixtures;
using Moq;
using System;
using System.Linq;
using LocalNuget.Core.Results;
using LocalNuget.Storage;
using Xunit;
// ReSharper disable UnusedMember.Global

namespace LocalNuget.Tests
{
    [Trait("", "List command tests")]
    public class ListCommandTests : IClassFixture<SettingsFixture>, IClassFixture<NuspecFixture>
    {

        private readonly SettingsFixture settingsFixture;
        private readonly NuspecFixture nuspecFixture;

        public ListCommandTests(SettingsFixture settings, NuspecFixture nuspec)
        {
            settingsFixture = settings;
            nuspecFixture = nuspec;

        }

        [Fact(DisplayName = "Create and list 2 specs")]
        public void AddAndListingTest()
        {
            nuspecFixture.ClearNuspecs();
            AddNuscpec(new AddLocalNugetOptions { VisualStudioProject = NuspecFixture.CsProjLocation, Force = true });
            AddNuscpec(new AddLocalNugetOptions { VisualStudioProject = NuspecFixture.CsProjLocation2, Force = true });
            var cmd = new ListNugetCommand(new JsonFileStorage(settingsFixture.Settings));
            var resultBus = new Mock<IResultBus<PackageInfoModel[]>>();
            cmd.ResultBus = resultBus.Object;
            Func<PackageInfoModel[], bool> resultIs = result =>
            {
                if (result.Length != 2) return false;
                var firstResult = result.FirstOrDefault(package => package.VisualStudioProject == NuspecFixture.CsProjLocation);
                if (firstResult == null) return false;
                var secResult = result.FirstOrDefault(package => package.VisualStudioProject == NuspecFixture.CsProjLocation2);
                return secResult != null;
            };
            resultBus.Setup(bus => bus.SetResult(It.Is<PackageInfoModel[]>(result => resultIs(result))));
            cmd.Execute(); //TODO: lepiej zostawić generyczne, ale pokombinować z callbackiem o resultbus
            resultBus.Verify(bus => bus.SetResult(It.Is<PackageInfoModel[]>(result => resultIs(result))), Times.Once);
        }

        //TODO: Add test to create pack and have some properties like: have package, all versions (future setted by options) itd.

        private void AddNuscpec(AddLocalNugetOptions options = null)
        {
            var cmd = new AddLocalNugetCommand(settingsFixture.Settings, new JsonFileStorage(settingsFixture.Settings))
            {
                Options = options ?? new AddLocalNugetOptions()
            };
            if (string.IsNullOrEmpty(cmd.Options.VisualStudioProject)) cmd.Options.VisualStudioProject = NuspecFixture.CsProjLocation;
            cmd.Execute();
        }

    }
}
