using BasicModule.Models;
using BasicModule.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;

namespace BasicModule.Utils
{
    public class PrintService
    {
        public LabelObject Label { get; set; }
        public ObservableCollection<BasicObject> ObjectList { get; set; }

        public List<string> GetPrinterList()
        {
            var pNameList = new List<string>();
            
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                pNameList.Add(printer);
                Console.WriteLine(printer);
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
