using ResortHotelRev2.Models.DB;
using ResortHotelRev2.Models.ViewModel;
using ResortHotelRev2.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResortHotelRev2.Models.EntityManager
{
    public class ReservationManager
    {
        public void AddReservation(RoomAndReservationModel roomResModel, int userId)
        {
            ResortDBEntities db = new ResortDBEntities();
          
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
                

                SYSGuestToRoomTable guestToRoom = new SYSGuestToRoomTable();
                guestToRoom.GuestId = userId;
                guestToRoom.ReservationId = reservationTable.Id;
                guestToRoom.RoomReserved = rm.RoomId;

                db.SYSGuestToRoomTables.Add(guestToRoom);
                db.SaveChanges();
            }
        
                        
        } //end AddReservation

        public List<RoomAndReservationModel> GetMyReservations(int userId) 
        {
            
            List<int> myReservationIds = new List<int>();
            
            List<RoomAndReservationModel> myReservationsInfo = new List<RoomAndReservationModel>();
            ResortDBEntities db = new ResortDBEntities();

            //Refactor Get all reservation Ids
            foreach (var reservation in db.SYSReservationTables)
            {
                if (reservation.ReservedByUserId == userId)
                {
                    myReservationIds.Add(reservation.Id);
                }

            }

            foreach (var reservationId in myReservationIds)
            {
                SYSReservationTable reservationTable = new SYSReservationTable();
                RoomAndReservationModel roomResModel = new RoomAndReservationModel();
                

                var reservationTableProfile = db.SYSReservationTables.Find(reservationId);
                var occupiedRoomTableProfile = db.SYSOccupiedRoomTables.Find(reservationId);
                var userProfile = db.SYSUserProfiles.Find(userId);

                List<int> myReservedRooms = new List<int>();



                roomResModel.Id = reservationTableProfile.Id;
                roomResModel.DatePlaced = reservationTableProfile.ReservationPlaced;
                roomResModel.CheckIn = reservationTableProfile.DateIN;
                roomResModel.CheckOut = reservationTableProfile.DateOUT;

                RoomManager roomManager = new RoomManager();
                roomResModel.RoomResRmProfile = roomManager.GetRoomsByReservation(reservationId);
                

                myReservationsInfo.Add(roomResModel);

                
            }

            return myReservationsInfo;
        } //End GetMyReservations

        public RoomAndReservationModel FindReservationById(int resId)  
        {
            ResortDBEntities db = new ResortDBEntities();
            RoomAndReservationModel selectedReservation = new RoomAndReservationModel();
            RoomManager roomManager = new RoomManager();
                        
            var reservationTableProfile = db.SYSReservationTables.Find(resId);
            
            selectedReservation.CheckIn = reservationTableProfile.DateIN;
            selectedReservation.CheckOut = reservationTableProfile.DateOUT;
            selectedReservation.DatePlaced = reservationTableProfile.ReservationPlaced;
            selectedReservation.GuestId = (int) reservationTableProfile.ReservedByUserId;

            
            int guestId = (int) reservationTableProfile.ReservedByUserId;
            var guestInfo = db.SYSUserProfiles.Find(guestId);

            selectedReservation.LastName = guestInfo.LastName;
            selectedReservation.FirstName = guestInfo.FirstName;
            selectedReservation.PhoneNumber = guestInfo.PhoneNumber;
            selectedReservation.Email = guestInfo.Email;

            selectedReservation.RoomResRmProfile = roomManager.GetRoomsByReservation(resId);


            return selectedReservation;
        }

        
    }
}