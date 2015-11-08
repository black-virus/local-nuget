using System;

namespace LocalNuget.Core.Exceptions
{
    [Serializable]
    public abstract class LocalNugetException : Exception
    {

        public string Code => $"C:{Space.SpaceCode}{ExceptionCode}";
        // ReSharper disable once MemberCanBePrivate.Global
        public ExceptionSpace Space { get; }
        // ReSharper disable once MemberCanBePrivate.Global
        public string ExceptionCode { get; }

        protected LocalNugetException(ExceptionSpace space, string exceptionCode, Exception innerException = null)
            : base($"C:{space.SpaceCode}{exceptionCode}", innerException)
        {
            Space = space;
            ExceptionCode = exceptionCode;
        }

    }
}
