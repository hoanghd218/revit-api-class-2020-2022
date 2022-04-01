using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Utils.GeometryUtils;
namespace RevitAPIAddins.Formwork.Model
{
   public class ElementFormworkModel
   {
      public double FormWorkArea { get; set; }
      public Element Element { get; set; }
      public List<Solid> Solids { get; set; }
      public List<PlanarFace> SideFaces { get; set; }
      public ElementFormworkModel(Element ele)
      {
         Element = ele;
         Solids = ele.GetAllSolids(true);
         var faces = Solids.Select(solid => solid.Faces).Flatten().Where(x => x is PlanarFace).Cast<PlanarFace>().ToList();

         SideFaces = faces.Where(x => x.IsVerticalPlanarFace()).ToList();
      }

   
   }
}
