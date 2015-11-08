namespace LocalNuget.Settings
{
    public class JsonFileSettingsReader : FileSettingsReader
    {
        public JsonFileSettingsReader() : base(new JSonSettingsReader()) { }
    }
}