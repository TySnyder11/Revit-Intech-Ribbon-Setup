using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using WinForms = System.Windows.Forms;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class RotateConnector : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            try
            {
                // STEP 1: Select the fitting (a FamilyInstance with a valid MEPModel).


                // Get the current selection
                Selection sel = uidoc.Selection;
                ICollection<ElementId> selectedIds = sel.GetElementIds();

                Reference fittingRef = null;

                // If something is already selected, use it
                if (selectedIds.Count == 1)
                {
                    ElementId selectedId = selectedIds.First();
                    Element selectedElement = doc.GetElement(selectedId);

                    // Optional: check if it's a fitting
                    if (new FittingSelectionFilter().AllowElement(selectedElement))
                    {
                        fittingRef = new Reference(selectedElement);
                    }
                }

                // If nothing selected or not a fitting, prompt the user
                if (fittingRef == null)
                {
                    fittingRef = sel.PickObject(
                    ObjectType.Element,
                    new FittingSelectionFilter(),
                    "Select the fitting to rotate.");
                }

                Element fitting = doc.GetElement(fittingRef.ElementId);
                if (fitting == null)
                {
                    message = "Selected element is not a valid fitting.";
                    return Result.Failed;
                }
                // Optionally, highlight the fitting.
                uidoc.Selection.SetElementIds(new List<ElementId> { fitting.Id });

                // STEP 2: Retrieve the connectors from the fitting.
                ConnectorSet connectors = GetConnectors(fitting);
                if (connectors == null || connectors.IsEmpty || connectors.Size < 2)
                {
                    message = "The selected fitting must have at least two connectors.";
                    return Result.Failed;
                }

                // STEP 3: Determine the main axis by finding the pair of connectors with the maximum separation.                
                if (!GetMainAxisConnectorPair(connectors, out Connector connectorA, out Connector connectorB))
                {
                    message = "Unable to determine the main axis from the fitting's connectors.";
                    return Result.Failed;
                }
                XYZ pointA = connectorA.Origin;
                XYZ pointB = connectorB.Origin;
                XYZ axisDirection = (pointB - pointA).Normalize();
                // Use the midpoint of the two connectors as the base point of the axis.
                XYZ basePoint = (pointA + pointB) * 0.5;

                // STEP 4: Prompt the user for a rotation angle (in degrees).
                string angleStr = MyInputBox.Show("Enter rotation angle in degrees:", "Rotation Angle", "");
                if (string.IsNullOrEmpty(angleStr))
                {
                    message = "No rotation angle provided.";
                    return Result.Cancelled;
                }
                if (!double.TryParse(angleStr, out double degrees))
                {
                    message = "Invalid rotation angle.";
                    return Result.Failed;
                }
                double radAngle = degrees * Math.PI / 180.0; // Convert to radians

                // STEP 5: Rotate the fitting about the axis determined from the connectors.
                using (Transaction t = new Transaction(doc, "Rotate Fitting"))
                {
                    t.Start();
                    // Create a rotation axis line through the basePoint in the computed direction.
                    Line rotationAxis = Line.CreateBound(basePoint, basePoint + axisDirection);
                    ElementTransformUtils.RotateElement(doc, fitting.Id, rotationAxis, radAngle);
                    t.Commit();
                }

                TaskDialog.Show("Success", $"Rotated fitting {fitting.Id} by {degrees}° about its main axis.");
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        /// <summary>
        /// Retrieves the connector set from the fitting.
        /// </summary>
        private ConnectorSet GetConnectors(Element element)
        {
            ConnectorSet connectors = null;
            if (element is FabricationPart fabPart)
            {
                return fabPart.ConnectorManager.Connectors;
            }
            if (element is FamilyInstance fi && fi.MEPModel != null)
            {
                connectors = fi.MEPModel.ConnectorManager.Connectors;
            }
            return connectors;
        }

        /// <summary>
        /// Finds the pair of connectors with the maximum distance between them.
        /// </summary>
        private bool GetMainAxisConnectorPair(ConnectorSet connectors, out Connector bestA, out Connector bestB)
        {
            bestA = null;
            bestB = null;
            foreach (Connector conn1 in connectors)
            {
                XYZ basis1 = conn1.CoordinateSystem.BasisZ;
                XYZ xyz1 = conn1.CoordinateSystem.Origin;
                foreach (Connector conn2 in connectors)
                {
                    if (conn1.Equals(conn2)) continue;
                    XYZ xyz2 = conn2.CoordinateSystem.Origin;
                    XYZ delta = xyz2 - xyz1;

                    // Check if delta is parallel to basis1
                    XYZ cross = delta.CrossProduct(basis1);
                    if (cross.GetLength() < 1e-9)
                    {
                        if(xyz1.GetLength() >= xyz2.GetLength())
                        {
                            bestA = conn1;
                            bestB = conn2;
                        }
                        else
                        {
                            bestA = conn2;
                            bestB = conn1;
                        }
                    }
                }
            }
            return (bestA != null && bestB != null);
        }

        /// <summary>
        /// A selection filter that accepts only fittings – FamilyInstances that have a valid MEPModel.
        /// </summary>
        public class FittingSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return (elem is FamilyInstance fi && fi.MEPModel != null) || 
                    (elem is FabricationPart fp);
            }

            public bool AllowReference(Reference r, XYZ p) => true;
        }
    }

    /// <summary>
    /// A simple input box implemented using Windows Forms.
    /// This replaces Microsoft.VisualBasic.Interaction.InputBox for C# projects.
    /// </summary>
    public static class MyInputBox
    {
        public static string Show(string prompt, string title, string defaultValue)
        {
            WinForms.Form form = new WinForms.Form();
            WinForms.Label label = new WinForms.Label();
            WinForms.TextBox textBox = new WinForms.TextBox();
            WinForms.Button buttonOk = new WinForms.Button();
            WinForms.Button buttonCancel = new WinForms.Button();

            form.Text = title;
            label.Text = prompt;
            textBox.Text = defaultValue;

            label.Left = 10;
            label.Top = 10;
            label.AutoSize = true;
            textBox.Left = 10;
            textBox.Top = label.Bottom + 10;
            textBox.Width = 400;

            buttonOk.Text = "OK";
            buttonOk.DialogResult = WinForms.DialogResult.OK;
            buttonOk.Left = 10;
            buttonOk.Top = textBox.Bottom + 10;

            buttonCancel.Text = "Cancel";
            buttonCancel.DialogResult = WinForms.DialogResult.Cancel;
            buttonCancel.Left = buttonOk.Right + 10;
            buttonCancel.Top = textBox.Bottom + 10;

            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(buttonOk);
            form.Controls.Add(buttonCancel);
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.StartPosition = WinForms.FormStartPosition.CenterScreen;
            form.ClientSize = new System.Drawing.Size(430, buttonOk.Bottom + 20);

            WinForms.DialogResult result = form.ShowDialog();
            return result == WinForms.DialogResult.OK ? textBox.Text : null;
        }
    }
}
