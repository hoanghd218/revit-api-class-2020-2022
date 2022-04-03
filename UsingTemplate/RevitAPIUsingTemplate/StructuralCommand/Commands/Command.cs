using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using StructuralCommand.Commands.Handlers;
using StructuralCommand.Core;
using StructuralCommand.ViewModels;
using StructuralCommand.Views;

namespace StructuralCommand.Commands
{
   [UsedImplicitly]
   [Transaction(TransactionMode.Manual)]
   public class Command : IExternalCommand
   {
      private static StructuralCommandView _view;
      public static readonly CommandEventHandler EventHandler = new();

      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         if (_view is not null && _view.IsLoaded)
         {
            _view.Focus();
            return Result.Succeeded;
         }

         RevitApi.UiApplication = commandData.Application;
         var viewModel = new StructuralCommandViewModel();
         _view = new StructuralCommandView(viewModel);
         _view.Show();

         return Result.Succeeded;
      }
   }
}