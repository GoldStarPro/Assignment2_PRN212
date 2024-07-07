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
using System.Windows.Shapes;

namespace TranHuyHoangWPF.Views.User
{
    /// <summary>
    /// Interaction logic for ReservationHistoryDetailsWindow.xaml
    /// </summary>
    public partial class ReservationHistoryDetailsWindow : Window
    {
        private int _bookingReservationId;
        private readonly IBookingDetailService bookingDetailService = new BookingDetailService();
        private readonly IBookingReservationService reservationService = new BookingReservationService();

        public ReservationHistoryDetailsWindow(int bookingReservationId)
        {
            InitializeComponent();
            DataContext = this;
            _bookingReservationId = bookingReservationId;
            LoadReservationHistoryDetails(bookingReservationId);
        }

        private void LoadReservationHistoryDetails(int bookingReservationId)
        {

            var details = bookingDetailService.GetBookingDetails()
                .Where(bd => bd.BookingReservationId == bookingReservationId)
                .ToList();
            var reservationHistoryDetails = new List<dynamic>();

            foreach (var detail in details)
            {
                reservationHistoryDetails.Add(new
                {
                    BookingReservationId = detail.BookingReservationId,
                    RoomNumber = detail.Room.RoomNumber,
                    StartDate = detail.StartDate,
                    EndDate = detail.EndDate,
                    ActualPrice = detail.ActualPrice,
                });
            }

            dgReservationHistoryDetails.ItemsSource = null;
            dgReservationHistoryDetails.ItemsSource = reservationHistoryDetails;
        }

        private void txtSearchRoomNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchRoomNumber.Text.Trim();
            var reservations = reservationService.GetBookingReservations()
                .Where(r => r.BookingReservationId == _bookingReservationId)
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
                            BookingReservationId = detail.BookingReservationId,
                            RoomNumber = detail.Room.RoomNumber,
                            StartDate = detail.StartDate,
                            EndDate = detail.EndDate,
                            ActualPrice = detail.ActualPrice,
                        });
                    }
                }
            }

            dgReservationHistoryDetails.ItemsSource = null;
            dgReservationHistoryDetails.ItemsSource = bookingDetails;
        }

    }
}
