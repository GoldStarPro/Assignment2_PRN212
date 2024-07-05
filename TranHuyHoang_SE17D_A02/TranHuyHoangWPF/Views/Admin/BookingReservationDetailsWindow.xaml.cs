using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BusinessObject;
using Repository;
using Service;

namespace TranHuyHoangWPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for BookingReservationDetailsWindow.xaml
    /// </summary>
    public partial class BookingReservationDetailsWindow : Window
    {
        private readonly IBookingDetailService bookingDetailService = new BookingDetailService();

        public BookingReservationDetailsWindow(int bookingReservationId)
        {
            InitializeComponent();
            DataContext = this;
            LoadBookingReservationDetails(bookingReservationId);
        }

        private void LoadBookingReservationDetails(int bookingReservationId)
        {

            var details = bookingDetailService.GetBookingDetails()
                .Where(bd => bd.BookingReservationId == bookingReservationId)
                .ToList();
            var bookingDetails = new List<dynamic>();

            foreach (var detail in details)
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

            dgBookingDetails.ItemsSource = null;
            dgBookingDetails.ItemsSource = bookingDetails;

        }
    }
}
