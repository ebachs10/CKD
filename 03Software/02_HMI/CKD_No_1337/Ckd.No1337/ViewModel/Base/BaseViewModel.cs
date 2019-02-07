using PropertyChanged;
using System.ComponentModel;

namespace Ckd.No1337
{
  ///[ImplementPropertyChanged] - seems obsolete [ebs´]
  /// <summary>
  /// A base view model that fires Property Changed events as needed
  /// </summary>
  public class BaseViewModel : INotifyPropertyChanged
  {
    /// <summary>
    /// The event that is fired when any child property changes its value
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

    public void OnPropertyChanged(string name)
    {
      PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
  }
}
