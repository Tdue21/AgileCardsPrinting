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
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using PrintIssueCards.Common;
using PrintIssueCards.Interfaces;

namespace PrintIssueCards.ViewModels
{
    [POCOViewModel]
    public class MainViewModel
    {
        public MainViewModel(FilterSearchViewModel filterSearch, BasicSearchViewModel basicSearch, AdvancedSearchViewModel advancedSearch)
        {
            if (filterSearch == null)
            {
                throw new ArgumentNullException(nameof(filterSearch));
            }
            if (basicSearch == null)
            {
                throw new ArgumentNullException(nameof(basicSearch));
            }
            if (advancedSearch == null)
            {
                throw new ArgumentNullException(nameof(advancedSearch));
            }
            FilterSearchViewModel = filterSearch;
            BasicSearchViewModel = basicSearch;
            AdvancedSearchViewModel = advancedSearch;
        }

        public virtual int SelectionIndex { get; set; }

        protected virtual IWindowService WindowService => null;

        protected virtual ICurrentWindowService CurrentWindowService => null;

        protected virtual IMessageBoxService MessageBoxService => null;

        public virtual FilterSearchViewModel FilterSearchViewModel { get; protected set; }

        public virtual BasicSearchViewModel BasicSearchViewModel { get; protected set; }

        public virtual AdvancedSearchViewModel AdvancedSearchViewModel { get; protected set; }

        public void OpenSettings(Type child) => CreateWindow(child, true);

        //  public bool CanPerformSearch(Type child) => !string.IsNullOrEmpty(Jql);

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

        //private string GetKeyList()
        //{
        //    var keys = KeyList.Split(" ;,\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).OrderBy(i => i);
        //    var keyList = keys.Aggregate(string.Empty, (c, n) => c + "," + n).Substring(1);

        //    var jql = $"Key in ({keyList})";
        //    if (!string.IsNullOrEmpty(SelectedOrderBy))
        //    {
        //        jql += " order by " + SelectedOrderBy;
        //    }

        //    return jql;
        //}


        private void CreateWindow(Type childType, bool modal = false)
        {
            var message = new CreateWindowMessage
            {
                Owner = (CurrentWindowService as CurrentWindowService)?.ActualWindow,
                ChildType = childType,
                Modal = modal
            };

            Messenger.Default.Send(message);
        }
    }
}