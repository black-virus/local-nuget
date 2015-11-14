namespace LocalNuget.Settings
{
    public interface ISettingsDefaults
    {
        string LicenceUrl { get; }
        string ProjectUrl { get; }
        string Author { get; }
        string Copyright { get; }
    }
}