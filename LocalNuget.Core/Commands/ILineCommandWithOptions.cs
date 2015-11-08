// ReSharper disable UnusedMemberInSuper.Global
namespace LocalNuget.Core.Commands
{

    public interface ILineCommandWithOptions<TOpt> : ILineCommand
        where TOpt : ICommandOptions
    {

        TOpt Options { get; set; }

    }

}
