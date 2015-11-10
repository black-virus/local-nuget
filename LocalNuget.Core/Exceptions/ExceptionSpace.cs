namespace LocalNuget.Core.Exceptions
{
    public class ExceptionSpace
    {
        public string SpaceCode { get; }
        public string SpaceDefaultDescription { get; }

        private ExceptionSpace(string code, string defaultDescription)
        {
            SpaceCode = code;
            SpaceDefaultDescription = defaultDescription;
        }

        public static ExceptionSpace AddCommandExceptions { get; } = new ExceptionSpace("CMD", "Add nuspec exceptions");
        public static ExceptionSpace StorageExceptions { get; } = new ExceptionSpace("STR", "Storage exceptions");
    }

    public abstract class ExceptionData
    {

        public string Code { get; }
        public string Description { get; }

        protected ExceptionData(string code, string description)
        {
            Code = code;
            Description = description;
        }

    }

}