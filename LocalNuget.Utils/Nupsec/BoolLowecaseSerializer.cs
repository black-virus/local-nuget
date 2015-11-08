using System.Xml.Linq;
using YAXLib;

namespace LocalNuget.Utils
{
    public class BoolLowecaseSerializer : ICustomSerializer<bool>
    {
        public void SerializeToAttribute(bool objectToSerialize, XAttribute attrToFill) {
            attrToFill.SetValue(objectToSerialize.ToString().ToLower());
        }

        public void SerializeToElement(bool objectToSerialize, XElement elemToFill) {
            elemToFill.SetValue(objectToSerialize.ToString().ToLower());
        }

        public string SerializeToValue(bool objectToSerialize) {
            return objectToSerialize.ToString().ToLower();
        }

        public bool DeserializeFromAttribute(XAttribute attrib) {
            return attrib.Value.ToLower() == "true";
        }

        public bool DeserializeFromElement(XElement element) {
            return element.Value.ToLower() == "true";
        }

        public bool DeserializeFromValue(string value) {
            return value.ToLower() == "true";
        }
    }
}