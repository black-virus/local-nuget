using System.Reflection;

namespace LocalNuget.Core.Results
{
    public class PropertyFormat
    {

        public string Title { get; set; }
        public string Format { get; set; }
        public int Order { get; set; }
        public PropertyDisplay Display { get; set; }
        internal MethodInfo Get { private get; set; }

        public object GetValue(object obj)
        {
            return Get.Invoke(obj, null);
        }

    }
}