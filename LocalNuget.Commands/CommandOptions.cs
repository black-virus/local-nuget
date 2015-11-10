using CommandLine;
using CommandLine.Text;

namespace LocalNuget.Commands
{

    public abstract class CommandOptions
    {
        [ParserState]
        // ReSharper disable once UnusedMember.Global
        public IParserState LastParserState { get; set; }

        [HelpOption]
        // ReSharper disable once UnusedMember.Global
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("<>", "<>"),
                Copyright = new CopyrightInfo("<>", 2012),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("<>");
            //help.AddPreOptionsLine("Usage: app -pSomeone");
            help.AddOptions(this);
            return help;
        }

    }

}
