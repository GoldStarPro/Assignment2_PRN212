using Azure.Messaging;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BookingDetailDAO
    {
        public static List<BookingDetail> GetBookingDetails()
        {
            using var context = new FuminiHotelManagementContext();
           
            return context.BookingDetails.Include(bd => bd.BookingReservation).Include(bd => bd.Room).ToList();
        }

        public static void AddBookingDetail(BookingDetail bookingDetail)
        {
            using var context = new FuminiHotelManagementContext();
            context.BookingDetails.Add(bookingDetail);
            context.SaveChanges();
        }

        public static void RemoveBookingDetail(int id)
        {
            using var context = new FuminiHotelManagementContext();
            var bd = context.BookingDetails.FirstOrDefault(bd => bd.BookingReservationId == id);

            if (bd != null) context.BookingDetails.Remove(bd);

            context.SaveChanges();
        }

        //public static void UpdateBookingDetail(BookingDetail bookingDetail)
        //{
        //    using var context = new FuminiHotelManagementContext();
        //    var bd = context.BookingDetails.FirstOrDefault(bd => bd.BookingReservationId == bookingDetail.BookingReservationId
        //    && bd.RoomId == bookingDetail.RoomId);

        //    if (bd != null)
        //    {
        //        bd.RoomId = bookingDetail.RoomId;
        //        bd.StartDate = bookingDetail.StartDate;
        //        bd.EndDate = bookingDetail.EndDate;
        //        bd.ActualPrice = bookingDetail.ActualPrice;
        //    }

        //    context.SaveChanges();
        //}

        public static void UpdateBookingDetail(BookingDetail bookingDetail, int currentRoomID)
        {
            using var context = new FuminiHotelManagementContext();

            // Tìm BookingDetail dựa trên BookingReservationId và RoomId
            var bd = context.BookingDetails.FirstOrDefault(bd =>
                bd.BookingReservationId == bookingDetail.BookingReservationId &&
                bd.RoomId == bookingDetail.RoomId);

            if (bd == null)
            {
                // Tìm và xóa BookingDetail hiện tại đang chọn
                var bookingDetailToRemove = context.BookingDetails
                                                  .FirstOrDefault(bdr => bdr.BookingReservationId == bookingDetail.BookingReservationId 
                                                  && bdr.RoomId == currentRoomID);

                if (bookingDetailToRemove != null)
                {
                    context.BookingDetails.Remove(bookingDetailToRemove);
                    context.SaveChanges();
                }

                // Tạo mới BookingDetail với thông tin đã cập nhật
                var updatedBookingDetail = new BookingDetail
                {
                    BookingReservationId = bookingDetail.BookingReservationId,
                    RoomId = bookingDetail.RoomId,
                    StartDate = bookingDetail.StartDate,
                    EndDate = bookingDetail.EndDate,
                    ActualPrice = bookingDetail.ActualPrice
                };

                // Thêm BookingDetail mới vào context và lưu thay đổi
                context.BookingDetails.Add(updatedBookingDetail);
                context.SaveChanges();
            }

            if (bd != null)
            {
                // Xóa BookingDetail hiện tại
                context.BookingDetails.Remove(bd);
                context.SaveChanges();

                // Tạo mới BookingDetail với thông tin đã cập nhật
                var updatedBookingDetail = new BookingDetail
                {
                    BookingReservationId = bookingDetail.BookingReservationId,
                    RoomId = bookingDetail.RoomId,
                    StartDate = bookingDetail.StartDate,
                    EndDate = bookingDetail.EndDate,
                    ActualPrice = bookingDetail.ActualPrice
                };

                // Thêm BookingDetail mới vào context và lưu thay đổi
                context.BookingDetails.Add(updatedBookingDetail);
                context.SaveChanges();
            }
        }




        public static BookingDetail? GetBookingDetail(int id)
        {
            using var context = new FuminiHotelManagementContext();
            return context.BookingDetails.FirstOrDefault(bd => bd.BookingReservationId == id);
        }
    }
}
