// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Windows;

namespace PrintIssueCards.Common
{
    public class CreateWindowMessage
    {
        public Window Owner { get; set; }

        public Type ChildType { get; set; }
        public object[] Parameters { get; set; }

        public bool Modal { get; set; }
    }
}