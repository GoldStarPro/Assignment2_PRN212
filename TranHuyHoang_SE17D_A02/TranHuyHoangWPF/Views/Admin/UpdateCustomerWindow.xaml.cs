using BusinessObject;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TranHuyHoangWPF
{
    /// <summary>
    /// Interaction logic for UpdateCustomerWindow.xaml
    /// </summary>
    public partial class UpdateCustomerWindow : Window
    {
        private Customer customer;
        private readonly ICustomerService customerService = new CustomerService();

        public UpdateCustomerWindow(Customer _customer)
        {
            InitializeComponent();
            customer = _customer;
            DataContext = customer;
            txtEmail.IsEnabled = false;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            customer.CustomerFullName = txtFullName.Text.Trim();
            customer.Telephone = txtPhone.Text.Trim();
            customer.CustomerBirthday = DateOnly.FromDateTime(DateTime.Parse(dptbBirthday.Text.Trim()));

            customerService.UpdateCustomer(customer);

            DialogResult = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
