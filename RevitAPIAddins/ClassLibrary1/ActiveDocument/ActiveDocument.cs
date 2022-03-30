using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Utils.ActiveDocument
{
   public class ActiveDocument
   {
      public static Document Document { get; private set; }
      public static View ActiveView { get; private set; }
      public static Selection Selection { get; private set; }

      public static void GetData(ExternalCommandData externalCommandData)
      {
         Document = externalCommandData.Application.ActiveUIDocument.Document;
         Selection = externalCommandData.Application.ActiveUIDocument.Selection;
         ActiveView = Document.ActiveView;
      }

   }
}
