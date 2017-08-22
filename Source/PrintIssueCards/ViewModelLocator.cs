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

using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using Microsoft.Practices.Unity;
using PrintIssueCards.Common;
using PrintIssueCards.Interfaces;
using PrintIssueCards.Services;
using PrintIssueCards.ViewModels;

namespace PrintIssueCards
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer _container;

        public ViewModelLocator()
        {
            _container = new UnityContainer()

                .RegisterType<IFileSystemService, FileSystemService>(new ContainerControlledLifetimeManager())
                .RegisterType<ISettingsHandler, SettingsHandler>(new ContainerControlledLifetimeManager())
                .RegisterType<IJiraService, JiraService>(new ContainerControlledLifetimeManager())

                .RegisterPocoType<MainViewModel>()
                .RegisterPocoType<PreviewViewModel>()
                .RegisterPocoType<SettingsViewModel>()
                
                .RegisterInstance(Messenger.Default);

            Messenger.Default.Register<CreateWindowMessage>(this, CreateWindow);
        }

        private void CreateWindow(CreateWindowMessage message)
        {
            if (message == null)
            {
                return;
            }

            var window = (Window) _container.Resolve(message.ChildType);
            window.Owner = message.Owner;

            var vm = window.DataContext as ISupportParameter;
            if (vm != null)
            {
                vm.Parameter = message.Parameters;
            }

            if (message.Modal)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        public MainViewModel MainViewModel => _container.Resolve<MainViewModel>();

        public PreviewViewModel PreviewViewModel => _container.Resolve<PreviewViewModel>();

        public SettingsViewModel SettingsViewModel => _container.Resolve<SettingsViewModel>();
    }
}