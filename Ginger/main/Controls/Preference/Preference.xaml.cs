using Ginger.Config;
using Ginger.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Ginger.Controls.Preference
{
    /// <summary>
    /// Preference.xaml の相互作用ロジック
    /// </summary>
    public partial class Preference : UserControl
    {
        public Preference()
        {
            this.InitializeComponent();
            this.Model = new PreferenceViewModel();
            this.Model.ServerUrl.Value = AppContext.ServerUrl;
            this.Model.LoginId.Value = AppContext.LoginId;
            this.Model.UserName.Value = AppContext.UserName;
            this.Model.MailAddress.Value = AppContext.MailAddress;
            this.Model.AuthToken.Value = AppContext.AuthToken;
            this.Model.TargetDirectoryPath.Value = AppContext.TargetDirectoryPath;
            this.Model.TargetFileExtensionList.Value = string.Join(",", AppContext.TargetFileExtensionList);
            this.Model.CountPerPage.Value = AppContext.CountPerPage.ToString();
            this.DataContext = this.Model;
        }

        private PreferenceViewModel Model { get; set; }

        private void CheckServerConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.CheckServerConnection();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.SaveServerSettings();
            this.Model.SaveUserSettings();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.Register();
        }

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Model.Authenticate();
        }

        private void FileWatcherToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.Model.StartFileWatch();
        }

        private void FileWatcherToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Model.StopFileWatch();
        }
    }
}
