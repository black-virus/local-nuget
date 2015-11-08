namespace LocalNuget.Settings
{
    public interface ISettingsReader
    {
        T Read<T>();
    }
}
