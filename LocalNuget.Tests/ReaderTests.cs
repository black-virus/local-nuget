using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using LocalNuget.Settings;
using LocalNuget.Tests.Structures;
using Xunit;
// ReSharper disable UnusedMember.Global

namespace LocalNuget.Tests
{
    [Trait("", "Settings reader")]
    public class ReaderTests
    {
        #region settings

        private const string Settings = @"{""Setting1"":1,""Setting2"":""abc"",""Settings3"":[1,2,3,4],""Settings4"":{""InnerSetting1"":2,""InnerSetting2"":""efg"",""InnerSettings3"":[5,6,7,8]},""Settings5"":[{""InnerSetting1"":3,""InnerSetting2"":""hi"",""InnerSettings3"":[9]}]}";

        #endregion

        private readonly ExampleSettings settingsExepted = new ExampleSettings
        {
            Setting1 = 1,
            Setting2 = "abc",
            Settings3 = new[] { 1, 2, 3, 4 },
            Settings4 = new ExampleSettings.ExampleSettingsInner
            {
                InnerSetting1 = 2,
                InnerSetting2 = "efg",
                InnerSettings3 = new[] { 5, 6, 7, 8 }
            },
            Settings5 = new[]
            {
                new ExampleSettings.ExampleSettingsInner
                {
                    InnerSetting1 = 3,
                    InnerSetting2 = "hi",
                    InnerSettings3 = new[] {9}
                }
            }
        };

        [Fact(DisplayName = "JSON reader")]
        public void JsonReaderTest()
        {
            var reader = new JSonSettingsReader();
            reader.InitializeData(Settings);
            var result = reader.Read<ExampleSettings>();
            result.ShouldBeEquivalentTo(settingsExepted);
        }

        [Fact(DisplayName = "JSON reader should throw exeption when no have data")]
        public void JsonReaderNoDataTest()
        {
            var reader = new JSonSettingsReader();
            Assert.Throws<InvalidOperationException>(() => reader.Read<ExampleSettings>());
        }

        [Fact(DisplayName = "JSON read from file")]
        public void JsonFileReaderTest()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var exampleFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "file-reader-test.json");
            var exampleFile = new FileInfo(exampleFilePath);
            if (exampleFile.Exists) exampleFile.Delete();
            FileStream fileStream = null;
            try
            {
                fileStream = exampleFile.OpenWrite();
                using (var writer = new StreamWriter(fileStream))
                {
                    fileStream = null;
                    writer.Write(Settings);
                }
                var reader = new JsonFileSettingsReader();
                reader.SetPath(exampleFilePath);
                var result = reader.Read<ExampleSettings>();
                result.ShouldBeEquivalentTo(settingsExepted);
            }
            finally
            {
                fileStream?.Dispose();
            }
        }

    }
}