// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace LocalNuget.Models
{
    public class PackageInfoModel
    {
        public string Id { get; set; }
        public string VisualStudioProject { get; set; }
        public string NuspecInProject { get; set; }
        public string NuspecInWork { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
