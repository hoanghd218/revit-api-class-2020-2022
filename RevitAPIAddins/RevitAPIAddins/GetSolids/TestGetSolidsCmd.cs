using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAPIAddins.GridDimension.View;
using RevitAPIAddins.GridDimension.ViewModel;
using Utils.GeometryUtils;

namespace RevitAPIAddins.GetSolids
{
   [Transaction(TransactionMode.Manual)]
   public class TestGetSolidsCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var doc = commandData.Application.ActiveUIDocument.Document;
         var selection = commandData.Application.ActiveUIDocument.Selection;
         var rf = selection.PickObject(ObjectType.Element, "Select an element to get solids");
         var ele = doc.GetElement(rf);


         var solids = ele.GetAllSolids(true);


         CreateDirectShape(solids, doc);


         return Result.Succeeded;
      }

      void CreateDirectShape(List<Solid> solids,Document doc)
      {
         using (var tx= new Transaction(doc,"Create DirectShape"))
         {
            tx.Start();

            var geo = new List<GeometryObject>();
            solids.ForEach(x => geo.Add(x));
            var directShape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            directShape.SetShape(geo);

            tx.Commit();
         }
  
      }
   }
}
