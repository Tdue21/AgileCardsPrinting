// ********************************************************************
// * Copyright © 2014 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************
// 	

namespace PrintIssueCards.Jira
{
    public class Issue
    {
        public Issue()
        {
            Fields = new Fields();
        }

        public string Expand { get; set; }
        public string Id { get; set; }
        public string Self { get; set; }
        public string Key { get; set; }

        public Fields Fields { get; set; }
    }     
}
