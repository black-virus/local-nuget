namespace LocalNuget.Settings
{
    public class DefaultSettings : ISettings
    {
        public string WorkDirectory { get; set; }
        public ISettingsDefaults Defaults { get; set; }
    }
}