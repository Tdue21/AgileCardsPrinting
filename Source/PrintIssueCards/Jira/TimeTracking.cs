// ********************************************************************
// * Copyright © 2014 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************
// 	

namespace PrintIssueCards.Jira
{
    public class TimeTracking
    {
        public string OriginalEstimate { get; set; }
        public string RemainingEstimate { get; set; }
        public int OriginalEstimateSeconds { get; set; }
        public int RemainingEstimateSeconds { get; set; }
    }
}
