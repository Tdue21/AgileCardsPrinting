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
using System.Windows.Input;
using AgileCards.Common.Interfaces;
using AgileCards.Common.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace AgileCardsPrinting.ViewModels
{
	/// <summary>ViewModel for the issue card preview view.</summary>
	/// <seealso cref="DevExpress.Mvvm.ISupportParameter" />
	[POCOViewModel]
	public class PreviewViewModel : ViewModelBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewViewModel"/> class.
        /// </summary>
        /// <param name="fileSystemService"></param>
        /// <param name="settingsHandler">The settings handler.</param>
        /// <exception cref="System.ArgumentNullException">settingsHandler</exception>
        public PreviewViewModel(IFileSystemService fileSystemService, ISettingsService settingsHandler)
		{
            if (fileSystemService == null)
            {
                throw new ArgumentNullException(nameof(fileSystemService));
            }

            if (settingsHandler == null)
            {
                throw new ArgumentNullException(nameof(settingsHandler));
            }

            var data = settingsHandler.LoadSettings();
			ReportFile =  fileSystemService.GetFullPath($"Reports\\{data.ReportName}.rdlc");
		}

		/// <summary>Gets the current window service.</summary>
		public ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

		///// <summary>Used to pass the list of issues to the view model.</summary>
		//[BindableProperty(OnPropertyChangedMethodName = "OnParameterChanged")]
		//public virtual object Parameter { get; set; }

		/// <summary>Gets or sets the issue list. </summary>
		public List<IssueCard> Issues { get; protected set; }

		/// <summary>Gets or sets the report file. </summary>
		public string ReportFile { get; protected set; }

		/// <summary>Closes the view. </summary>
		public ICommand CloseViewCommand => new DelegateCommand( () => CurrentWindowService?.Close());

		/// <summary>
		/// </summary>
		/// <param name="parameter"></param>
		protected override void OnParameterChanged(object parameter)
        {
            base.OnParameterChanged(parameter);

            if (parameter is IEnumerable<IssueCard> issues)
			{
				Issues = new List<IssueCard>(issues);
			}
		}
	}
}
