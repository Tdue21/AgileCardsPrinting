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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using PrintIssueCards.Models;

namespace PrintIssueCards.Views
{
    public partial class PreviewWindow : Window
    {
        public PreviewWindow()
        {
            InitializeComponent();
        }

        public PreviewWindow(IEnumerable<JiraIssue> issues) : this()
        {
            if (issues == null)
            {
                throw new ArgumentNullException(nameof(issues));
            }

            PrepareReport(issues);
        }

        private void PrepareReport(IEnumerable<JiraIssue> issues)
        {
            var reportDataSource = new ReportDataSource
            {
                Name = "Issues",
                Value = new BindingSource
                {
                    DataSource = issues
                }
            };

            IssuesReportViewer.LocalReport.EnableExternalImages = true;
            IssuesReportViewer.LocalReport.DataSources.Add(reportDataSource);
            IssuesReportViewer.LocalReport.ReportPath = @"Resources\\IssueCards.rdlc";
            IssuesReportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            IssuesReportViewer.RefreshReport();
            IssuesReportViewer.ZoomMode = ZoomMode.PageWidth;
        }
    }
}