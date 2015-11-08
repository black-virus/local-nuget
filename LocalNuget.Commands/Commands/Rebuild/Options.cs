using CommandLine;

namespace NugetRunner.Commands.Commands.Rebuild
{

    [CommandOptions(Const.CommandName)]
    public class Options : CommandOptions
    {

        [Option('f', "force", DefaultValue = false, HelpText = "Wymusza przebudowanie wszystkich paczek")]
        public bool Force { get; set; }

    }
}
