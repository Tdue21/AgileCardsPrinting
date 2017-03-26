// *********************************************************************
// * Copyright © 2015 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

namespace PrintIssueCards.Jira
{
    public class JiraFilter
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public string Jql { get; set; }
        public string Name { get; set; }
        public string SearchUrl { get; set; }
        public string Self { get; set; }
        public string ViewUrl { get; set; }
    }
}
