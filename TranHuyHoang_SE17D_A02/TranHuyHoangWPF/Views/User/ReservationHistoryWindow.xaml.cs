using BusinessObject;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TranHuyHoangWPF.Views.Admin;

namespace TranHuyHoangWPF.Views.User
{
    /// <summary>
    /// Interaction logic for ReservationHistoryWindow.xaml
    /// </summary>
    public partial class ReservationHistoryWindow : Window
    {
        private Customer _customer;
        private readonly IBookingReservationService reservationService = new BookingReservationService();
        private readonly ICustomerService customerService = new CustomerService();

        public List<BookingReservation> Reservations { get; set; } = new List<BookingReservation>();

        public ReservationHistoryWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            LoadReservations();
            btnProfile.Content = _customer.CustomerFullName;
            DataContext = this;
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new ProfileWindow(_customer);

            if (profileWindow.ShowDialog() == true)
            {
                _customer = customerService.GetCustomer(_customer.EmailAddress)!;
                btnProfile.Content = _customer.CustomerFullName;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        //private void txtSearchRoomNumber_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string searchText = txtSearchRoomNumber.Text.Trim();
        //    dgReservations.ItemsSource = Reservations.Where(r => r.CustomerId == _customer.CustomerId && r.BookingDetails.ElementAt(0).Room.RoomNumber.Contains(searchText)).ToList();
        //}

        private void txtSearchRoomNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchRoomNumber.Text.Trim();
            var reservations = reservationService.GetBookingReservations()
                .Where(r => r.CustomerId == _customer.CustomerId)
                .ToList();

            var bookingDetails = new List<dynamic>();

            foreach (var reservation in reservations)
            {
                foreach (var detail in reservation.BookingDetails)
                {
                    if (detail.Room.RoomNumber.Contains(searchText))
                    {
                        bookingDetails.Add(new
                        {
                            RoomNumber = detail.Room.RoomNumber,
                            BookingDate = reservation.BookingDate,
                            StartDate = detail.StartDate,
                            EndDate = detail.EndDate
                        });
                    }
                }
            }

            dgReservations.ItemsSource = bookingDetails;
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var bookingWindow = new BookingWindow(_customer);

            if (bookingWindow.ShowDialog() == true)
            {
                LoadReservations();
            }
        }

        //private void LoadReservations()
        //{
        //    Reservations = reservationService.GetBookingReservations().Where(r => r.CustomerId == _customer.CustomerId).ToList();

        //    dgReservations.ItemsSource = null;
        //    dgReservations.ItemsSource = Reservations;
        //}

        private void LoadReservations()
        {
            var reservations = reservationService.GetBookingReservations()
                .Where(r => r.CustomerId == _customer.CustomerId)
                .ToList();

            var bookingReservations = new List<dynamic>();

            foreach (var reservation in reservations)
            {
                bookingReservations.Add(new
                {
                    BookingReservationId = reservation.BookingReservationId,
                    CustomerFullName = reservation.Customer.CustomerFullName,
                    BookingDate = reservation.BookingDate,
                    TotalPrice = reservation.TotalPrice,
                    BookingStatus = reservation.BookingStatus
                });
            }

            dgReservations.ItemsSource = bookingReservations;
        }


        private void ViewHistoryDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int bookingReservationId)
            {
                var reservationHistoryDetailsWindow = new ReservationHistoryDetailsWindow(bookingReservationId);
                reservationHistoryDetailsWindow.ShowDialog();
            }
        }

    }
}
