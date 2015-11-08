using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalNuget.Core.Results
{
    public class TextOutputFormat : IOutputFormat
    {
        public TextOutputFormat() { }

        public TextOutputFormat(IPropertyFormatInfoProvider formatInfoProvider)
        {
            FormatInfoProvider = formatInfoProvider;
        }

        #region Properties

        private IPropertyFormatInfoProvider FormatInfoProvider { get; }

        #endregion

        #region Methods

        public string FormatSingle<T>(T data)
        {
            var resolver = new FormatPropertiesResolver(typeof(T), FormatInfoProvider);
            var result = new StringBuilder();
            var firstLine = true;
            var properties = resolver.PropertiesFormat.OrderBy(prop => prop.Order).ThenBy(prop => prop.Title).ToArray();
            foreach (var property in properties)
            {
                if (!firstLine)
                    result.AppendLine();
                firstLine = false;
                result.AppendFormat("{0}:", property.Title);
                result.Append(property.Display == PropertyDisplay.Block ? Environment.NewLine : " ");
                result.AppendFormat(property.Format, property.GetValue(data));
            }
            return result.ToString();
        }

        public string FormatArray<T>(IEnumerable<T> collection)
        {
            var arr = collection.ToArray();
            var result = new StringBuilder();
            var firstLine = true;
            for (var i = 0; i < arr.Length; i++)
            {
                if (!firstLine)
                    result.AppendLine();
                firstLine = false;
                result.AppendFormat("---------- # {0} # ----------", i + 1);
                result.AppendLine();
                result.Append(FormatSingle(arr[i]));
            }
            return result.ToString();
        }

        #endregion

    }
}
