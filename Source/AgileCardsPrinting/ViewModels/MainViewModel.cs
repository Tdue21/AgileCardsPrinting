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
using System.Threading.Tasks;
using System.Windows.Input;

using AgileCardsPrinting.Interfaces;
using AgileCardsPrinting.Models;

using DevExpress.Mvvm;

namespace AgileCardsPrinting.ViewModels
{
	/// <summary>Main view model. This is the entry point of the view model.</summary>
	public class MainViewModel : ViewModelBase
	{
		private readonly IJiraService _jiraService;
		private readonly ISettingsHandler _settingsHandler;

		/// <summary>Initializes a new instance of the <see cref="MainViewModel"/> class.</summary>
		/// <param name="jiraHandler">The service for querying Jira.</param>
		/// <param name="settingsHandler"></param>
		/// <exception cref="System.ArgumentNullException">messenger</exception>
		public MainViewModel(IJiraService jiraHandler, ISettingsHandler settingsHandler)
		{
			_jiraService = jiraHandler ?? throw new ArgumentNullException(nameof(jiraHandler));
			_settingsHandler = settingsHandler ?? throw new ArgumentNullException(nameof(settingsHandler));
		}

		/// <summary>
		/// Gets the <see cref="IMessageBoxService"/> instance.
		/// </summary>
		public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

		/// <summary>
		/// Gets the <see cref="IDialogService" /> instance for the SettingsDialog.
		/// </summary>
		public IDialogService SettingsDialog => GetService<IDialogService>("SettingsDialog");

		/// <summary>
		/// Gets the <see cref="IDialogService" /> instance for the PreviewDialog.
		/// </summary>
		public IDialogService PreviewDialog => GetService<IDialogService>("PreviewDialog");

		/// <summary>
		/// Gets or sets the index of the selected search view. 
		/// </summary>
		public int SelectedSearchIndex
		{
			get => GetProperty(() => SelectedSearchIndex);
			set => SetProperty(() => SelectedSearchIndex, value);
		}

		/// <summary>
		/// Gets or sets the list of favorite filters. 
		/// </summary>
		public ObservableCollection<FilterInformation> Filters
		{
			get => GetProperty(() => Filters);
			set => SetProperty(() => Filters, value);
		}

		/// <summary>
		/// Gets or sets the list of reports available.
		/// </summary>
		public ObservableCollection<ReportItem> Reports
		{
			get => GetProperty(() => Reports);
			set => SetProperty(() => Reports, value);
		}

		/// <summary>
		/// Gets or sets the selected filter.
		/// </summary>
		public FilterInformation SelectedFilter
		{
			get => GetProperty(() => SelectedFilter);
			set => SetProperty(() => SelectedFilter, value);
		}

		/// <summary>
		/// Gets or sets the key list.
		/// </summary>
		public string KeyList
		{
			get => GetProperty(() => KeyList);
			set => SetProperty(() => KeyList, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public SettingsModel Settings
		{
			get => GetProperty(() => Settings);
			set => SetProperty(() => Settings, value);
		}

		/// <summary>Gets or sets the JQL.</summary>
		public string Jql
		{
			get => GetProperty(() => Jql);
			set => SetProperty(() => Jql, value);
		}

		/// <summary>Gets or sets the preview issues.</summary>
		public ObservableCollection<JiraIssue> PreviewIssues
		{
			get => GetProperty(() => PreviewIssues);
			set => SetProperty(() => PreviewIssues, value);
		}


		/// <summary>Gets or sets the selected issues.</summary>
		public IList SelectedIssues
		{
			get => GetProperty(() => SelectedIssues);
			set => SetProperty(() => SelectedIssues, value);
		}


		/// <summary>Gets or sets a value indicating whether this instance is busy.</summary>
		public bool IsBusy
		{
			get => GetProperty(() => IsBusy);
			set => SetProperty(() => IsBusy, value);
		}


		/// <summary>Gets or sets the selected report. </summary>
		public ReportItem SelectedReport
		{
			get => GetProperty(() => SelectedReport);
			set => SetProperty(() => SelectedReport, value, OnSelectedReportChanged);
		}


		/// <summary>Gets or sets the sorting information.</summary>
		public SortingInformation SortingInformation
		{
			get => GetProperty(() => SortingInformation);
			set => SetProperty(() => SortingInformation, value);
		}

		public ICommand InitializationCommand =>
			new AsyncCommand(async () =>
				{
					Settings = _settingsHandler.LoadSettings();
					await RefreshFilters();
				});

		/// <summary>Sorts the issue list.</summary>
		public ICommand<SortingInformation> SortingChangedCommand => 
			new DelegateCommand<SortingInformation>(si => SortingInformation = si);

		/// <summary>Refreshes the issues list.</summary>
		/// <remarks>Implementation of RefreshIssuesListCommand.</remarks>
		public ICommand RefreshIssuesCommand => 
			new AsyncCommand(async () =>
				{
					IEnumerable<string> GetKeyList() => KeyList
														.Split(" ;,\n\r".ToCharArray(),
															StringSplitOptions.RemoveEmptyEntries)
														.OrderBy(i => i);

					IsBusy = true;
					try
					{
						var issues = SelectedSearchIndex == 0
								? await _jiraService.GetIssuesFromFilterAsync(SelectedFilter)
								: SelectedSearchIndex == 1
									? await _jiraService.GetIssuesFromKeyListAsync(GetKeyList())
									: SelectedSearchIndex == 2
										? await _jiraService.GetIssuesFromQueryAsync(Jql)
										: throw new IndexOutOfRangeException();
						PreviewIssues = new ObservableCollection<JiraIssue>(issues ?? new List<JiraIssue>());
					}
					catch (Exception e)
					{
						HandleJiraException(e);
					}

					IsBusy = false;
				});

		/// <summary>Opens the settings dialog.</summary>
		/// <remarks>Implementation of OpenSettingsCommand.</remarks>
		public ICommand OpenSettingsCommand => 
			new AsyncCommand(async () =>
				{
					var commands = new List<UICommand>
									{
										new UICommand("OK", "OK", null, true, false, MessageResult.OK),
										new UICommand("CANCEL", "Cancel", null, false, true, MessageResult.Cancel)
									};

					var result = SettingsDialog.ShowDialog(commands, "Settings", "SettingsView", Settings);
					if ((MessageResult)result.Tag == MessageResult.OK)
					{
						_settingsHandler.SaveSettings(Settings);
						Settings = _settingsHandler.LoadSettings();
						await RefreshFilters();
					}
				});

		/// <summary>
		/// Performs the search.
		/// </summary>
		public ICommand PreparePrintCommand => 
			new DelegateCommand(() =>
				{
					var list = SelectedIssues != null && SelectedIssues.Count > 0
									? new List<JiraIssue>(SelectedIssues.OfType<JiraIssue>())
									: new List<JiraIssue>(PreviewIssues);

					PreviewDialog.ShowDialog(MessageButton.OK, "Preview", "PreviewView", list, this);
				});


		/// <summary>Refreshes the list of filters.</summary>
		/// <remarks>Implementation of RefreshFilterListCommand.</remarks>
		private async Task RefreshFilters()
		{
			IsBusy = true;
			try
			{
				Settings = _settingsHandler.LoadSettings();

				Reports = new ObservableCollection<ReportItem>(_settingsHandler.GetReports());
				SelectedReport = Reports.FirstOrDefault(r => r.Name == Settings.ReportName);

				var filters = await _jiraService.GetFavoriteFiltersAsync().ConfigureAwait(true);
				Filters = filters != null && filters.Any() ? new ObservableCollection<FilterInformation>(filters) : null;
			}
			catch (Exception e)
			{
				HandleJiraException(e);
			}

			IsBusy = false;
		}

		/// <summary>
		/// </summary>
		private void OnSelectedReportChanged()
		{
			if (Settings.ReportName != SelectedReport.Name)
			{
				Settings.ReportName = SelectedReport.Name;
				_settingsHandler.SaveSettings(Settings);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="ex"></param>
		private void HandleJiraException(Exception ex) =>
			MessageBoxService.ShowMessage($"It was not possible to connect to Jira. Check your settings.\nException: {ex.Message}",
										  "Error", MessageButton.OK, MessageIcon.Error);
	}
}
