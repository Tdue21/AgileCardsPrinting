// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Linq;
using System.Xml.Linq;

namespace PrintIssueCards.Common
{
    public static class XElementExtension
    {
        public static T GetDescendantValue<T>(this XElement element, string descendant, T defaultValue)
        {
            try
            {
                var node = element.Descendants(descendant).FirstOrDefault();
                return node != null
                           ? (T) Convert.ChangeType(node.Value, typeof (T))
                           : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
