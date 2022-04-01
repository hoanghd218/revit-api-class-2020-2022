using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAPIAddins.GridDimension.View;
using RevitAPIAddins.GridDimension.ViewModel;
using RevitAPIAddins.WallLegend.View;
using RevitAPIAddins.WallLegend.ViewModel;
using Utils.ActiveDocument;
using Utils.GeometryUtils;

namespace RevitAPIAddins.GetSolids
{
   [Transaction(TransactionMode.Manual)]
   public class WallLegendCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         ActiveDocument.GetData(commandData);
         var vm = new WallLegendViewModel();
         var view = new WallLegendView() { DataContext= vm};
         view.ShowDialog();


         return Result.Succeeded;
      }

  
   }
}
