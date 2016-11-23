using Common.Logging;
using Ginger.Client.Models;
using Ginger.Util;
using Ginger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Ginger.Controls.SearchRecord
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class SearchRecord : UserControl
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SearchRecord()
        {
            this.InitializeComponent();
        }

        private void OpenDirectory_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var hit = button.DataContext as VmHit;
            if (hit == null)
            {
                return;
            }

            var dirName = System.IO.Path.GetDirectoryName(hit.Document.FilePath);
            if (!System.IO.File.Exists(hit.Document.FilePath))
            {
                MessageBox.Show("ファイルが存在しません。");
                return;
            }

            System.Diagnostics.Process.Start("EXPLORER.EXE", @"/select," + hit.Document.FilePath);
            Logger.Warn(e.Source);
            Logger.Warn(e.RoutedEvent);
        }

        private void FileNameTextBlock_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var hit = e.NewValue as VmHit;

            if (textBlock == null || hit == null)
            {
                return;
            }

            List<Run> runs = HighlightUtils.GetHighlightRunList(hit.Title, Colors.Red);

            textBlock.FontSize = 20;
            textBlock.Inlines.Clear();
            textBlock.Inlines.AddRange(runs);
        }

        private void FileContentTextBlock_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var hit = e.NewValue as VmHit;

            if (textBlock == null || hit == null)
            {
                return;
            }

            List<Run> runs = HighlightUtils.GetHighlightRunList(hit.Document.FileContentForView, Colors.Red);

            textBlock.Inlines.Clear();
            textBlock.Inlines.AddRange(runs);
        }
    }
}
