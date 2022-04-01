using Autodesk.Revit.DB;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.ActiveDocument;
using Utils.CategoryUtils;
using Utils.GeometryUtils;
using Utils.XYZUtils;

namespace RevitAPIAddins.WallLegend.Model
{
   public class WallLegendModel
   {
      public List<TextNote> TextNotes { get; set; }
      public Element WallLegend { get; set; }
      public WallType WallType { get; set; }

      public string WallTypeName { get; set; }
      public string Model { get; set; }
      public string Manufacture { get; set; }

      public PlanarFace LeftFace { get; set; }

      public PlanarFace RightFace { get; set; }
      public List<WallLayerModel> WallLayerModels { get; set; } = new List<WallLayerModel>();
      public XYZ TopPoint { get; set; }
      public WallLegendModel(WallType wallType, List<ElementId> ids)
      {
         WallType = wallType;
         WallTypeName = WallType.Name;
         Model = WallType.get_Parameter(BuiltInParameter.ALL_MODEL_MODEL).AsString();
         Manufacture = WallType.get_Parameter(BuiltInParameter.ALL_MODEL_MANUFACTURER).AsString();
         var newEles = ids.Select(id => ActiveDocument.Document.GetElement(id)).ToList();
         TextNotes = newEles.Where(x => x is TextNote).Cast<TextNote>().ToList();
         WallLegend = newEles.FirstOrDefault(x => x.Category.ToBuiltinCategory() == BuiltInCategory.OST_LegendComponents);
         GetLegendComponentData();
      }

      public void GetLegendComponentData()
      {
         var solids = WallLegend.GetAllSolids(true);
         var faces = solids.Select(x => x.Faces).Flatten().Where(x => x is PlanarFace).Cast<PlanarFace>().ToList();
         var sideFaces = faces.Where(x => x.FaceNormal.IsParallel(XYZ.BasisY)).OrderBy(x => x.FaceNormal.DotProduct(XYZ.BasisX));
         LeftFace = sideFaces.First();
         RightFace = sideFaces.Last();

         var solid = solids.OrderByDescending(x => x.Volume).First();
         var bb = solid.GetBoundingBox();
         var bbTransform = bb.Transform;

         var maxPoint = bbTransform.OfPoint(bb.Max);
         TopPoint = new XYZ(maxPoint.X, maxPoint.Y, 0);

         var compoundStructure = WallType.GetCompoundStructure();
         var layers = compoundStructure.GetLayers();


         var index = 0;
         var endLayerPoint = new XYZ(TopPoint.X, TopPoint.Y, 0);

         foreach (var layer in layers)
         {
            var layerModel = new WallLayerModel(layer);
        
               layerModel.MidPoint = endLayerPoint.Add(layerModel.Width * 0.5 * XYZ.BasisX * -1);
               endLayerPoint= endLayerPoint.Add(layerModel.Width * XYZ.BasisX * -1);

            layerModel.LayerIndex = index;
            index++;
            WallLayerModels.Add(layerModel);
         }
      }

   }
}
