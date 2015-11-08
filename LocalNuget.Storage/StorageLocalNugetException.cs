using System;
using LocalNuget.Core.Exceptions;

namespace LocalNuget.Storage
{
    [Serializable]
    internal class StorageLocalNugetException : LocalNugetException
    {

        public StorageLocalNugetException(string reason, Exception innerException = null)
            : base(ExceptionSpace.StorageExceptions, reason, innerException)
        { }

    }
}