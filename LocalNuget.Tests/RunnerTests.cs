using LocalNuget.Settings;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using FluentAssertions;
using Xunit;
// ReSharper disable UnusedMember.Global

namespace LocalNuget.Tests
{

    // ReSharper disable once UnusedMember.Global
    [Trait("", "NUGET Runner")]
    public class RunnerTests
    {

        [Fact(DisplayName = "Create default settings")]
        public void JsonSettingsSerializeTest()
        {
            var settings = new DefaultSettings
            {
                WorkDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Defaults = new DefaultSettingsDefaults
                {
                    Author = "virus",
                    ProjectUrl = "http://project.url",
                    Copyright="2015"
                }
            };
            var s = JsonConvert.SerializeObject(settings);
            s.Should()
                .Be(
                    "{\"WorkDirectory\":\"C:\\\\00_Praca\\\\99_WorkingCopy\\\\Local NUGET\\\\LocalNuget.Tests\\\\bin\\\\Debug\",\"Defaults\":{\"LicenceUrl\":null,\"ProjectUrl\":\"http://project.url\",\"Author\":\"virus\",\"Copyright\":\"2015\"}}");
        }

    }
}
