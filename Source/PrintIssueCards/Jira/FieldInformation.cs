// ********************************************************************
// * Copyright © 2014 Scanvaegt Nordic A/S
// *
// * This file is the property of Scanvaegt Nordic A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************
// 	

namespace PrintIssueCards.Jira
{
    public class FieldInformation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Custom { get; set; }
        public bool Orderable { get; set; }
        public bool Navigable { get; set; }
        public bool Searchable { get; set; }
    }
}
