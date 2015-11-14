using System;
using System.Linq;
using FluentAssertions;
using LocalNuget.Models;
using LocalNuget.Storage;
using LocalNuget.Tests.Fixtures;
using Xunit;
// ReSharper disable UnusedMember.Global

namespace LocalNuget.Tests
{

    [Trait("", "Storage tests")]
    public class StorageTests : IClassFixture<SettingsFixture>, IClassFixture<NuspecFixture>
    {
        private readonly SettingsFixture settingsFixture;
        private readonly NuspecFixture nuspecFixture;

        public StorageTests(SettingsFixture settings, NuspecFixture nuspecFixture)
        {
            settingsFixture = settings;
            this.nuspecFixture = nuspecFixture;
            this.nuspecFixture.AttachToWorkDirectory(settingsFixture.Settings.WorkDirectory);
        }

        [Fact(DisplayName = "Add and get")]
        public void AddAndGetTest()
        {
            IStorage storage = new JsonFileStorage(settingsFixture.Settings);
            var name = "test";
            var csProj = nuspecFixture.CsProjLocation;
            var nuspec = csProj.Replace(".csproj", ".nuspec");
            storage.Add(name, csProj, nuspec);
            var package = storage.Get("test");
            Action<StoragePackage> validate = actPackage =>
            {
                actPackage.Should().NotBeNull();
                actPackage.Name.Should().Be(name);
                actPackage.CsProjectFile.Should().Be(csProj);
                actPackage.NuspecProjectFile.Should().Be(nuspec);
            };
            validate(package);
            storage = new JsonFileStorage(settingsFixture.Settings);
            package = storage.Get("test");
            validate(package);
        }

        [Fact(DisplayName = "Add 2 entities and get list")]
        public void AddTwoAndGetList()
        {
            IStorage storage = new JsonFileStorage(settingsFixture.Settings);
            Action<string, string> act = (actName, actCsProj) =>
            {
                var csProj = actCsProj;
                var nuspec = csProj.Replace(".csproj", ".nuspec");
                storage.Add(actName, csProj, nuspec);

            };
            var name1 = "test-1";
            var name2 = "test-2";
            var csProj1 = nuspecFixture.CsProjLocation;
            var csProj2 = nuspecFixture.CsProjLocation2;
            act(name1, csProj1);
            act(name2, csProj2);
            var packages = storage.List();
            var storagePackages = packages as StoragePackage[] ?? packages.ToArray();
            storagePackages.Should().NotBeNull().And.HaveCount(2);
            storagePackages.Should().ContainSingle(package => package.Name == name1);
            storagePackages.Should().ContainSingle(package => package.Name == name2);
        }

    }
}
