namespace SimpleHotelRoomManagementProject_CSharpProject2_OOP
{
    internal class Program
    {

        // List to store rooms and reservations
        static List<Room> rooms = new List<Room>();
        static List<Reservation> reservations = new List<Reservation>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nHotel Room Management System");
                Console.WriteLine("1. Add New Room");
                Console.WriteLine("2. View All Rooms");
                Console.WriteLine("3. Reserve a Room");
                Console.WriteLine("4. View All Reservations");
                Console.WriteLine("5. Search Reservation by Guest Name");
                Console.WriteLine("6. Find Highest Paying Guest");
                Console.WriteLine("7. Cancel Reservation by Room Number");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddNewRoom(); break;
                    case "2": ViewAllRooms(); break;
                    case "3": ReserveRoom(); break;
                    case "4": ViewAllReservations(); break;
                    case "5": SearchReservation(); break;
                    //case "6": FindHighestPayingGuest(); break;
                    //case "7": CancelReservation(); break;
                    case "8": return; // Exit the system
                    default: Console.WriteLine("Invalid choice. Try again."); break;
                }
            }



        }// End of Main method

        // add a new room
        static void AddNewRoom()
        {
            Console.Clear();
            Console.WriteLine("Add New Room");
            Console.Write("Enter Room Number: ");
            int roomNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter Daily Rate: ");
            double rate = double.Parse(Console.ReadLine());

            Console.Write("Enter Room Type (Single, Double, Suite): ");
            string type = Console.ReadLine();

            if (rooms.Any(r => r.RoomNumber == roomNumber))
            {
                Console.WriteLine("Room already exists.");
            }
            else
            {
                Room room = new Room(roomNumber, rate, type);
                rooms.Add(room);
                Console.WriteLine("Room added successfully.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        // View all rooms
        static void ViewAllRooms()
        {
            Console.Clear();
            Console.WriteLine("Available Rooms:");
            if (rooms.Count == 0)
            {
                Console.WriteLine("No rooms available.");
            }
            else
            {
                foreach (var room in rooms)
                {
                    if (!room.IsReserved)
                    {
                        Console.WriteLine($"Room Number: {room.RoomNumber}, Daily Rate: {room.DailyRate}, Status : Not reserved");
                    }
                    else if (room.IsReserved)
                    {
                        // Find the reservation for this room
                        var reservation = reservations.FirstOrDefault(r => r.Room.RoomNumber == room.RoomNumber);
                        if (reservation != null)
                        {
                            Console.WriteLine($"Room Number: {room.RoomNumber}, Daily Rate: {room.DailyRate}, Status: Reserved, Guest Name: {reservation.Guest.Name} , Total cost : {reservation.TotalCost} OMR");

                            //Console.WriteLine($"Room Number: {room.RoomNumber}, Daily Rate: {room.DailyRate}, Status: Reserved , Guest Name : {reservations.");
                        }

                    }
                }
            }

            Console.WriteLine("\nPress any key ...");
            Console.ReadKey();

        }

        // Reserve a room
        static void ReserveRoom()
        {
            Console.Clear();
            Console.WriteLine("Reserve a Room");

            Console.Write("Enter Guest Name: ");
            string guestName = Console.ReadLine();

            Console.Write("Enter Contact Number: ");
            string contact = Console.ReadLine();

            Console.Write("Enter Room Number to Reserve: ");
            int roomNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter Check-In Date (yyyy-MM-dd): ");
            DateTime checkIn = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Check-Out Date (yyyy-MM-dd): ");
            DateTime checkOut = DateTime.Parse(Console.ReadLine());

            if (checkOut <= checkIn)
            {
                Console.WriteLine("Check-out must be after check-in.");
                Console.ReadKey();
                return;
            }
            else if (checkIn < DateTime.Now)
            {
                Console.WriteLine("Check-in date cannot be in the past.");
                Console.ReadKey();
                return;
            }
            // Check if the room exists and is available
            if (!rooms.Any(r => r.RoomNumber == roomNumber))
            {
                Console.WriteLine("Room not found.");
                Console.ReadKey();
                return;
            }

            Room room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);


            bool isOverlapping = reservations.Any(r => r.Room.RoomNumber == roomNumber &&
            ((checkIn >= r.CheckIn && checkIn < r.CheckOut) || (checkOut > r.CheckIn && checkOut <= r.CheckOut) || (checkIn <= r.CheckIn && checkOut >= r.CheckOut))
            );

            if (isOverlapping)
            {
                Console.WriteLine("Room is already reserved in the selected date range.");
                return;
            }

            Guest guest = new Guest(guestName, contact);
            Reservation reservation = new Reservation(room, guest, checkIn, checkOut);
            room.IsReserved = true;
            reservations.Add(reservation);

            Console.WriteLine($"Room reserved successfully for {guestName}. Total cost: {reservation.TotalCost} OMR");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();


        }

        // View all reservations
        static void ViewAllReservations()
        {
            Console.Clear();
            Console.WriteLine("All Reservations:");

            if (reservations.Count == 0)
            {
                Console.WriteLine("No reservations available.");
                return;
            }
            foreach (Reservation reservation in reservations)
            {
                Console.WriteLine($"Guest Name: {reservation.Guest.Name}, Room Number: {reservation.Room.RoomNumber}, Check In {reservation.CheckIn},check Out {reservation.CheckOut}, Total Cost: {reservation.TotalCost} OMR");
            }

            Console.WriteLine("Press any key ...");
            Console.ReadKey();

        }

        // Search reservation by guest

        static void SearchReservation()
        {
            Console.Clear();
            Console.WriteLine("Search Reservation by Guest Name");
            Console.WriteLine("Enter Guest Name to Search:");
            string guestName = Console.ReadLine();


            foreach (Reservation res in reservations)
            {
                if (res.Guest.Name == guestName)
                {
                    Console.WriteLine($"Reservation found: Guest Name: {res.Guest.Name}, Room Number: {res.Room.RoomNumber}, Check In  {res.CheckIn} ,check Out  {res.CheckOut}, Total Cost: {res.TotalCost} OMR");

                }
                else
                {
                    Console.WriteLine("No reservation found for this guest.");

                }

            }

            Console.WriteLine("\nPress any key ...");
            Console.ReadKey();
        }


    } // End of Program class


    // Classes for Room , Reservation.

    class Room
    {
        // Properties for Room class
        public int RoomNumber { get; private set; }
        public double DailyRate { get; private set; }
        public string RoomType { get; private set; }
        public bool IsReserved { get; set; }

        // Room Constructor
        public Room(int number, double rate, string roomType)
        {
            RoomNumber = number;
            DailyRate = rate;
            RoomType = roomType;
            IsReserved = false;
        }

       
    }


    
    class Guest
    {
        public string Name { get; private set; }
        public string ContactNumber { get; private set; }

        public Guest(string name, string contact)
        {
            Name = name;
            ContactNumber = contact;
        }
    }

    class Reservation
    {
        

        // Properties for Reservation class
        public Guest Guest { get; private set; }
        public Room Room { get; private set; }
        public DateTime CheckIn { get; private set; }
        public DateTime CheckOut { get; private set; }
        public double TotalCost
        {
            get
            {
                return (CheckOut - CheckIn).Days * Room.DailyRate;
            }
        }
        // Reservation Constructor
        public Reservation(Room room, Guest guest, DateTime checkIn, DateTime checkOut)
        {
            Room = room;
            Guest = guest;
            CheckIn = checkIn;
            CheckOut = checkOut;
        }


    }















} // End of namespace SimpleHotelRoomManagementProject_CSharpProject2_OOP
