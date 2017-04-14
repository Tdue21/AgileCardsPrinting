// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm.DataAnnotations;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Models;

namespace PrintIssueCards.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [POCOViewModel]
    public class FilterSearchViewModel
    {
        private readonly IJiraHandler _jiraHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSearchViewModel"/> class.
        /// </summary>
        /// <param name="jiraHandler">The jira handler.</param>
        /// <exception cref="ArgumentNullException">jiraHandler</exception>
        public FilterSearchViewModel(IJiraHandler jiraHandler)
        {
            if (jiraHandler == null)
            {
                throw new ArgumentNullException(nameof(jiraHandler));
            }
            _jiraHandler = jiraHandler;
        }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        public virtual ObservableCollection<FilterInformation> Filters { get; set; }

        /// <summary>
        /// Gets or sets the selected filter.
        /// </summary>
        /// <value>
        /// The selected filter.
        /// </value>
        public virtual FilterInformation SelectedFilter { get; set; }

        /// <summary>
        /// Performs the refresh.
        /// </summary>
        public async void PerformRefresh()
        {
            var filters = await _jiraHandler.GetFavoriteFiltersAsync();
            Filters = new ObservableCollection<FilterInformation>(filters);
            SelectedFilter = filters.FirstOrDefault();
        }
    }
}
