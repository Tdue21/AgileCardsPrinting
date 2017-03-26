// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using DevExpress.Mvvm.POCO;
using Microsoft.Practices.Unity;

namespace PrintIssueCards.Common
{
    public static class UnitContainerPocoExtensions
    {
        public static IUnityContainer RegisterPocoType<TType>(this IUnityContainer container)
        {
            return container.RegisterType(typeof(TType), ViewModelSource.GetPOCOType(typeof(TType)));
        }

        public static IUnityContainer RegisterPocoType<TType>(this IUnityContainer container, LifetimeManager lifetimeManager)
        {
            return container.RegisterType(typeof(TType), ViewModelSource.GetPOCOType(typeof(TType)), lifetimeManager);
        }
    }
}