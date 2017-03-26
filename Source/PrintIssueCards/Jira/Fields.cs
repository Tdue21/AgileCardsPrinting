// ********************************************************************
// * Copyright © 2014 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************
// 	

using System;
using System.Collections.Generic;

namespace PrintIssueCards.Jira
{
    public class Fields
    {
        public Fields()
        {
            TimeTracking = new TimeTracking();
            IssueType = new IssueType();
            Reporter = new Person();
            Assignee = new Person();
            Priority = new Priority();
            Status = new Status();
            CustomFields = new List<CustomField>();
        }

        public string Summary { get; set; }
        public string Description { get; set; }
        public string Environment { get; set; }
        public TimeTracking TimeTracking { get; set; }
        public IssueType IssueType { get; set; }
        public Person Reporter { get; set; }
        public Person Assignee { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public int? TimeEstimate { get; set; }
        public List<CustomField> CustomFields { get; private set; }
    }
}
