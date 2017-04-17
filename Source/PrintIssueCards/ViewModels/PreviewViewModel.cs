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
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.ViewModels
{
    [POCOViewModel]
    public class PreviewViewModel : ISupportParameter
    {
        private readonly ISettingsHandler _settingsHandler;

        public PreviewViewModel(ISettingsHandler settingsHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentNullException(nameof(settingsHandler));
            }
            _settingsHandler = settingsHandler;
        }

        /// <summary>Gets or sets the parameter.</summary>
        [BindableProperty(OnPropertyChangedMethodName = "OnParameterChanged")]
        public virtual object Parameter { get; set; }

        [BindableProperty(false)]
        public List<JiraIssue> Issues { get; protected set; }

        [BindableProperty(false)]
        public string ReportFile { get; protected set; }

        protected void OnParameterChanged()
        {
            var parameters = Parameter as object[];
            var issues = parameters?[0] as IEnumerable<JiraIssue>;
            if (issues != null)
            {
                Issues = new List<JiraIssue>(issues);
            }
        }
    }
}