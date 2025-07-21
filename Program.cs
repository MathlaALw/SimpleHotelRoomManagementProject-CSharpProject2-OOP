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
                    //case "2": ViewAllRooms(); break;
                    //case "3": ReserveRoom(); break;
                    //case "4": ViewAllReservations(); break;
                    //case "5": SearchReservation(); break;
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
