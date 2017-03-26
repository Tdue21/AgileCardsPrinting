// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.Windows.Input;

namespace PrintIssueCards.Common
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        private readonly Action<object> _executeWithParameter;

        public DelegateCommand(Action<object> executeWithParameter) : this(executeWithParameter, null)
        {
        }

        public DelegateCommand(Action<object> executeWithParameter, Func<bool> canExecute)
        {
            if (executeWithParameter == null)
            {
                throw new ArgumentNullException("executeWithParameter");
            }
            _executeWithParameter = executeWithParameter;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action execute) : this(execute, null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            if (_executeWithParameter != null)
            {
                _executeWithParameter(parameter);
            }
            else if (_execute != null)
            {
                _execute();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
