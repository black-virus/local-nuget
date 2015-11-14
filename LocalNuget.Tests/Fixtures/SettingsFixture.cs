using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using LocalNuget.Settings;
using Moq;

namespace LocalNuget.Tests.Fixtures
{
    public class SettingsFixture
    {

        private static bool _addMapperProfiles = true;
        private static volatile object _addMapperLocker = new object();
        private static volatile object _newDirectoryLocker = new object();

        public ISettings Settings { get; }

        public SettingsFixture()
        {
            var mockSettings = new Mock<ISettings>();
            var mockSettingsDefaults = new Mock<ISettingsDefaults>();
            mockSettingsDefaults.SetupGet(defaults => defaults.ProjectUrl).Returns("http://domain.org");
            mockSettingsDefaults.SetupGet(defaults => defaults.Author).Returns("Adrian Kaczmarek");
            mockSettingsDefaults.SetupGet(defaults => defaults.Copyright).Returns("Black House (R) 2015");
            mockSettings.SetupGet(lsetting => lsetting.WorkDirectory)
                .Returns(GetNewDirectory());
            mockSettings.SetupGet(lsettings => lsettings.Defaults).Returns(mockSettingsDefaults.Object);
            Settings = mockSettings.Object;
            var executingNugetExeFileInfo = new FileInfo(Path.Combine(mockSettings.Object.WorkDirectory, "nuget.exe"));
            if (!executingNugetExeFileInfo.Exists)
                File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nuget.exe"), Path.Combine(mockSettings.Object.WorkDirectory, "nuget.exe"));
            AddMappings();
        }

        private static void AddMappings()
        {
            if (!_addMapperProfiles) return;
            lock (_addMapperLocker)
            {
                if (!_addMapperProfiles) return;
                Mapper.AddProfile<Models.MapperProfile>();
                Mapper.AddProfile<MapperProfile>();
                _addMapperProfiles = false;
            }
        }

        private string GetNewDirectory()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            lock (_newDirectoryLocker)
            {
                var newDir =
                    new DirectoryInfo(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            Guid.NewGuid().ToString()));
                if (newDir.Exists)
                    return GetNewDirectory();
                newDir.Create();
                return newDir.FullName;
            }
        }

    }
}