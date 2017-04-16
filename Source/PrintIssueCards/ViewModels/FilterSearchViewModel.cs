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
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.ViewModels
{
    /// <summary>
    /// </summary>
    [POCOViewModel]
    public class FilterSearchViewModel  : ISupportParentViewModel
    {
        private readonly IJiraService _jiraHandler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterSearchViewModel" /> class.
        /// </summary>
        /// <param name="jiraHandler">The jira handler.</param>
        /// <exception cref="ArgumentNullException">jiraHandler</exception>
        public FilterSearchViewModel(IJiraService jiraHandler)
        {
            if (jiraHandler == null)
            {
                throw new ArgumentNullException(nameof(jiraHandler));
            }
            _jiraHandler = jiraHandler;
        }
    }
}