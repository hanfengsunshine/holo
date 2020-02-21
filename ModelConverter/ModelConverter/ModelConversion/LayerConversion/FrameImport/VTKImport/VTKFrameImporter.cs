using System.IO;
using System.Text;
using Kitware.VTK;

namespace ModelConversion.LayerConversion.FrameImport
{
    using VTKImport;

    class VTKFrameImporter : IFrameImporter
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VTKFrameImporter()
        {
            var vtkOutput = vtkWin32OutputWindow.New();
            vtkOutput.SendToStdErrOn();
            vtkOutputWindow.SetInstance(vtkOutput);
        }

        public Frame Import(string inputPath, string outputRootDir, string dataType)
        {
            vtkDataSet vtkData = ReadVTKDataSet(inputPath);
            string filename = Path.GetFileNameWithoutExtension(inputPath);
            return ImportFrameData(vtkData, filename, dataType);
        }

        private vtkDataSet ReadVTKDataSet(string path)
        {
            using (vtkDataSetReader reader = new vtkDataSetReader())
            {
                reader.ReadAllScalarsOn();
                reader.GetReadAllScalars();
                reader.ReadAllVectorsOn();
                reader.GetReadAllVectors();
                reader.ReadAllColorScalarsOn();
                reader.GetReadAllColorScalars();
                reader.SetFileName(path);
                reader.Update();
                return reader.GetOutput();
            }

        }

        private Frame ImportFrameData(vtkDataSet vtkData, string filename, string dataType)
        {
            var vtkFrame = InitializeDataImport(vtkData, dataType);
            return new Frame()
            {
                Filename = filename,
                BoundingBox = vtkFrame.BoundingBox,
                Vertices = vtkFrame.Vertices,
                NumberOfFacetEdges = vtkFrame.NumberOfFacetEdges,
                Indices = vtkFrame.Indices,
                Vectors = vtkFrame.Vectors,
                Scalars = vtkFrame.Scalars,
            };
        }

        private VTKFrame InitializeDataImport(vtkDataSet vtkFrame, string dataType)
        {
            switch (dataType)
            {
                case "anatomy":
                    return new AnatomyFrame(vtkFrame);
                case "fibre":
                    return new FibreFrame(vtkFrame);
                case "flow":
                    return new FlowFrame(vtkFrame);
                default:
                    throw Log.ThrowError("Wrong model type!", new IOException());
            }
        }
    }
}