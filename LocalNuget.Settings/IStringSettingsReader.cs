namespace LocalNuget.Settings
{
    public interface IStringSettingsReader : ISettingsReader
    {
        void InitializeData(string data);
    }
}