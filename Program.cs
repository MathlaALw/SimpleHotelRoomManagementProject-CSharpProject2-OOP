namespace SimpleHotelRoomManagementProject_CSharpProject2_OOP
{
    internal class Program
    {

        // List to store rooms and reservations
        static List<Room> rooms = new List<Room>();
        static List<Reservation> reservations = new List<Reservation>();
        // Path to save info in file
        static string roomsFilePath = "rooms.txt";
        static string reservationsFilePath = "reservations.txt";


        static void Main(string[] args)
        {
            
            LoadRoomsFromFile();
            LoadReservationsFromFile();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nHotel Room Management System");
                Console.WriteLine("1. Add New Room");
                Console.WriteLine("2. View All Rooms");
                Console.WriteLine("3. Reserve a Room");
                Console.WriteLine("4. View All Reservations");
                Console.WriteLine("5. Search Reservation by Guest Name");
                Console.WriteLine("6. Find Highest Paying Guest");
                Console.WriteLine("7. Cancel Reservation by Room Number");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddNewRoom(); break;
                    case "2": ViewAllRooms(); break;
                    case "3": ReserveRoom(); break;
                    case "4": ViewAllReservations(); break;
                    case "5": SearchReservation(); break;
                    case "6": FindHighestPayingGuest(); break;
                    case "7": CancelReservation(); break;
                    case "0":
                        //SaveRoomsToFile();
                        //SaveReservationsToFile();
                        Console.WriteLine("Data saved. Exiting...");
                        return; // Exit the system
                    default: Console.WriteLine("Invalid choice. Try again."); break;
                }
            }



        }// End of Main method

        // add a new room
        static void AddNewRoom()
        {
            Console.Clear();
            Console.WriteLine("Add New Room");
            int roomNumber;
            while (true)
            {
                Console.Write("Enter Room Number: ");
                if (int.TryParse(Console.ReadLine(), out roomNumber))
                {
                   
                    if (rooms.Any(r => r.RoomNumber == roomNumber))
                    {
                        Console.WriteLine("Room number already exists. Try a different one.");
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            double dailyRate;
            while (true)
            {
                Console.Write("Enter Daily Rate: ");
                if (double.TryParse(Console.ReadLine(), out dailyRate) && dailyRate > 0)
                    break;
                else
                    Console.WriteLine("Invalid rate. Please enter a positive number.");
            }

            string roomType;
            do
            {
                Console.Write("Enter Room Type (Single, Double, Suite): ");
                roomType = Console.ReadLine();
                roomType = roomType.Trim().ToLower();
                if (roomType != "single" && roomType != "double" && roomType != "suite")
                {
                    Console.WriteLine("Invalid room type. Please enter Single, Double, or Suite.");
                    roomType = string.Empty; // Reset roomType to prompt again
                }
            } while (string.IsNullOrWhiteSpace(roomType));


            Room room = new Room(roomNumber, dailyRate, roomType);
            rooms.Add(room);
            SaveRoomsToFile();
            Console.WriteLine("Room added successfully.");

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

            string guestName;
            do
            {
                Console.Write("Enter Guest Name: ");
                guestName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(guestName))
                {
                    Console.WriteLine("Guest name cannot be empty. Please enter a valid name.");
                }
            } while (string.IsNullOrWhiteSpace(guestName));

            string contact;
            do
            {
                Console.Write("Enter Contact Number: ");
                contact = Console.ReadLine();
                if (contact.Length < 8 || contact.Length > 10)
                {
                    Console.WriteLine("Contact number must be between 10 and 15 digits.");
                    contact = string.Empty; // Reset contact to prompt again
                }
            } while (string.IsNullOrWhiteSpace(contact));

            // Check if there are any rooms available
            int roomNumber;
            Room room = null;
            while (true)
            {
                Console.Write("Enter Room Number to Reserve: ");
                if (int.TryParse(Console.ReadLine(), out roomNumber))
                {
                    room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
                    if (room != null)
                        break;
                }
                Console.WriteLine("Invalid Room Number. Try again.");
            }

            DateTime checkIn, checkOut;

            while (true)
            {
                Console.Write("Enter Check-In Date (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out checkIn))
                    break;
                Console.WriteLine("Invalid date format. Try again.");
            }

            while (true)
            {
                Console.Write("Enter Check-Out Date (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out checkOut))
                {
                    if (checkOut > checkIn)
                        break;
                    else
                        Console.WriteLine("Check-Out must be after Check-In.");
                }
                else
                    Console.WriteLine("Invalid date format. Try again.");
            }

            bool isOverlapping = reservations.Any(r =>
        r.Room.RoomNumber == roomNumber &&
        (
            (checkIn >= r.CheckIn && checkIn < r.CheckOut) ||
            (checkOut > r.CheckIn && checkOut <= r.CheckOut) ||
            (checkIn <= r.CheckIn && checkOut >= r.CheckOut)
        )
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
            SaveReservationsToFile();
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

        // Find the highest paying guest
        static void FindHighestPayingGuest()
        {
            Console.Clear();
            Console.WriteLine("Find Highest Paying Guest");
            if (reservations.Count == 0)
            {
                Console.WriteLine("No reservations available.");
                return;
            }
            var highestPayingReservation = reservations.OrderByDescending(r => r.TotalCost).FirstOrDefault();
            if (highestPayingReservation != null)
            {
                Console.WriteLine($"Highest Paying Guest: {highestPayingReservation.Guest.Name}, Room Number: {highestPayingReservation.Room.RoomNumber}, Total Cost: {highestPayingReservation.TotalCost} OMR");
            }
            else
            {
                Console.WriteLine("No reservations found.");
            }
            Console.WriteLine("\nPress any key ...");
            Console.ReadKey();
        }

        // Cancel reservation by room number
        static void CancelReservation()
        {
            Console.Clear();
            Console.WriteLine("Cancel Reservation by Room Number");
            int roomNumber;
            while (true)
            {
                Console.Write("Enter Room Number: ");
                if (int.TryParse(Console.ReadLine(), out roomNumber))
                    break;
                else
                    Console.WriteLine("Invalid input. Please enter a valid room number.");
            }

            var matchingReservations = reservations
                .Where(r => r.Room.RoomNumber == roomNumber)
                .ToList();

            if (matchingReservations.Count == 0)
            {
                Console.WriteLine("No reservations found for this room.");
                return;
            }

            
            Console.WriteLine("\nMatching Reservations:");
            for (int i = 0; i < matchingReservations.Count; i++)
            {
                var res = matchingReservations[i];
                Console.WriteLine($"{i + 1}. Guest: {res.Guest.Name}, Contact: {res.Guest.ContactNumber}, " +
                                  $"Check-In: {res.CheckIn:yyyy-MM-dd}, Check-Out: {res.CheckOut:yyyy-MM-dd}");
            }

          
            int choice;
            while (true)
            {
                Console.Write("\nEnter the number of the reservation to cancel: ");
                if (int.TryParse(Console.ReadLine(), out choice) &&
                    choice >= 1 && choice <= matchingReservations.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Please try again.");
                }
            }

           
            var selectedReservation = matchingReservations[choice - 1];
            Console.Write($"Are you sure you want to cancel the reservation for {selectedReservation.Guest.Name}? (y/n): ");
            string confirm = Console.ReadLine().ToLower();
            if (confirm == "y")
            {
                reservations.Remove(selectedReservation);
                SaveReservationsToFile(); 
                Console.WriteLine("Reservation cancelled successfully.");
            }
            else
            {
                Console.WriteLine("Cancellation aborted.");
            }
            Console.WriteLine("\nPress any key ...");
            Console.ReadKey();
        }

        // save room info
        static void SaveRoomsToFile()
        {
            using (StreamWriter sw = new StreamWriter(roomsFilePath))
            {
                foreach (var room in rooms)
                {
                    sw.WriteLine($"{room.RoomNumber},{room.DailyRate},{room.RoomType},{room.IsReserved}");
                }
            }
        }
        // save Reservation
        static void SaveReservationsToFile()
        {
            using (StreamWriter sw = new StreamWriter(reservationsFilePath))
            {
                foreach (var res in reservations)
                {
                    sw.WriteLine($"{res.Room.RoomNumber},{res.Guest.Name},{res.Guest.ContactNumber},{res.CheckIn:yyyy-MM-dd},{res.CheckOut:yyyy-MM-dd}");
                }
            }
        }

        // Load info
        static void LoadRoomsFromFile()
        {
            if (File.Exists(roomsFilePath))
            {
                foreach (var line in File.ReadAllLines(roomsFilePath))
                {
                    var parts = line.Split(',');
                    int roomNumber = int.Parse(parts[0]);
                    double rate = double.Parse(parts[1]);
                    string type = parts[2];
                    bool isReserved = bool.Parse(parts[3]);

                    rooms.Add(new Room(roomNumber, rate, type) { IsReserved = isReserved });
                }
            }
        }


        static void LoadReservationsFromFile()
        {
            if (File.Exists(reservationsFilePath))
            {
                foreach (var line in File.ReadAllLines(reservationsFilePath))
                {
                    var parts = line.Split(',');

                    int roomNumber = int.Parse(parts[0]);
                    string guestName = parts[1];
                    string contact = parts[2];
                    DateTime checkIn = DateTime.Parse(parts[3]);
                    DateTime checkOut = DateTime.Parse(parts[4]);

                    Room room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
                    if (room != null)
                    {
                        Guest guest = new Guest(guestName, contact);
                        Reservation res = new Reservation(room, guest, checkIn, checkOut);
                        room.IsReserved = true;
                        reservations.Add(res);
                    }
                }
            }
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
