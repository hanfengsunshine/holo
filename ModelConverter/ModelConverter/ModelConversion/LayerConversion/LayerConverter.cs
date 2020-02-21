using System.IO;

namespace ModelConversion.LayerConversion
{
    using FrameImport;
    using LayerExport;

    class LayerConverter
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string fileExtension;
        private string outputLayerDir;
        private ModelLayerInfo layerInfo;
        private IExporter layerExporter;

        public LayerConverter(string exportType)
        {
            switch (exportType)
            {
                case "binary":
                    layerExporter = new BinaryExporter();
                        break;
                case "txt":
                    layerExporter = new TxtExporter();
                    break;
                default:
                    throw Log.ThrowError("Export type not supported!", new IOException());
            }
        }

        public void Convert(ModelLayerInfo layerInfo, string outputRootDir)
        {
            this.layerInfo = layerInfo;
            string[] inputPaths = LoadFilepaths();
            CreateOutputLayerDir(outputRootDir);
            var frameImporter = InitializeFrameImporter();
            foreach (string inputPath in inputPaths)
            {
                ConvertFrame(inputPath, frameImporter);
            }
        }

        private string[] LoadFilepaths()
        {
            string[] filePaths = Directory.GetFiles(layerInfo.Directory + @"\");
            if (filePaths == null)
            {
                throw Log.ThrowError("No files found in: " + layerInfo.Directory, new FileNotFoundException());
            }
            fileExtension = LoadExtension(filePaths);
            return filePaths;
        }

        private IFrameImporter InitializeFrameImporter()
        {
            switch (fileExtension)
            {
                case ".vtk":
                    return new VTKFrameImporter();
                default:
                    throw Log.ThrowError("Unsupported file extension at: " + layerInfo.Directory, new IOException());
            }
        }

        private void ConvertFrame(string inputPath, IFrameImporter frameImporter)
        {
            Frame frame = frameImporter.Import(inputPath, outputLayerDir, layerInfo.DataType);
            string filename = Path.GetFileNameWithoutExtension(inputPath);
            layerExporter.WriteFrameToFile(frame, outputLayerDir);
            Log.Info(filename + " converted sucessfully.");
        }

        private string LoadExtension(string[] filePaths)
        {
            string extension = Path.GetExtension(filePaths[0]);
            foreach (string inputPath in filePaths)
            {
                if (!extension.Equals(Path.GetExtension(inputPath)))
                {
                    throw Log.ThrowError("File extensions are not consistent at: " + layerInfo.Directory, new IOException());
                };
            }
            return extension;
        }

        private string CreateOutputLayerDir(string outputRootDir)
        {
            outputLayerDir = outputRootDir + @"\" + Path.GetFileName(layerInfo.Directory);
            Directory.CreateDirectory(outputLayerDir);
            return outputLayerDir;
        }
    }
}
