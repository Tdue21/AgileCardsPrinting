// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

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
