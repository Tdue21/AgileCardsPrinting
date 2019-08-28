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

using System.Collections.Generic;
using System.Windows;

using AgileCardsPrinting.Views;

using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;

namespace AgileCardsPrinting.Services
{
	public class CustomDialogService : ViewServiceBase, IDialogService
	{
		public static readonly DependencyProperty DialogStartupLocationProperty = 
			DependencyProperty.Register("DialogStartupLocation", 
				typeof(WindowStartupLocation), 
				typeof(CustomDialogWindow), 
				new PropertyMetadata(null));

		public static readonly DependencyProperty OwnerWindowProperty = 
			DependencyProperty.Register("OwnerWindow", 
				typeof(Window), 
				typeof(CustomDialogWindow), 
				new PropertyMetadata(null));

		public static readonly DependencyProperty HeightProperty = 
			DependencyProperty.Register("Height", 
				typeof(double), 
				typeof(CustomDialogWindow), 
				new PropertyMetadata(null));

		public static readonly DependencyProperty WidthProperty = 
			DependencyProperty.Register("Width", 
				typeof(double), 
				typeof(CustomDialogWindow), 
				new PropertyMetadata(null));

		public static readonly DependencyProperty ResizableProperty = 
			DependencyProperty.Register("Resizable", 
				typeof(bool), 
				typeof(CustomDialogWindow), 
				new PropertyMetadata(null));

		public WindowStartupLocation DialogStartupLocation
		{
			get => (WindowStartupLocation) GetValue(DialogStartupLocationProperty);
			set => SetValue(DialogStartupLocationProperty, value);
		}

		public Window OwnerWindow
		{
			get => (Window) GetValue(OwnerWindowProperty);
			set => SetValue(OwnerWindowProperty, value);
		}

		public double Height
		{
			get => (double) GetValue(HeightProperty);
			set => SetValue(HeightProperty, value);
		}

		public double Width
		{
			get => (double) GetValue(WidthProperty);
			set => SetValue(WidthProperty, value);
		}

		public bool Resizable
		{
			get => (bool)GetValue(ResizableProperty);
			set => SetValue(ResizableProperty, value);
		}

		public UICommand ShowDialog(IEnumerable<UICommand> dialogCommands, string title, string documentType, object viewModel, object parameter, object parentViewModel)
		{
			var content = CreateAndInitializeView(documentType, viewModel, parameter, parentViewModel);
			var window = new CustomDialogWindow
			             {
				             Height = Height + 40,
				             Width = Width,
							 ResizeMode = Resizable ? ResizeMode.CanResize : ResizeMode.NoResize,
				             Owner = OwnerWindow,
				             Title = title,
				             WindowStartupLocation = DialogStartupLocation,
				             CommandsSource = dialogCommands,
				             Content = content
			             };
			window.ShowDialog();

			return window.Result;
		}
	}
}
