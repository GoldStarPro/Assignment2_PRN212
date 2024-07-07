using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBookingDetailService
    {
        void AddBookingDetail(BookingDetail bookingDetail);
        void UpdateBookingDetail(BookingDetail bookingDetail, int currentRoomID);
        void DeleteBookingDetail(int bookingDetailId, int roomID);
        List<BookingDetail> GetBookingDetails();
        BookingDetail? GetBookingDetail(int id);
    }
}
