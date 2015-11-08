namespace LocalNuget.Settings
{
    public interface ISettings
    {
        string WorkDirectory { get; }
        ISettingsDefaults Defaults { get; }
    }
}