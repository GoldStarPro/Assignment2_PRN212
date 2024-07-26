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

namespace TranHuyHoangWPF.Views.User
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private Customer customer;
        private readonly ICustomerService customerService = new CustomerService();

        public ProfileWindow()
        {
            InitializeComponent();
            customer = new Customer();
            DataContext = customer;
            btnUpdate.Content = "Add";
            dptbBirthday.SelectedDate = DateTime.Today;
        }

        public ProfileWindow(Customer _customer)
        {
            InitializeComponent();
            customer = _customer;
            DataContext = customer;
            txtEmail.IsEnabled = false;

            // Gán giá trị cho DatePicker nếu CustomerBirthday có giá trị
            if (customer.CustomerBirthday != default)
            {
                dptbBirthday.SelectedDate = new DateTime(customer.CustomerBirthday!.Value.Year, customer.CustomerBirthday.Value.Month, customer.CustomerBirthday.Value.Day);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOldPassword.Password))
            {
                MessageBox.Show("Password is required to update profile", "Profile", MessageBoxButton.OK, MessageBoxImage.Exclamation); 
                return;
            }

            if (!string.IsNullOrEmpty(txtOldPassword.Password) && !txtOldPassword.Password.Equals(customer.Password))
            {
                MessageBox.Show("Password is incorrect", "Profile", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                customer.CustomerFullName = txtFullName.Text.Trim();
                customer.Telephone = txtPhone.Text.Trim();
                customer.CustomerBirthday = DateOnly.FromDateTime(DateTime.Parse(dptbBirthday.Text.Trim()));

                if (!string.IsNullOrEmpty(txtNewPassword.Password))
                {
                    customer.Password = txtNewPassword.Password;
                }

                customerService.UpdateCustomer(customer);

                DialogResult = true;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
