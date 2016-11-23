using Ginger.Config;
using Ginger.Models;
using Ginger.Models.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ginger.Controls.FileList
{
    /// <summary>
    /// LocalFileList.xaml の相互作用ロジック
    /// </summary>
    public partial class FileList : UserControl
    {
        public FileList()
        {
            this.InitializeComponent();
            this.Model = new FileListViewModel();
            this.DataContext = this.Model;
            this.Model.UpdateProgress = this.UpdateProgress;
        }

        public FileListViewModel Model { get; set; }

        private void UpdateProgress(string message)
        {
            this.Message.Text = message;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.Search();
        }

        private void GetLocalFileListButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.GetLocalFileList1();
        }

        private void GetServerFileListButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.GetServerFileList1();
        }

        private void SynchroButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.SyncrhoAll();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.ClearDocumentFile();
        }
    }
}
