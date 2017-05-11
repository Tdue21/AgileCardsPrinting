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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Atlassian.Jira;
using PrintIssueCards.Common;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;
using PrintIssueCards.Properties.Annotations;

namespace PrintIssueCards.Services
{
    public class JiraService : IJiraService
    {
        private static readonly Dictionary<string, Image> ImageList = new Dictionary<string, Image>();

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
            var filters = await jira.Filters.GetFavouritesAsync();
            var result = filters.Select(f => new FilterInformation {Id = f.Id, Name = f.Name}).ToList();
            return result;
        }

        public async Task<IEnumerable<JiraIssue>> GetIssuesFromFilterAsync(FilterInformation selectedFilter)
        {
            if (selectedFilter != null)
            {
                var jira = GetJiraClient();
                var issues = await jira.Filters.GetIssuesFromFavoriteAsync(selectedFilter.Name);
                return await TransformResult(issues);
            }
            return null;
        }

        public async Task<IEnumerable<JiraIssue>> GetIssuesFromQueryAsync(string query)
        {
            var jira = GetJiraClient();
            var issues = await jira.Issues.GetIssuesFromJqlAsync(query);
            return await TransformResult(issues);
        }

        public async Task<IEnumerable<JiraIssue>> GetIssuesFromKeyListAsync(IEnumerable<string> keyList)
        {
            var jira = GetJiraClient();
            var issues = await jira.Issues.GetIssuesAsync(keyList);
            return await TransformResult(issues.Values);
        }

        private Jira GetJiraClient()
        {
            var data = _settingsHandler.LoadSettings();
            if (string.IsNullOrEmpty(data.HostAddress))
            {
                throw new NullReferenceException("Host Address is not set.");
            }
            return Jira.CreateRestClient(data.HostAddress, data.UserId, data.Password.ConvertToUnsecureString());
        }

        private async Task<IEnumerable<JiraIssue>> TransformResult(IEnumerable<Issue> issues)
        {

            var result = new List<JiraIssue>();
            foreach (var issue in issues)
            {
                var item = await CreateFromIssue(issue);
                result.Add(item);
            }

            return result;
        }

        private async Task<JiraIssue> CreateFromIssue(Issue issue)
        {
            if (issue == null)
            {
                throw new ArgumentNullException(nameof(issue));
            }

            var data = _settingsHandler.LoadSettings();
            var customField1 = GetCustomField(issue.CustomFields, data.CustomField1);
            var customField2 = GetCustomField(issue.CustomFields, data.CustomField2);
            var customField3 = GetCustomField(issue.CustomFields, data.CustomField3);
            var customField4 = GetCustomField(issue.CustomFields, data.CustomField4);
            var timeTrack = await issue.GetTimeTrackingDataAsync();

            var item = new JiraIssue
            {
                Key = issue.Key.Value,
                Summary = issue.Summary,
                Description = issue.Description,
                IssueType = issue.Type.Name,
                Estimate = timeTrack.RemainingEstimate,
                Assignee = issue.Assignee != null ? await GetUserFullName(issue.Jira, issue.Assignee) : string.Empty,
                Reporter = issue.Reporter != null ? await GetUserFullName(issue.Jira, issue.Reporter) : string.Empty,
                Created = issue.Created.GetValueOrDefault(),
                Updated = issue.Updated.GetValueOrDefault(),
                DueDate = issue.DueDate.GetValueOrDefault(),
                Status = issue.Status != null ? issue.Status.Name : string.Empty,
                Priority = issue.Priority != null ? issue.Priority.Name : string.Empty,
                TypeIconUrl = issue.Type != null ? issue.Type.IconUrl : null,
                TypeIconImage = issue.Type != null ? LoadImage(issue.Type.IconUrl) : null,
                SeverityIconUrl = issue.Priority != null ? issue.Priority.IconUrl : null,
                SeverityIconImage = issue.Priority != null ? LoadImage(issue.Priority.IconUrl) : null,
                AffectedVersion = issue.AffectsVersions.OrderBy(i => i.Name).FirstOrDefault()?.Name ?? string.Empty,
                FixedVersion = issue.FixVersions.OrderBy(i => i.Name).FirstOrDefault()?.Name ?? string.Empty,
                CustomField1 = customField1,
                CustomField2 = customField2,
                CustomField3 = customField3,
                CustomField4 = customField4,
            };
            return item;
        }

        private string GetCustomField(CustomFieldValueCollection issueCustomFields, string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            var field = issueCustomFields.FirstOrDefault(i => string.Equals(i.Name, fieldName, StringComparison.InvariantCultureIgnoreCase));
            return field != null ? field.Values.FirstOrDefault() : string.Empty;
        }

        private async Task<string> GetUserFullName(Jira jira, string userName)
        {
            var user = await jira.Users.GetUserAsync(userName, CancellationToken.None);
            return user.DisplayName;

        }
        private Image LoadImage([NotNull] string uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (!ImageList.ContainsKey(uri))
            {
                using (var client = new WebClient())
                {
                    var bytes = client.DownloadData(uri);
                    using (var stream = new MemoryStream(bytes))
                    {
                        var image = Image.FromStream(stream);
                        ImageList.Add(uri, image);
                    }
                }
            }

            return ImageList[uri];
        }

    }
}