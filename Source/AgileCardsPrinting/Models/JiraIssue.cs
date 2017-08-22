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

namespace PrintIssueCards.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class JiraIssue
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the issue.
        /// </summary>
        /// <value>
        /// The type of the issue.
        /// </value>
        public string IssueType { get; set; }

        /// <summary>
        /// Gets or sets the assignee.
        /// </summary>
        /// <value>
        /// The assignee.
        /// </value>
        public string Assignee { get; set; }

        /// <summary>
        /// Gets or sets the reporter.
        /// </summary>
        /// <value>
        /// The reporter.
        /// </value>
        public string Reporter { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        /// <value>
        /// The updated.
        /// </value>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        /// <value>
        /// The due date.
        /// </value>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public string Priority { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status image.
        /// </summary>
        /// <value>
        /// The status image.
        /// </value>
        public byte[] StatusImage { get; set; }

        /// <summary>
        /// Gets or sets the status image URL.
        /// </summary>
        /// <value>
        /// The status image URL.
        /// </value>
        public string StatusImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the estimate.
        /// </summary>
        /// <value>
        /// The estimate.
        /// </value>
        public string Estimate { get; set; }

        /// <summary>
        /// Gets or sets the type icon URL.
        /// </summary>
        /// <value>
        /// The type icon URL.
        /// </value>
        public string TypeIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the type icon image.
        /// </summary>
        /// <value>
        /// The type icon image.
        /// </value>
        public byte[] TypeIconImage { get; set; }

        /// <summary>
        /// Gets or sets the severity icon URL.
        /// </summary>
        /// <value>
        /// The severity icon URL.
        /// </value>
        public string SeverityIconUrl { get; set; }

        /// <summary>
        /// Gets or sets the severity icon image.
        /// </summary>
        /// <value>
        /// The severity icon image.
        /// </value>
        public byte[] SeverityIconImage { get; set; }

        /// <summary>
        /// Gets or sets the affected version.
        /// </summary>
        /// <value>
        /// The affected version.
        /// </value>
        public string AffectedVersion { get; set; }

        /// <summary>
        /// Gets or sets the fixed version.
        /// </summary>
        /// <value>
        /// The fixed version.
        /// </value>
        public string FixedVersion { get; set; }

        /// <summary>
        /// Gets or sets the custom field1.
        /// </summary>
        /// <value>
        /// The custom field1.
        /// </value>
        public string CustomField1 { get; set; }

        /// <summary>
        /// Gets or sets the custom field2.
        /// </summary>
        /// <value>
        /// The custom field2.
        /// </value>
        public string CustomField2 { get; set; }

        /// <summary>
        /// Gets or sets the custom field3.
        /// </summary>
        /// <value>
        /// The custom field3.
        /// </value>
        public string CustomField3 { get; set; }

        /// <summary>
        /// Gets or sets the custom field4.
        /// </summary>
        /// <value>
        /// The custom field4.
        /// </value>
        public string CustomField4 { get; set; }
    }
}