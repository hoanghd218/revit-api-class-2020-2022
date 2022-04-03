using Autodesk.Revit.UI;
using MainApp.Commands;

namespace MainApp
{
   [UsedImplicitly]
   public class Application : IExternalApplication
   {
      private const string RibbonImageUri = "/MainApp;component/Resources/Icons/RibbonIcon16.png";
      private const string RibbonLargeImageUri = "/MainApp;component/Resources/Icons/RibbonIcon32.png";

      public Result OnStartup(UIControlledApplication application)
      {
         var panel = application.CreatePanel("Panel name", "MainApp");

         var showButton = panel.AddPushButton(typeof(Command), "Button text");
         showButton.ToolTip = "Tooltip";
         showButton.SetImage(RibbonImageUri);
         showButton.SetLargeImage(RibbonLargeImageUri);

         return Result.Succeeded;
      }

      public Result OnShutdown(UIControlledApplication application)
      {
         return Result.Succeeded;
      }
   }
}