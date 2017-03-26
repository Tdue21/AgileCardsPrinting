// ********************************************************************
// * Copyright © 2014 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************
// 	

namespace PrintIssueCards.Jira
{
    public class Person
    {
        public string Self { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
    }
}
