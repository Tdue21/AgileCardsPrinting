// *********************************************************************
// * Copyright © 2015 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System.Collections.Generic;

namespace PrintIssueCards.Jira
{
    public class JiraSearch
    {
        public JiraSearch()
        {
            Issues = new List<Issue>();
        }

        public string Expand { get; set; }
        public List<Issue> Issues { get; set; }
        public int MaxResults { get; set; }
        public int StartAt { get; set; }
        public int Total { get; set; }
    }
}
