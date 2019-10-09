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
using System.Net;
using System.Threading.Tasks;
using AgileCards.Common;
using AgileCards.Common.Interfaces;
using AgileCards.Common.Models;
using Atlassian.Jira;
// ReSharper disable TooManyChainedReferences

namespace AgileCards.JiraIntegration
{
	/// <summary>
	/// </summary>
	public class JiraService : IIssueTrackerService
	{
		/// <summary>
		/// </summary>
		private static readonly Dictionary<string, byte[]> ImageList = new Dictionary<string, byte[]>();

		/// <summary>
		/// </summary>
		private readonly ISettingsHandler _settingsHandler;

		/// <summary>
		/// </summary>
		/// <param name="settingsHandler"></param>
		public JiraService(ISettingsHandler settingsHandler)
		{
			_settingsHandler = settingsHandler ?? throw new ArgumentNullException(nameof(settingsHandler));
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public async Task<IList<FilterInformation>> GetFavoriteFiltersAsync()
		{
			var jira = GetJiraClient();
			var filters = await jira.Filters
			                        .GetFavouritesAsync()
			                        .ConfigureAwait(true);

			var result = filters.Select(f => new FilterInformation {Id = f.Id, Name = f.Name}).ToList();
			return result;
		}

		/// <summary>
		/// </summary>
		/// <param name="selectedFilter"></param>
		/// <returns></returns>
		public async Task<IEnumerable<IssueCard>> GetIssuesFromFilterAsync(FilterInformation selectedFilter)
		{
			if (selectedFilter != null)
			{
				var jira = GetJiraClient();
				var issues = await jira.Filters.GetIssuesFromFavoriteAsync(selectedFilter.Name).ConfigureAwait(true);
				return await TransformResultAsync(jira, issues).ConfigureAwait(true);
			}
			return null;
		}

		/// <summary>
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public async Task<IEnumerable<IssueCard>> GetIssuesFromQueryAsync(string query)
		{
			var jira = GetJiraClient();
			var issues = await jira.Issues.GetIssuesFromJqlAsync(query).ConfigureAwait(true);
			return await TransformResultAsync(jira, issues).ConfigureAwait(true);
		}

		/// <summary>
		/// </summary>
		/// <param name="keyList"></param>
		/// <returns></returns>
		public async Task<IEnumerable<IssueCard>> GetIssuesFromKeyListAsync(IEnumerable<string> keyList)
		{
			var jira = GetJiraClient();
			var issues = await jira.Issues.GetIssuesAsync(keyList).ConfigureAwait(true);
			return await TransformResultAsync(jira, issues.Values).ConfigureAwait(true);
		}

		/// <summary>
		/// </summary>
		/// <param name="jira"></param>
		/// <param name="issues"></param>
		/// <returns></returns>
		private async Task<IEnumerable<IssueCard>> TransformResultAsync(Jira jira, IEnumerable<Issue> issues)
		{
			var data = _settingsHandler.LoadSettings();
			var list = new List<IssueCard>();
			foreach (var issue in issues)
			{
				var item = await GetIssueCardAsync(jira, issue, data);
				list.Add(item);
			}
			return list;
		}

		/// <summary>
		/// </summary>
		/// <param name="jira"></param>
		/// <param name="issue"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		// ReSharper disable once TooManyDeclarations
		private async Task<IssueCard> GetIssueCardAsync(Jira jira, Issue issue, SettingsModel data)
		{
			var timeTrack = await issue.GetTimeTrackingDataAsync().ConfigureAwait(true);
			var assignee  = issue.Assignee != null ? await jira.Users.GetUserAsync(issue.Assignee).ConfigureAwait(true) : null;
			var reporter  = issue.Reporter != null ? await jira.Users.GetUserAsync(issue.Reporter).ConfigureAwait(true) : null;
			var version   = issue.FixVersions.OrderBy(i => i.Id).FirstOrDefault();

			// ReSharper disable once ComplexConditionExpression
			var item = new IssueCard
			           {
				           Key               = issue.Key.Value,
				           Url               = $"{issue.Jira.Url}browse/{issue.Key.Value}",
				           Summary           = issue.Summary,
				           Description       = issue.Description,
				           IssueType         = issue.Type.Name,
				           Estimate          = timeTrack.OriginalEstimate,
				           EstimateSeconds   = timeTrack.OriginalEstimateInSeconds,
				           Spent             = timeTrack.TimeSpent,
				           SpentSeconds      = timeTrack.TimeSpentInSeconds,
				           Remaining         = timeTrack.RemainingEstimate,
				           RemainingSeconds  = timeTrack.RemainingEstimateInSeconds,
				           Assignee          = assignee?.DisplayName ?? string.Empty,
				           Reporter          = reporter?.DisplayName ?? string.Empty,
				           Created           = issue.Created.GetValueOrDefault(),
				           Updated           = issue.Updated.GetValueOrDefault(),
				           DueDate           = issue.DueDate.GetValueOrDefault(),
				           Status            = issue.Status != null   ? issue.Status.Name : string.Empty,
				           StatusImage       = issue.Status != null   ? LoadImage(issue.Status.IconUrl) : null,
				           StatusImageUrl    = issue.Status != null   ? issue.Status.IconUrl : string.Empty,
				           Priority          = issue.Priority != null ? issue.Priority.Name : string.Empty,
				           TypeIconUrl       = issue.Type != null     ? issue.Type.IconUrl : null,
				           TypeIconImage     = issue.Type != null     ? LoadImage(issue.Type.IconUrl) : null,
				           PriorityIconUrl   = issue.Priority != null ? issue.Priority.IconUrl : null,
				           PriorityIconImage = issue.Priority != null ? LoadImage(issue.Priority.IconUrl) : null,
				           AffectedVersion   = issue.AffectsVersions.OrderBy(i => i.Name).FirstOrDefault()?.Name ?? string.Empty,
				           FixedVersion      = version?.Name ?? string.Empty,
				           FixedVersionId    = version?.Id ?? string.Empty,
				           ReleaseDate       = version?.ReleasedDate.GetValueOrDefault() ?? DateTime.MaxValue,
				           CustomField1      = GetCustomField(issue.CustomFields, data.CustomField1),
				           CustomField2      = GetCustomField(issue.CustomFields, data.CustomField2),
				           CustomField3      = GetCustomField(issue.CustomFields, data.CustomField3),
				           CustomField4      = GetCustomField(issue.CustomFields, data.CustomField4)
			};

			return item;
		}

		/// <summary>
		/// </summary>
		/// <param name="issueCustomFields"></param>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		private string GetCustomField(CustomFieldValueCollection issueCustomFields, string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return string.Empty;
			}

			var field = issueCustomFields.FirstOrDefault(i => string.Equals(i.Name, fieldName, StringComparison.InvariantCultureIgnoreCase));
			return field != null ? field.Values.FirstOrDefault() : string.Empty;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		private Jira GetJiraClient()
		{
			var data = _settingsHandler.LoadSettings();
			if (string.IsNullOrEmpty(data.HostAddress))
			{
				throw new NullReferenceException("Host Address is not set.");
			}
			var settings = new JiraRestClientSettings { EnableRequestTrace = true};
			var client = Jira.CreateRestClient(data.HostAddress, data.UserId, data.Password.ConvertToInsecureString(), settings);
			client.MaxIssuesPerRequest = data.MaxResult;
			return client;
		}

		/// <summary>
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		private byte[] LoadImage(string uri)
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
					ImageList.Add(uri, bytes);
					return bytes;
				}
			}

			return ImageList[uri];
		}
	}
}
