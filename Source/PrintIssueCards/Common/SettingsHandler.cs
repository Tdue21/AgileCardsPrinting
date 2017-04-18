//  ****************************************************************************
//  * The MIT License(MIT)
//  * Copyright © 2017 Thomas Due
//  * 
//  * Permission is hereby granted, free of charge, to any person obtaining a 
//  * copy of this software and associated documentation files (the “Software”), 
//  * to deal in the Software without restriction, including without limitation 
//  * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//  * and/or sell copies of the Software, and to permit persons to whom the  
//  * Software is furnished to do so, subject to the following conditions:
//  * 
//  * The above copyright notice and this permission notice shall be included in  
//  * all copies or substantial portions of the Software.
//  * 
//  * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS  
//  * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL  
//  * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING  
//  * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//  * IN THE SOFTWARE.
//  ****************************************************************************

using System;
using System.Xml.Linq;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.Common
{
    public class SettingsHandler : ISettingsHandler
    {
        private readonly IFileSystemService _fileSystem;
        private readonly string _settingsFile;

        public SettingsHandler(IFileSystemService fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            _fileSystem = fileSystem;
            _settingsFile = _fileSystem.GetFullPath(".\\Settings.xml");
        }

        public SettingsModel LoadSettings()
        {
            var settings = new SettingsModel();
            if (_fileSystem.FileExists(_settingsFile))
            {
                using (var stream = _fileSystem.OpenReadStream(_settingsFile))
                {
                    var data = XDocument.Load(stream, LoadOptions.PreserveWhitespace);
                    if (data.Root != null)
                    {
                        settings.HostAddress = data.Root.GetDescendantValue("HostAddress", string.Empty);
                        settings.MaxResult = data.Root.GetDescendantValue("MaxResult", 50);
                        settings.UserId = data.Root.GetDescendantValue("UserId", string.Empty);
                        settings.ReportName = data.Root.GetDescendantValue("ReportName", string.Empty);
                        var password = data.Root.GetDescendantValue("Password", string.Empty);
                        settings.Password = (!string.IsNullOrEmpty(password)
                            ? EncryptionHelper.Decrypt(password)
                            : password).ConvertToSecureString();
                    }
                }
            }

            return settings;
        }

        public void SaveSettings(SettingsModel settings)
        {
            var data = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("Settings",
                    new XElement("HostAddress", settings.HostAddress),
                    new XElement("UserId",      settings.UserId),
                    new XElement("Password",    EncryptionHelper.Encrypt(settings.Password?.ConvertToUnsecureString() ?? string.Empty)),
                    new XElement("MaxResult",   settings.MaxResult),
                    new XElement("ReportName",   settings.ReportName)
                    ));

            using (var stream = _fileSystem.OpenWriteStream(_settingsFile))
            {
                data.Save(stream, SaveOptions.None);
            }
        }
    }
}