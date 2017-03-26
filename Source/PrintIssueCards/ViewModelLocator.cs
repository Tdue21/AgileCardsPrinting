// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

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
                .RegisterPocoType<PreviewViewModel>()
                .RegisterPocoType<SettingsViewModel>();
        }

        public MainViewModel MainViewModel => _container.Resolve<MainViewModel>();

        public PreviewViewModel PreviewViewModel => _container.Resolve<PreviewViewModel>();

        public SettingsViewModel SettingsViewModel => _container.Resolve<SettingsViewModel>();
    }
}