using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;




namespace TentaDatabaser
{
    class Orgenize
    {
        private const int MaximumStorageCapacity = 2;
        private const int HalfStorageCapacity = 1;
        private const int isFree = 0;
        private static bool wantToBeFull;
        private static bool wantToExcist;
        private static bool wantToRemove;
        private static bool wantToMove;
    
        public static bool OrgenizeMenu(string input)
        {
            switch (input)
            {
                case "1":
                    AddVehicle();
                    return true;
                case "2":
                    RemoveVehicle();
                    return true;
                case "3":
                    MoveVehicle();
                    return true;
                case "4":
                    ShowAllVehicles();
                    return true;
                case "5":
                    SearchByRegNumber();
                    return true;
                case "6":
                    SearchBySlotPosition();
                    return true;
                case "7":
                    ShowHistoryData();
                    return true;
                case "0":
                    Console.WriteLine("Shutting down....");
                    Console.ReadKey();
                return false;
                default:
                    Console.WriteLine("wrong input!");
                    return true;


            }
        }






        public static void AddVehicle()
        {
            Vehicle createdVehicle = new Vehicle();
            try
            {
                
                createdVehicle = CreateNewVehicleObject();
                HandleVehicleType(createdVehicle, wantToRemove = false, wantToMove = false);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadKey();
            }

        }


        public static void RemoveVehicle()
        {
          
            Vehicle tempVehicle = new Vehicle();
            try
            {
                tempVehicle.Regnumber = UserInput.SetRegNumber();
                tempVehicle = FindVehicle(tempVehicle);
                HandleVehicleType(tempVehicle, wantToRemove = true,wantToMove = false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }


        public static void MoveVehicle()
        {
            Vehicle grabVehicleDataForMove = new Vehicle();
            try
            {
                 grabVehicleDataForMove.Regnumber = UserInput.SetRegNumber();
                 grabVehicleDataForMove = FindVehicle(grabVehicleDataForMove);
                 HandleVehicleType(grabVehicleDataForMove, wantToRemove = false, wantToMove = true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadKey();
            }

        }




        public static void SearchByRegNumber()
        {

            Vehicle findVehicle = new Vehicle();
            List<Vehicle> ListOfVehicles = new List<Vehicle>();
            string regnr = string.Empty;
            try
            {
                findVehicle.Regnumber = UserInput.SetRegNumber();
                findVehicle = FindVehicle(findVehicle);
                Console.WriteLine(findVehicle.ToString());
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }


        private static void GrabSlotData(int position)
        {
            string regnr = string.Empty;
            string vehicleType = string.Empty;
            string occupied_By = string.Empty;

            using (SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();

                Console.WriteLine($"Slot number {position} Info:\n********************************\n");
            using (SqlCommand cmd = new SqlCommand($@"SELECT a.Slot_Number, b.Regnumber,c.Vehicle_Type,a.Occupied_By
                                                    FROM Parking_Slot a
                                                    INNER JOIN Parking_Slot_Reservation b
                                                    ON a.ID = b.Parking_Slot_ID
                                                    INNER JOIN Vehicle c
                                                    ON b.Vehicle_ID = c.ID
                                                    WHERE Slot_Number = {position}"))
            {
                cmd.Connection = connect;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        vehicleType = (string)rdr["Vehicle_Type"];
                        regnr = (string)rdr["Regnumber"].ToString();
                        occupied_By = (string)rdr["Occupied_By"].ToString();
                        Console.WriteLine($"Regnumber: {regnr}\nType: {vehicleType}");
                    }

                }

            }
            if (Convert.ToInt32(occupied_By) == 1)
            {
                Console.WriteLine($"\nSlot number: {position}\nPosition: IsFree\n" +
              "\n********************************");
            }
            else
            {
                Console.WriteLine($"\nSlot number: {position}\nPosition: IsFull\n" +
                "\n********************************");
            }
            }
        }



        public static void SearchBySlotPosition()
        {
            Vehicle tempVehice = new Vehicle();
            try
            {
                tempVehice.SlotPosition = UserInput.SetSlotPosition(tempVehice, wantToBeFull = true);
                if (UserInput.IsPositionFull(tempVehice.SlotPosition, 1))
                {
                     GrabSlotData(tempVehice.SlotPosition);   
                }
                else
                {
                    throw new Exception($"Slot number {tempVehice.SlotPosition} is Free!");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }


        public static void ShowAllVehicles()
        {
            List<Vehicle> ListOfVehicles = new List<Vehicle>();
            Vehicle madeVehicleFromSQLData;
                ListOfVehicles = GrabVehicleDataFromSQL();
                    foreach (var s in ListOfVehicles)
                    {
                        Console.WriteLine($"Slot Number: {s.SlotPosition} Regnumber: {s.Regnumber} Type: {s.Type}");
                    }
                

                Console.ReadKey();
        }


        private static Vehicle FindVehicle(Vehicle vehicle)
        {
            List<Vehicle> listOfVehicles = new List<Vehicle>();
            if (UserInput.RegNumberExcist(vehicle.Regnumber))//does it excist in the database
            {
                listOfVehicles = GrabVehicleDataFromSQL();//grab all vehicles from database

                foreach (var s in listOfVehicles)
                {
                    if(s.Regnumber == vehicle.Regnumber)
                    {
                        vehicle.Regnumber = s.Regnumber;
                        vehicle.Size = s.Size;
                        vehicle.SlotPosition = s.SlotPosition;
                        vehicle.TimeArrived = s.TimeArrived;
                        vehicle.Type = s.Type;
                        vehicle.TimeParked = s.TimeParked;
                        vehicle.Cost = s.Cost;
                        return vehicle;
                    }
                }
                return vehicle;
            }
            else
            {
                throw new Exception($"{vehicle.Regnumber} dont excist!");
            }
        }

        private static string RemoveDataFromDataBase(Vehicle vehicle,int size)
        {
            using (SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                    using (SqlCommand cmd = new SqlCommand($@"DELETE FROM Parking_Slot_Reservation
                                                        WHERE Regnumber = '{vehicle.Regnumber}' UPDATE Parking_Slot SET Occupied_By = {size} WHERE Slot_Number = {vehicle.SlotPosition}", connect))
                    {
                        cmd.Connection = connect;
                        cmd.ExecuteNonQuery();
                        return $"\n{vehicle.Regnumber} is now deleted from its reservation on slotnumber {vehicle.SlotPosition} and added to history table";
                    }
                
            }
        }


        private static void StoreToHistory(Vehicle vehicle)
        {
            using(SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using (SqlCommand cmd = new SqlCommand($@"DECLARE @parked DATETIME
                                                          SET @parked = GETDATE() - @TimeArrived
                                                    INSERT INTO Parking_Slip_History_Table(Regnumber,Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost)
                                                    VALUES(@regnr,@type,@TimeArrived,@parked,GETDATE(),@Price)", connect))
                {
                    cmd.Parameters.AddWithValue("@regnr", vehicle.Regnumber);
                    cmd.Parameters.AddWithValue("@type", vehicle.Type.ToString());
                    cmd.Parameters.AddWithValue("@TimeArrived", vehicle.TimeArrived);
                    cmd.Parameters.AddWithValue("@TimeParked", @"DATEDIFF(DAY,@TimeArrived,@TimeExit)");
                    cmd.Parameters.AddWithValue("@TimeExit", @"GETDATE()");
                    cmd.Parameters.AddWithValue("@Price",Calculate.Price(vehicle.Type,vehicle));
                    cmd.Connection = connect;
                    cmd.ExecuteNonQuery();
                }

            }
        }


        private static Vehicle CreateNewVehicleObject()
        {
            Vehicle createNewVehicle = new Vehicle();
            try
            {
                createNewVehicle.Regnumber = UserInput.SetRegNumber();
                if (UserInput.RegNumberExcist(createNewVehicle.Regnumber))
                {
                    throw new Exception($"{createNewVehicle.Regnumber} already excist!");
                }
                createNewVehicle.Type = UserInput.SetVehicleType();
                createNewVehicle.SlotPosition = UserInput.SetSlotPosition(createNewVehicle,wantToBeFull = false);
                createNewVehicle.TimeArrived = DateTime.Now;

                if (createNewVehicle.Type == VehicleType.Car)
                    createNewVehicle.Size = 2;
                else
                    createNewVehicle.Size = 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
             
            }
            return createNewVehicle;
        }
        private static void HandleVehicleType(Vehicle vehicle, bool wantToRemove,bool wantToMove)
        {
            try
            {
                if (vehicle.Type == VehicleType.Car)
                {
                    Console.WriteLine(HandleTypeCar(vehicle, wantToRemove, wantToMove));
                }
                else
                {
                    Console.WriteLine(HandleTypeBike(vehicle, wantToRemove, wantToMove));
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
           
        }


        private static string HandleTypeCar(Vehicle vehicle,bool wantToRemove,bool wantToMove)
        {
           

                if (wantToRemove)
                {
                    StoreToHistory(vehicle);
                    Console.WriteLine(vehicle.ToString() + "\n\nAbout to be removed!");
                    Console.ReadKey();
                    return RemoveDataFromDataBase(vehicle,isFree);
                
                }
                if (wantToMove)
                {
                    int oldpos = vehicle.SlotPosition;
                    vehicle.SlotPosition = UserInput.SetSlotPosition(vehicle, wantToBeFull = false);
                    if (!UserInput.IsPositionFull(vehicle.SlotPosition, HalfStorageCapacity))
                    {
                       return MakeMove(vehicle, MaximumStorageCapacity, oldpos, isFree);
                    }   
                    else
                    {
                        throw new Exception($"Slot number {vehicle.SlotPosition} is already full!");
                    }
                   

                }

                else if(!wantToRemove && !wantToMove)
                {
                    if (!UserInput.IsPositionFull(vehicle.SlotPosition,HalfStorageCapacity))
                    {
                        return AddObjectToDataBase(vehicle,MaximumStorageCapacity);
                    }
                    else
                    {
                        throw new Exception($"Could not add {vehicle.Regnumber} to slotnumber {vehicle.SlotPosition}! Position is already full");
                    }
                }

            return "";
        }

        private static string HandleTypeBike(Vehicle vehicle,bool wantToRemove,bool wantToMove)
        
        {
            string output = string.Empty;
            if (wantToRemove)
            {
                if (UserInput.IsPositionFull(vehicle.SlotPosition, MaximumStorageCapacity))
                {
                    StoreToHistory(vehicle);
                    Console.WriteLine(vehicle.ToString() + "\n\nAbout to be removed!");
                    Console.ReadKey();
                    return RemoveDataFromDataBase(vehicle,HalfStorageCapacity);
                }
                if(UserInput.IsPositionFull(vehicle.SlotPosition,HalfStorageCapacity))
                {
                    StoreToHistory(vehicle);
                    Console.WriteLine(vehicle.ToString() + "\n\nAbout to be removed!");
                    Console.ReadKey();
                    return RemoveDataFromDataBase(vehicle, isFree);
                }
             

            }
            if (wantToMove)
            {
                int oldpos = vehicle.SlotPosition;
                vehicle.SlotPosition = UserInput.SetSlotPosition(vehicle, wantToBeFull = false);

                if (!UserInput.IsPositionFull(vehicle.SlotPosition, MaximumStorageCapacity))
                {
                    if (UserInput.IsPositionFull(vehicle.SlotPosition, HalfStorageCapacity))
                    {
                        if(UserInput.IsPositionFull(oldpos, MaximumStorageCapacity))
                        {
                            return MakeMove(vehicle, MaximumStorageCapacity, oldpos, HalfStorageCapacity);
                        }
                        if (UserInput.IsPositionFull(oldpos, HalfStorageCapacity))
                        {
                            return MakeMove(vehicle, MaximumStorageCapacity, oldpos, isFree);
                        }
                    }
                    else
                    {
                        if (UserInput.IsPositionFull(oldpos, MaximumStorageCapacity))
                        {
                            return MakeMove(vehicle, HalfStorageCapacity, oldpos, HalfStorageCapacity);
                        }
                        if (UserInput.IsPositionFull(oldpos, HalfStorageCapacity))
                        {
                            return MakeMove(vehicle, HalfStorageCapacity, oldpos, isFree);
                        }
                    }
                }
                else
                {
                    throw new Exception($"{vehicle.SlotPosition} is already full!");
                }
            }

            else
            {
                if (!UserInput.IsPositionFull(vehicle.SlotPosition, MaximumStorageCapacity))

                {
                    if (UserInput.IsPositionFull(vehicle.SlotPosition, HalfStorageCapacity))
                    {
                        return AddObjectToDataBase(vehicle,MaximumStorageCapacity);
                    }
                    else
                    {
                        return AddObjectToDataBase(vehicle,HalfStorageCapacity);
                    }

                }
                else
                {
                    throw new Exception($"Could not add {vehicle.Regnumber} to slotnumber {vehicle.SlotPosition}! Position is already full");
                }
            }

            return "";
        }

        private static string MakeMove(Vehicle vehicle,int size,int oldpos,int oldOccupied)
        {
            using (SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using (SqlCommand cmd = new SqlCommand($@"UPDATE Parking_Slot SET Occupied_By = {size} WHERE Slot_Number = {vehicle.SlotPosition}
                                                          UPDATE Parking_Slot SET Occupied_By = {oldOccupied} WHERE Slot_Number = {oldpos}

                                                         UPDATE Parking_Slot_Reservation
                                                         SET Parking_Slot_ID = (SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = {vehicle.SlotPosition})
                                                         WHERE Regnumber = '{vehicle.Regnumber}'", connect))
                {
                    cmd.Connection = connect;
                    cmd.ExecuteNonQuery();
                }

                return $"Vehicle {vehicle.Regnumber} moved to {vehicle.SlotPosition} from {oldpos}";
            }
        }


        private static string AddObjectToDataBase(Vehicle createdVehicle,int size)
        {
            using (SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using(SqlCommand cmd = new SqlCommand($@"
                                                        INSERT INTO Vehicle(Vehicle_Type, Registration)
                                                        VALUES(@type,GETDATE())
                                                        INSERT INTO Parking_Slot_Reservation(Vehicle_ID, Parking_Slot_ID, Regnumber, Booking_DATE_TIME, Cost)
                                                        VALUES((SELECT Vehicle.ID FROM Vehicle WHERE Vehicle.ID = (SELECT MAX(ID)FROM Vehicle)),
                                                        (SELECT Parking_Slot.ID FROM Parking_Slot WHERE Slot_Number = @slot_number),
                                                        @regnr,GETDATE(),{0})

                                                        UPDATE Parking_Slot
                                                        SET Occupied_By = @size
                                                        WHERE Slot_Number = @slot_number",connect))
                {
                    cmd.Parameters.AddWithValue("@slot_number", createdVehicle.SlotPosition);
                    cmd.Parameters.AddWithValue("@regnr", createdVehicle.Regnumber);
                    cmd.Parameters.AddWithValue("@type", createdVehicle.Type.ToString());
                    cmd.Parameters.AddWithValue("@Size", size);
                    cmd.Connection = connect; 
                    cmd.ExecuteNonQuery();
                }

                return createdVehicle.Regnumber + " is added to slot " + createdVehicle.SlotPosition;
            }
        }

        private static string ReadData(SqlConnection cnn, string data, string regnr)//reads data from the sql server
        {
            string grabData = "";
            using (SqlCommand cmd = new SqlCommand($@"SELECT b.Slot_Number,a.Regnumber,c.Vehicle_Type,a.Booking_Date_Time,b.Occupied_By,a.Vehicle_ID,Parking_Slot_ID
              FROM Parking_Slot_Reservation a
              INNER JOIN Parking_Slot b
              ON a.Parking_Slot_ID = b.ID
              INNER JOIN Vehicle c
              ON a.Vehicle_ID = c.ID
              WHERE a.Regnumber = '{regnr}'", cnn))
            {
                cmd.Connection = cnn;

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        grabData = rdr[data].ToString();
                        Console.Clear();
                        break;
                    }

                }
                return grabData;
            }
        }


        public static void ShowHistoryData()
        {
            using (SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using(SqlCommand cmd = new SqlCommand(@"SELECT ID, Regnumber, Vehicle_Type,Entry_DateTime,Time_Parked,Exit_DateTime,Cost  FROM Parking_Slip_History_Table"))
                {
                    cmd.Connection = connect;
                    using(SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Console.WriteLine($"ID: {rdr.GetInt32(0)}  {rdr.GetString(1)}  {rdr.GetString(2)}  {rdr.GetDateTime(3)}  {rdr.GetTimeSpan(4)}  {rdr.GetDateTime(5)}  {rdr.GetSqlMoney(6)}");
                        }
                    }
                    Console.ReadKey();
                }
            }
        }


        private static List<Vehicle> GrabVehicleDataFromSQL()
        {

            string type = string.Empty;

            Vehicle grabVehicleData;
            List<Vehicle> ListOfVehicles = new List<Vehicle>();


            using(SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using (SqlCommand cmd = new SqlCommand($@"SELECT a.Regnumber,b.Slot_Number, a.Booking_DATE_TIME,c.Vehicle_Type
                                                   FROM Parking_Slot_Reservation a INNER JOIN Parking_Slot b ON b.ID = a.Parking_Slot_ID 
                                                   INNER JOIN Vehicle c ON a.Vehicle_ID = c.ID"))
                {
                    cmd.Connection = connect;
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {


                        while (rdr.Read())
                        {
                            grabVehicleData = new Vehicle();
                            grabVehicleData.Regnumber = rdr.GetString(0);
                            grabVehicleData.SlotPosition = rdr.GetInt32(1);
                            grabVehicleData.TimeArrived = rdr.GetDateTime(2);
                            type = rdr.GetString(3);
                            if (type == VehicleType.Car.ToString())
                                grabVehicleData.Type = VehicleType.Car;
                            else
                                grabVehicleData.Type = VehicleType.Bike;

                            ListOfVehicles.Add(grabVehicleData);
                        }

                    }
                    return ListOfVehicles;
                }
            }
         
        }
    }
}
