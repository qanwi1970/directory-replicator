using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DirectoryReplicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in replicateList.Items)
            {
                var itemText = item.Content.ToString();
                var folders = itemText.Split(new[] {" ==> "}, StringSplitOptions.None);
                var sourceRoot = new DirectoryInfo(folders[0]);
                var destinationRoot = new DirectoryInfo(folders[1]);
                CopyDirectoryContents(sourceRoot, destinationRoot);
            }
            lblProgress.Content = "";
        }

        private void CopyDirectoryContents(DirectoryInfo sourceRoot, DirectoryInfo destinationRoot)
        {
            foreach (var directoryInfo in sourceRoot.EnumerateDirectories())
            {
                lblProgress.Content = directoryInfo.Name;
                lblProgress.Refresh();
                var newDirectory = new DirectoryInfo(destinationRoot.FullName + "\\" + directoryInfo.Name);
                if (!newDirectory.Exists)
                {
                    newDirectory.Create();
                }
                CopyDirectoryContents(directoryInfo, newDirectory);
            }
            foreach (var fileInfo in sourceRoot.EnumerateFiles())
            {
                lblProgress.Content = fileInfo.DirectoryName + "\\" + fileInfo.Name;
                lblProgress.Refresh();
                var newFile = destinationRoot.FullName + "\\" + fileInfo.Name;
                if (File.Exists(newFile))
                {
                    var newFileInfo = new FileInfo(newFile);
                    if (newFileInfo.LastWriteTimeUtc >= fileInfo.LastWriteTimeUtc)
                    {
                        continue;
                    }
                    // make sure it's not readonly
                    if (newFileInfo.IsReadOnly)
                    {
                        newFileInfo.IsReadOnly = false;
                    }
                }
                fileInfo.CopyTo(newFile, true);
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Documents ==> C:\Users\qanwi\OneDrive\Documents"
            });
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Music ==> C:\Users\qanwi\OneDrive\Music"
            });
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Pictures ==> C:\Users\qanwi\OneDrive\Pictures"
            });
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Videos ==> C:\Users\qanwi\OneDrive\Videos"
            });
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Documents ==> C:\Users\qanwi\Google Drive\Documents"
            });
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Music ==> C:\Users\qanwi\Google Drive\Music"
            });
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Pictures ==> C:\Users\qanwi\Google Drive\Pictures"
            });
            replicateList.Items.Add(new ListBoxItem
            {
                Content = @"C:\Users\qanwi\Videos ==> C:\Users\qanwi\Google Drive\Videos"
            });
        }
    }
}
