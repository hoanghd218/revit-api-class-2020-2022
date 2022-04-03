using MainApp.ViewModels;

namespace MainApp.Views
{
   public partial class MainAppView
   {
      public MainAppView(MainAppViewModel viewModel)
      {
         InitializeComponent();
         DataContext = viewModel;
      }
   }
}