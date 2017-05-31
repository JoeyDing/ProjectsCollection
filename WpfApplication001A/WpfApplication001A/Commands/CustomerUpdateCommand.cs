using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApplication001A.ViewModels;

namespace WpfApplication001A.Commands
{
    internal class CustomerUpdateCommand : ICommand
    {
        private CustomerViewModel _customerViewModel;

        public CustomerUpdateCommand(CustomerViewModel customerViewModel)
        {
            _customerViewModel = customerViewModel;
        }

        //public event EventHandler CanExecuteChanged;
        /// <summary>
        /// pass the event to command manager
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            //instead of putting logic into this customerupdatecommand class, we
            //for control ,enable....based on the returned value
            //return _customerViewModel.CanUpdate;
            return String.IsNullOrWhiteSpace(_customerViewModel.Customer.Error);
        }

        public void Execute(object parameter)
        {
            _customerViewModel.SaveChanges();
        }
    }
}