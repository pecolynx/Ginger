using Ginger.Client.Exceptions;
using Ginger.Client.Services;
using Ginger.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reactive.Bindings;
using Ginger.Models;

namespace Ginger.Controls.Preference
{
    public class PreferenceViewModel
    {
        private AuthService authService;

        private FileWatcher fileWatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferenceViewModel"/> class.
        /// </summary>
        public PreferenceViewModel()
        {
            //this.UserSettingsMessage.Value = "ユーザー登録に失敗しました。このIDは既に使用されています。";
            this.UserSettingsMessage.Value = "";
            this.authService = AppContext.GetObject<AuthService>();
            this.fileWatcher = AppContext.GetObject<FileWatcher>();
        }

        public ReactiveProperty<string> ServerUrl { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> ServerStatus { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> LoginId { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> LoginPassword { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> MailAddress { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> UserName { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> AuthToken { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> UserSettingsMessage { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> TargetDirectoryPath { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> TargetFileExtensionList { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> CountPerPage { get; } = new ReactiveProperty<string>();

        public void CheckServerConnection()
        {
            this.ServerStatus.Value = "接続中・・・";
            try
            {
                this.ServerStatus.Value = "接続成功(Version : " + new ServerStatusService().GetServerStatus(this.ServerUrl.Value) + ")";
            }
            catch (Exception)
            {
                this.ServerStatus.Value = "接続失敗";
            }
        }

        public void Register()
        {
            var loginId = this.LoginId.Value ?? string.Empty;
            var loginPassword = this.LoginPassword.Value ?? string.Empty;
            var mailAddress = this.MailAddress.Value ?? string.Empty;
            var userName = this.UserName.Value ?? string.Empty;

            if (string.IsNullOrEmpty(loginId.Trim()))
            {
                this.UserSettingsMessage.Value = "ログインIDを設定してください。";
                return;
            }

            if (string.IsNullOrEmpty(loginPassword.Trim()))
            {
                this.UserSettingsMessage.Value = "パスワードを設定してください。";
                return;
            }

            if (string.IsNullOrEmpty(mailAddress.Trim()))
            {
                this.UserSettingsMessage.Value = "メールアドレスを設定してください。";
                return;
            }

            if (string.IsNullOrEmpty(userName.Trim()))
            {
                this.UserSettingsMessage.Value = "ユーザー名を設定してください。";
                return;
            }

            this.LoginId.Value = loginId.Trim();
            this.LoginPassword.Value = loginPassword.Trim();
            this.MailAddress.Value = mailAddress.Trim();
            this.UserName.Value = userName.Trim();

            try
            {
                this.authService.Register(this.ServerUrl.Value, this.LoginId.Value, this.LoginPassword.Value, this.MailAddress.Value, this.UserName.Value);
                this.SaveUserSettings();
                this.UserSettingsMessage.Value = "ユーザー登録しました。";
            }
            catch (AuthException)
            {
                this.UserSettingsMessage.Value = "ユーザー登録に失敗しました。";
            }
            catch (Exception)
            {
                this.UserSettingsMessage.Value = "失敗しました。";
            }
        }

        public void Authenticate()
        {
            var loginId = this.LoginId.Value ?? string.Empty;
            var loginPassword = this.LoginPassword.Value ?? string.Empty;
            var mailAddress = this.MailAddress.Value ?? string.Empty;
            var userName = this.UserName.Value ?? string.Empty;

            if (string.IsNullOrEmpty(loginId.Trim()))
            {
                this.UserSettingsMessage.Value = "ログインIDを設定してください。";
                return;
            }

            if (string.IsNullOrEmpty(loginPassword.Trim()))
            {
                this.UserSettingsMessage.Value = "パスワードを設定してください。";
                return;
            }

            this.LoginId.Value = loginId.Trim();
            this.LoginPassword.Value = loginPassword.Trim();

            try
            {
                this.AuthToken.Value = this.authService.Authenticate(this.ServerUrl.Value, this.LoginId.Value, this.LoginPassword.Value);
                this.SaveUserSettings();
                this.UserSettingsMessage.Value = "認証しました。";
            }
            catch (AuthException)
            {
                this.UserSettingsMessage.Value = "認証に失敗しました。";
            }
            catch (Exception)
            {
                this.UserSettingsMessage.Value = "失敗しました。";
            }
        }

        public void SaveServerSettings()
        {
            AppContext.ServerUrl = this.ServerUrl.Value;
        }

        public void SaveUserSettings()
        {
            var loginId = this.LoginId.Value ?? string.Empty;
            var mailAddress = this.MailAddress.Value ?? string.Empty;
            var userName = this.UserName.Value ?? string.Empty;
            var authToken = this.AuthToken.Value ?? string.Empty;
            var targetDirectoryPath = this.TargetDirectoryPath.Value ?? string.Empty;
            var targetFileExtensionList = this.TargetFileExtensionList.Value ?? string.Empty;

            this.LoginId.Value = loginId.Trim();
            this.AuthToken.Value = authToken.Trim();
            this.MailAddress.Value = mailAddress.Trim();
            this.UserName.Value = userName.Trim();
            this.TargetDirectoryPath.Value = targetDirectoryPath.Trim();
            this.TargetFileExtensionList.Value = targetFileExtensionList.Trim();

            int value;
            if (!int.TryParse(this.CountPerPage.Value, out value))
            {
                this.CountPerPage.Value = "10";
            }

            AppContext.LoginId = this.LoginId.Value;
            AppContext.AuthToken = this.AuthToken.Value;
            AppContext.MailAddress = this.MailAddress.Value;
            AppContext.UserName = this.UserName.Value;
            AppContext.TargetDirectoryPath = this.TargetDirectoryPath.Value;
            AppContext.TargetFileExtensionList = this.TargetFileExtensionList.Value.Split(',').Select(x => x.Trim()).ToList();
            AppContext.CountPerPage = int.Parse(this.CountPerPage.Value);
        }

        public void StartFileWatch()
        {
            this.fileWatcher.Start();
        }

        public void StopFileWatch()
        {
            this.fileWatcher.Stop();
        }
    }
}
