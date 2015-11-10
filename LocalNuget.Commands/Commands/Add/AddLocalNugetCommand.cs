using System;
using System.Diagnostics;
using System.IO;
using LocalNuget.Settings;
using LocalNuget.Utils;
using LocalNuget.Core.Exceptions;
using LocalNuget.Core.Commands;
using LocalNuget.Storage;

namespace LocalNuget.Commands.Add
{
    public class AddLocalNugetCommand : ILineCommand
    {

        #region Fields

        private readonly ISettings settings;
        private readonly IStorage storage;

        #endregion

        #region Properties

        public AddLocalNugetOptions Options { get; set; }

        #endregion

        #region Constructors

        public AddLocalNugetCommand(ISettings settings, IStorage storage)
        {
            this.settings = settings;
            this.storage = storage;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register new package. If any nuspec exist throwing exception
        /// </summary>
        public void Execute()
        {
            var csProjFileInfo = new FileInfo(Options.VisualStudioProject);
            if (!csProjFileInfo.Exists) throw CommandException.CsProjNotExistException(csProjFileInfo.FullName);
            var nuspecFileName = csProjFileInfo.Name.Replace(csProjFileInfo.Extension, ".nuspec");
            // ReSharper disable once PossibleNullReferenceException
            var nuspecProjFile = new FileInfo(Path.Combine(csProjFileInfo.Directory.FullName, nuspecFileName));
            var nuspecWorkFile = new FileInfo(Path.Combine(settings.WorkDirectory, nuspecFileName));
            if (nuspecProjFile.Exists)
            {
                if (Options.Force)
                {
                    nuspecProjFile.Delete();
                    nuspecProjFile.Refresh();
                }
                else
                    throw CommandException.NuspecInProjectExistException(nuspecProjFile.FullName);
            }
            if (nuspecWorkFile.Exists)
            {
                if (Options.Force)
                {
                    nuspecWorkFile.Delete();
                    nuspecWorkFile.Refresh();
                }
                else
                    throw CommandException.NuspecInWorkExistException(nuspecWorkFile.FullName);
            }

            var procInfo = new ProcessStartInfo(Path.Combine(settings.WorkDirectory, "nuget.exe"))
            {
                Arguments = $"spec {csProjFileInfo.Name} -f",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                WorkingDirectory = csProjFileInfo.Directory.FullName
            };
            var proc = Process.Start(procInfo);
            if (proc != null)
            {
                var output = proc.StandardOutput.ReadToEnd();
                Console.Write(output);
            }
            proc?.WaitForExit();

            nuspecProjFile.Refresh();
            if (!nuspecProjFile.Exists) throw CommandException.CreateNuspecInProjectFailedException();
            // setup links
            var nuspec = new Nuspec(nuspecProjFile.FullName)
            {
                ProjectUrl = String.Empty,
                IconUrl = Options.UseSettingsDefaults ? settings.Defaults.IconUrl : String.Empty,
                LicenseUrl = Options.UseSettingsDefaults ? settings.Defaults.LicenceUrl : String.Empty
            };
            nuspec.Save();
            nuspecProjFile.CopyTo(nuspecWorkFile.FullName);
            storage.Add(nuspecProjFile.Name, csProjFileInfo.FullName, nuspecProjFile.FullName, Options.Force);
        }

        #endregion

    }
}
