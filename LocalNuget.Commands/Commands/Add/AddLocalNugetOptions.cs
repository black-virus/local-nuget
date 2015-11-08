using CommandLine;

namespace LocalNuget.Commands.Add
{
    public class AddLocalNugetOptions : CommandOptions
    {

        #region Properties

        [Option('p', "project", Required = true, HelpText = "Path to Visual Studio project file")]
        public string VisualStudioProject { get; set; }
        [Option('d', "defaults", DefaultValue = true, HelpText = "Use defaults from settings")]
        public bool UseSettingsDefaults { get; set; }
        [Option('f', "force", DefaultValue = false, HelpText = "Force overwrite exist spec")]
        public bool Force { get; set; }

        #endregion

        #region Constructors

        public AddLocalNugetOptions()
        {
            SetDefaults();
        }

        #endregion

        #region Methods

        private void SetDefaults()
        {
            UseSettingsDefaults = true;
        }

        #endregion

    }
}