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
            var destinationRoot = new DirectoryInfo(@"C:\Temp\dircopy");
            var sourceRoot = new DirectoryInfo(@"C:\Users\qanwi\Documents");
            foreach (ListBoxItem item in replicateList.Items)
            {
                var itemText = item.Content.ToString();
                var folders = itemText.Split(new[] {" ==> "}, StringSplitOptions.None);
                sourceRoot = new DirectoryInfo(folders[0]);
                destinationRoot = new DirectoryInfo(folders[1]);
                //CopyDirectoryContents(sourceRoot, destinationRoot);
            }
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
                    // make sure it's not readonly
                    var newFileInfo = new FileInfo(newFile);
                    if (newFileInfo.IsReadOnly)
                    {
                        newFileInfo.IsReadOnly = false;
                    }
                }
                fileInfo.CopyTo(newFile, true);
            }
        }
    }
}
