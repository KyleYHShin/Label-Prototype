using System.Collections.Generic;
using System.Drawing.Printing;

namespace BasicModule.Utils
{
    public class PrintService
    {

        public List<string> GetUsablePrinterList()
        {
            var pNameList = new List<string>();
            
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                pNameList.Add(printer);
            }

            return pNameList;
        }

        public void PrintZebraProduct(string printerName, string zplCode)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings = new PrinterSettings();
            pd.PrinterSettings.PrinterName = printerName;

            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, zplCode);
        }

    }
}
