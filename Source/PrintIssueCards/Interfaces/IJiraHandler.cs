// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;
using PrintIssueCards.Models;

namespace PrintIssueCards.Interfaces
{
    public interface IJiraHandler
    {
        Task<IList<FilterInformation>> GetFavoriteFiltersAsync();
        Task<IEnumerable<JiraIssue>> GetIssuesFromFilterAsync(FilterInformation selectedFilter);
        Task<IEnumerable<JiraIssue>> GetIssuesFromQueryAsync(string getKeyList);
    }
}