using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LocalNuget.Core.Exceptions
{
    [Serializable]
    public abstract class LocalNugetException : Exception
    {

        public string Code => $"{Space.SpaceCode}{ExceptionData.Code}";
        public string Description => $"{Space.SpaceDefaultDescription}\r\n{ExceptionData.Description}";
        // ReSharper disable once MemberCanBePrivate.Global
        public abstract ExceptionSpace Space { get; }
        // ReSharper disable once MemberCanBePrivate.Global
        public ExceptionData ExceptionData { get; }
        protected Dictionary<string, string> additionalInfo = new Dictionary<string, string>();

        public override IDictionary Data => new Dictionary<string, string>
        {
            {"SpaceCode", Space.SpaceCode},
            {"SpaceDescription", Space.SpaceDefaultDescription},
            {"ExceptionDataCode", ExceptionData.Code},
            {"ExceptionDataDescription", ExceptionData.Description}
        }.Union(additionalInfo).ToDictionary(k => k.Key, v => v.Value);

        protected LocalNugetException(ExceptionData data, Exception innerException = null)
            : base(data.Description, innerException)
        {
            ExceptionData = data;
        }

    }
}
