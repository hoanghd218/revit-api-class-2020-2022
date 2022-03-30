using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using RevitAPIAddins.GridDimension.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.SelectionFilter;
using Utils.WPFUtils;
using Utils.XYZUtils;

namespace RevitAPIAddins.GridDimension.ViewModel
{
   public class GridDimensionViewModel : ViewModelBase
   {
      private string fileDataPath =
         Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GridDimension.txt";
      private Document doc;
      private Selection selection;
      public List<DimensionType> DimensionTypes { get; set; }
      public DimensionType SelectedDimensionType { get; set; }
      public RelayCommand OkCommand { get; set; }

      private List<GridModel> allGridModels;

      public GridDimensionViewModel(Document doc, Selection selection)
      {
         this.doc = doc;
         this.selection = selection;
         DimensionTypes = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>()
            .OrderBy(x => x.Name).Where(x => x.StyleType == DimensionStyleType.Linear || x.StyleType == DimensionStyleType.LinearFixed).Where(x => x.Name != "Linear Dimension Style").ToList();
         var previousSelectedDimensionTypeName = GetDataFromTextFile();
         SelectedDimensionType = DimensionTypes.FirstOrDefault(x => x.Name.Equals(previousSelectedDimensionTypeName));

         if (SelectedDimensionType == null)
         {
            SelectedDimensionType = DimensionTypes.FirstOrDefault();
         }

         OkCommand = new RelayCommand(Ok);

         var grids = new FilteredElementCollector(doc, doc.ActiveView.Id).OfClass(typeof(Grid)).Cast<Grid>().ToList();

         allGridModels = grids.Select(x => new GridModel(x)).ToList();

      }

      void Ok(object obj)
      {
         if (obj is System.Windows.Window window)
         {
            window.Close();
         }

         SaveDataToTextFile();

         while (true)
         {
            try
            {
               var rf = selection.PickObject(ObjectType.Element, new GridSelectionFilter(), "Select Grid");
               var grid = doc.GetElement(rf) as Grid;
               var selectedGridModel = new GridModel(grid);

               var parallelGrids = allGridModels.Where(x => x.Direction.IsParallel(selectedGridModel.Direction)).ToList();

               var referenceArray = new ReferenceArray();
               parallelGrids.ForEach(x => referenceArray.Append(x.Reference));

               var selectedPoint = rf.GlobalPoint;
               var lineDimDirection = selectedGridModel.Direction.CrossProduct(XYZ.BasisZ);
               var lineDim = Line.CreateBound(selectedPoint, selectedPoint.Add(lineDimDirection));

               using (var tx = new Transaction(doc, "Create Dim"))
               {
                  tx.Start();
                  doc.Create.NewDimension(doc.ActiveView, lineDim, referenceArray);

                  tx.Commit();
               }
            }
            catch (Exception e)
            {

               return;
            }
         }


      }

      void SaveDataToTextFile()
      {
         File.WriteAllText(fileDataPath, SelectedDimensionType.Name);
      }

      string GetDataFromTextFile()
      {
         var text = File.ReadAllText(fileDataPath);

         return text;
      }
   }
}
