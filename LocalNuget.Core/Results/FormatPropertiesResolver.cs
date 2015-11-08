using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LocalNuget.Core.Results
{
    public class FormatPropertiesResolver
    {

        #region Fields

        private readonly IPropertyFormatInfoProvider formatInfoProvider;

        #endregion

        #region Properties

        public IEnumerable<PropertyFormat> PropertiesFormat { get; }

        #endregion

        #region Constructors

        public FormatPropertiesResolver(IReflect type, IPropertyFormatInfoProvider formatInfoProvider)
        {
            this.formatInfoProvider = formatInfoProvider;
            PropertiesFormat =
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => prop.CanRead)
                    .Select(ConvertToPropertyFormat)
                    .Where(pform => pform != null);
        }

        #endregion

        #region Methods

        private PropertyFormat ConvertToPropertyFormat(PropertyInfo property)
        {
            var getMethod = property.GetGetMethod(false);
            if (getMethod == null) return null;
            var format = property.GetCustomAttribute<FormatInfoAttribute>(true);
            PropertyFormat result = null;


            if (formatInfoProvider != null)
                result = formatInfoProvider.GetFormat(property);

            if (result == null) result = new PropertyFormat();

            if (string.IsNullOrEmpty(result.Title)) result.Title = property.Name;
            if (string.IsNullOrEmpty(result.Format)) result.Format = "{0}";
            if (result.Order <= 0) result.Order = int.MaxValue;
            if (!Enum.IsDefined(typeof(PropertyDisplay), result.Display)) result.Display = PropertyDisplay.Inline;

            if (format != null)
            {
                if (!string.IsNullOrEmpty(format.Title)) result.Title = format.Title;
                if (!string.IsNullOrEmpty(format.Format)) result.Format = format.Format;
                if (format.Order > 0) result.Order = format.Order;
                if (Enum.IsDefined(typeof(PropertyDisplay), format.Display)) result.Display = format.Display;
            }
            result.Get = getMethod;
            return result;
        }

        #endregion

    }
}