using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RevitAPIAddins.App
{
   public class RevitApiApp : IExternalApplication
   {
      public Result OnShutdown(UIControlledApplication application)
      {





         return Result.Succeeded;


      }

      public Result OnStartup(UIControlledApplication application)
      {
         var path = Assembly.GetExecutingAssembly().Location;
         var image = ToBitmapImage(Properties.Resources._32);
         application.CreateRibbonTab("My Ribbons");
         var panel = application.CreateRibbonPanel("My Ribbons", "More Tools");
         var pushButton1 =
             panel.AddItem(new PushButtonData("Family Editor", "Family Editor", path,
                 "RevitAPIAddins.GetSolids.FormworkCmd")) as PushButton;
         pushButton1.LargeImage = image;

         //var pushButton2 =
         //    panel.AddItem(new PushButtonData("Split Wall", "Split Wall", path,
         //        "WpfApp1.SplitWallLayersCmd")) as PushButton;
         //pushButton2.LargeImage = image;


         return Result.Succeeded;
      }


      public static BitmapImage ToBitmapImage(Bitmap bitmap)
      {
         using (var memory = new MemoryStream())
         {
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
         }
      }

   }
}
