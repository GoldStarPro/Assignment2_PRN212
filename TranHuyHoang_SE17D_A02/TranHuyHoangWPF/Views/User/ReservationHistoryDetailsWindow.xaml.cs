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
        private readonly IBookingDetailService bookingDetailService = new BookingDetailService();

        public ReservationHistoryDetailsWindow(int reservationHistoryId)
        {
            InitializeComponent();
            DataContext = this;
            LoadReservationHistoryDetails(reservationHistoryId);
        }

        private void LoadReservationHistoryDetails(int reservationHistoryId)
        {

            var details = bookingDetailService.GetBookingDetails()
                .Where(bd => bd.BookingReservationId == reservationHistoryId)
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
    }
}
