using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BookingReservationDAO
    {
        public static List<BookingReservation> GetReservations()
        {
            using var context = new FuminiHotelManagementContext();
            return context.BookingReservations.Include(br => br.Customer).Include(br => br.BookingDetails).ThenInclude(bd => bd.Room).ToList();
        }

        public static void AddReservation(BookingReservation reservation)
        {
            using var context = new FuminiHotelManagementContext();
            context.BookingReservations.Add(reservation);
            context.SaveChanges();
        }

        public static void RemoveReservation(int id)
        {
            using var context = new FuminiHotelManagementContext();
            var r = context.BookingReservations.FirstOrDefault(br => br.BookingReservationId == id);

            if (r != null)
            {
                r.BookingStatus = 2;
                context.SaveChanges();
            };

        }

        public static void UpdateReservation(BookingReservation reservation)
        {
            using var context = new FuminiHotelManagementContext();
            BookingReservation? r = context.BookingReservations.FirstOrDefault(x => x.BookingReservationId == reservation.BookingReservationId);

            if (r != null)
            {
                r.BookingDate = reservation.BookingDate;
                r.TotalPrice = reservation.TotalPrice;
                r.BookingStatus = reservation.BookingStatus;
            }

            context.SaveChanges();
        }

        public static BookingReservation? GetReservationById(int id)
        {
            using var context = new FuminiHotelManagementContext();
            return context.BookingReservations.FirstOrDefault(x => x.BookingReservationId == id);
        }

        public static int GenerateNewBookingReservationId(int customerId)
        {
            using var context = new FuminiHotelManagementContext();

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // Tìm BookingReservation của khách hàng trong ngày hôm nay
            var existingReservation = context.BookingReservations
                .FirstOrDefault(b => b.CustomerId == customerId && b.BookingDate == today);

            if (existingReservation != null)
            {
                // Nếu có, trả về BookingReservationId hiện tại
                return existingReservation.BookingReservationId;
            }
            else
            {
                // Nếu không có, tạo mới BookingReservationId dựa trên giá trị lớn nhất hiện tại
                var maxId = context.BookingReservations.Max(b => (int?)b.BookingReservationId) ?? 0;
                return maxId + 1;
            }
        }

    }
}
