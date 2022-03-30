using Autodesk.Revit.DB;
using Utils.CurveUtils;

namespace RevitAPIAddins.GridDimension.Model
{
   public class GridModel
   {
      public Grid Grid { get; set; }
      public Reference Reference { get; set; }
      public XYZ Direction { get; set; }

      public GridModel(Grid grid)
      {
         this.Grid = grid;
         Reference = new Reference(grid);
         Direction = grid.Curve.Direction();
      }
   }
}
