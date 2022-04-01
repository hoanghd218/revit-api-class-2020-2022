using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPIAddins.Formwork.View;
using RevitAPIAddins.Formwork.ViewModel;
using Utils.ActiveDocument;

namespace RevitAPIAddins.GetSolids
{
   [Transaction(TransactionMode.Manual)]
   public class FormworkCmd : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {

         ActiveDocument.GetData(commandData);

         var viewModel = new FormworkViewModel();
         var view = new FormworkView() { DataContext = viewModel };
         view.ShowDialog();

         return Result.Succeeded;
      }


   }
}
