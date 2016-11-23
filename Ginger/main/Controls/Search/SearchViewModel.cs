using Ginger.Client.Exceptions;
using Ginger.Client.Models;
using Ginger.Client.Services;
using Ginger.Config;
using Ginger.LocalDataBase.Config;
using Ginger.LocalDataBase.Models;
using Ginger.LocalDataBase.Services;
using Ginger.LocalDataBase.Services.LocalFile;
using Ginger.LocalDataBase.Services.ServerFile;
using Ginger.Models;
using Ginger.Models.Io;
using Ginger.Services;
using Ginger.ViewModels;
using log4net;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ginger.Controls.Search
{
    internal class SearchViewModel
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private AddFileJobToDataBase addFileJobToDataBase;

        private SynchronizeDocumentFile synchronizeDocumentFile;

        private DocumentFileService documentFileService;

        private GetFileJobList getFileJobList;

        private SearchHelper searchHelper;

        private ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();

        private string savedSearchKeyword;

        private Status status = Status.Wait;

        public SearchViewModel()
        {
            var isDesignMode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
            if (isDesignMode)
            {
                return;
            }

            this.addFileJobToDataBase = AppContext.GetObject<AddFileJobToDataBase>();

            this.synchronizeDocumentFile = AppContext.GetObject<SynchronizeDocumentFile>();
            this.documentFileService = AppContext.GetObject<DocumentFileService>();

            this.getFileJobList = AppContext.GetObject<GetFileJobList>();

            this.searchHelper = AppContext.GetObject<SearchHelper>();

            this.HitList.Value = new List<VmHit>();
            this.PageList.Value = new List<Page>();

            this.status = Status.Wait;
            this.UpdateControlStatus();
        }

        public delegate void StringEventHandler(string value);

        public delegate void NotifyEventHandler();

        private enum Status
        {
            Wait,
            Search
        }

        public StringEventHandler UpdateProgress { private get; set; }

        public StringEventHandler UpdateProgressAsync { private get; set; }

        public NotifyEventHandler FinishClearDocumentFile { get; set; }

        public ReactiveProperty<List<VmHit>> HitList { get; } = new ReactiveProperty<List<VmHit>>();

        public ReactiveProperty<List<Page>> PageList { get; } = new ReactiveProperty<List<Page>>();

        public ReactiveProperty<string> SearchKeyword { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> TotalHitCountString { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<Visibility> HitListVisibility { get; } = new ReactiveProperty<Visibility>();

        public ReactiveProperty<Visibility> SearchProgressBarVisibility { get; } = new ReactiveProperty<Visibility>();

        public ReactiveProperty<string> SearchTimeString { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<bool> ButtonEnabled { get; } = new ReactiveProperty<bool>();

        public int TotalHitCount
        {
            set
            {
                this.TotalHitCountString.Value = value.ToString() + " 件 ";
            }
        }

        public double SearchTime
        {
            set
            {
                this.SearchTimeString.Value = value.ToString("F2") + " 秒 ";
            }
        }

        public string Message { get; set; }

        public async void Search()
        {
            this.SearchInit();
            this.savedSearchKeyword = this.SearchKeyword.Value;

            try
            {
                var pageNumber = 1;
                var keywordList = this.ToKeywordList(this.SearchKeyword.Value);

                var sw = Stopwatch.StartNew();
                var searchResult = new VmSearchResult(await Task.Run(() =>
                    this.documentFileService.SearchFile(AppContext.ServerUrl, AppContext.AuthToken, keywordList, new List<string>(), pageNumber, AppContext.CountPerPage)));
                sw.Stop();

                this.SearchFinalize(pageNumber, searchResult, sw.Elapsed.TotalSeconds);
            }
            catch (Exception)
            {
            }
            finally
            {
                this.status = Status.Wait;
                this.UpdateControlStatus();
            }
        }

        public async void Search(int pageNumber)
        {
            this.SearchInit();
            this.SearchKeyword.Value = this.savedSearchKeyword;

            try
            {
                var keywordList = this.ToKeywordList(this.SearchKeyword.Value);

                var sw = Stopwatch.StartNew();
                var searchResult = new VmSearchResult(await Task.Run(() =>
                    this.documentFileService.SearchFile(AppContext.ServerUrl, AppContext.AuthToken, keywordList, new List<string>(), pageNumber, AppContext.CountPerPage)));
                sw.Stop();

                this.SearchFinalize(pageNumber, searchResult, sw.Elapsed.TotalSeconds);
            }
            catch (Exception)
            {
            }
            finally
            {
                this.status = Status.Wait;
                this.UpdateControlStatus();
            }
        }

        private List<string> ToKeywordList(string value)
        {
            var keyword = value == null ? string.Empty : value;
            return keyword.Replace('　', ' ').Replace('\t', ' ').Split(' ').ToList();
        }

        private void SearchInit()
        {
            this.status = Status.Search;
            this.UpdateControlStatus();
            this.HitList.Value = new List<VmHit>();
            this.PageList.Value = new List<Page>();
            this.SearchTimeString.Value = string.Empty;
            this.TotalHitCountString.Value = string.Empty;
        }

        private void SearchFinalize(int pageNumber, VmSearchResult searchResult, double totalSeconds)
        {
            this.SearchTime = totalSeconds;
            this.HitList.Value = searchResult.HitList;
            this.PageList.Value = new Pagination(searchResult.TotalCount, AppContext.CountPerPage).GetPageList(pageNumber);
            this.TotalHitCount = searchResult.TotalCount;
            Logger.Warn(this.HitList.Value.Count);
        }

        private void ThrowExceptionIfNotNull(Exception ex)
        {
            if (ex != null)
            {
                throw ex;
            }
        }

        private void UpdateControlStatus()
        {
            if (this.status == Status.Search)
            {
                this.ButtonEnabled.Value = false;
                this.SearchProgressBarVisibility.Value = Visibility.Visible;
                this.HitListVisibility.Value = Visibility.Collapsed;
            }
            else if (this.status == Status.Wait)
            {
                this.ButtonEnabled.Value = true;
                this.SearchProgressBarVisibility.Value = Visibility.Collapsed;
                this.HitListVisibility.Value = Visibility.Visible;
            }
        }
    }
}
