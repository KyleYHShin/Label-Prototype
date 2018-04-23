using BasicModule.Models;
using BasicModule.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
            printerName = "ZDesigner GT800-300dpi EPL";//추후 삭제

            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings = new PrinterSettings();
            pd.PrinterSettings.PrinterName = printerName;

            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, zplCode);
        }
        
        public bool PrintLabel()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(PrintSinglePage);
            }catch(Exception e)
            {
            }

            return false;
        }

        private void PrintSinglePage(object sender, PrintPageEventArgs ev) { 
            Font printFont = new Font("3 of 9 Barcode", 17);
            Font printFont1 = new Font("Times New Roman", 9, FontStyle.Bold);

            SolidBrush br = new SolidBrush(Color.Black);

            ev.Graphics.DrawString("*AAAAAAFFF*", printFont, br, 10, 65);
            ev.Graphics.DrawString("*AAAAAAFFF*", printFont1, br, 10, 85);
        }
    }
}
