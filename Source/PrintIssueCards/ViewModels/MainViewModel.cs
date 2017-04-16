//  ****************************************************************************
//  * The MIT License(MIT)
//  * Copyright © 2017 Thomas Due
//  * 
//  * Permission is hereby granted, free of charge, to any person obtaining a 
//  * copy of this software and associated documentation files (the “Software”), 
//  * to deal in the Software without restriction, including without limitation 
//  * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//  * and/or sell copies of the Software, and to permit persons to whom the  
//  * Software is furnished to do so, subject to the following conditions:
//  * 
//  * The above copyright notice and this permission notice shall be included in  
//  * all copies or substantial portions of the Software.
//  * 
//  * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS  
//  * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL  
//  * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING  
//  * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//  * IN THE SOFTWARE.
//  ****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using PrintIssueCards.Common;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.ViewModels
{
    /// <summary>Main view model. This is the entry point of the view model.</summary>
    [POCOViewModel]
    public class MainViewModel
    {
        private readonly IMessenger _messenger;
        private readonly IJiraService _jiraService;

        /// <summary>Initializes a new instance of the <see cref="MainViewModel"/> class.</summary>
        /// <param name="messenger">The messenger service instance..</param>
        /// <param name="jiraHandler">The service for querying Jira.</param>
        /// <exception cref="System.ArgumentNullException">messenger</exception>
        public MainViewModel(IMessenger messenger, IJiraService jiraHandler)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }
            if (jiraHandler == null)
            {
                throw new ArgumentNullException(nameof(jiraHandler));
            }

            _messenger = messenger;
            _jiraService = jiraHandler;
        }

        /// <summary>Gets the current window service.</summary>
        protected virtual ICurrentWindowService CurrentWindowService => null;

        /// <summary>Gets or sets the index of the selected search view. </summary>
        public virtual int SelectedSearchIndex { get; set; }

        /// <summary>Gets or sets the list of favorite filters. </summary>
        public virtual ObservableCollection<FilterInformation> Filters { get; set; }

        /// <summary>Gets or sets the selected filter.</summary>
        public virtual FilterInformation SelectedFilter { get; set; }

        /// <summary>Gets or sets the key list.</summary>
        public virtual string KeyList { get; set; }

        /// <summary>Gets or sets the selected order by.</summary>
        public virtual string SelectedOrderBy { get; set; }

        /// <summary>Gets or sets the JQL.</summary>
        public virtual string Jql { get; set; }

        /// <summary>Gets or sets the preview issues.</summary>
        public virtual ObservableCollection<JiraIssue> PreviewIssues { get; set; }

        /// <summary>Gets or sets the selected issues.</summary>
        public virtual IList SelectedIssues { get; set; }

        /// <summary>Sets the selected items.</summary>
        /// <param name="args">The list of selected items.</param>
        public void SetSelectedItems(IList args)
        {
            SelectedIssues = args;
        }

        /// <summary>Refreshes the list of filters.</summary>
        public async void PerformRefresh()
        {
            var filters = await _jiraService.GetFavoriteFiltersAsync();
            if (filters != null && filters.Any())
            {
                Filters = new ObservableCollection<FilterInformation>(filters);
                SelectedFilter = filters.FirstOrDefault();
            }
            else
            {
                Filters = null;
                SelectedFilter = null;
            }
        }

        public async void PreviewSearch(string tab)
        {
            var jql = tab == "Basic" ? GetQueryFromKeyList() : Jql;
            var issues = await _jiraService.GetIssuesFromQueryAsync(jql);
            SetPreviewIssues(issues);
        }

        /// <summary>Fires when <see cref="SelectedFilter"/> changes.</summary>
        protected async void OnSelectedFilterChanged(FilterInformation oldInformation)
        {
            var issues = await _jiraService.GetIssuesFromFilterAsync(SelectedFilter);
            SetPreviewIssues(issues);
        }

        /// <summary>
        /// Sets the preview issues.
        /// </summary>
        /// <param name="issues">The issues.</param>
        public void SetPreviewIssues(IEnumerable<JiraIssue> issues)
        {
            PreviewIssues = new ObservableCollection<JiraIssue>(issues ?? new List<JiraIssue>());
        }

        /// <summary>
        /// Opens the settings.
        /// </summary>
        /// <param name="child">The child.</param>
        public void OpenSettings(Type child) => CreateWindow(child, true);

        //  public bool CanPerformSearch(Type child) => !string.IsNullOrEmpty(Jql);

        /// <summary>
        /// Performs the search.
        /// </summary>
        /// <param name="child">The child.</param>
        public void PerformSearch(Type child)
        {
            //IEnumerable<JiraIssue> result = null;

            //switch (SelectionIndex)
            //{
            //    case 0:
            //        result = await _jiraHandler.GetIssuesFromFilterAsync(SelectedFilter);
            //        break;
            //    case 1:
            //        result = await _jiraHandler.GetIssuesFromQueryAsync(GetKeyList());
            //        break;
            //    case 2:
            //        result = await _jiraHandler.GetIssuesFromQueryAsync(Jql);
            //        break;
            //    default:
            //        throw new IndexOutOfRangeException();
            //}

            //CreateWindow(child, true);


            //var handler = new SettingsHandler();
            //var data = handler.LoadSettings();
            //var client = Jira.CreateRestClient(data.HostAddress, data.UserId, data.Password);
            //var searchString = !string.IsNullOrEmpty(KeyList) ? GetKeyList() : null;
            //var result = await (!string.IsNullOrEmpty(searchString)
            //                        ? client.SearchAsync(searchString, data.MaxResult)
            //                        : string.Equals(SelectedFilter.Jql, Jql, StringComparison.InvariantCultureIgnoreCase)
            //                              ? client.GetIssuesAsync(SelectedFilter.Id, data.MaxResult)
            //                              : client.SearchAsync(Jql, data.MaxResult)
            //                   );

            //var issues = result.Select(JiraIssue.CreateFromIssue);
            //var window = new PreviewWindow(issues);
            //window.ShowDialog();
        }

        private string GetQueryFromKeyList()
        {
            var keys = KeyList.Split(" ;,\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).OrderBy(i => i);
            var keyList = keys.Aggregate(string.Empty, (c, n) => c + "," + n).Substring(1);

            var jql = $"Key in ({keyList})";
            if (!string.IsNullOrEmpty(SelectedOrderBy))
            {
                jql += " order by " + SelectedOrderBy;
            }

            return jql;
        }


        private void CreateWindow(Type childType, bool modal = false)
        {
            var message = new CreateWindowMessage
            {
                Owner = (CurrentWindowService as CurrentWindowService)?.ActualWindow,
                ChildType = childType,
                Modal = modal
            };

            _messenger.Send(message);
        }
    }
}