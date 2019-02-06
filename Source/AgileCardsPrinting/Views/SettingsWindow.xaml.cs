// ****************************************************************************
// * The MIT License(MIT)
// * Copyright © 2018 Thomas Due
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

using System.Security;
using System.Windows;
using System.Windows.Controls;
using AgileCardsPrinting.Common;
using AgileCardsPrinting.ViewModels;

namespace AgileCardsPrinting.Views
{
	public partial class SettingsWindow
	{
		public SettingsWindow()
		{
			InitializeComponent();
		}

		/// <summary>Called when the view is loaded. This is necessary in order to pass the 
		/// <seealso cref="SecureString"/> property <see cref="SettingsViewModel.SettingsData.Password"/> 
		/// from the view model to the  <see cref="PasswordBox"/> control.</summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			if (DataContext is SettingsViewModel vm && vm.SettingsData?.Password != null)
			{
				PasswordTextBox.Password = vm.SettingsData.Password.ConvertToUnsecureString();
			}
		}

		/// <summary>Called when The <seealso cref="PasswordBox.SettingsData.Password"/> property changes.
		/// Passes the <seealso cref="SecureString"/> property to the view model. </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void OnPasswordChanged(object sender, RoutedEventArgs e)
		{
			if (DataContext is SettingsViewModel vm)
			{
				vm.SettingsData.Password = PasswordTextBox.SecurePassword;
			}
		}
	}
}
