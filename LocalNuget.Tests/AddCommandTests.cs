using System.IO;
using System.Xml;
using LocalNuget.Commands.Add;
using LocalNuget.Tests.Fixtures;
using Xunit;
using LocalNuget.Core.Exceptions;
using LocalNuget.Storage;
// ReSharper disable UnusedMember.Global

namespace LocalNuget.Tests
{
    [Trait("", "Add command tests")]
    public class AddCommandTests : IClassFixture<SettingsFixture>, IClassFixture<NuspecFixture>
    {

        private readonly SettingsFixture settingsFixture;
        private readonly NuspecFixture nuspecFixture;

        public AddCommandTests(SettingsFixture settings, NuspecFixture nuspec)
        {
            settingsFixture = settings;
            nuspecFixture = nuspec;

        }

        [Fact(DisplayName = "Testing default register nuspec")]
        public void AddLocalNugetTest()
        {
            nuspecFixture.ClearNuspecs();
            AddNuscpec(new AddLocalNugetOptions
            {
                VisualStudioProject = NuspecFixture.CsProjLocation,
                UseSettingsDefaults = false
            });

            var dir = Path.GetDirectoryName(NuspecFixture.CsProjLocation);
            var csProjName = new FileInfo(NuspecFixture.CsProjLocation);
            if (dir == null) return;
            var nuspecFile = Path.Combine(dir, csProjName.Name.Replace(csProjName.Extension, ".nuspec"));
            var workNuspecFile = Path.Combine(settingsFixture.Settings.WorkDirectory, csProjName.Name.Replace(csProjName.Extension, ".nuspec"));
            Assert.True(new FileInfo(nuspecFile).Exists);
            Assert.True(new FileInfo(workNuspecFile).Exists);
            CheckNuspecFile(nuspecFile);
        }

        [Fact(DisplayName = "Testing nuspec with links provided from settings")]
        public void AddLocalNugetWithSettingsTest()
        {
            nuspecFixture.ClearNuspecs();
            AddNuscpec();
            var dir = Path.GetDirectoryName(NuspecFixture.CsProjLocation);
            var csProjName = new FileInfo(NuspecFixture.CsProjLocation);
            if (dir == null) return;
            var nuspecFile = Path.Combine(dir, csProjName.Name.Replace(csProjName.Extension, ".nuspec"));
            Assert.False(string.IsNullOrEmpty(settingsFixture.Settings.Defaults.LicenceUrl)); // make sure we testing right behaviour
            Assert.False(string.IsNullOrEmpty(settingsFixture.Settings.Defaults.IconUrl));
            CheckNuspecFile(nuspecFile, licenceUrl: settingsFixture.Settings.Defaults.LicenceUrl, iconUrl: settingsFixture.Settings.Defaults.IconUrl);
        }

        [Fact(DisplayName = "Testing add spec, if already exist")]
        public void TestingAddSpecIfAlreadyExist()
        {
            nuspecFixture.ClearNuspecs();
            AddNuscpec();
            LocalNugetException exc = Assert.ThrowsAny<LocalNugetException>(() => AddNuscpec());
            Assert.Equal("C:LNC01C05", exc.Code);

            AddNuscpec(new AddLocalNugetOptions
            {
                Force = true
            });
            var dir = Path.GetDirectoryName(NuspecFixture.CsProjLocation);
            var csProjName = new FileInfo(NuspecFixture.CsProjLocation);
            if (dir == null) return;
            var nuspecFile = Path.Combine(dir, csProjName.Name.Replace(csProjName.Extension, ".nuspec"));
            Assert.False(string.IsNullOrEmpty(settingsFixture.Settings.Defaults.LicenceUrl)); // make sure we testing right behaviour
            Assert.False(string.IsNullOrEmpty(settingsFixture.Settings.Defaults.IconUrl));
            CheckNuspecFile(nuspecFile, licenceUrl: settingsFixture.Settings.Defaults.LicenceUrl, iconUrl: settingsFixture.Settings.Defaults.IconUrl);
        }

        private void AddNuscpec(AddLocalNugetOptions options = null)
        {
            var cmd = new AddLocalNugetCommand(settingsFixture.Settings, new JsonFileStorage(settingsFixture.Settings))
            {
                Options = options ?? new AddLocalNugetOptions()
            };
            if (string.IsNullOrEmpty(cmd.Options.VisualStudioProject)) cmd.Options.VisualStudioProject = NuspecFixture.CsProjLocation;
            cmd.Execute();
        }

        private void CheckNuspecFile(string nuspecFile,
            string id = "$id$",
            string version = "$version$",
            string title = "$title$",
            string authors = "$author$",
            string owners = "$author$",
            string requireLicenseAcceptance = "false",
            string description = "$description$",
            string licenceUrl = "",
            string projectUrl = "",
            string iconUrl = "")
        {
            var nuspecXmlDoc = new XmlDocument();
            nuspecXmlDoc.Load(nuspecFile);
            var xmlMeta = nuspecXmlDoc.SelectSingleNode("package/metadata") as XmlElement;
            Assert.Equal(id, GetElementValue(xmlMeta, "id"));
            Assert.Equal(version, GetElementValue(xmlMeta, "version"));
            Assert.Equal(title, GetElementValue(xmlMeta, "title"));
            Assert.Equal(authors, GetElementValue(xmlMeta, "authors"));
            Assert.Equal(owners, GetElementValue(xmlMeta, "owners"));
            Assert.Equal(requireLicenseAcceptance, GetElementValue(xmlMeta, "requireLicenseAcceptance"));
            Assert.Equal(description, GetElementValue(xmlMeta, "description"));
            Assert.Equal(licenceUrl, GetElementValue(xmlMeta, "licenseUrl"));
            Assert.Equal(projectUrl, GetElementValue(xmlMeta, "projectUrl"));
            Assert.Equal(iconUrl, GetElementValue(xmlMeta, "iconUrl"));
        }

        private string GetElementValue(XmlNode xmlElement, string elementId, string fallback = "")
        {
            if (xmlElement == null) return fallback;
            var xmlFindElement = xmlElement.SelectSingleNode(elementId);
            return xmlFindElement?.InnerText ?? fallback;
        }

    }
}
