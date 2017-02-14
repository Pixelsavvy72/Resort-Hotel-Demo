using ResortHotelRev2.Models.DB;
using ResortHotelRev2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResortHotelRev2.Models.EntityManager
{
    public class ReservationManager
    {
        public void AddReservation(RoomAndReservationModel roomResModel)
        {
            ResortDBEntities db = new ResortDBEntities();




            SYSUser user = new SYSUser();            
            SYSReservationTable reservationTable = new SYSReservationTable();

            reservationTable.DateIN = roomResModel.CheckIn;
            reservationTable.DateOUT = roomResModel.CheckOut;
            reservationTable.Status = roomResModel.ReservationStatus.ToString();
            reservationTable.MadeBy = roomResModel.MadeBy.ToString();
            reservationTable.ReservationPlaced = DateTime.UtcNow;
            reservationTable.ReservedByUserId = roomResModel.GuestId;

            db.SYSReservationTables.Add(reservationTable);
            db.SaveChanges();

            
            foreach (var rm in roomResModel.RoomResRmProfile)
            {
                SYSOccupiedRoomTable roomOccupied = new SYSOccupiedRoomTable();
                roomOccupied.CheckIN = roomResModel.CheckIn;
                roomOccupied.CheckOUT = roomResModel.CheckOut;
                roomOccupied.ReservationID = reservationTable.Id;
                roomOccupied.RoomID = rm.RoomId;

                db.SYSOccupiedRoomTables.Add(roomOccupied);
                db.SaveChanges();
            }
                        
        } //end AddReservation
        
    }
}