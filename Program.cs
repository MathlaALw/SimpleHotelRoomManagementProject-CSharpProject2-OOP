namespace SimpleHotelRoomManagementProject_CSharpProject2_OOP
{
    internal class Program
    {
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
                    //case "1": AddNewRoom(); break;
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









    } // End of Program class






















} // End of namespace SimpleHotelRoomManagementProject_CSharpProject2_OOP
