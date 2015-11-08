using System;
using System.IO;
using System.Reflection;
using LocalNuget.Models;
using LocalNuget.Settings;
using Moq;

namespace LocalNuget.Tests.Fixtures
{
    public class SettingsFixture
    {
        public ISettings Settings { get; }

        public SettingsFixture()
        {
            var mockSettings = new Mock<ISettings>();
            var mockSettingsDefaults = new Mock<ISettingsDefaults>();
            mockSettingsDefaults.SetupGet(defaults => defaults.LicenceUrl).Returns("http://domain.org");
            mockSettingsDefaults.SetupGet(defaults => defaults.IconUrl).Returns("http://domain.org/project.png");
            mockSettings.SetupGet(lsetting => lsetting.WorkDirectory)
                .Returns(GetNewDirectory());
            mockSettings.SetupGet(lsettings => lsettings.Defaults).Returns(mockSettingsDefaults.Object);
            Settings = mockSettings.Object;
            var executingNugetExeFileInfo = new FileInfo(Path.Combine(mockSettings.Object.WorkDirectory, "nuget.exe"));
            if (!executingNugetExeFileInfo.Exists)
                File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nuget.exe"), Path.Combine(mockSettings.Object.WorkDirectory, "nuget.exe"));
            AutoMapperModels.CreateMap();
        }

        private string GetNewDirectory()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var newDir = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Guid.NewGuid().ToString()));
            newDir.Create();
            return newDir.FullName;
        }

    }
}