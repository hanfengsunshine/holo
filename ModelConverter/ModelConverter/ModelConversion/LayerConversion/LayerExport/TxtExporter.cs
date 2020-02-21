using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelConversion.LayerConversion.LayerExport
{
    using FrameImport;

    public class TxtExporter : IExporter
    {
        public void WriteFrameToFile(Frame frame, string outputLayerDir)
        {
            string modelString = GetFrameAsString(frame);
            string outputPath = outputLayerDir + @"\" + frame.Filename + ".txt";
            using (StreamWriter file = new StreamWriter(outputPath, false, Encoding.ASCII, ushort.MaxValue))
            {
                file.Write(modelString);
            }
        }

        private string GetFrameAsString(Frame frame)
        {
            StringBuilder modelString = new StringBuilder();
            modelString.Append("BOUNDS\n" + ConvertArrayToString(frame.BoundingBox) + "\n");
            modelString.Append("NUMBER OF FACET EDGES " + frame.NumberOfFacetEdges.ToString() + "\n");
            modelString.Append("VERTICES " + frame.Vertices.Length.ToString() + "\n" + ConvertArrayToString(frame.Vertices) + "\n");
            modelString.Append("INDICES\n" + ConvertArrayToString(frame.Indices) + "\n");
            if (frame.Vectors != null)
            {
                modelString.Append("VECTORS " + frame.Vectors.Length.ToString() + "\n" + ConvertArrayToString(frame.Vectors) + "\n");
            }
            if (frame.Scalars != null)
            {
                modelString.Append("SCALARS " + frame.Scalars.Length.ToString() + "\n" + ConvertArrayToString(frame.Scalars) + "\n");
            }
            return modelString.ToString();
        }

        private string ConvertArrayToString(double[][] jaggedArray)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                string vertexStr = string.Join(" ", jaggedArray[i].Select(p => Math.Round(p, 5).ToString(CultureInfo.InvariantCulture)).ToArray());
                stringBuilder.Append(vertexStr + ' ');
            }
            string finalString = stringBuilder.ToString();
            finalString = finalString.TrimEnd();
            return finalString;
        }

        private string ConvertArrayToString(int[] indicesArray)
        {
            string txtArray = string.Join(" ", indicesArray.Select(p => p.ToString()).ToArray());
            return txtArray;
        }
        private string ConvertArrayToString(double[] indicesArray)
        {
            string txtArray = string.Join(" ", indicesArray.Select(p => Math.Round(p, 5).ToString(CultureInfo.InvariantCulture)).ToArray());
            return txtArray;
        }
    }
}

