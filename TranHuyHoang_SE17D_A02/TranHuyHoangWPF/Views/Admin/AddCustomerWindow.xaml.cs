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
            //if (string.IsNullOrWhiteSpace(txtFullName.Text))
            //{
            //    MessageBox.Show("Please enter the full name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                !dptbBirthday.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(txtOldPassword.Password) ||
                string.IsNullOrWhiteSpace(txtcfPassword.Password))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra xem mật khẩu có khớp không
            if (txtOldPassword.Password != txtcfPassword.Password)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Nếu tất cả các trường hợp lệ, tiến hành thêm Customer
            customer.CustomerFullName = txtFullName.Text.Trim();
            customer.Telephone = txtPhone.Text.Trim();
            customer.CustomerBirthday = DateOnly.FromDateTime(dptbBirthday.SelectedDate.Value);
            customer.EmailAddress = txtEmail.Text.Trim();
            customer.Password = txtOldPassword.Password;
            customer.CustomerStatus = 1;
            customerService.AddCustomer(customer);

            DialogResult = true;
            Close();
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
