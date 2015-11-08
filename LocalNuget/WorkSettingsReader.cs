using System;
using LocalNuget.Settings;
using System.IO;

namespace LocalNuget
{
    public class WorkSettingsReader : FileSettingsReader
    {
        public WorkSettingsReader(IStringSettingsReader reader)
            : base(reader)
        {
            SetPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json"));
        }
    }
}