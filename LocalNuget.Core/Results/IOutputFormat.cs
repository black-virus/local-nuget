using System.Collections.Generic;

namespace LocalNuget.Core.Results
{
    public interface IOutputFormat
    {
        //IPropertyFormatInfoProvider FormatInfoProvider { get; set; }
        string FormatSingle<T>(T data);
        string FormatArray<T>(IEnumerable<T> arr);
    }
}