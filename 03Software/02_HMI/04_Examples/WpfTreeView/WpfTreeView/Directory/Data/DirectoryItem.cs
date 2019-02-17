﻿namespace WpfTreeView
{
  /// <summary>
  /// Information about a directorey item such as a drive, afile or a folder
  /// </summary>
  public class DirectoryItem
  {
    /// <summary>
    /// The type of this item
    /// </summary>
    public DirectoryItemType Type {get;set;}

    /// <summary>
    /// The absolute path to this item
    /// </summary>

    public string FullPath { get; set; }

    /// <summary>
    /// THe name of this directory item
    /// </summary>
    public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFileFolderName(this.FullPath); } }
  }
}