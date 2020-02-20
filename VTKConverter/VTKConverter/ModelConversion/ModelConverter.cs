using System.IO;
using Newtonsoft.Json;

namespace VTKConverter
{
    class ModelConverter
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ModelInfo Info { get; private set; }
        private string outputRootDir;

        public void Convert(string inputRootDir, string outputFolder)
        {
            CreateOutputRoot(inputRootDir, outputFolder);
            ReadInfoFile(inputRootDir);
            ConvertSingleModel();
        }

        private void CreateOutputRoot(string inputRootDir, string outputFolder)
        {
            outputRootDir = outputFolder + @"\" + Path.GetFileName(inputRootDir);
            Directory.CreateDirectory(outputRootDir);
            File.Copy(inputRootDir + @"\ModelInfo.json", outputRootDir + @"\ModelInfo.json", true);
        }

        private void ConvertSingleModel()
        {
            var layerConverter = new LayerConverter();
            Log.Info(Info.Caption + " conversion started!");
            foreach (ModelLayerInfo layerInfo in Info.Layers)
            {
                layerConverter.Convert(layerInfo, outputRootDir);
            }
        }

        protected void ReadInfoFile(string rootDirectory)
        {
            try
            {
                ReadModelInfoJson(rootDirectory);
            }
            catch (FileNotFoundException ex)
            {
                throw Log.ThrowError("No ModelInfo.json found in root folder!", ex);
            }
            foreach (ModelLayerInfo layerInfo in Info.Layers)
            {
                layerInfo.Directory = rootDirectory + @"\" + layerInfo.Directory;
            }

            if (Info.Layers.Count == 0)
            {
                throw Log.ThrowError("No layers found in ModelInfo.json file", new InvalidDataException());
            }
        }

        private void ReadModelInfoJson(string rootDirectory)
        {
            using (StreamReader r = new StreamReader(rootDirectory + @"\" + "ModelInfo.json"))
            {
                string json = r.ReadToEnd();
                Info = JsonConvert.DeserializeObject<ModelInfo>(json);
            }
        }
    }
}
