using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class RibbonTab : IExternalApplication
    {
        // ExternalCommands assembly path
        static string AddInPath = typeof(RibbonTab).Assembly.Location;
        
        // uiApplication
        static UIApplication uiApplication = null;

        #region IExternalApplication Members
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // create customer Ribbon Items
                CreateRibbonTab(application);

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Intech Ribbon", ex.ToString());

                return Autodesk.Revit.UI.Result.Failed;
            }
        }


        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion

        private void CreateRibbonTab(UIControlledApplication application)
        {
            String tabName = "Intech Ribbon";
            application.CreateRibbonTab(tabName);
            /*
            // Basic Formating (Replace # in b#data with button number)

            PushButtonData b2Data = new PushButtonData("TigerExport", "Tiger Export", AddInPath, "IntechRibbon.TigerExport");
            b#Data.ToolTip = "Export all schedules into individual CSV files.";
            b#Data.Image = new BitmapImage(new Uri(IconPath));
            b#Data.LargeImage = new BitmapImage(new Uri(IconPath))
            PushButton pb# = ribbonSamplePanel.AddItem(b#Data) as PushButton;
            */

            string BasePath = AddInPath;
            BasePath = BasePath.Replace("RibbonSetup.dll", null);
            string IconPath = BasePath + @"Icon.png";
            Debug.WriteLine("Search " + IconPath);

            //Create different panels
            RibbonPanel SheetPanel = application.CreateRibbonPanel(tabName, "Sheets");
            RibbonPanel TaggingPanel = application.CreateRibbonPanel(tabName, "Tagging");
            RibbonPanel ExportPanel = application.CreateRibbonPanel(tabName, "Exports");

            //Sheets Ribbon
            {
                PushButtonData b1Data = new PushButtonData("TB Select", "TitleBlock Select", AddInPath, "Intech.TitleBlockSelector");
                b1Data.ToolTip = "Selects Title Blocks Inside Selected Sheets.";
                b1Data.Image = new BitmapImage(new Uri(IconPath));
                b1Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb1 = SheetPanel.AddItem(b1Data) as PushButton;
            }

            //Export Ribbon
            {
                String ExcelLogo = BasePath + @"SmallExcelLogo.png";
                PushButtonData b2Data = new PushButtonData("BOM", "BOM Export", AddInPath, "Intech.ExportBOM");
                b2Data.ToolTip = "Export schedules into Excel in a BOM format.";
                b2Data.LargeImage = new BitmapImage(new Uri(ExcelLogo));
                PushButton pb2 = ExportPanel.AddItem(b2Data) as PushButton;

                String CSVIcon = BasePath + @"CSVIcon.jpg";
                PushButtonData b3Data = new PushButtonData("TS Export", "TigerStop Export", AddInPath, "Intech.TigerStopExport");
                b3Data.ToolTip = "Export schedules in tigerstop format";
                b3Data.LargeImage = new BitmapImage(new Uri(CSVIcon));
                PushButton pb3 = ExportPanel.AddItem(b3Data) as PushButton;
            }
            
            //Tagging Ribbon
            {
                PushButtonData b4Data = new PushButtonData("Size", "Size", AddInPath, "Intech.Size");
                b4Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButtonData b5Data = new PushButtonData("Elevation", "Elevation", AddInPath, "Intech.Elevation");
                b5Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb4 = TaggingPanel.AddStackedItems(b4Data, b5Data) as PushButton;
               
                PushButtonData b6Data = new PushButtonData("OffSet", "OffSet", AddInPath, "Intech.Offset");
                b6Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButtonData b7Data = new PushButtonData("Renumber", "Number", AddInPath, "Intech.Number");
                b7Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb5 = TaggingPanel.AddStackedItems(b6Data, b7Data) as PushButton;

                PushButtonData b8Data = new PushButtonData("Length", "Length", AddInPath, "Intech.Length");
                b8Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButtonData b9Data = new PushButtonData("Equipment", "Equipment", AddInPath, "Intech.Equipment");
                b9Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb6 = TaggingPanel.AddStackedItems(b8Data, b9Data) as PushButton;

                PushButtonData b10Data = new PushButtonData("Hanger", "Hanger", AddInPath, "Intech.Hanger");
                b10Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButtonData b11Data = new PushButtonData("Tag Settings", "Tag Settings", AddInPath, "Intech.TagSettings");
                b11Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb7 = TaggingPanel.AddStackedItems(b10Data, b11Data) as PushButton;

                
            }
        }
    }
}