// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm.DataAnnotations;
using PrintIssueCards.Common;
using PrintIssueCards.Models;
using PrintIssueCards.Properties;

namespace PrintIssueCards.ViewModels
{
    [POCOViewModel]
    public class SettingsViewModel 
    {
        public SettingsViewModel()
        {
            PerformLoadSettings();
        }

        public virtual string HostAddress { get; set; }
        public virtual string Password { get; set; }
        public virtual string UserId { get; set; }
        public virtual int MaxResult { get; set; }

        public void PerformCancelSettings(object view)
        {
            //var window = view as Window;
            //if (window != null)
            //{
            //    window.Close();
            //}
        }

        public void PerformCloseSettings(object view)
        {
            PerformSaveSettings();
            PerformCancelSettings(view);
        }

        public void PerformLoadSettings()
        {
            var handler = new SettingsHandler();
            var data = handler.LoadSettings();
            HostAddress = data.HostAddress;
            UserId = data.UserId;
            Password = data.Password;
            MaxResult = data.MaxResult;
        }

        public void PerformSaveSettings()
        {
            var data = new SettingsModel
                       {
                           HostAddress = HostAddress,
                           UserId = UserId,
                           Password = Password,
                           MaxResult = MaxResult
                       };

            var handler = new SettingsHandler();
            handler.SaveSettings(data);
        }
    }
}
