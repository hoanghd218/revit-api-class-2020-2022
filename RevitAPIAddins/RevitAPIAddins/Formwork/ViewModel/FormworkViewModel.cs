using Autodesk.Revit.DB;
using RevitAPIAddins.Formwork.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Utils.ActiveDocument;
using Utils.CategoryUtils;
using Utils.GeometryUtils;
using Utils.WPFUtils;

namespace RevitAPIAddins.Formwork.ViewModel
{
   public class FormworkViewModel : ViewModelBase
   {

      private int _selectionType = 1;
      /// <summary>
      /// 1 for active view 0 for project
      /// </summary>
      public int SelectionType
      {
         get { return _selectionType; }
         set
         {
            _selectionType = value;
            OnPropertyChanged();
         }
      }

      public bool IsCreateFormworkColumn { get; set; } = true;
      public bool IsCreateFormworkBeam { get; set; } = true;
      public bool IsCreateFormworkFloor { get; set; } = false;
      public bool IsCreateFormworkStair { get; set; } = false;
      public bool IsCreateFormworkFoundation { get; set; } = true;
      public RelayCommand OkCommand { get; set; }

      public FormworkViewModel()
      {
         OkCommand = new RelayCommand(Ok);
      }

      private void Ok(object obj)
      {
         if (obj is Window window)
         {
            window.Close();
         }

         var elementFormworkModels = GetElementsToCreateFormwork().Select(x => new ElementFormworkModel(x));

         using (var tx = new Transaction(ActiveDocument.Document, "Formwork"))
         {
            tx.Start();

            foreach (var elementFormworkModel in elementFormworkModels)
            {
               foreach (var sideFace in elementFormworkModel.OtherSideFaces)
               {
                  var sideFaceSolid = sideFace.CreateOriginalSolidFromPlanarFace();

                  var intersectElements = GetElementsAroundFormworkElement(elementFormworkModel);


                  var intersectSolids = intersectElements.SelectMany(x => x.GetAllSolids()).ToList();

                  var realFormworkSolid = CutSolidBySolids(sideFaceSolid, intersectSolids);

                  var area = realFormworkSolid.Volume / 0.082020997375;

                  var ds = realFormworkSolid.CreateDirectShape();
                  ds.LookupParameter("AREA").Set(area);
               }

               foreach (var sideFace in elementFormworkModel.LeftRightFaces)
               {
                  var sideFaceSolid = elementFormworkModel.GetSolidExtend2Sides(sideFace);

                  var intersectElements = GetElementsAroundFormworkElement(elementFormworkModel);


                  var intersectSolids = intersectElements.SelectMany(x => x.GetAllSolids()).ToList();

                  var realFormworkSolid = CutSolidBySolids(sideFaceSolid, intersectSolids);

                  var area = realFormworkSolid.Volume / 0.082020997375;

                  var ds = realFormworkSolid.CreateDirectShape();
                  ds.LookupParameter("AREA").Set(area);
               }
            }

            tx.Commit();
         }

      }

      Solid CutSolidBySolids(Solid originalSolid, List<Solid> cuttingSolids)
      {
         var cutted = originalSolid;
         foreach (var cuttingSolid in cuttingSolids)
         {
            cutted = BooleanOperationsUtils.ExecuteBooleanOperation(cutted, cuttingSolid, BooleanOperationsType.Difference);
         }

         return cutted;
      }


      private List<Element> GetElementsToCreateFormwork()
      {
         var eles = new List<Element>();
         FilteredElementCollector col = new FilteredElementCollector(ActiveDocument.Document).WhereElementIsNotElementType();
         if (SelectionType == 1)
         {
            col = new FilteredElementCollector(ActiveDocument.Document, ActiveDocument.ActiveView.Id).WhereElementIsNotElementType();
         }

         if (IsCreateFormworkColumn)
         {
            eles = col.OfCategory(BuiltInCategory.OST_StructuralColumns).ToList();
         }

         return eles;
      }

      List<Element> GetElementsAroundFormworkElement(ElementFormworkModel formworkElement)
      {
         // Use BoundingBoxIntersects filter to find elements with a bounding box that intersects the 
         // given Outline in the document. 

         // Create a Outline, uses a minimum and maximum XYZ point to initialize the outline. 

         var bbOfFormworkElement = formworkElement.Element.get_BoundingBox(null);

         Outline myOutLn = new Outline(bbOfFormworkElement.Min, bbOfFormworkElement.Max);

         // Create a BoundingBoxIntersects filter with this Outline
         BoundingBoxIntersectsFilter filter = new BoundingBoxIntersectsFilter(myOutLn);

         // Apply the filter to the elements in the active document
         // This filter excludes all objects derived from View and objects derived from ElementType
         FilteredElementCollector collector = new FilteredElementCollector(ActiveDocument.Document);
         IList<Element> elements = collector.WherePasses(filter).ToElements();


         // Find all walls which don't intersect with BoundingBox: use an inverted filter to match elements
         // Use shortcut command OfClass() to find walls only
         BoundingBoxIntersectsFilter invertFilter = new BoundingBoxIntersectsFilter(myOutLn, false); // inverted filter
         collector = new FilteredElementCollector(ActiveDocument.Document);
         if (SelectionType == 1)
         {
            collector = new FilteredElementCollector(ActiveDocument.Document, ActiveDocument.ActiveView.Id);
         }

         var intersect =
             collector.WhereElementIsNotElementType().WherePasses(invertFilter).Where(x => x.Category.ToBuiltinCategory() == BuiltInCategory.OST_StructuralFraming || x.Category.ToBuiltinCategory() == BuiltInCategory.OST_Floors).ToList();

         return intersect;

      }


   }
}
