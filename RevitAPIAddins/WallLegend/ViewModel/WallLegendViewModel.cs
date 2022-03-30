using Autodesk.Revit.DB;
using RevitAPIAddins.WallLegend.Model;
using RevitAPIAddins.WallLegend.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Utils.ActiveDocument;
using Utils.CategoryUtils;
using Utils.WPFUtils;

namespace RevitAPIAddins.WallLegend.ViewModel
{
   public class WallLegendViewModel : ViewModelBase
   {
      public List<WallType> WallTypes { get; set; }

      public RelayCommand OkCommand { get; set; }
      public WallLegendViewModel()
      {
         GetData();
         OkCommand = new RelayCommand(Ok);
      }

      void Ok(object obj)
      {
         var selectedWallTypes = new List<WallType>();
         if (obj is WallLegendView window)
         {
            selectedWallTypes = window.LbWallTypes.SelectedItems.Cast<WallType>().ToList();
            window.Close();
         }

         var legendTemplate = new FilteredElementCollector(ActiveDocument.Document).OfClass(typeof(Autodesk.Revit.DB.View)).Cast<Autodesk.Revit.DB.View>().FirstOrDefault(x => x.ViewType == ViewType.Legend && x.Name == "wall template");



         using (var tx = new Transaction(ActiveDocument.Document, "Wall legend"))
         {
            tx.Start();

            var newLegendViewId = legendTemplate.Duplicate(ViewDuplicateOption.WithDetailing);

            var itemsInNewLegend = new FilteredElementCollector(ActiveDocument.Document, newLegendViewId).WhereElementIsNotElementType().Where(x =>
            {
               if (x is DetailLine || x is TextNote || x is FilledRegion || x.Category.ToBuiltinCategory() == BuiltInCategory.OST_LegendComponents)
               {
                  return true;
               }

               return false;

            }).ToList();

            var ids = itemsInNewLegend.Select(x => x.Id).ToList();

            var i = 1;

            foreach (var wallType in selectedWallTypes)
            {
               var newIds = ElementTransformUtils.CopyElements(ActiveDocument.Document, ids, XYZ.BasisX * 6.56167979 * i).ToList();
               i++;

               var model = new WallLegendModel(wallType, newIds);



               var v1 = GetTextNoteByKey(model.TextNotes, "v1");
               v1.Text = model.WallTypeName;


            }


            tx.Commit();
         }

      }


      TextNote GetTextNoteByKey(List<TextNote> textNotes, string key)
      {
         foreach (var item in textNotes)
         {
            var text = Regex.Replace(item.Text, "\n|\r", String.Empty);
            if (text == key)
            {
               return item;
            }
         }

         return null;
      }

      void GetData()
      {
         WallTypes = new FilteredElementCollector(ActiveDocument.Document).OfClass(typeof(WallType)).Cast<WallType>().OrderBy(x => x.Name).ToList();
      }
   }
}
