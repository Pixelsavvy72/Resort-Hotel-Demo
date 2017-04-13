# Description

This program is an in-progress hotel reservation system. It allows for the creation of guests and reservations, assigning rooms and guests to individual reservations, editing, and account management.

**A video displaying functionality** is available at: https://youtu.be/9rFQa7FNuLU
- Video Timestamp Key:
- 0:00 Account creation 
- 1:26 Place a reservation
- 3:20 Admin control panel demonstration
- 6:51 Responsive design across multiple breakpoints for small phones, tablets, desktops, and large screen desktops.

Please note that this is primarily a functionality demo and I'll be fleshing out the design more soon.

## Current Functionality

**Account Management**
- Create new user: User or Admin created account
- Login/ Logout out of accounts.
- Edit user profile information: Available to users after login, or admins from the Admin Control Panel. 
- Delete user: Available to Admin from Admin Control Panel.
- Assign roles (member / admin) to accounts.
- Search from within Admin Control Panel. Display all users, Search by last name, search by User Id, search by reservation number.

**Reservations**
- Search for available rooms by date range using dual JavaScript calendars.
- Select room or rooms to reserve and add to reservation.
- Persist reservation and related data to various database tables (see below).
- Reservation is assigned to User. User created reservations are assigned to that user. Admin created reservations are assigned to selected guest Id.
- Rooms are made unavailable for those dates.
- User or Admin has ability to cancel reservations. Canceled reservations return rooms to available status for those dates.

## Backend To-Dos:

- [ ] Add Associate Role – mid-level restricted permission role for general employees.
- [x] Add ability for user to view a placed reservation.
- [x] Add Associate and Admin notes fields attached to individual users. (Ex: Likes extra towels.)
- [x] Add user email.
- [x] Add password confirmation field during sign-up.
- [ ] Add reset password functionality.
- [ ] Allow users to delete their own account.
- [ ] Allow for cancelation of part of a reservation (ex: 1 room of a 3 room reservation)
- [x] Finish Admin Control Panel – ~~Create New User~~ (Done), ~~Search for Reservation~~ (Done), ~~Add Reservation~~ (Done), 
- [ ] Format phone numbers throughout based on user’s country of residence
- [ ] Add more validation and try-catch throughout.
- [ ] Figure out why custom MVC annotation validation isn’t working in partial-view.
- [ ] Create drop-down in Admin Control Panel for Reservation Placed type (Online, Phone, Front Desk, Travel Agent) and status (confirmed, canceled, completed, in progress).
- [ ] Allow user to reserve multiple rooms with more than one date range per reservation.
- [ ] Implement tables not yet implemented.

## Frontend To-Dos:
- [x] Design large size homepage
- [ ] Think about Ajaxifying the login-dropdown.
- [x] Add special notifications to large size pages. Sidebar? Drop-in?
- [ ] Choose better fonts
- [x] Choose better colors for indexBlocks.
- [ ] Design secondary pages

## Database Table Explanation 

- LOOKUPRole: Stores the available roles in the system.
- SYSGuestTable: Not yet implemented. Will store guests that are not registered users. May not be necessary depending on requirements.
- SYSGuestToRoomTable: Not yet implemented. Stores stores which guest is staying in which rooms. May not be necessary depending on requirements.
- SYSOccupiedRoomTable: Stores which rooms belong to which reservations, the check-in and check-out date.
- SYSReservationTable: Stores check-in / check-out date of reservation, reservation status, how it was made, which user placed the reservation, and when the reservation was placed.
- SYSRoomReservedTable: Not yet implemented. Stores room number, reservation id, status. Will allow for multiple rooms / dates on a single reservation and cancellation of individual rooms without canceling the entire reservation.
- SYSRoomTypeTable: No longer needed.
- SYSRoomsTable: Stores information about rooms such as style, occupancy, bed type, smoking, etc.
- SYSUser: Stores system user id, login name, password
- SYSUserProfile: Stores individual user information such as name, phone number, notes about the user, etc
- SYSUserRole: Stores the user role, and whether the user is currently active.

## Notes

Hat-tip to user Vincent Durano for his information and examples on user account creation.
