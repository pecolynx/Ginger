using Ginger.Client.Exceptions;
using Ginger.Client.Models;
using Ginger.Client.Services;
using Ginger.Config;
using Ginger.LocalDataBase.Config;
using Ginger.LocalDataBase.Services;
using Ginger.Models.Io;
using Ginger.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Ginger.Controls.Search
{
    /// <summary>
    /// SearchControl.xaml の相互作用ロジック
    /// </summary>
    public partial class Search : UserControl
    {
        /// <summary>ロガー</summary>
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Search()
        {
            this.InitializeComponent();
            this.Model = new SearchViewModel();
            this.DataContext = this.Model;
        }

        /// <summary>Gets or sets ビューモデル</summary>
        private SearchViewModel Model { get; set; }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.Search();
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var page = button.DataContext as Ginger.Models.Page;
            if (page == null)
            {
                return;
            }

            this.Model.Search(page.PageNumber);
        }
    }
}
