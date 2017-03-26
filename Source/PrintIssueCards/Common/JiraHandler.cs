// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.Common
{
    public class JiraHandler : IJiraHandler
    {
        public Task<IList<FilterInformation>> GetFavoriteFiltersAsync()
        {
            throw new System.NotImplementedException();




            //var data = _settingsHandler.LoadSettings();
            //if (!string.IsNullOrEmpty(data.HostAddress))
            //{
            //    var client = Jira.CreateRestClient(data.HostAddress, data.UserId, data.Password);
            //    var result = await client.Filters.GetFavouritesAsync();
            //}

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