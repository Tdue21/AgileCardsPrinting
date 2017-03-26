// ********************************************************************
// * Copyright © 2014 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************
// 	

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrintIssueCards.Jira.Rest;

namespace PrintIssueCards.Jira
{
    public class JiraClient
    {
        private readonly string _hostAddress;
        private readonly string _password;
        private readonly string _userName;

        public JiraClient(string hostAddress, string userName, string password)
        {
            if (hostAddress == null)
            {
                throw new ArgumentNullException("hostAddress");
            }
            _hostAddress = hostAddress;
            _userName = userName;
            _password = password;
        }

        public JiraClient(string hostAddress) : this(hostAddress, null, null)
        {
        }

        public async Task<IEnumerable<JiraFilter>> GetFiltersAsync()
        {
            var client = new SimpleRestClient(_hostAddress, _userName, _password);
            var result = await client.GetAsync("rest/api/2/filter/favourite");
            var filters = JsonConvert.DeserializeObject<IEnumerable<JiraFilter>>(result);

            return filters;
        }

        public async Task<Issue> GetIssueAsync(string key)
        {
            var client = new SimpleRestClient(_hostAddress, _userName, _password);
            var result = await client.GetAsync("rest/api/2/issue/" + key);

            var item = JsonConvert.DeserializeObject<Issue>(result);

            return item;
        }

        public async Task<IEnumerable<Issue>> GetIssuesAsync(int filterId, int maxResult)
        {
            var client = new SimpleRestClient(_hostAddress, _userName, _password);
            var result = await client.GetAsync("rest/api/2/filter/" + filterId);
            var filter = JsonConvert.DeserializeObject<JiraFilter>(result);
            return filter != null ? await PerformSearch(client, filter.SearchUrl, maxResult) : null;
        }

        public async Task<IEnumerable<Issue>> SearchAsync(string jql, int maxResult)
        {
            var client = new SimpleRestClient(_hostAddress, _userName, _password);

            return await PerformSearch(client, "rest/api/2/search?jql=" + jql, maxResult);
        }

        private async Task<IEnumerable<Issue>> PerformSearch(SimpleRestClient client, string jql, int maxResult)
        {
            var result = await client.GetAsync(jql + @"&maxResults=" + maxResult);

            var fields = await client.GetAsync("rest/api/2/field");
            var fieldList = JArray.Parse(fields)
                                  .Children()
                                  .Where(i => i["id"].ToString().StartsWith(@"customfield"))
                                  .Select(i => i.ToObject<CustomField>())
                                  .ToList();


            var resultObject = JObject.Parse(result);
            var issues = resultObject["issues"] as JArray;
            return issues.Children().Select(issue => ParseIssue(issue, fieldList)).ToList();
        }

        private Issue ParseIssue(JToken json, IEnumerable<CustomField> fieldList)
        {
            var issue = json.ToObject<Issue>();
            var fields = json["fields"] as JObject;
            foreach (var fieldItem in fieldList)
            {
                var fieldValue = fields[fieldItem.Id] as JValue;
                if (fieldValue != null)
                {
                    issue.Fields.CustomFields.Add(new CustomField {Id = fieldItem.Id, Name = fieldItem.Name, Value = fieldValue.ToString(CultureInfo.InvariantCulture)});
                }
            }

            return issue;
        }
    }
}
