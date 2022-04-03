using StructuralCommand.ViewModels;

namespace StructuralCommand.Views
{
   public partial class StructuralCommandView
   {
      public StructuralCommandView(StructuralCommandViewModel viewModel)
      {
         InitializeComponent();
         DataContext = viewModel;
      }
   }
}