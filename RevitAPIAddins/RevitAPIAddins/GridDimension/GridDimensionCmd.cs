using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPIAddins.GridDimension.View;
using RevitAPIAddins.GridDimension.ViewModel;

namespace RevitAPIAddins.GridDimension
{
   [Transaction(TransactionMode.Manual)]
   public class GridDimensionCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var viewModel = new GridDimensionViewModel(commandData.Application.ActiveUIDocument.Document, commandData.Application.ActiveUIDocument.Selection);

         //set data context to the view
         var view = new GridDimensionView() { DataContext = viewModel };
         view.ShowDialog();

         return Result.Succeeded;
      }
   }
}
