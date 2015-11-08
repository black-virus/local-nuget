using System.Reflection;

namespace LocalNuget.Core.Results
{
    public interface IPropertyFormatInfoProvider
    {

        PropertyFormat GetFormat(PropertyInfo property);

    }
}