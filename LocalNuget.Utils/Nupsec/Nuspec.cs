using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using YAXLib;
// ReSharper disable UnusedMember.Global

namespace LocalNuget.Utils
{
    public class Nuspec
    {

        #region Fields

        private NuspecModel model;
        private readonly string fileLocation;

        #endregion

        #region Properties

        public string Id {
            get { return model.Id; }
            set { model.Id = value; }
        }

        public string Version {
            get { return model.Version; }
            set { model.Version = value; }
        }

        public string Title {
            get { return model.Title; }
            set { model.Title = value; }
        }

        public string[] Authors {
            get { return model.Authors; }
            set { model.Authors = value; }
        }

        public string[] Owners {
            get { return model.Owners; }
            set { model.Owners = value; }
        }

        public string Description {
            get { return model.Description; }
            set { model.Description = value; }
        }

        public string ReleaseNotes {
            get { return model.ReleaseNotes; }
            set { model.ReleaseNotes = value; }
        }

        public string Summary {
            get { return model.Summary; }
            set { model.Summary = value; }
        }

        public CultureInfo Language {
            get { return model.Language ?? (model.Language = CultureInfo.CurrentCulture); }
            set { model.Language = value; }
        }

        public string ProjectUrl {
            get { return model.ProjectUrl; }
            set { model.ProjectUrl = value; }
        }

        public string IconUrl {
            get { return model.IconUrl; }
            set { model.IconUrl = value; }
        }

        public string LicenseUrl {
            get { return model.LicenseUrl; }
            set { model.LicenseUrl = value; }
        }

        public string Copyright {
            get { return model.Copyright; }
            set { model.Copyright = value; }
        }

        public bool RequireLicenseAcceptance {
            get { return model.RequireLicenseAcceptance; }
            set { model.RequireLicenseAcceptance = value; }
        }

        public IDictionary<string, Version> Dependencies => model.Dependencies;

        public NuspecFileModel[] Files {
            get {
                return model.Files
                    .Select(file => new NuspecFileModel
                    {
                        Source = file.Source,
                        Target = file.Target,
                        Excludes = file.Excludes
                    })
                    .ToArray();
            }
        }

        #endregion

        #region Constructors

        public Nuspec(string xmlFile) {
            InitFromXmlFile(xmlFile);
            fileLocation = xmlFile;
        }

        #endregion

        #region Methods

        public void AddFile(string source, string target, string[] exclude = null) {
            var nFiles = new NuspecFileModel[model.Files.Length + 1];
            Array.Copy(model.Files, nFiles, model.Files.Length);
            nFiles[model.Files.Length] = new NuspecFileModel
            {
                Source = source,
                Target = target,
                Excludes = exclude
            };
            model.Files = nFiles;
        }

        public void AddDependency(string dependencyId, Version version = null) {
            model.Dependencies.Add(dependencyId, version);
        }

        private void InitFromXmlFile(string xmlFile) {
            var serializer = new YAXSerializer(typeof(NuspecModel), YAXExceptionHandlingPolicies.ThrowWarningsAndErrors,
                YAXExceptionTypes.Error, YAXSerializationOptions.DontSerializeNullObjects);
            model = serializer.DeserializeFromFile(xmlFile) as NuspecModel;
            if (model == null) throw new Exception("Can't deseralize xml nuspec file");
        }

        public void Save() {
            var serializer = new YAXSerializer(typeof(NuspecModel), YAXExceptionHandlingPolicies.ThrowWarningsAndErrors,
                YAXExceptionTypes.Error, YAXSerializationOptions.DontSerializeNullObjects);
            serializer.SerializeToFile(model, fileLocation);
        }

        #endregion

    }
}