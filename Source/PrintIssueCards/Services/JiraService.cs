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
            List<FilterInformation> result = null;
            var data = _settingsHandler.LoadSettings();
            if (!string.IsNullOrEmpty(data.HostAddress))
            {
                var jira = Jira.CreateRestClient(data.HostAddress, data.UserId, data.Password.ConvertToUnsecureString());
                var filters = await jira.Filters.GetFavouritesAsync();

                result = filters.Select(f => new FilterInformation {Id = f.Id, Name = f.Name}).ToList();
            }
            return result;
        }

        public Task<IEnumerable<JiraIssue>> GetIssuesFromFilterAsync(FilterInformation selectedFilter)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<JiraIssue>> GetIssuesFromQueryAsync(string getKeyList)
        {
            throw new System.NotImplementedException();
        }
    }
}