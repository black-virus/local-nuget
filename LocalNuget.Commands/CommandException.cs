using System;
using System.Collections.Generic;
using LocalNuget.Core.Exceptions;

namespace LocalNuget.Commands
{
    [Serializable]
    internal class CommandException : LocalNugetException
    {

        #region ExceptionData

        private class CommandExceptionData : ExceptionData
        {
            private CommandExceptionData(string code, string description) : base(code, description) { }

            public static CommandExceptionData CsProjNotExist { get; } = new CommandExceptionData("ADD01", "Visual Studio project isn't exist");
            public static CommandExceptionData NuspecInProjectExist { get; } = new CommandExceptionData("ADD02", "nuspec file in project directory already exist");
            public static CommandExceptionData NuspecInWorkExist { get; } = new CommandExceptionData("ADD03", "nuspec file in project directory already exist");
            public static CommandExceptionData CreateNuspecInProjectFailed { get; } = new CommandExceptionData("ADD04", "NUGET has not created nuspec file");

        }

        #endregion

        public override ExceptionSpace Space { get; } = ExceptionSpace.AddCommandExceptions;

        private CommandException(CommandExceptionData data, Exception innerException = null)
            : base(data, innerException)
        { }

        public static CommandException CsProjNotExistException(string csProjFile)
        {
            return new CommandException(CommandExceptionData.CsProjNotExist)
            {
                additionalInfo = new Dictionary<string, string> { { "CsProj", csProjFile } }
            };
        }

        public static CommandException NuspecInProjectExistException(string nuspecFile)
        {
            return new CommandException(CommandExceptionData.NuspecInProjectExist)
            {
                additionalInfo = new Dictionary<string, string> { { "NuspecFile", nuspecFile } }
            };
        }

        public static CommandException NuspecInWorkExistException(string nuspecFile)
        {
            return new CommandException(CommandExceptionData.NuspecInWorkExist)
            {
                additionalInfo = new Dictionary<string, string> { { "NuspecFile", nuspecFile } }
            };
        }

        public static CommandException CreateNuspecInProjectFailedException() { return new CommandException(CommandExceptionData.CreateNuspecInProjectFailed); }

    }

}