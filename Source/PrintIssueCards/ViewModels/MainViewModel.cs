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
using System.Windows.Documents;
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

        /// <summary>Gets the message box service.</summary>
        protected virtual IMessageBoxService MessageBoxService => null;

        /// <summary>Gets or sets the index of the selected search view. </summary>
        public virtual int SelectedSearchIndex { get; set; }

        /// <summary>Gets or sets the list of favorite filters. </summary>
        public virtual ObservableCollection<FilterInformation> Filters { get; set; }

        /// <summary>Gets or sets the selected filter.</summary>
        public virtual FilterInformation SelectedFilter { get; set; }

        /// <summary>Gets or sets the key list.</summary>
        public virtual string KeyList { get; set; }

        /// <summary>Gets or sets the JQL.</summary>
        public virtual string Jql { get; set; }

        /// <summary>Gets or sets the preview issues.</summary>
        public virtual ObservableCollection<JiraIssue> PreviewIssues { get; set; }

        /// <summary>Gets or sets the selected issues.</summary>
        public virtual IList SelectedIssues { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is busy.</summary>
        public virtual bool IsBusy { get; set; }

        /// <summary>Gets or sets the sorting information.</summary>
        public virtual SortingInformation SortingInformation { get; set; }

        /// <summary>Refreshes the list of filters.</summary>
        public async void RefreshFilterList()
        {
            IsBusy = true;
            try
            {
                var filters = await _jiraService.GetFavoriteFiltersAsync();
                Filters = filters != null && filters.Any() ? new ObservableCollection<FilterInformation>(filters) : null;
            }
            catch (Exception e)
            {
                MessageBoxService.ShowMessage(
                    $"It was not possible to connect to Jira. Check your settings.\nException: {e.Message}",
                    "Error",
                    MessageButton.OK,
                    MessageIcon.Error);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Refreshes the issues list.
        /// </summary>
        public async void RefreshIssuesList()
        {
            IsBusy = true;
            try
            {
                IEnumerable<JiraIssue> issues;

                switch (SelectedSearchIndex)
                {
                    case 0:
                        issues = await _jiraService.GetIssuesFromFilterAsync(SelectedFilter);
                        break;
                    case 1:
                        issues = await _jiraService.GetIssuesFromKeyListAsync(GetKeyList());
                        break;
                    case 2:
                        issues = await _jiraService.GetIssuesFromQueryAsync(Jql);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }

                PreviewIssues = new ObservableCollection<JiraIssue>(issues ?? new List<JiraIssue>());
            }
            catch (Exception e)
            {
                MessageBoxService.ShowMessage(
                    $"It was not possible to connect to Jira. Check your settings.\nException: {e.Message}",
                    "Error",
                    MessageButton.OK,
                    MessageIcon.Error);
            }
            IsBusy = false;
        }

        /// <summary>Opens the settings dialog.</summary>
        /// <param name="child">The child.</param>
        public void OpenSettings(Type child) => CreateWindow(child, true);

        /// <summary>Sortings the changed.</summary>
        /// <param name="sortingInformation">The sorting information.</param>
        public void SortingChanged(SortingInformation sortingInformation) => SortingInformation = sortingInformation;

        /// <summary>
        /// Performs the search.
        /// </summary>
        /// <param name="child">The child.</param>
        public void PreparePrint(Type child)
        {
            var list = SelectedIssues != null && SelectedIssues.Count > 0
                ? new List<JiraIssue>(SelectedIssues.OfType<JiraIssue>())
                : new List<JiraIssue>(PreviewIssues);

            CreateWindow(child, true, new object[] {list, SortingInformation});
        }

        private IEnumerable<string> GetKeyList() => KeyList.Split(" ;,\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).OrderBy(i => i);


        private void CreateWindow(Type childType, bool modal = false, object[] parameters = null)
        {
            var message = new CreateWindowMessage
            {
                Owner = (CurrentWindowService as CurrentWindowService)?.ActualWindow,
                ChildType = childType,
                Modal = modal,
                Parameters = parameters
            };

            _messenger.Send(message);
        }
    }
}