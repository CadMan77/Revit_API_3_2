using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_3_2
{
    [Transaction(TransactionMode.Manual)]

    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            uidoc.RefreshActiveView();

            try
            {

                IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберите трубы:");

                if (selectedElementRefList.Count>0)
                {

                var pipeList = new List<Element>();
                double totLength = 0;

                foreach (var selectedRef in selectedElementRefList)
                {
                    Pipe oPipe = doc.GetElement(selectedRef) as Pipe;
                    pipeList.Add(oPipe);
                    //totLength += oPipe.LookupParameter("Length").AsDouble();
                    totLength += oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
                }
                double totLenMeter = Math.Round(UnitUtils.ConvertFromInternalUnits(totLength, UnitTypeId.Meters),2);
                TaskDialog.Show("Результат", $"Труб выбрано - {pipeList.Count}{Environment.NewLine}Общая длина выборки - {totLenMeter} м");
                }
            }
            catch { }
            return Result.Succeeded;
        }
    }
}
