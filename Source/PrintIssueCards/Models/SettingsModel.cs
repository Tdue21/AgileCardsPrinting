// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

namespace PrintIssueCards.Models
{
    public class SettingsModel
    {
        public SettingsModel()
        {
            HostAddress = string.Empty;
            UserId = string.Empty;
            Password = string.Empty;
            MaxResult = 50;
        }

        public string HostAddress { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int MaxResult { get; set; }
    }
}
