using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObject;
using DataAccessLayer;
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

        public event EventHandler? BookingUpdated;
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

        private void UpdateBookingDetailsWindow_BookingUpdated(object? sender, EventArgs? e)
        {
            // Truyền sự kiện lên cho StatisticReportPage
            BookingUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            decimal sum = 0;
            var selectedBookingDetails = dgBookingDetails.SelectedItems.Cast<BookingDetail>().ToList();

            if (selectedBookingDetails.Count > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the selected the booking detail(s)?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    foreach (var bookingDetail in selectedBookingDetails)
                    {
                        if (bookingDetail.ActualPrice != null)
                        {
                            sum += (decimal)bookingDetail.ActualPrice;
                            sum *= 1.1m;
                        }
                        // Cập nhật lại Total Price
                        UpdateTotalPrice(bookingDetail.BookingReservationId, sum);


                        // Xóa BookingDetail khỏi database
                        bookingDetailService.DeleteBookingDetail(bookingDetail.BookingReservationId, bookingDetail.RoomId);
                    }

                    // Thông báo cập nhật tới StatisticReportPage
                    BookingUpdated?.Invoke(this, EventArgs.Empty);

                }
            }
            else
            {
                MessageBox.Show("Please select at least one booking detail to delete.", "Selection Error");
            }


            LoadBookingReservationDetails(_bookingReservationId);
        }

        private void UpdateTotalPrice(int bookingReservationId, decimal sumToDeduct)
        {
            using var context = new FuminiHotelManagementContext();
            var reservation = context.BookingReservations.FirstOrDefault(br => br.BookingReservationId == bookingReservationId);

            if (reservation != null)
            {
                // Cập nhật giá trị của TotalPrice
                reservation.TotalPrice -= sumToDeduct;

                // Lưu các thay đổi vào database
                context.SaveChanges();
            }
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
