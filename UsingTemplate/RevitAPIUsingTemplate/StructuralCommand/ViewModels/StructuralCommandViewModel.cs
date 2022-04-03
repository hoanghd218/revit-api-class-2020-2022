using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StructuralCommand.ViewModels
{
   public sealed class StructuralCommandViewModel : INotifyPropertyChanged
   {
      public event PropertyChangedEventHandler PropertyChanged;

      [NotifyPropertyChangedInvocator]
      private void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
   }
}