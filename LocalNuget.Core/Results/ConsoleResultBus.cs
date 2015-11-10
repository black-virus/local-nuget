using System;
using System.Collections.Generic;

namespace LocalNuget.Core.Results
{
    public class ConsoleResultBus<T> : IResultBus<T>
    {

        #region Fields

        private readonly IOutputFormat formatter;

        #endregion

        #region Constructors

        public ConsoleResultBus(IOutputFormat formatter)
        {
            this.formatter = formatter;
        }

        #endregion

        #region Methods

        public void SetResult(T result)
        {
            Console.WriteLine(formatter.FormatSingle(result));
        }

        public void SetResult(T[] result)
        {
            Console.WriteLine(formatter.FormatArray(result));
        }

        #endregion

    }
}
