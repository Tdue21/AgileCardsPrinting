// ****************************************************************************
// * The MIT License(MIT)
// * Copyright © 2019 Thomas Due
// *
// * Permission is hereby granted, free of charge, to any person obtaining a
// * copy of this software and associated documentation files (the “Software”),
// * to deal in the Software without restriction, including without limitation
// * the rights to use, copy, modify, merge, publish, distribute, sublicense,
// * and/or sell copies of the Software, and to permit persons to whom the
// * Software is furnished to do so, subject to the following conditions:
// *
// * The above copyright notice and this permission notice shall be included in
// * all copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS
// * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// * IN THE SOFTWARE.
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgileCards.Common.Interfaces;
using AgileCards.Common.Models;

namespace AgileCards.YouTrackIntegration
{
	public class YouTrackService : IIssueTrackerService
	{
		/// <inheritdoc />
		public Task<IList<FilterInformation>> GetFavoriteFiltersAsync()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task<IEnumerable<IssueCard>> GetIssuesFromFilterAsync(FilterInformation selectedFilter)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task<IEnumerable<IssueCard>> GetIssuesFromQueryAsync(string query)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public Task<IEnumerable<IssueCard>> GetIssuesFromKeyListAsync(IEnumerable<string> keyList)
		{
			throw new NotImplementedException();
		}
	}
}