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
using System.Windows;
using Microsoft.Reporting.WinForms;
using AgileCardsPrinting.ViewModels;
using DevExpress.Mvvm;

namespace AgileCardsPrinting.Views
{
	/// <summary>
	/// 
	/// </summary>
    public partial class PreviewView
    {
		/// <summary>
		/// 
		/// </summary>
        public PreviewView()
        {
            InitializeComponent();
        }

        /// <summary>Called when the view is loaded. Necessary in order to bind the Winforms component.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            // This is necessary, as the ReportViewer component does not support MVVM. 
            // This does not however break MVVM as it is purely related to the UI, and 
            // the ViewModel does not know the View. 
	        if(DataContext is PreviewViewModel vm)
            {
                try
                {
                    var reportDataSource = new ReportDataSource("Issues") { Value = vm.Issues };
	                IssuesReportViewer.ReportError += (o, args) => 
	                                                  {
		                                                  args.Handled = true;
		                                                  throw args.Exception;
	                                                  };
                    IssuesReportViewer.LocalReport.EnableExternalImages = true;
                    IssuesReportViewer.LocalReport.ReportPath = vm.ReportFile;
                    IssuesReportViewer.LocalReport.DataSources.Add(reportDataSource);
                    IssuesReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                    IssuesReportViewer.RefreshReport();
                }
                catch(Exception ex)
                {
					MessageBox.Show($"An error occurred during report processing.\nException: {ex.Message}.", 
					                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}