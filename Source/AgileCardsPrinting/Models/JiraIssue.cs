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

namespace AgileCardsPrinting.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class JiraIssue
    {
        /// <summary>Gets or sets the key.</summary>
        public string Key { get; set; }

        /// <summary>Gets or sets the URL.</summary>
        public string Url { get; set; }

        /// <summary>Gets or sets the summary.</summary>
        public string Summary { get; set; }

        /// <summary>Gets or sets the description.</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the type of the issue.</summary>
        public string IssueType { get; set; }

        /// <summary>Gets or sets the assignee.</summary>
        public string Assignee { get; set; }

        /// <summary>Gets or sets the reporter.</summary>
        public string Reporter { get; set; }

        /// <summary>Gets or sets the created.</summary>
        public DateTime Created { get; set; }

        /// <summary>Gets or sets the updated.</summary>
        public DateTime Updated { get; set; }

        /// <summary>Gets or sets the due date.</summary>
        public DateTime DueDate { get; set; }

        /// <summary>Gets or sets the priority.</summary>
        public string Priority { get; set; }

        /// <summary>Gets or sets the status.</summary>
        public string Status { get; set; }

        /// <summary>Gets or sets the status image.</summary>
        public byte[] StatusImage { get; set; }

        /// <summary>Gets or sets the status image URL.</summary>
        public string StatusImageUrl { get; set; }

        /// <summary>Gets or sets the estimate.</summary>
        public string Estimate { get; set; }

		/// <summary>Gets or sets the estimated time to complete.</summary>
        public int EstimateSeconds { get; set; }

	    /// <summary>Gets or sets the time spent.</summary>
	    public string Spent { get; set; }

		/// <summary>Gets or sets the time spent.</summary>
	    public int SpentSeconds { get; set; }
		
	    /// <summary>Gets or sets time spent.</summary>
	    public string Remaining { get; set; }
		
	    /// <summary>Gets or sets time spent.</summary>
	    public int RemainingSeconds { get; set; }

        /// <summary>Gets or sets the type icon URL.</summary>
        public string TypeIconUrl { get; set; }

        /// <summary>Gets or sets the type icon image.</summary>
        public byte[] TypeIconImage { get; set; }

        /// <summary>Gets or sets the severity icon URL.</summary>
        public string PriorityIconUrl { get; set; }

        /// <summary>Gets or sets the severity icon image.</summary>
        public byte[] PriorityIconImage { get; set; }

        /// <summary>Gets or sets the affected version.</summary>
        public string AffectedVersion { get; set; }

		/// <summary>Gets of sets the Id of the Fixed Version item.</summary>
		public string FixedVersionId { get; set; }

	    /// <summary>Gets or sets the fixed version.</summary>
		public string FixedVersion { get; set; }

		/// <summary></summary>
		public DateTime ReleaseDate { get; set; } 

        /// <summary>Gets or sets the custom field1.</summary>
        public string CustomField1 { get; set; }

        /// <summary>Gets or sets the custom field2.</summary>
        public string CustomField2 { get; set; }

        /// <summary>Gets or sets the custom field3.</summary>
        public string CustomField3 { get; set; }

        /// <summary>Gets or sets the custom field4.</summary>
        public string CustomField4 { get; set; }
    }
}