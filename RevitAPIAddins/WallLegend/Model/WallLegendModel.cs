using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.ActiveDocument;
using Utils.CategoryUtils;

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
      public WallLegendModel(WallType wallType, List<ElementId> ids)
      {
         WallType = wallType;
         WallTypeName = WallType.Name;
         Model = WallType.get_Parameter(BuiltInParameter.ALL_MODEL_MODEL).AsString();
         Manufacture = WallType.get_Parameter(BuiltInParameter.ALL_MODEL_MANUFACTURER).AsString();
         var newEles = ids.Select(id => ActiveDocument.Document.GetElement(id)).ToList();
         TextNotes = newEles.Where(x => x is TextNote).Cast<TextNote>().ToList();
         WallLegend = newEles.FirstOrDefault(x => x.Category.ToBuiltinCategory() == BuiltInCategory.OST_LegendComponents);
      }

   }
}
