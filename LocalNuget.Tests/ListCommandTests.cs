using LocalNuget.Commands.Add;
using LocalNuget.Commands.List;
using LocalNuget.Models;
using LocalNuget.Tests.Fixtures;
using Moq;
using System;
using System.IO;
using System.Linq;
using FluentAssertions;
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
            nuspecFixture.AttachToWorkDirectory(settingsFixture.Settings.WorkDirectory);
        }

        [Fact(DisplayName = "Create and list 2 specs")]
        public void AddAndListingTest()
        {
            nuspecFixture.ClearNuspecs();
            AddNuscpec(new AddLocalNugetOptions { VisualStudioProject = nuspecFixture.CsProjLocation, Force = true });
            AddNuscpec(new AddLocalNugetOptions { VisualStudioProject = nuspecFixture.CsProjLocation2, Force = true });
            var resultBus = new Mock<IResultBus<PackageInfoModel>>();
            var cmd = new ListNugetCommand(new JsonFileStorage(settingsFixture.Settings), resultBus.Object, settingsFixture.Settings);
            Func<PackageInfoModel[], bool> resultIs = result =>
            {
                if (result.Length != 2) return false;
                var firstResult = result.FirstOrDefault(package => package.VisualStudioProject == nuspecFixture.CsProjLocation);
                if (firstResult == null) return false;
                var secResult = result.FirstOrDefault(package => package.VisualStudioProject == nuspecFixture.CsProjLocation2);
                return secResult != null;
            };
            resultBus.Setup(bus => bus.SetResult(It.Is<PackageInfoModel[]>(result => resultIs(result))));
            cmd.Execute(); //TODO: lepiej zostawić generyczne, ale pokombinować z callbackiem o resultbus
            resultBus.Verify(bus => bus.SetResult(It.Is<PackageInfoModel[]>(result => resultIs(result))), Times.Once);
        }

        [Fact(DisplayName = "Create and list spec with checking dates")]
        public void AddThenListingAndCheckDates()
        {
            var nuspecFileName = new FileInfo(nuspecFixture.CsProjLocation).Name.Replace(".csproj", ".nuspec");
            var workDirFile = new FileInfo(Path.Combine(settingsFixture.Settings.WorkDirectory, nuspecFileName));
            // ReSharper disable once AssignNullToNotNullAttribute
            var projDirFile = new FileInfo(Path.Combine(Path.GetDirectoryName(nuspecFixture.CsProjLocation), nuspecFileName));
            var minCreateDate = DateTime.MaxValue;
            var maxUpdateDate = DateTime.MinValue;
            nuspecFixture.ClearNuspecs();
            AddNuscpec(new AddLocalNugetOptions { VisualStudioProject = nuspecFixture.CsProjLocation, Force = true });
            workDirFile.Refresh();
            projDirFile.Refresh();
            if (workDirFile.LastWriteTime > maxUpdateDate) maxUpdateDate = workDirFile.LastWriteTime;
            if (projDirFile.LastWriteTime > maxUpdateDate) maxUpdateDate = projDirFile.LastWriteTime;
            if (workDirFile.CreationTime < minCreateDate) minCreateDate = workDirFile.CreationTime;
            if (projDirFile.CreationTime < minCreateDate) minCreateDate = projDirFile.CreationTime;
            var resultBus = new Mock<IResultBus<PackageInfoModel>>();
            var cmd = new ListNugetCommand(new JsonFileStorage(settingsFixture.Settings), resultBus.Object, settingsFixture.Settings);
            PackageInfoModel[] result = null;
            resultBus.Setup(bus => bus.SetResult(It.IsAny<PackageInfoModel[]>()))
                .Callback<PackageInfoModel[]>(models => result = models);
            cmd.Execute();
            resultBus.Verify(bus => bus.SetResult(It.IsAny<PackageInfoModel[]>()), Times.Once);
            result.Should().NotBeNull().And.HaveCount(1);
            var firstPackage = result.First();
            firstPackage.NuspecInProject.Should().Be(projDirFile.FullName);
            firstPackage.NuspecInWork.Should().Be(workDirFile.FullName);
            firstPackage.Created.Should().Be(minCreateDate);
            firstPackage.Updated.Should().Be(maxUpdateDate);
        }

        private void AddNuscpec(AddLocalNugetOptions options = null)
        {
            var cmd = new AddLocalNugetCommand(settingsFixture.Settings, new JsonFileStorage(settingsFixture.Settings))
            {
                Options = options ?? new AddLocalNugetOptions()
            };
            if (string.IsNullOrEmpty(cmd.Options.VisualStudioProject)) cmd.Options.VisualStudioProject = nuspecFixture.CsProjLocation;
            cmd.Execute();
        }

    }
}
