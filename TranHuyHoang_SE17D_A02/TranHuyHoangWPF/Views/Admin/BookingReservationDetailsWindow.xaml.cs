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

        private void LoadBookingReservationDetails(int bookingReservationId)
        {

            BookingDetails = bookingDetailService.GetBookingDetails()
                .Where(bd => bd.BookingReservationId == bookingReservationId)
                .ToList();

            dgBookingDetails.ItemsSource = null;
            dgBookingDetails.ItemsSource = BookingDetails;

        }

        private void txtSearchRoomNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchRoomNumber.Text.Trim();
            dgBookingDetails.ItemsSource = BookingDetails!.Where(bd => bd.Room.RoomNumber.Contains(searchText));
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
            var selectedBookingDetails = dgBookingDetails.SelectedItems.Cast<BookingDetail>().ToList();

            if (selectedBookingDetails.Count > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the selected the booking detail(s)?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    foreach (var bookingDetail in selectedBookingDetails)
                    {
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

    }
}
