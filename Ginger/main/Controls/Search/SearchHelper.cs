using Ginger.Client.Services;
using Ginger.Config;
using Ginger.LocalDataBase.Config;
using Ginger.LocalDataBase.Models;
using Ginger.LocalDataBase.Services;
using Ginger.LocalDataBase.Services.LocalFile;
using Ginger.LocalDataBase.Services.ServerFile;
using Ginger.Models.Io;
using Ginger.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Controls.Search
{
    internal class SearchHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SynchronizeDocumentFile SynchronizeDocumentFile { get; set; }

        public DocumentFileService DocumentFileService { get; set; }

        public ClearDb ClearDb { get; set; }

        public MigrateDataBase MigrateDataBase { get; set; }

        public AddLocalFile AddLocalFile { get; set; }

        public AddServerFile AddServerFile { get; set; }

        public GetFileListToBeAdded GetFileListToBeAdded { get; set; }

        public GetFileListToBeRemoved GetFileListToBeRemoved { get; set; }

        public GetFileListToBeUpdated GetFileListToBeUpdated { get; set; }

        public AddFileJobToDataBase AddFileJobToDataBase { get; set; }

        public RemoveAllLocalFile RemoveAllLocalFile { get; set; }

        public RemoveAllServerFile RemoveAllServerFile { get; set; }

        private string ServerUrl
        {
            get { return AppContext.ServerUrl; }
        }

        private string LoginId
        {
            get { return AppContext.LoginId; }
        }

        private string AuthToken
        {
            get { return AppContext.AuthToken; }
        }
    }
}
