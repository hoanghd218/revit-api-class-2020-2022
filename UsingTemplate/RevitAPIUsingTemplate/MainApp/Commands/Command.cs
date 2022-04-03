using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MainApp.ViewModels;
using MainApp.Views;

namespace MainApp.Commands
{
   [Transaction(TransactionMode.Manual)]
   public class Command : IExternalCommand
   {
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var uiDocument = commandData.Application.ActiveUIDocument;
         var document = uiDocument.Document;

         var viewModel = new MainAppViewModel();
         var view = new MainAppView(viewModel);
         view.ShowDialog();

         return Result.Succeeded;
      }
   }
}