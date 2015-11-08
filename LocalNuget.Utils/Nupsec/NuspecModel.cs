using System;
using System.Collections.Generic;
using System.Globalization;
using YAXLib;

namespace LocalNuget.Utils
{

    [YAXSerializeAs("package")]
    internal class NuspecModel
    {

        [YAXSerializeAs("id")]
        [YAXElementFor("metadata")]
        [YAXErrorIfMissed(YAXExceptionTypes.Error)]
        public string Id { get; set; }

        [YAXSerializeAs("version")]
        [YAXElementFor("metadata")]
        [YAXErrorIfMissed(YAXExceptionTypes.Error)]
        public string Version { get; set; }

        [YAXSerializeAs("title")]
        [YAXElementFor("metadata")]
        public string Title { get; set; }

        [YAXSerializeAs("authors")]
        [YAXElementFor("metadata")]
        [YAXCollection(YAXCollectionSerializationTypes.Serially, SeparateBy = ", ")]
        [YAXErrorIfMissed(YAXExceptionTypes.Error)]
        public string[] Authors { get; set; }

        [YAXSerializeAs("owners")]
        [YAXElementFor("metadata")]
        [YAXCollection(YAXCollectionSerializationTypes.Serially, SeparateBy = ", ")]
        [YAXErrorIfMissed(YAXExceptionTypes.Error)]
        public string[] Owners { get; set; }

        [YAXSerializeAs("description")]
        [YAXElementFor("metadata")]
        [YAXErrorIfMissed(YAXExceptionTypes.Error)]
        public string Description { get; set; }

        [YAXSerializeAs("releaseNotes")]
        [YAXElementFor("metadata")]
        public string ReleaseNotes { get; set; }

        [YAXSerializeAs("summary")]
        [YAXElementFor("metadata")]
        public string Summary { get; set; }

        [YAXSerializeAs("language")]
        [YAXElementFor("metadata")]
        public CultureInfo Language { get; set; }

        [YAXSerializeAs("projectUrl")]
        [YAXElementFor("metadata")]
        public string ProjectUrl { get; set; }

        [YAXSerializeAs("iconUrl")]
        [YAXElementFor("metadata")]
        public string IconUrl { get; set; }

        [YAXSerializeAs("licenseUrl")]
        [YAXElementFor("metadata")]
        public string LicenseUrl { get; set; }

        [YAXSerializeAs("copyright")]
        [YAXElementFor("metadata")]
        public string Copyright { get; set; }

        [YAXSerializeAs("requireLicenseAcceptance")]
        [YAXElementFor("metadata")]
        [YAXCustomSerializer(typeof(BoolLowecaseSerializer))]
        public bool RequireLicenseAcceptance { get; set; }

        [YAXSerializeAs("dependencies")]
        [YAXElementFor("metadata")]
        [YAXDictionary(EachPairName = "dependency", KeyName = "id", SerializeKeyAs = YAXNodeTypes.Attribute, ValueName = "version", SerializeValueAs = YAXNodeTypes.Attribute)]
// ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Dictionary<string, Version> Dependencies { get; set; }

        [YAXSerializeAs("files")]
        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "file")]
        public NuspecFileModel[] Files { get; set; }

    }
}
