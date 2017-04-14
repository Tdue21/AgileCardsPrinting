// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System.Windows;
using DevExpress.Mvvm;
using Microsoft.Practices.Unity;
using PrintIssueCards.Common;
using PrintIssueCards.Interfaces;
using PrintIssueCards.ViewModels;

namespace PrintIssueCards
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer _container;

        public ViewModelLocator()
        {
            _container = new UnityContainer()
                .RegisterType<ISettingsHandler, SettingsHandler>(new ContainerControlledLifetimeManager())
                .RegisterType<IJiraHandler, JiraHandler>(new ContainerControlledLifetimeManager())

                .RegisterPocoType<MainViewModel>()
                .RegisterPocoType<FilterSearchViewModel>()
                .RegisterPocoType<BasicSearchViewModel>()
                .RegisterPocoType<PreviewViewModel>()
                .RegisterPocoType<SettingsViewModel>();

            Messenger.Default.Register<CreateWindowMessage>(this, CreateWindow);
        }

        private void CreateWindow(CreateWindowMessage message)
        {
            if (message == null)
            {
                return;
            }

            var window = (Window)_container.Resolve(message.ChildType);
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