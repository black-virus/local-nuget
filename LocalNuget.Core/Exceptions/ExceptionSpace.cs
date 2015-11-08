namespace LocalNuget.Core.Exceptions
{
    public class ExceptionSpace
    {
        internal string SpaceCode { get; }

        private ExceptionSpace(string code)
        {
            SpaceCode = code;
        }

        public static ExceptionSpace AddCommandExceptions { get; } = new ExceptionSpace(ExceptionSpaces.AddCommandExceptions);
        public static ExceptionSpace StorageExceptions { get; } = new ExceptionSpace(ExceptionSpaces.StorageExceptions);
    }
}