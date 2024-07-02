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
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        private Customer customer;
        private readonly ICustomerService customerService = new CustomerService();

        public AddCustomerWindow()
        {
            InitializeComponent();
            customer = new Customer();
            DataContext = customer;
            //dptbBirthday.SelectedDate = DateTime.Today;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content.ToString()!.Equals("Add"))
            {
                if (txtOldPassword.Password != txtNewPassword.Password)
                {
                    MessageBox.Show("Passwords do not match", "Profile");
                }
                else
                {
                    customer.CustomerFullName = txtFullName.Text.Trim();
                    customer.Telephone = txtPhone.Text.Trim();
                    customer.CustomerBirthday = DateOnly.FromDateTime(DateTime.Parse(dptbBirthday.Text.Trim()));
                    customer.EmailAddress = txtEmail.Text.Trim();
                    customer.Password = txtOldPassword.Password;
                    customer.CustomerStatus = 1;
                    customerService.AddCustomer(customer);
                }

                DialogResult = true;

                Close();

            }
            else if (!string.IsNullOrEmpty(txtOldPassword.Password) && !string.IsNullOrEmpty(txtOldPassword.Password) && !txtOldPassword.Password.Equals(customer.Password))
            {
                MessageBox.Show("Password is incorrect", "Profile");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
