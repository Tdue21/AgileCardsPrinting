// *********************************************************************
// * Copyright © 2008 - 2017 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.DataAnnotations;

namespace PrintIssueCards.ViewModels
{
    [POCOViewModel]
    public class BasicSearchViewModel
    {
        public virtual string KeyList { get; set; }

        public virtual object OrderByItems => new[] { "Key", "Priority", "IssueType", "Assignee", "Reporter", "Status" };

        public virtual string SelectedOrderBy { get; set; }
    }
}
