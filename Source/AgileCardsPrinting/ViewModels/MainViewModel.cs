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
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using AgileCardsPrinting.Common;
using AgileCardsPrinting.Interfaces;
using AgileCardsPrinting.Models;

namespace AgileCardsPrinting.ViewModels
{
    /// <summary>Main view model. This is the entry point of the view model.</summary>
    [POCOViewModel]
    public class MainViewModel
    {
        private readonly IJiraService _jiraService;
        private readonly IMessenger _messenger;

        /// <summary>Initializes a new instance of the <see cref="MainViewModel"/> class.</summary>
        /// <param name="messenger">The messenger service instance..</param>
        /// <param name="jiraHandler">The service for querying Jira.</param>
        /// <exception cref="System.ArgumentNullException">messenger</exception>
        public MainViewModel(IMessenger messenger, IJiraService jiraHandler)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _jiraService = jiraHandler ?? throw new ArgumentNullException(nameof(jiraHandler));

            // This sucks a bit, but I can't think of another way right now. 
            _messenger.Register<bool>(this, doRefresh => { if(doRefresh) { RefreshFilterList(); } });
        }

        /// <summary>Gets the <see cref="ICurrentWindowService"/> instance.</summary>
        protected virtual ICurrentWindowService CurrentWindowService => null;

        /// <summary>Gets the <see cref="IWindowService"/> instance.</summary>
        protected virtual IWindowService WindowService => null;

        /// <summary>Gets the <see cref="IMessageBoxService"/> instance.</summary>
        protected virtual IMessageBoxService MessageBoxService => null;
        protected virtual IDialogService CustomeDialogService => null;

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

        /// <summary>Refreshes the list of filters.</summary>
        /// <remarks>Implementation of RefreshFilterListCommand.</remarks>
        public async void RefreshFilterList()
        {
            IsBusy = true;
            try
            {
                var filters = await _jiraService.GetFavoriteFiltersAsync();
                Filters = filters != null && filters.Any() ? new ObservableCollection<FilterInformation>(filters) : null;
            }
            catch(Exception e)
            {
                MessageBoxService.ShowMessage($"It was not possible to connect to Jira. Check your settings.\nException: {e.Message}", 
                    "Error", MessageButton.OK, MessageIcon.Error);
            }
            IsBusy = false;
        }

        /// <summary>Refreshes the issues list.</summary>
        /// <remarks>Implementation of RefreshIssuesListCommand.</remarks>
        public async void RefreshIssuesList()
        {
            IsBusy = true;
            try
            {
                var issues = 
                    SelectedSearchIndex == 0 ? await _jiraService.GetIssuesFromFilterAsync(SelectedFilter) :
                    SelectedSearchIndex == 1 ? await _jiraService.GetIssuesFromKeyListAsync(GetKeyList()) :
                    SelectedSearchIndex == 2 ? await _jiraService.GetIssuesFromQueryAsync(Jql) :
                    throw new IndexOutOfRangeException();

                PreviewIssues = new ObservableCollection<JiraIssue>(issues ?? new List<JiraIssue>());
            }
            catch(Exception e)
            {
                MessageBoxService.ShowMessage(
                    $"It was not possible to connect to Jira. Check your settings.\nException: {e.Message}",
                    "Error", MessageButton.OK, MessageIcon.Error);
            }
            IsBusy = false;
        }

        /// <summary>Opens the settings dialog.</summary>
        /// <param name="child">The child.</param>
        /// <remarks>Implementation of OpenSettingsCommand.</remarks>
        public void OpenSettings(Type child)
        {
            
            var result = CustomeDialogService.ShowDialog(MessageButton.OKCancel, "Settings", child.Name);
            if (result == MessageResult.OK)
            {
                RefreshFilterList();
            }
            //var result = CustomeDialogService.ShowDialog(MessageButton.OKCancel, "Settings", child.Name, null);
            //WindowService.Show(child.Name, null);
            //CreateWindow(child, true);
        }

        /// <summary>
        /// Performs the search.
        /// </summary>
        /// <param name="child">The child.</param>
        public void PreparePrint(Type child)
        {
            var list = SelectedIssues != null && SelectedIssues.Count > 0
                ? new List<JiraIssue>(SelectedIssues.OfType<JiraIssue>())
                : new List<JiraIssue>(PreviewIssues);

            CreateWindow(child, true, list);
        }

        /// <summary>Creates a child window by using the <seealso cref="IMessenger"/> 
        /// implementation. 
        /// I would prefer to find a way of using a more direct way so I can get a 
        /// return value from the child window.</summary>
        /// <param name="childType">Type of the child.</param>
        /// <param name="modal"><c>true</c> if the child window should be modal.</param>
        /// <param name="parameters">Parameters passed to the child window.</param>
        private void CreateWindow(Type childType, bool modal = false, object parameters = null)
        {
            var obj = ViewLocator.Default.ResolveView(childType.Name);

            var message = new CreateWindowMessage
            {
                Owner = (CurrentWindowService as CurrentWindowService)?.ActualWindow,
                ChildType = childType,
                Modal = modal,
                Parameters = parameters
            };

            _messenger.Send(message);
        }     

        private IEnumerable<string> GetKeyList() => KeyList.Split(" ;,\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).OrderBy(i => i);
    }
}