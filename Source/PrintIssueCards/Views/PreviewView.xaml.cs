﻿//  ****************************************************************************
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

using System.Windows;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using PrintIssueCards.ViewModels;

namespace PrintIssueCards.Views
{
    public partial class PreviewView
    {
        public PreviewView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [view loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            // This is necessary, as the ReportViewer component does not support MVVM. 
            // This does not however break MVVM as it is purely related to the UI, and 
            // the ViewModel does not know the View. 
            var vm = DataContext as PreviewViewModel;
            if (vm != null)
            {
                var reportDataSource = new ReportDataSource("Issues")
                {
                    Value = new BindingSource {DataSource = vm.Issues}
                };

                IssuesReportViewer.LocalReport.EnableExternalImages = true;
                IssuesReportViewer.LocalReport.DataSources.Add(reportDataSource);
                IssuesReportViewer.LocalReport.ReportPath = vm.ReportFile;
                IssuesReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                IssuesReportViewer.RefreshReport();
                IssuesReportViewer.ZoomMode = ZoomMode.FullPage;
            }
        }
    }
}