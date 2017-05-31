using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApplicationExercise.Models;

namespace WPFApplicationExercise.ViewModels
{
    public class CustomerViewModel
    {
        private Customer customer;

        public CustomerViewModel()
        {
            customer = new Customer()
            {
                CustomerName = "Joey"
            };

            //how do we know whether a model validated from the viewmodel
            //way 1 customer.IsValid
            //way 2: through the indexer (Customer as IDataErrorInfo)["CustomerName"]
            //way 3 Validation.GetValidationErrors();
        }

        public Customer Customer
        {
            get
            {
                return customer;
            }
        }
    }
}