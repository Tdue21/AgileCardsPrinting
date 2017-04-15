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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atlassian.Jira;
using PrintIssueCards.Common;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.Services
{
    public class JiraService : IJiraService
    {
        private readonly ISettingsHandler _settingsHandler;

        public JiraService(ISettingsHandler settingsHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentNullException(nameof(settingsHandler));
            }
            _settingsHandler = settingsHandler;
        }

        public async Task<IList<FilterInformation>> GetFavoriteFiltersAsync()
        {
            var jira = GetJiraClient();
            if (jira != null)
            {
                var filters = await jira.Filters.GetFavouritesAsync();
                var result = filters.Select(f => new FilterInformation {Id = f.Id, Name = f.Name}).ToList();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<JiraIssue>> GetIssuesFromFilterAsync(FilterInformation selectedFilter)
        {
            var jira = GetJiraClient();
            if (jira != null)
            {
                var issues = await jira.Filters.GetIssuesFromFavoriteAsync(selectedFilter.Name);
                var result =
                    issues.Select(i =>
                            new JiraIssue
                            {
                                Key = i.Key.Value,
                                IssueType = i.Type.Name,
                                Priority = i.Priority.Name,
                                Status = i.Status.Name,
                                Summary = i.Summary,
                                Reporter = i.Reporter,
                                Assignee = i.Assignee
                            }).ToList();
                return result;
            }
            return null;
        }

        public async Task<IEnumerable<JiraIssue>> GetIssuesFromQueryAsync(string query)
        {
            var jira = GetJiraClient();
            if (jira != null)
            {
                var issues = await jira.Issues.GetIssuesFromJqlAsync(query);
                var result =
                    issues.Select(i =>
                            new JiraIssue
                            {
                                Key = i.Key.Value,
                                IssueType = i.Type.Name,
                                Priority = i.Priority.Name,
                                Status = i.Status.Name,
                                Summary = i.Summary,
                                Reporter = i.Reporter,
                                Assignee = i.Assignee
                            }).ToList();
                return result;
            }
            return null;
        }

        private Jira GetJiraClient()
        {
            var data = _settingsHandler.LoadSettings();
            if (!string.IsNullOrEmpty(data.HostAddress))
            {
                return Jira.CreateRestClient(data.HostAddress, data.UserId, data.Password.ConvertToUnsecureString());
            }

            return null;
        }
    }
}