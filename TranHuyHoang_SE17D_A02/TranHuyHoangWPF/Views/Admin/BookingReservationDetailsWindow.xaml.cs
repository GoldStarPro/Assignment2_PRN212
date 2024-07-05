using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private int _bookingReservationId;
        private readonly IBookingReservationService reservationService = new BookingReservationService();
        private readonly IBookingDetailService bookingDetailService = new BookingDetailService();
        public List<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

        public BookingReservationDetailsWindow(int bookingReservationId)
        {
            InitializeComponent();
            DataContext = this;
            _bookingReservationId = bookingReservationId;
            LoadBookingReservationDetails(bookingReservationId);
        }

        //// LoadBookingReservationDetails version 1. Sửa lại Binding Room.RoomNumber
        //private void LoadBookingReservationDetails(int bookingReservationId)
        //{

        //    BookingDetails = bookingDetailService.GetBookingDetails()
        //        .Where(bd => bd.BookingReservationId == bookingReservationId)
        //        .ToList();

        //    dgBookingDetails.ItemsSource = null;
        //    dgBookingDetails.ItemsSource = BookingDetails;

        //}


        //LoadBookingReservationDetails Version 2

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


        // Search theo kiểu của Load
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

            dgBookingDetails.ItemsSource = null;
            dgBookingDetails.ItemsSource = bookingDetails;
        }

    }
}
