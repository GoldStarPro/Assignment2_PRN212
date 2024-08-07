﻿using BusinessObject;
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

namespace TranHuyHoangWPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for UpdateBookingDetailsWindow.xaml
    /// </summary>
    public partial class UpdateBookingDetailsWindow : Window
    {
        private BookingDetail bookingDetail;
        private readonly IBookingDetailService bookingDetailService = new BookingDetailService();
        private readonly IBookingReservationService bookingReservationService = new BookingReservationService();
        private readonly IRoomInformationService roomInformationService = new RoomInformationService();
        public event EventHandler? BookingUpdated;
        public int currentRoomID;
        public UpdateBookingDetailsWindow(BookingDetail _bookingDetail)
        {
            InitializeComponent();
            bookingDetail = _bookingDetail;
            DataContext = bookingDetail;
            txtActualPrice.IsEnabled = false;
            cboRoomNumber.ItemsSource = roomInformationService.GetRoomInformation();
            cboRoomNumber.DisplayMemberPath = "RoomNumber";
            cboRoomNumber.SelectedValuePath = "RoomId";
            cboRoomNumber.SelectedValue = bookingDetail.RoomId;
            currentRoomID = _bookingDetail.RoomId;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Check if the exist of other booking detail entry with the same BookingReservationID and RoomID
            var existingBookingDetail = bookingDetailService.GetBookingDetails().Where(bd => bd.BookingReservationId == bookingDetail.BookingReservationId
                && bd.RoomId == (int)cboRoomNumber.SelectedValue).FirstOrDefault();
            if (existingBookingDetail != null && currentRoomID != (int)cboRoomNumber.SelectedValue)
            {
                bookingDetail.RoomId = currentRoomID;
                MessageBox.Show("This room is already booked for the selected reservation.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            // Xử lí Total Price trong BookingReservation

            var reservation = bookingDetail.BookingReservation;

            var days = (decimal)(bookingDetail.EndDate.DayNumber - bookingDetail.StartDate.DayNumber);
            var previousTotal = (days * bookingDetail.Room.RoomPricePerDay * 1.1m);

            reservation.TotalPrice = reservation.TotalPrice - previousTotal + decimal.Parse(txtTotalPrice.Text);

            bookingReservationService.UpdateBookingReservation(reservation);

            // Booking Detail

            bookingDetail.RoomId = (int)cboRoomNumber.SelectedValue;

            bookingDetail.StartDate = DateOnly.FromDateTime(DateTime.Parse(dptbStartDate.Text.Trim()));
            bookingDetail.EndDate = DateOnly.FromDateTime(DateTime.Parse(dptbEndDate.Text.Trim()));
            bookingDetail.ActualPrice = decimal.Parse(txtActualPrice.Text);

            bookingDetailService.UpdateBookingDetail(bookingDetail, currentRoomID);


            // Gọi sự kiện BookingUpdated sau khi cập nhật xong
            BookingUpdated?.Invoke(this, EventArgs.Empty);

            DialogResult = true;
        }

        private void RecaculateTotalPrice(object sender, EventArgs e)
        {
            if (cboRoomNumber.SelectedItem is RoomInformation room && dptbStartDate.SelectedDate.HasValue && dptbEndDate.SelectedDate.HasValue)
            {
                var days = (decimal)(dptbEndDate.SelectedDate.Value - dptbStartDate.SelectedDate.Value).TotalDays;
                txtActualPrice.Text = (days * room.RoomPricePerDay).ToString();
                txtTotalPrice.Text = (days * room.RoomPricePerDay * 1.1m).ToString();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
