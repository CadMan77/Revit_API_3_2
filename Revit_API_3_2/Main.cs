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

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new PipeFilter(), "Выберите трубы:");

            var pipeList = new List<Element>();
            double totLength = 0;

            foreach (var selectedRef in selectedElementRefList)
            {
                Pipe oPipe = doc.GetElement(selectedRef) as Pipe;
                pipeList.Add(oPipe);
                totLength += oPipe.LookupParameter("Length").AsDouble();
                //totLength += oPipe.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
            }

            double totLenMeter = UnitUtils.ConvertFromInternalUnits(totLength, UnitTypeId.Meters);
            TaskDialog.Show("Результат", $"Труб выбрано - {pipeList.Count}{Environment.NewLine}Общая длина выборки - {totLenMeter} м");
            return Result.Succeeded;
        }
    }
}
