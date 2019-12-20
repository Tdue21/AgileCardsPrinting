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


using AgileCards.Common.Interfaces;
using AgileCards.Common.Services;
using AgileCards.JiraIntegration;
using AgileCardsPrinting.Common;
using AgileCardsPrinting.ViewModels;

using SimpleInjector;

namespace AgileCardsPrinting
{
	/// <summary>
	/// </summary>
	public class DependencyInjector
	{
		private readonly Container _container;

		/// <summary>
		/// </summary>
		public DependencyInjector()
		{
			_container = new Container();

			_container.RegisterType<IFileSystemService, FileSystemService>(Lifestyle.Singleton)
			          .RegisterType<ISettingsHandler, JsonFileSettingsHandler>(Lifestyle.Singleton)
			          .RegisterType<IIssueTrackerService, JiraService>(Lifestyle.Singleton)
			          .RegisterType<MainViewModel>()
			          .RegisterType<PreviewViewModel>()
			          .RegisterType<SettingsViewModel>()
				;
		}

		/// <summary>
		/// </summary>
		public MainViewModel MainViewModel => _container.GetInstance<MainViewModel>();

		/// <summary>
		/// </summary>
		public PreviewViewModel PreviewViewModel => _container.GetInstance<PreviewViewModel>();

		/// <summary>
		/// </summary>
		public SettingsViewModel SettingsViewModel => _container.GetInstance<SettingsViewModel>();
	}
}
