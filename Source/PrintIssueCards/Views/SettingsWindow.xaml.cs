// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Windows;
using PrintIssueCards.ViewModels;

namespace PrintIssueCards.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void PasswordTextBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as SettingsViewModel;
            if (dc != null)
            {
                dc.Password = PasswordTextBox.Password;
            }
        }

        private void SettingsView_Initialized(object sender, EventArgs e)
        {
            var dc = DataContext as SettingsViewModel;
            if (dc != null)
            {
                PasswordTextBox.Password = dc.Password;
            }
        }
    }
}
