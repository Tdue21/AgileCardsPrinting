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
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [POCOViewModel]
    public class SettingsViewModel
    {
        private readonly ISettingsHandler _settingsHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="settingsHandler">The settings handler.</param>
        /// <exception cref="System.ArgumentNullException">settingsHandler</exception>
        public SettingsViewModel(ISettingsHandler settingsHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentNullException(nameof(settingsHandler));
            }
            _settingsHandler = settingsHandler;

            LoadSettings();
        }

        protected virtual ICurrentWindowService CurrentWindowService => null;

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
        /// Closes the settings.
        /// </summary>
        public void CloseSettings(bool saveBeforeClosing = false)
        {
            if (saveBeforeClosing)
            {
                SaveSettings();
            }
            CurrentWindowService.Close();
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
                MaxResult = MaxResult
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
            MaxResult = data.MaxResult;
        }
    }
}