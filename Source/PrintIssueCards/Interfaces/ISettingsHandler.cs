// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using PrintIssueCards.Models;

namespace PrintIssueCards.Interfaces
{
    public interface ISettingsHandler
    {
        SettingsModel LoadSettings();
        void SaveSettings(SettingsModel settings);
    }
}