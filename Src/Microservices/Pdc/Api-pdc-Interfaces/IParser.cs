using Api_pdc_Entities.PrintDevice;
using static Api_pdc_Entities.Enums;

namespace Api_pdc_Parser
{
    public interface IParser
    {
        Task<Printer> ParseRawDataFromFile(string filePath, PrinterType printerType);
    }
}