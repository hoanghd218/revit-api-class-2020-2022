using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Utils.SelectionFilter
{
   public class GridSelectionFilter : ISelectionFilter
   {
      public bool AllowElement(Element elem)
      {
         return elem is Grid;
      }

      public bool AllowReference(Reference reference, XYZ position)
      {
         return false;
      }
   }
}
