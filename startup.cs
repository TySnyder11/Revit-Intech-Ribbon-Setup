using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Intech;
using Intech.Revit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using AW = Autodesk.Windows;

public class App : IExternalApplication
{

    public Result OnStartup(UIControlledApplication application)
    {
        // Task 1: Register document opened event
        application.ControlledApplication.DocumentOpened += OnDocumentOpened;

        // Task 2: Register document saved event
        application.ControlledApplication.DocumentSaved += OnDocumentSaved;

        // Task 3: Register document sync event
        application.ControlledApplication.DocumentSynchronizedWithCentral += OnDocumentSynced;

        // Task 4: Initialize ribbon UI (if needed)
        CreateRibbonTab(application);

        return Result.Succeeded;
    }


    public Result OnShutdown(UIControlledApplication application)
    {
        try
        {
            string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Intech");
            string tempPath = Path.Combine(baseDir, "temp.txt");

            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Shutdown Cleanup", $"Failed to delete temp file:\n{ex.Message}");
        }

        return Result.Succeeded;
    }


    private void OnDocumentOpened(object sender, DocumentOpenedEventArgs e)
    {
        var manager = new RevitFileManager(e.Document, new TxtFormat());
        manager.InitializeTempFromLocal();
        manager.SyncToSharedWithDeletions();
    }

    private void OnDocumentSaved(object sender, DocumentSavedEventArgs e)
    {
        var manager = new RevitFileManager(e.Document, new TxtFormat());
        manager.SaveToLocal();
    }

    private void OnDocumentSynced(object sender, DocumentSynchronizedWithCentralEventArgs e)
    {
        var manager = new RevitFileManager(e.Document, new TxtFormat());
        manager.SyncToSharedWithDeletions();
    }

    // ExternalCommands assembly path
    static string AddInPath = typeof(App).Assembly.Location;
    public static string BasePath
    {
        get
        {
            string basePath = Path.GetDirectoryName(AddInPath);
            basePath = basePath.Replace("RibbonSetup.dll", null);
            return basePath;
        }
    }
    public void CreateRibbonTab(UIControlledApplication application)
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

        string IconPath = BasePath + @"\Icon.png";
        Debug.WriteLine("Search " + IconPath);

        //Create different panels
        RibbonPanel SheetPanel = application.CreateRibbonPanel(tabName, "Sheets");
        RibbonPanel TaggingPanel = application.CreateRibbonPanel(tabName, "Tagging");
        RibbonPanel ConnectTools = application.CreateRibbonPanel(tabName, "Quick Tools");
        RibbonPanel ExportPanel = application.CreateRibbonPanel(tabName, "Exports");
        RibbonPanel ImportPanel = application.CreateRibbonPanel(tabName, "Import");

        //Sheets Ribbon
        {
            PushButtonData b1Data = new PushButtonData("TB Select", "TitleBlock Select", AddInPath, "Intech.TitleBlockSelector");
            b1Data.ToolTip = "Selects Title Blocks Inside Selected Sheets.";
            b1Data.Image = new BitmapImage(new Uri(IconPath));
            b1Data.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton pb1 = SheetPanel.AddItem(b1Data) as PushButton;
            PushButtonData dvData = new PushButtonData("Dependent View Creator", "Dependent View", AddInPath, "Intech.DependentView");
            dvData.ToolTip = "Create dependent views for a plan view.";
            dvData.Image = new BitmapImage(new Uri(IconPath));
            dvData.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton dv = SheetPanel.AddItem(dvData) as PushButton;
            PushButtonData scData = new PushButtonData("Sheet Create", "Sheet Create", AddInPath, "Intech.SheetCreateInit");
            scData.ToolTip = "Create dependent views for a plan view.";
            scData.Image = new BitmapImage(new Uri(IconPath));
            scData.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton sc = SheetPanel.AddItem(scData) as PushButton;
            PushButtonData ssData = new PushButtonData("Sheet Settings", "Sheet Settings", AddInPath, "Intech.SheetSettingsMenu");
            ssData.ToolTip = "Create dependent views for a plan view.";
            ssData.Image = new BitmapImage(new Uri(IconPath));
            ssData.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton ss = SheetPanel.AddItem(ssData) as PushButton;
        }

        //Quick Tools Ribbon
        {
            PushButtonData b1Data = new PushButtonData("Connect", "Connect", AddInPath, "Intech.ConnectElementsCommand");
            b1Data.ToolTip = "Connects fitting to pipe or duct to other duct element.";
            b1Data.Image = new BitmapImage(new Uri(IconPath));
            b1Data.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton pb01 = ConnectTools.AddItem(b1Data) as PushButton;

            PushButtonData b2Data = new PushButtonData("Rotate", "Rotate", AddInPath, "Intech.RotateConnector");
            b2Data.ToolTip = "Selects a fitting then promps angle.";
            b2Data.Image = new BitmapImage(new Uri(IconPath));
            b2Data.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton pb02 = ConnectTools.AddItem(b2Data) as PushButton;

            PushButtonData b3Data = new PushButtonData("Rotate Around", "Rotate Around", AddInPath, "Intech.RotateConnectedElements");
            b3Data.ToolTip = "Selects a fitting and duct/pipe then promps angle.";
            b3Data.Image = new BitmapImage(new Uri(IconPath));
            b3Data.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton pb03 = ConnectTools.AddItem(b3Data) as PushButton;

            PulldownButtonData pulldownData = new PulldownButtonData("ParameterTools", "Parameter Tools");
            pulldownData.LargeImage = new BitmapImage(new Uri(IconPath));
            PulldownButton pulldownButton = ConnectTools.AddItem(pulldownData) as PulldownButton;

            //Parameter Tool Pull Down
            {
                PushButtonData b4Data = new PushButtonData("ParameterSync", "Parameter Sync", AddInPath, "Intech.ParameterSyncMenu");
                b4Data.ToolTip = "Opens menu to do fancy parameter stuff.";
                b4Data.Image = new BitmapImage(new Uri(IconPath));
                b4Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb04 = pulldownButton.AddPushButton(b4Data);

                PushButtonData b5Data = new PushButtonData("SharedParam", "Add Shared Parameter", AddInPath, "Intech.SharedParameter.FamilyTypeParameterAdd");
                b5Data.ToolTip = "Opens menu to do fancy parameter stuff.";
                b5Data.Image = new BitmapImage(new Uri(IconPath));
                b5Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb05 = pulldownButton.AddPushButton(b5Data);

                PushButtonData b6Data = new PushButtonData("FormulaPush", "Formula Push", AddInPath, "Intech.SharedParameter.FormulaAddMain");
                b6Data.ToolTip = "Opens menu to do fancy parameter stuff.";
                b6Data.Image = new BitmapImage(new Uri(IconPath));
                b6Data.LargeImage = new BitmapImage(new Uri(IconPath));
                PushButton pb06 = pulldownButton.AddPushButton(b6Data);
            }
        }

        //Export Ribbon
        {
            String ExcelLogo = BasePath + @"\SmallExcelLogo.png";
            PushButtonData b2Data = new PushButtonData("BOM", "BOM Export", AddInPath, "Intech.ExportBOM");
            b2Data.ToolTip = "Export schedules into Excel in a BOM format.";
            b2Data.LargeImage = new BitmapImage(new Uri(ExcelLogo));
            PushButton pb2 = ExportPanel.AddItem(b2Data) as PushButton;

            String CSVIcon = BasePath + @"\CSVIcon.jpg";
            PushButtonData b3Data = new PushButtonData("TS Export", "TigerStop Export", AddInPath, "Intech.TigerStopExport");
            b3Data.ToolTip = "Export schedules in tigerstop format";
            b3Data.LargeImage = new BitmapImage(new Uri(CSVIcon));
            PushButton pb3 = ExportPanel.AddItem(b3Data) as PushButton;
        }

        //Import Ribbon
        {
            String ExcelLogo = BasePath + @"\SmallExcelLogo.png";
            PushButtonData b4Data = new PushButtonData("Import", "Excel Import", AddInPath, "Intech.linkUI");
            b4Data.ToolTip = "Export schedules into Excel in a BOM format.";
            b4Data.LargeImage = new BitmapImage(new Uri(ExcelLogo));
            PushButton pb4 = ImportPanel.AddItem(b4Data) as PushButton;
        }

        List<Autodesk.Windows.RibbonItem> buttons = new List<Autodesk.Windows.RibbonItem>();

        //Tagging Ribbon
        {
            PushButtonData b4Data = new PushButtonData("Size", "Size", AddInPath, "Intech.Size");
            b4Data.LargeImage = new BitmapImage(new Uri(BasePath + @"\Tag Images\Size.png"));
            b4Data.Image = new BitmapImage(new Uri(BasePath + @"\Tag Images\SmallSize.png"));
            PushButtonData b5Data = new PushButtonData("Elevation", "Elevation", AddInPath, "Intech.Elevation");
            b5Data.LargeImage = new BitmapImage(new Uri(BasePath + @"\Tag Images\Elevation.png"));
            b5Data.Image = new BitmapImage(new Uri(BasePath + @"\Tag Images\SmallElevation.png"));
            PushButton pb4 = TaggingPanel.AddStackedItems(b4Data, b5Data) as PushButton;
            buttons.Add(GetButton(tabName, TaggingPanel.Name, "Size"));
            buttons.Add(GetButton(tabName, TaggingPanel.Name, "Elevation"));

            PushButtonData b6Data = new PushButtonData("OffSet", "OffSet", AddInPath, "Intech.Offset");
            b6Data.LargeImage = new BitmapImage(new Uri(BasePath + @"\Tag Images\OffSet.png"));
            b6Data.Image = new BitmapImage(new Uri(BasePath + @"\Tag Images\SmallOffSet.png"));
            PushButtonData b7Data = new PushButtonData("Renumber", "Number", AddInPath, "Intech.Number");
            b7Data.LargeImage = new BitmapImage(new Uri(BasePath + @"\Tag Images\Number.png"));
            b7Data.Image = new BitmapImage(new Uri(BasePath + @"\Tag Images\SmallNumber.png"));
            List<PushButton> pb5 = TaggingPanel.AddStackedItems(b6Data, b7Data) as List<PushButton>;
            buttons.Add(GetButton(tabName, TaggingPanel.Name, "OffSet"));
            buttons.Add(GetButton(tabName, TaggingPanel.Name, "Renumber"));

            PushButtonData b8Data = new PushButtonData("Length", "Length", AddInPath, "Intech.Length");
            b8Data.LargeImage = new BitmapImage(new Uri(BasePath + @"\Tag Images\Length.png"));
            b8Data.Image = new BitmapImage(new Uri(BasePath + @"\Tag Images\SmallLength.png"));
            PushButtonData b9Data = new PushButtonData("Hanger", "Hanger", AddInPath, "Intech.Hanger");
            b9Data.LargeImage = new BitmapImage(new Uri(BasePath + @"\Tag Images\Hanger.png"));
            b9Data.Image = new BitmapImage(new Uri(BasePath + @"\Tag Images\SmallHanger.png"));
            IList<Autodesk.Revit.UI.RibbonItem> pb6 = TaggingPanel.AddStackedItems(b8Data, b9Data);
            buttons.Add(GetButton(tabName, TaggingPanel.Name, "Length"));
            buttons.Add(GetButton(tabName, TaggingPanel.Name, "Hanger"));


            foreach (Autodesk.Windows.RibbonItem i in buttons)
            {
                i.Size = AW.RibbonItemSize.Large;
                i.ShowText = false;
            }

            SplitButtonData b12Data = new SplitButtonData("Extra", "Extra");
            SplitButton sb1 = TaggingPanel.AddItem(b12Data) as SplitButton;
            for (int i = 1; i <= 10; i++)
            {
                PushButtonData newb = new PushButtonData("Tag" + i.ToString(), "Tag" + i.ToString(), AddInPath, "Intech.Tag" + i.ToString());
                newb.LargeImage = new BitmapImage(new Uri(BasePath + @"\Tag Images\Tag" + i.ToString() + ".png"));
                newb.Image = new BitmapImage(new Uri(BasePath + @"\Tag Images\SmallTag" + i.ToString() + ".png"));
                Debug.WriteLine("Search " + BasePath + @"\Tag Images\Tag" + i.ToString() + ".png");
                sb1.AddPushButton(newb);
            }

            PushButtonData b11Data = new PushButtonData("Tag Settings", "Tag Settings", AddInPath, "Intech.TagSettings");
            b11Data.LargeImage = new BitmapImage(new Uri(IconPath));
            PushButton pb3 = TaggingPanel.AddItem(b11Data) as PushButton;

        }
    }

    public AW.RibbonItem GetButton(string tabName, string panelName, string itemName)
    {
        AW.RibbonControl ribbon = AW.ComponentManager.Ribbon;
        foreach (AW.RibbonTab tab in ribbon.Tabs)
        {
            if (tab.Name == tabName)
            {
                foreach (AW.RibbonPanel panel in tab.Panels)
                {
                    if (panel.Source.Title == panelName)
                    {
                        return panel.FindItem("CustomCtrl_%CustomCtrl_%" + tabName + "%" + panelName + "%" + itemName, true);
                    }
                }
            }
        }
        return null;
    }
}
