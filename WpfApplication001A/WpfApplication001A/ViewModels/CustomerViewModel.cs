using System;
using System.Diagnostics;
using System.Windows.Input;
using WpfApplication001A.Commands;
using WpfApplication001A.Models;
using WpfApplication001A.Views;

namespace WpfApplication001A.ViewModels
{
    /// <summary>
    /// In this class we have our logic, we could access custome properties and update customer name and we can call the save change method to persist the changes to our data source.
    /// </summary>
    internal class CustomerViewModel
    {
        private Customer customer;

        private CustomerInfoViewModel childViewModel;

        public CustomerViewModel()
        {
            //normally we we will popualte this by loading from database, maybe an xml file wherever you store your customers
            customer = new Customer("Joey");
            childViewModel = new CustomerInfoViewModel() { Info = "Instantiate in CustomerViewModel() constructor" };
            UpdateCommand = new CustomerUpdateCommand(this);
        }

        //cos we need to manipulate the customer we need a nstance of that one

        public Customer Customer
        {
            get
            {
                return customer;
            }
        }

        //Get the UpdateCommand for the viewmodel
        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        //since we want to modify the customer info then we should save the changes as well
        public void SaveChanges()
        {
            //Debug.Assert(false, string.Format("{0} was updated.", Customer.Name));
            CustomerInfoView view = new CustomerInfoView();
            view.DataContext = childViewModel;
            //childViewModel.Info = Customer.Name + " was udpated in the database";
            view.ShowDialog();
        }
    }
}