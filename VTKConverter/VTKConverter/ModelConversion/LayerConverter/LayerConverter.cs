using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTKConverter
{
    class LayerConverter
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Convert(ModelLayerInfo layerInfo, string outputRootDir)
        {
            string outputLayerDir = outputRootDir + @"\" + Path.GetFileName(layerInfo.Directory);
            Directory.CreateDirectory(outputLayerDir);
            var fileConverter = new VTKImporter(outputRootDir);
            string[] inputPaths = GetFilepaths(layerInfo.Directory);
            foreach (string inputPath in inputPaths)
            {
                fileConverter.Convert(inputPath, outputLayerDir, layerInfo.DataType);
            }
        }

        private string[] GetFilepaths(string rootDirectory)
        {
            string[] filePaths = Directory.GetFiles(rootDirectory + @"\");
            if (filePaths == null)
            {
                throw Log.ThrowError("No files found in: " + rootDirectory, new FileNotFoundException());
            }
            return filePaths;
        }
    }
}
