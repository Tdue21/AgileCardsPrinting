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

using System.Windows.Controls;
using AgileCards.Common;
using AgileCards.Common.Models;

namespace AgileCardsPrinting.Views
{
	using AgileCardsPrinting.Common;
    using AgileCardsPrinting.Models;
	using System.Security;
	using System.Windows;

	/// <summary>
	/// Defines the <see cref="SettingsView" />
	/// </summary>
	public partial class SettingsView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SettingsView"/> class.
		/// </summary>
		public SettingsView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// On load the <seealso cref="SecureString"/> password is converted into an insecure string and input into the PasswordTextBox. 
		/// The <seealso cref="PasswordBox"/> will store the password internally in a SecureString. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			if (DataContext is SettingsModel vm && vm.Password != null)
			{
				PasswordTextBox.Password = vm.Password.ConvertToInsecureString();
			}
		}

		/// <summary>
		/// As it is not possible to bind to the Password property of a <seealso cref="PasswordBox"/> for security reasons, the PasswordChanged event is bound instead. 
		/// When triggered the password is stored securely in the <see cref="SecureString" /> Password property on the view model.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void OnPasswordChanged(object sender, RoutedEventArgs e)
		{
			if (DataContext is SettingsModel vm)
			{
				vm.Password = PasswordTextBox.SecurePassword;
			}
		}
	}
}
