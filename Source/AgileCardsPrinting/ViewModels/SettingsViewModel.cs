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
using System.Security;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using AgileCardsPrinting.Interfaces;
using AgileCardsPrinting.Models;

namespace AgileCardsPrinting.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [POCOViewModel]
    public class SettingsViewModel
    {
        private readonly ISettingsHandler _settingsHandler;
        private readonly IFileSystemService _fileSystem;
        private readonly IMessenger _messenger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="settingsHandler">The settings handler.</param>
        /// <param name="fileSystem"></param>
        /// <param name="messenger"></param>
        /// <exception cref="System.ArgumentNullException">settingsHandler</exception>
        public SettingsViewModel(ISettingsHandler settingsHandler, IFileSystemService fileSystem, IMessenger messenger)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentNullException(nameof(settingsHandler));
            }
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }

            _settingsHandler = settingsHandler;
            _fileSystem = fileSystem;
            _messenger = messenger;

            LoadSettings();
        }

        protected virtual ICurrentWindowService CurrentWindowService => null;

        protected virtual IOpenFileDialogService OpenFileDialogService => null;

        /// <summary>
        /// Gets or sets the host address.
        /// </summary>
        /// <value>
        /// The host address.
        /// </value>
        public virtual string HostAddress { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public virtual SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the maximum result.
        /// </summary>
        /// <value>
        /// The maximum result.
        /// </value>
        public virtual int MaxResult { get; set; }

        /// <summary>
        /// Gets or sets the report file.
        /// </summary>
        /// <value>
        /// The report file.
        /// </value>
        public virtual string ReportFile { get; set; }

        public virtual  string CustomField1 { get; set; }
        
        public virtual  string CustomField2 { get; set; }
        
        public virtual  string CustomField3 { get; set; }
        
        public virtual  string CustomField4 { get; set; }

        /// <summary>
        /// Closes the settings.
        /// </summary>
        public void CloseSettings(bool saveBeforeClosing = false)
        {
            if (saveBeforeClosing)
            {
                SaveSettings();
            }
            
            _messenger.Send(saveBeforeClosing);
            CurrentWindowService.Close();
        }


        public void OpenFile()
        {
            if (OpenFileDialogService.ShowDialog(_fileSystem.GetFullPath("Reports")))
            {
                ReportFile = _fileSystem.GetFileName(OpenFileDialogService.GetFullFileName());
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            var data = new SettingsModel
            {
                HostAddress = HostAddress,
                UserId = UserId,
                Password = Password,
                MaxResult = MaxResult,
                ReportName = ReportFile,
                CustomField1 = CustomField1,
                CustomField2 = CustomField2,
                CustomField3 = CustomField3,
                CustomField4 = CustomField4,
            };

            _settingsHandler.SaveSettings(data);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            var data = _settingsHandler.LoadSettings();
            HostAddress = data.HostAddress;
            UserId = data.UserId;
            Password = data.Password;
            ReportFile = data.ReportName;
            MaxResult = data.MaxResult;
            CustomField1 = data.CustomField1;
            CustomField2 = data.CustomField2;
            CustomField3 = data.CustomField3;
            CustomField4 = data.CustomField4;
        }
    }
}