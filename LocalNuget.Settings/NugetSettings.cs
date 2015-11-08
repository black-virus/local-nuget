using AutoMapper;
using Newtonsoft.Json;
using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LocalNuget.Settings
{
    public class NugetSettings : ISettings
    {

        #region Fields

        private readonly SettingsModel model;

        #endregion

        #region Properties

        public string WorkDirectory => model.WorkDirectory;
        public ISettingsDefaults Defaults => Mapper.Map<DefaultSettingsDefaults>(model.Defaults);

        #endregion

        #region Constructors

        public NugetSettings(ISettingsReader reader)
        {
            model = reader.Read<SettingsModel>() ??
               new SettingsModel
               {
                   WorkDirectory = AppDomain.CurrentDomain.BaseDirectory,
                   Defaults = new SettingsDefaultsModel
                   {
                       IconUrl = "INPUT-DEFAULT-ICON-URL",
                       LicenceUrl = "INPUT-DEFAULT-LICENCE-URL"
                   }
               };
        }

        #endregion

        #region Model

        private class SettingsModel
        {
            [JsonProperty("directory")]
            public string WorkDirectory { get; set; }
            [JsonProperty("defaults")]
            public SettingsDefaultsModel Defaults { get; set; }
        }

        internal class SettingsDefaultsModel
        {
            [JsonProperty("icon_url")]
            public string IconUrl { get; set; }
            [JsonProperty("licence_url")]
            public string LicenceUrl { get; set; }
        }

        #endregion

    }
}
