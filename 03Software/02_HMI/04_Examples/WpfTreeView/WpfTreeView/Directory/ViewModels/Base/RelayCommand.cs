using System;
using System.Windows.Input;

namespace WpfTreeView
{
  public class RelayCommand : ICommand
  {
    ///A basic command that runs an Action 

    #region Private Members

    /// <summary>
    /// The action to run
    /// </summary>
    private Action mAction;

    #endregion

    #region Public Events

    /// <summary>
    /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
    /// </summary>
    public event EventHandler CanExecuteChanged = (sender, e) => { };

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public RelayCommand(Action action)
    {
      mAction = action;



    }

    #endregion

    /// <summary>
    /// A relay command can always execute
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      throw new NotImplementedException();
    }
  }
}
