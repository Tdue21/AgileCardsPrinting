// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Atlassian.Jira;
using PrintIssueCards.Properties;

namespace PrintIssueCards.Models
{
    public class JiraIssue
    {
        private static readonly Dictionary<string, byte[]> ImageList = new Dictionary<string, byte[]>();

        public string Key { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string IssueType { get; set; }
        public string Assignee { get; set; }
        public string Reporter { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime DueDate { get; set; }
        public string OrderNo { get; set; }
        public string Severity { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Estimate { get; set; }
        public byte[] TypeIconUrl { get; set; }
        public byte[] SeverityIconUrl { get; set; }
        public string AffectedVersion { get; set; }
        public string FixedVersion { get; set; }


        public static JiraIssue CreateFromIssue(Issue issue)
        {
            if (issue == null)
            {
                throw new ArgumentNullException("issue");
            }

            // ReSharper disable AssignNullToNotNullAttribute
            var orderNoField = issue.CustomFields.FirstOrDefault(i => string.Equals(i.Name, @"ordre nummer", StringComparison.InvariantCultureIgnoreCase));
            var priorityField = issue.CustomFields.FirstOrDefault(i => string.Equals(i.Name, "Internal priority", StringComparison.InvariantCultureIgnoreCase));
            // ReSharper restore AssignNullToNotNullAttribute

            var orderNo = orderNoField != null ? orderNoField.Values.FirstOrDefault() : string.Empty;
            var priority = priorityField != null ? priorityField.Values.FirstOrDefault() : string.Empty;

            //var estimate = FormatEstimate(issue.Fields.TimeEstimate.HasValue && issue.Fields.TimeEstimate.Value > 0
            //                                  ? issue.Fields.TimeEstimate.Value
            //                                  : issue.Fields.TimeTracking.OriginalEstimateSeconds);

            var item = new JiraIssue
                       {
                           Key = issue.Key.Value,
                           Summary = issue.Summary,
                           Description = issue.Description,
                           IssueType = issue.Type.Name,
                           Assignee = issue.Assignee ?? string.Empty,
                           Reporter = issue.Reporter ?? string.Empty,
                           Created = issue.Created.GetValueOrDefault(),
                           Updated = issue.Updated.GetValueOrDefault(),
                           DueDate = issue.DueDate.GetValueOrDefault(),
                           Status = issue.Status != null ? issue.Status.Name : string.Empty,
                           Severity = issue.Priority != null ? issue.Priority.Name : string.Empty,
                           //Estimate = estimate,
                           TypeIconUrl = issue.Type != null ? LoadImage(issue.Type.IconUrl) : null,
                           SeverityIconUrl = issue.Priority != null ? LoadImage(issue.Priority.IconUrl) : null,
                           OrderNo = orderNo,
                           Priority = priority,
                       };
            return item;
        }

        private static string FormatEstimate(int seconds)
        {
            var time = new TimeSpan(0, 0, seconds);
            var days = Math.Floor(time.TotalHours / 7.5);
            var hours = Math.Floor(time.TotalHours % 7.5);
            var minutes = time.TotalMinutes % 60;
            var weeks = (int) days / 5;

            var result = string.Empty;
            if (weeks > 0)
            {
                result += string.Format("{0}w ", weeks);
            }
            if (days > 0)
            {
                result += string.Format("{0}d ", days);
            }
            if (hours > 0)
            {
                result += string.Format("{0}h ", hours);
            }
            if (minutes > 0)
            {
                result += string.Format("{0}m", minutes);
            }

            return result;
        }

        private static byte[] LoadImage([NotNull] string uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (!ImageList.ContainsKey(uri))
            {
                var client = new WebClient();
                var bytes = client.DownloadData(uri);

                //var result = (Bitmap) (new ImageConverter()).ConvertFrom(bytes);
                ImageList.Add(uri, bytes);
            }

            return ImageList[uri];
        }
    }
}
