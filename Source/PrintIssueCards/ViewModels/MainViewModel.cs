// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;
using PrintIssueCards.Views;

namespace PrintIssueCards.ViewModels
{
    [POCOViewModel]
    public class MainViewModel
    {
        private readonly ISettingsHandler _settingsHandler;
        private readonly IJiraHandler _jiraHandler;

        public MainViewModel(ISettingsHandler settingsHandler, IJiraHandler jiraHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentNullException(nameof(settingsHandler));
            }
            if (jiraHandler == null)
            {
                throw new ArgumentNullException(nameof(jiraHandler));
            }

            _settingsHandler = settingsHandler;
            _jiraHandler = jiraHandler;
        }

        public virtual ObservableCollection<FilterInformation> Filters { get; set; }

        public virtual FilterInformation SelectedFilter { get; set; }

        public virtual string Jql { get; set; }

        public virtual string KeyList { get; set; }

        public virtual object OrderByItems => new[] { "Key", "Priority", "IssueType", "Assignee", "Reporter", "Status" };

        public virtual string SelectedOrderBy { get; set; }

        public virtual int SelectionIndex { get; set; }

        public virtual IWindowService WindowService { get; set; }
        public virtual IMessageBoxService MessageBoxService { get; set; }

        public void OpenSettings(Type child)
        {
            WindowService.Show("SettingsWindow", null);
            
            //var window = new SettingsWindow();
            //if (window.ShowDialog() == true)
            //{
            //    PerformRefresh();
            //}
        }

        public async void PerformRefresh()
        {
            var filters = await _jiraHandler.GetFavoriteFiltersAsync();
            Filters = new ObservableCollection<FilterInformation>(filters);
            SelectedFilter = filters.FirstOrDefault();
        }

        public bool CanPerformSearch() => !string.IsNullOrEmpty(Jql);

        public async void PerformSearch()
        {
            IEnumerable<JiraIssue> result = null;

            switch (SelectionIndex)
            {
                case 0:
                    result = await _jiraHandler.GetIssuesFromFilterAsync(SelectedFilter);
                    break;
                case 1:
                    result = await _jiraHandler.GetIssuesFromQueryAsync(GetKeyList());
                    break;
                case 2:
                    result = await _jiraHandler.GetIssuesFromQueryAsync(Jql);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            

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

        private string GetKeyList()
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

            Messenger.Default.Send(message);
        }
    }
}
