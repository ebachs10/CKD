using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace WpfTreeView
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    #region Constructor

  /// <summary>
  /// Default constructor
  /// </summary>

    public MainWindow()
    {
      InitializeComponent();

      var d = new DirectoryStructureViewModel();

      this.DataContext = new DirectoryStructureViewModel();

    }

    #endregion
  }
}
