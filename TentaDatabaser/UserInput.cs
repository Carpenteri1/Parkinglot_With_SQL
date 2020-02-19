using System;
using System.Data.SqlClient;

namespace TentaDatabaser
{
    class UserInput
    {

        public static VehicleType SetVehicleType()
        {
            VehicleType type = VehicleType.Car;
            Console.Write("Is it a car? Y/N:> ");
            string input = Console.ReadLine().ToUpper() ;

            if (input == "Y")
            {
                type = VehicleType.Car;
            }
            else if (input == "N")
            {
                type = VehicleType.Bike;
            }
            else
            {
                throw new Exception("Input is not correct!");
            }
            return type;
        }

        public static string SetRegNumber()
        {
            Console.Write("Enter regnumber:> ");
            string regnr = Console.ReadLine().ToUpper();

            if (!RegNumberExcist(regnr))
            {
                if (string.IsNullOrEmpty(regnr))
                {
                    throw new Exception("Input cant be empty!");
                }
                if (regnr.Length < 3 || regnr.Length > 10)
                {
                    throw new Exception($"Regnumber {regnr} is to short or long");
                }

            }

            return regnr;
        }

        public static int SetSlotPosition(Vehicle tempVehicle, bool wantToBeFull)
        {
            Console.Write($"Enter a position value for {tempVehicle.Regnumber}\n" +
                "Must be between 1 - 100" +
                "\n:> ");
            tempVehicle.SlotPosition = int.Parse(Console.ReadLine());

            if (tempVehicle.SlotPosition > 100 || tempVehicle.SlotPosition < 1)
            {
                throw new Exception($"Input cant be {tempVehicle.SlotPosition}! Input must be between 1 - 100");
            }

            Console.Clear();
            return tempVehicle.SlotPosition;
        }

        public static bool IsPositionFull(int position, int spaceTaken)//spacetaken = how mutch space the vehicle takes, bike takes 1, car takes 2
        {
            int occupideBy;
            string type = string.Empty;

            using (SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using (SqlCommand cmd = new SqlCommand($@"SELECT b.Occupied_By, c.Vehicle_Type, b.Slot_Number
                                                       FROM Parking_Slot_Reservation a
                                                       INNER JOIN
                                                       Parking_Slot b
                                                       ON b.ID = a.Parking_Slot_ID
                                                       INNER JOIN Vehicle c
                                                       ON c.ID = a.Vehicle_ID
                                                       WHERE Slot_Number = {position}", connect))
                {
                    cmd.Connection = connect;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            type = (string)rdr["Vehicle_Type"];
                            occupideBy = (int)rdr["Occupied_By"];
                                if(occupideBy >= spaceTaken)
                                {
                                    return true;
                                }
                            
                          
                        }
                    }
                }
            }
            return false;
        }


        public static bool RegNumberExcist(string regnr)
        {
            bool regNumFound = false;

            using (SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using (SqlCommand cmd = new SqlCommand($@"SELECT Vehicle_ID FROM Parking_Slot_Reservation  
                                                   WHERE Regnumber = '{regnr}'", connect))
                {
                    cmd.Connection = connect;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            regNumFound = true;
                        }
                    }
                }
            }

            return regNumFound;
        }

    }
}
