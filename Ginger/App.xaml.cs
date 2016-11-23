using Ginger.Config;
using Ginger.LocalDataBase.Services;
using Ginger.Properties;
using Ginger.Windows.Main;
using log4net;
using Spring.Context.Support;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Ginger
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
            : base()
        {
            var text = "a<mark>b</mark>c<mark>d</mark>ef";
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                var regex = new Regex(@"<mark>([^>]+)</mark>");
                var match = regex.Match(text);
                if (match.Success)
                {
                    sb.Append(text.Substring(0, match.Index));
                    foreach (var x in match.Groups)
                    {
                        System.Diagnostics.Debug.WriteLine("<" + x + ">");
                    }

                    sb.Append(match.Groups[1].Value);
                }
                else
                {
                    break;
                }

                text = text.Substring(match.Index + match.Length);

                System.Diagnostics.Debug.WriteLine("next text :" + text);
            }

            sb.Append(text);
            System.Diagnostics.Debug.WriteLine(sb);

            Logger.Warn("START");
            this.InitializeContext();
            this.InitializeDatabase();
            //Settings.Default.LoginId = "user";
            //Settings.Default.TargetDirectoryPath = @"C:\onion-data";
            //Settings.Default.ServerUrl = "http://localhost:9000";
            //Settings.Default.Save();

            this.Startup += this.App_Startup;
        }

        private void InitializeContext()
        {
            AppContext.Context = new XmlApplicationContext("spring.xml");
        }

        private void InitializeDatabase()
        {
            var migrateDataBase = AppContext.GetObject<MigrateDataBase>();
            migrateDataBase.Execute();
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            this.MainWindow = new MainWindow();
            this.MainWindow.Show();
            this.MainWindow.Closed += (a, b) => this.Shutdown();
        }
    }
}
