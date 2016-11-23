using Ginger.Properties;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Config
{
    public class AppContext
    {
        public static XmlApplicationContext Context { get; set; }

        public static string ServerUrl
        {
            get
            {
                return Settings.Default.ServerUrl;
            }

            set
            {
                Settings.Default.ServerUrl = value;
                Settings.Default.Save();
            }
        }

        public static string LoginId
        {
            get
            {
                return Settings.Default.LoginId;
            }

            set
            {
                Settings.Default.LoginId = value;
                Settings.Default.Save();
            }
        }

        public static string MailAddress
        {
            get
            {
                return Settings.Default.MailAddress;
            }

            set
            {
                Settings.Default.MailAddress = value;
                Settings.Default.Save();
            }
        }

        public static string UserName
        {
            get
            {
                return Settings.Default.UserName;
            }

            set
            {
                Settings.Default.UserName = value;
                Settings.Default.Save();
            }
        }

        public static string AuthToken
        {
            get
            {
                return Settings.Default.AuthToken;
            }

            set
            {
                Settings.Default.AuthToken = value;
                Settings.Default.Save();
            }
        }

        public static string TargetDirectoryPath
        {
            get
            {
                return Settings.Default.TargetDirectoryPath;
            }

            set
            {
                Settings.Default.TargetDirectoryPath = value;
                Settings.Default.Save();
            }
        }

        public static List<string> TargetFileExtensionList
        {
            get
            {
                return Settings.Default.TargetFileExtensionList.Split(',').ToList();
            }

            set
            {
                Settings.Default.TargetFileExtensionList = string.Join(",", value);
                Settings.Default.Save();
            }
        }

        public static int CountPerPage
        {
            get
            {
                return Settings.Default.CountPerPage;
            }

            set
            {
                Settings.Default.CountPerPage = value;
                Settings.Default.Save();
            }
        }

        public static bool CheckFileSize(long fileSize)
        {
            return fileSize < 1024 * 1024;
        }

        public static T GetObject<T>()
        {
            return Context.GetObject<T>();
        }
    }
}
