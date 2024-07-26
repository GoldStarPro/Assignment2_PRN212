using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBookingReservationService
    {
        List<BookingReservation> GetBookingReservations();
        BookingReservation? GetBookingReservationById(int id);
        void AddBookingReservation(BookingReservation reservation);
        void UpdateBookingReservation(BookingReservation reservation);
        void DeleteBookingReservation(int id);
        int GenerateNewBookingReservationId(int customerID);
    }
}
