using Autodesk.Revit.DB;
using Utils.ActiveDocument;

namespace RevitAPIAddins.WallLegend.Model
{
   public class WallLayerModel
   {
      public int LayerIndex { get; set; }
      public string Name { get; set; }

      public double Width { get; set; }
      public XYZ MidPoint { get; set; }
      public WallLayerModel(CompoundStructureLayer layer)
      {
         Width = layer.Width;
         var material = ActiveDocument.Document.GetElement(layer.MaterialId);
         Name= material.Name;
      }
   }
}
