using Ginger.Config;
using Ginger.LocalDataBase.Migrations;
using Ginger.LocalDataBase.Services;
using Ginger.Windows.Main;
using log4net;
using Spring.Context.Support;
using System.Data.SQLite;
using System.Windows;

namespace ginger
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App() : base()
        {
            Logger.Warn("START");
            this.InitializeContext();
            this.InitializeDatabase();

            /*
            var modelId = new AddDocumentFileToService().Execute("aaa", "bbb");
            Logger.Warn("model Id = " + modelId);
            Thread.Sleep(1000);
            new UpdateDocumentFileToService().Execute(modelId, "ccc", "ddd");
            Thread.Sleep(1000);
            new RemoveDocumentFileToService().Execute(modelId);
            */

            this.Startup += new StartupEventHandler(App_Startup);
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

        void App_Startup(object sender, StartupEventArgs e)
        {
            this.MainWindow = new MainWindow();
            this.MainWindow.Show();
        }
    }
}
