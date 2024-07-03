using BusinessObject;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TranHuyHoangWPF
{
    /// <summary>
    /// Interaction logic for StaticReportPage.xaml
    /// </summary>
    public partial class StatisticReportPage : Page
    {
        public List<BookingReservation> BookingReservations { get; set; } = new List<BookingReservation>();

        private readonly IBookingReservationService bookingReservationService = new BookingReservationService();

        public StatisticReportPage()
        {
            InitializeComponent();
            DataContext = this;

            dpStartDate.SelectedDateChanged -= UpdateReport;
            dpEndDate.SelectedDateChanged -= UpdateReport;

            dpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            dpEndDate.SelectedDate = DateTime.Now;

            dpStartDate.SelectedDateChanged += UpdateReport;
            dpEndDate.SelectedDateChanged += UpdateReport;

            LoadReservationList();
        }

        private void UpdateReport(object? sender, SelectionChangedEventArgs? e)
        {
            if (sender == null || e == null) return;
            LoadReservationList();
        }

        //private void LoadReservationList()
        //{
        //    BookingReservations = bookingReservationService.GetBookingReservations().Where(br => br.BookingDate >= DateOnly.FromDateTime((DateTime)dpStartDate.SelectedDate!) && br.BookingDate <= DateOnly.FromDateTime((DateTime)dpEndDate.SelectedDate!)).ToList();

        //    dgReservations.ItemsSource = null;
        //    dgReservations.ItemsSource = BookingReservations;

        //    //txtRevenue.Text = BookingReservations.Sum(br => br.TotalPrice).ToString();
        //    //txtDays.Text = ((DateTime)dpEndDate.SelectedDate! - (DateTime)dpStartDate.SelectedDate!).TotalDays.ToString();
        //    //txtReservations.Text = BookingReservations.Count.ToString();
        //}

        private void LoadReservationList()
        {
            if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null)
                return;

            var reservations = bookingReservationService.GetBookingReservations()
                .Where(br => br.BookingDate >= DateOnly.FromDateTime((DateTime)dpStartDate.SelectedDate!) && br.BookingDate <= DateOnly.FromDateTime((DateTime)dpEndDate.SelectedDate!))
                .ToList();

            var bookingDetails = new List<dynamic>();

            foreach (var reservation in reservations)
            {
                foreach (var detail in reservation.BookingDetails)
                {
                    bookingDetails.Add(new
                    {
                        RoomNumber = detail.Room.RoomNumber,
                        Customer = reservation.Customer.CustomerFullName,
                        BookingDate = reservation.BookingDate,
                        StartDate = detail.StartDate,
                        EndDate = detail.EndDate
                    });
                }
            }

            dgReservations.ItemsSource = bookingDetails;

        }


        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Window.GetWindow(this).Close();
        }
    }
}
