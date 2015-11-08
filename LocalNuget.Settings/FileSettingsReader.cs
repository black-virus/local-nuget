using System;
using System.IO;

namespace LocalNuget.Settings
{
    public abstract class FileSettingsReader : ISettingsReader
    {

        #region Fields

        private readonly IStringSettingsReader reader;

        #endregion

        #region Constructors

        protected FileSettingsReader(IStringSettingsReader reader)
        {
            this.reader = reader;
        }

        #endregion

        #region Methods

        public void SetPath(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists) throw new ArgumentException($"File {path} not exist", nameof(path));
            FileStream fs = null;
            try
            {
                fs = fileInfo.OpenRead();
                using (var streamReader = new StreamReader(fs))
                {
                    fs = null;
                    reader.InitializeData(streamReader.ReadToEnd());
                }
            }
            finally
            {
                fs?.Dispose();
            }
        }

        public T Read<T>()
        {
            return reader.Read<T>();
        }

        #endregion

    }
}