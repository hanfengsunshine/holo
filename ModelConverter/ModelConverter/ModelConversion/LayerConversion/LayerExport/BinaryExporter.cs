using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ModelConversion.LayerConversion.FrameImport;

namespace ModelConversion.LayerConversion.LayerExport
{
    public class BinaryExporter : IExporter
    {
        private static BinaryFormatter formatter = new BinaryFormatter();

        public void WriteFrameToFile(Frame frame, string outputLayerDir)
        {
            string outputPath = outputLayerDir + @"\" + frame.Filename + ".bytes";
            using (FileStream stream = new FileStream(outputPath, FileMode.Append, FileAccess.Write))
            {
                formatter.Serialize(stream, frame);
            }
        }
    }
}
