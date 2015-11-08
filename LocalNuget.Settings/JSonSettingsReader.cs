using System;
using Newtonsoft.Json;

namespace LocalNuget.Settings
{
    public class JSonSettingsReader : IStringSettingsReader
    {

        #region Fields

        private string data;

        #endregion

        #region Methods

        public T Read<T>() {
            if (String.IsNullOrEmpty(data)) throw new InvalidOperationException("You should initialize data before read it.");
            return JsonConvert.DeserializeObject<T>(data);
        }

        public void InitializeData(string initData) {
            data = initData;
        }

        #endregion

    }
}