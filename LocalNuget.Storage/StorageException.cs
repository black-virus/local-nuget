using System;
using System.Collections.Generic;
using LocalNuget.Core.Exceptions;

namespace LocalNuget.Storage
{
    [Serializable]
    internal class StorageException : LocalNugetException
    {

        #region ExceptionData

        private class StorageExceptionData : ExceptionData
        {
            private StorageExceptionData(string code, string description) : base(code, description) { }

            public static StorageExceptionData PackageAlreadyExist { get; } = new StorageExceptionData("ADD01", "Package already exist");

        }

        #endregion

        public override ExceptionSpace Space { get; } = ExceptionSpace.StorageExceptions;

        private StorageException(StorageExceptionData data, Exception innerException = null)
            : base(data, innerException)
        { }

        public static StorageException PackageAlreadyExist(string packageName)
        {
            return new StorageException(StorageExceptionData.PackageAlreadyExist)
            {
                additionalInfo = new Dictionary<string, string> { { "Package", packageName } }
            };
        }

    }

}