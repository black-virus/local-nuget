using YAXLib;

namespace LocalNuget.Utils
{
    public class NuspecFileModel
    {

        [YAXSerializeAs("src")]
        [YAXAttributeForClass]
        [YAXErrorIfMissed(YAXExceptionTypes.Error)]
        public string Source { get; set; }

        [YAXSerializeAs("target")]
        [YAXAttributeForClass]
        [YAXErrorIfMissed(YAXExceptionTypes.Error)]
        public string Target { get; set; }

        [YAXSerializeAs("exclude")]
        [YAXAttributeForClass]
        [YAXCollection(YAXCollectionSerializationTypes.Serially, SeparateBy = ";")]
        public string[] Excludes { get; set; }

    }
}