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
    public class JiraIssue
    {
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
    }
}