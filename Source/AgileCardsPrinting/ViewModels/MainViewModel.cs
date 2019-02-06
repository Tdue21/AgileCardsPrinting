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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using AgileCardsPrinting.Interfaces;
using AgileCardsPrinting.Models;

namespace AgileCardsPrinting.ViewModels
{
    /// <summary>Main view model. This is the entry point of the view model.</summary>
    [POCOViewModel]
    public class MainViewModel
    {
        private readonly IJiraService _jiraService;
	    private readonly ISettingsHandler _settingsHandler;
	    private readonly SettingsViewModel _settingsVm;
	    private SettingsModel _settingsData;

	    /// <summary>Initializes a new instance of the <see cref="MainViewModel"/> class.</summary>
	    /// <param name="jiraHandler">The service for querying Jira.</param>
	    /// <param name="settingsHandler"></param>
	    /// <param name="settingsVm"></param>
	    /// <exception cref="System.ArgumentNullException">messenger</exception>
	    public MainViewModel(IJiraService jiraHandler, ISettingsHandler settingsHandler, SettingsViewModel settingsVm)
        {
            _jiraService = jiraHandler ?? throw new ArgumentNullException(nameof(jiraHandler));
	        _settingsHandler = settingsHandler ?? throw new ArgumentNullException(nameof(settingsHandler));
	        _settingsVm = settingsVm ?? throw new ArgumentNullException(nameof(settingsVm));
        }

        /// <summary>Gets the <see cref="IMessageBoxService"/> instance.</summary>
        protected virtual IMessageBoxService MessageBoxService => null;

	    [ServiceProperty(Key = "SettingsDialog")]
	    public virtual IDialogService SettingsDialog => null;

		[ServiceProperty(Key = "PreviewDialog")]
		public virtual IDialogService PreviewDialog => null;

		/// <summary>Gets or sets the index of the selected search view. </summary>
		public virtual int SelectedSearchIndex { get; set; }

        /// <summary>Gets or sets the list of favorite filters. </summary>
        public virtual ObservableCollection<FilterInformation> Filters { get; set; }

	    /// <summary>Gets or sets the list of reports available.</summary>
	    public virtual ObservableCollection<ReportItem> Reports { get; set; }
        
	    /// <summary>Gets or sets the selected filter.</summary>
		public virtual FilterInformation SelectedFilter { get; set; }

		/// <summary>Gets or sets the key list.</summary>
		public virtual string KeyList { get; set; }

        /// <summary>Gets or sets the JQL.</summary>
        public virtual string Jql { get; set; }

        /// <summary>Gets or sets the preview issues.</summary>
        public virtual ObservableCollection<JiraIssue> PreviewIssues { get; set; }

        /// <summary>Gets or sets the selected issues.</summary>
        public virtual IList SelectedIssues { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is busy.</summary>
        public virtual bool IsBusy { get; set; }

	    /// <summary>Gets or sets the selected report. </summary>
	    public virtual ReportItem SelectedReport { get; set; }

	    /// <summary>Gets or sets the sorting information.</summary>
	    public virtual SortingInformation SortingInformation { get; set; }

		/// <summary>Sortings the changed.</summary>
		/// <param name="sortingInformation">The sorting information.</param>
		public void SortingChanged(SortingInformation sortingInformation) => SortingInformation = sortingInformation;

	    public void Initialization()
	    {
		    _settingsData = _settingsHandler.LoadSettings();
		    if (string.IsNullOrEmpty(_settingsData.HostAddress))
		    {
			    var result = MessageBoxService.ShowMessage(
				    "The application does not appear to be configured correctly.\nDo you wish to open the Settings dialog?",
				    "Warning",
				    MessageButton.YesNo,
				    MessageIcon.Question);
			    if (result == MessageResult.Yes)
			    {	
					OpenSettings(null);
			    }
		    }
	    }

	    /// <summary>Refreshes the list of filters.</summary>
	    /// <remarks>Implementation of RefreshFilterListCommand.</remarks>
	    public async void RefreshFilters(bool refreshFilters = true)
	    {
		    if (refreshFilters)
		    {
				IsBusy = true;
				try
				{
					_settingsData = _settingsHandler.LoadSettings();

					Reports = new ObservableCollection<ReportItem>(_settingsHandler.GetReports());
					SelectedReport = Reports.FirstOrDefault(r => r.Name == _settingsData.ReportName);

					var filters = await _jiraService.GetFavoriteFiltersAsync().ConfigureAwait(true);
					Filters = filters != null && filters.Any() ? new ObservableCollection<FilterInformation>(filters) : null;
				}
				catch (Exception e)
				{
					HandleJiraException(e);
				}
				IsBusy = false;
			}
	    }

		/// <summary>Refreshes the issues list.</summary>
		/// <remarks>Implementation of RefreshIssuesListCommand.</remarks>
		public async void RefreshIssues()
        {
	        IEnumerable<string> GetKeyList() => KeyList.Split(" ;,\n\r".ToCharArray(), 
	                                                          StringSplitOptions.RemoveEmptyEntries)
	                                                   .OrderBy(i => i);
			IsBusy = true;
            try
            {
                var issues = 
                    SelectedSearchIndex == 0 ? await _jiraService.GetIssuesFromFilterAsync(SelectedFilter) :
                    SelectedSearchIndex == 1 ? await _jiraService.GetIssuesFromKeyListAsync(GetKeyList()) :
                    SelectedSearchIndex == 2 ? await _jiraService.GetIssuesFromQueryAsync(Jql) :
                    throw new IndexOutOfRangeException();

                PreviewIssues = new ObservableCollection<JiraIssue>(issues ?? new List<JiraIssue>());
            }
            catch(Exception e)
            {
				HandleJiraException(e);
            }
            IsBusy = false;
        }

        /// <summary>Opens the settings dialog.</summary>
        /// <param name="child">The child.</param>
        /// <remarks>Implementation of OpenSettingsCommand.</remarks>
        public void OpenSettings(Type child)
        {
	        var commands = new List<UICommand>
	                             {
		                             new UICommand("OK", "OK", SaveSettingsCommand, true, false),
		                             new UICommand("CANCEL", "Cancel", null, false, true)
	                             };
	        _settingsVm.SettingsData = _settingsHandler.LoadSettings();
			var result = SettingsDialog.ShowDialog(commands, "Settings", "SettingsWindow", _settingsVm, null, this);
            if ((string)result.Id == "OK")
            {
				_settingsHandler.SaveSettings(_settingsVm.SettingsData);
                RefreshFilters();
            }
        }

        public ICommand SaveSettingsCommand => new DelegateCommand(() => {});

        /// <summary>
        /// Performs the search.
        /// </summary>
        /// <param name="child">The child.</param>
        public void PreparePrint(Type child)
        {
            var list = SelectedIssues != null && SelectedIssues.Count > 0
                ? new List<JiraIssue>(SelectedIssues.OfType<JiraIssue>())
                : new List<JiraIssue>(PreviewIssues);

	        PreviewDialog.ShowDialog(MessageButton.OK, "Preview", child.Name, list, this);
        }

		/// <summary>
		/// 
		/// </summary>
	    protected void OnSelectedReportChanged()
	    {
		    var data = _settingsHandler.LoadSettings();
		    if (data.ReportName != SelectedReport.Name)
		    {
			    data.ReportName = SelectedReport.Name;
			    _settingsHandler.SaveSettings(data);
		    }
	    }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
	    private void HandleJiraException(Exception ex)
	    {
		    MessageBoxService.ShowMessage($"It was not possible to connect to Jira. Check your settings.\nException: {ex.Message}",
			    "Error", MessageButton.OK, MessageIcon.Error);

	    }
    }
}