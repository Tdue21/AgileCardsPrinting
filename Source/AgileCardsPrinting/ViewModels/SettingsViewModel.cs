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
	        _settingsHandler = settingsHandler ?? throw new ArgumentNullException(nameof(settingsHandler));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            LoadSettings();
        }

        protected virtual ICurrentWindowService CurrentWindowService => null;

        protected virtual IFolderBrowserDialogService FolderBrowserDialogService => null;

        /// <summary>
        /// Gets or sets the host address.
        /// </summary>
        public virtual string HostAddress { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public virtual SecureString Password { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the maximum result.
        /// </summary>
        public virtual int MaxResult { get; set; }

        /// <summary>
        /// Gets or sets the report file.
        /// </summary>
        public virtual string ReportFile { get; set; }

		/// <summary>
		/// Gets or sets the path to the reports folder.
		/// </summary>
		public virtual string ReportPath { get; set; }

        public virtual string CustomField1 { get; set; }
        
        public virtual string CustomField2 { get; set; }
        
        public virtual string CustomField3 { get; set; }
        
        public virtual string CustomField4 { get; set; }

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

		/// <summary>
		/// 
		/// </summary>
        public void SelectFolder()
        {
	        FolderBrowserDialogService.StartPath = _fileSystem.GetFullPath("Reports");

			if (FolderBrowserDialogService.ShowDialog())
            {
                ReportPath = FolderBrowserDialogService.ResultPath;
            }
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
		    ReportPath = data.ReportPath;
		    MaxResult = data.MaxResult;
		    CustomField1 = data.CustomField1;
		    CustomField2 = data.CustomField2;
		    CustomField3 = data.CustomField3;
		    CustomField4 = data.CustomField4;
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
				ReportPath = ReportPath,
                CustomField1 = CustomField1,
                CustomField2 = CustomField2,
                CustomField3 = CustomField3,
                CustomField4 = CustomField4
            };

            _settingsHandler.SaveSettings(data);
        }
    }
}