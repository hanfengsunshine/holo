using ModelConversion.LayerConversion.FrameImport;

namespace ModelConversion.LayerConversion.LayerExport
{
    interface IExporter
    {
        void WriteFrameToFile(Frame frame, string outputLayerDir);
    }
}
