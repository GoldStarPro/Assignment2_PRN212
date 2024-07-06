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

        public event EventHandler BookingUpdated;
        public BookingReservationDetailsWindow(int bookingReservationId)
        {
            InitializeComponent();
            DataContext = this;
            _bookingReservationId = bookingReservationId;
            LoadBookingReservationDetails(bookingReservationId);
        }

        // LoadBookingReservationDetails version 1. Sửa lại Binding Room.RoomNumber
        private void LoadBookingReservationDetails(int bookingReservationId)
        {

            BookingDetails = bookingDetailService.GetBookingDetails()
                .Where(bd => bd.BookingReservationId == bookingReservationId)
                .ToList();

            dgBookingDetails.ItemsSource = null;
            dgBookingDetails.ItemsSource = BookingDetails;

        }

        // Search theo kiểu Load1
        private void txtSearchRoomNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchRoomNumber.Text.Trim();

            dgBookingDetails.ItemsSource = BookingDetails!.Where(bd => bd.Room.RoomNumber.Contains(searchText));

            //var bookingDetails = bookingDetailService.GetBookingDetails()
            //    .Where(r => r.BookingReservationId == _bookingReservationId && r.Room.RoomNumber.Contains(searchText))
            //    .ToList();

            //dgBookingDetails.ItemsSource = null;
            //dgBookingDetails.ItemsSource = bookingDetails;

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookingDetails.SelectedItem is BookingDetail selectedBookingDetail)
            {
                var updateBookingDetailsWindow = new UpdateBookingDetailsWindow(selectedBookingDetail);
                updateBookingDetailsWindow.BookingUpdated += UpdateBookingDetailsWindow_BookingUpdated;
                if (updateBookingDetailsWindow.ShowDialog() == true)
                {
                    LoadBookingReservationDetails(_bookingReservationId);
                    BookingUpdated?.Invoke(this, EventArgs.Empty); // Truyền sự kiện lên
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to update.", "Selection Error");
            }
        }

        private void UpdateBookingDetailsWindow_BookingUpdated(object sender, EventArgs e)
        {
            // Truyền sự kiện lên cho StatisticReportPage
            BookingUpdated?.Invoke(this, EventArgs.Empty);
        }


        ////LoadBookingReservationDetails Version 2

        //private void LoadBookingReservationDetails(int bookingReservationId)
        //{

        //    var details = bookingDetailService.GetBookingDetails()
        //        .Where(bd => bd.BookingReservationId == bookingReservationId)
        //        .ToList();
        //    var bookingDetails = new List<dynamic>();

        //    foreach (var detail in details)
        //    {
        //        bookingDetails.Add(new
        //        {
        //            BookingReservationId = detail.BookingReservationId,
        //            RoomNumber = detail.Room.RoomNumber,
        //            StartDate = detail.StartDate,
        //            EndDate = detail.EndDate,
        //            ActualPrice = detail.ActualPrice,
        //        });
        //    }

        //    dgBookingDetails.ItemsSource = null;
        //    dgBookingDetails.ItemsSource = bookingDetails;

        //}


        //// Search theo kiểu của Load2
        //private void txtSearchRoomNumber_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string searchText = txtSearchRoomNumber.Text.Trim();
        //    var reservations = reservationService.GetBookingReservations()
        //        .Where(r => r.BookingReservationId == _bookingReservationId)
        //        .ToList();

        //    var bookingDetails = new List<dynamic>();

        //    foreach (var reservation in reservations)
        //    {
        //        foreach (var detail in reservation.BookingDetails)
        //        {
        //            if (detail.Room.RoomNumber.Contains(searchText))
        //            {
        //                bookingDetails.Add(new
        //                {
        //                    BookingReservationId = detail.BookingReservationId,
        //                    RoomNumber = detail.Room.RoomNumber,
        //                    StartDate = detail.StartDate,
        //                    EndDate = detail.EndDate,
        //                    ActualPrice = detail.ActualPrice,
        //                });
        //            }
        //        }
        //    }
        //    dgBookingDetails.ItemsSource = null;
        //    dgBookingDetails.ItemsSource = bookingDetails;
        //}

    }
}
