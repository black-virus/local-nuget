namespace LocalNuget.Commands.Add
{
    internal static class AddLocalNugetExceptionReason
    {
        public const string CsProjNotExist = "C01";
        public const string NuspecInProjectExist = "C02";
        public const string NuspecInWorkExist = "C03";
        public const string CreateNuspecInProjectFailed = "C04";
        public const string NuspecExist = "C05";
    }
}