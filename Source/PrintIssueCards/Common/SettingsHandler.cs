// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System.IO;
using System.Xml.Linq;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.Common
{
    public class SettingsHandler : ISettingsHandler
    {
        private readonly string _settingsFile;

        public SettingsHandler()
        {
            _settingsFile = Path.GetFullPath(".\\Settings.xml");
        }

        public SettingsModel LoadSettings()
        {
            var settings = new SettingsModel();
            if (File.Exists(_settingsFile))
            {
                var data = XDocument.Load(_settingsFile, LoadOptions.PreserveWhitespace);
                if (data.Root != null)
                {
                    settings.MaxResult = data.Root.GetDescendantValue("MaxResult", 50);
                    settings.HostAddress = data.Root.GetDescendantValue("HostAddress", string.Empty);
                    settings.UserId = data.Root.GetDescendantValue("UserId", string.Empty);
                    var password = data.Root.GetDescendantValue("Password", string.Empty);

                    settings.Password = string.IsNullOrEmpty(password) ? password : EncryptionHelper.Decrypt(password);
                }
            }

            return settings;
        }

        public void SaveSettings(SettingsModel settings)
        {
            var data = new XDocument(
                new XElement("Settings",
                    new XElement("HostAddress", settings.HostAddress),
                    new XElement("UserId", settings.UserId),
                    new XElement("Password", EncryptionHelper.Encrypt(settings.Password ?? string.Empty)),
                    new XElement("MaxResult", settings.MaxResult)));

            data.Save(_settingsFile, SaveOptions.None);
        }
    }
}
