using System;

namespace LocalNuget.Core.Results
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatInfoAttribute : Attribute
    {
        public string Title { get; set; }
        public string Format { get; set; }
        public int Order { get; set; }
        public PropertyDisplay Display { get; set; }
    }
}