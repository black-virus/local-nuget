using System;
using LocalNuget.Core.Exceptions;

namespace LocalNuget.Commands.Add
{
    [Serializable]
    internal class AddLocalNugetException : LocalNugetException
    {

        public AddLocalNugetException(string reason, Exception innerException = null)
            : base(ExceptionSpace.AddCommandExceptions, reason, innerException)
        { }

    }
}