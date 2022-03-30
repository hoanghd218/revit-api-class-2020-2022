using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MoreLinq;
using Utils.GeometryUtils;
using Utils.XYZUtils;

namespace RevitAPIAddins.DimensionColumn
{
   [Transaction(TransactionMode.Manual)]
   public class DimensionColumnCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var doc = commandData.Application.ActiveUIDocument.Document;
         var selection = commandData.Application.ActiveUIDocument.Selection;
         var rf = selection.PickObject(ObjectType.Element, "Select an element to get solids");
         var ele = doc.GetElement(rf);

         var solids = ele.GetAllSolids(false);
         var faces = solids.Select(x => x.Faces).Flatten().Where(x => x is PlanarFace).Cast<PlanarFace>().ToList();

         var rightDirection = doc.ActiveView.RightDirection;

         var leftFaceRightFace = faces.Where(x => x.FaceNormal.IsParallel(rightDirection));

         using (var tx = new Transaction(doc, "Create column dimension"))
         {
            tx.Start();
            var ra = new ReferenceArray();
            leftFaceRightFace.ForEach(x => ra.Append(x.Reference));
            var point = selection.PickPoint(ObjectSnapTypes.None);
            var line = Line.CreateBound(point, point.Add(rightDirection));
            doc.Create.NewDimension(doc.ActiveView, line, ra);

            tx.Commit();
         }

         return Result.Succeeded;
      }

      void CreateDirectShape(List<Solid> solids, Document doc)
      {
         using (var tx = new Transaction(doc, "Create DirectShape"))
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
