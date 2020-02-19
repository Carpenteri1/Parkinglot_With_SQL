using System;
using System.Data.SqlClient;

namespace TentaDatabaser
{
    class Calculate
    {
        private static int cost = 0;
        private const uint fiveMin = 5;
        private const uint sixtyMin = 60;

        public static int Price(VehicleType vehicleType, Vehicle vehicle)
        {
            if (vehicleType == VehicleType.Car)
            {
                if (TimeParked(vehicle).TotalMinutes > fiveMin && TimeParked(vehicle).TotalMinutes < ((sixtyMin * 2) - fiveMin))
                {
                    return cost = 40;

                }

                if (TimeParked(vehicle).TotalMinutes > ((sixtyMin * 2) + fiveMin))
                {
                    int hours = (int)TimeParked(vehicle).TotalHours;
                    return cost = hours * 20;
                }
            }

            if (vehicleType == VehicleType.Bike)
            {
                if (TimeParked(vehicle).TotalMinutes > fiveMin && TimeParked(vehicle).TotalMinutes < ((sixtyMin * 2) - fiveMin))
                {
                    return cost = 20;
                }
                if (TimeParked(vehicle).TotalMinutes > ((sixtyMin * 2) + fiveMin))
                {
                    int hours = (int)TimeParked(vehicle).TotalHours;
                    return cost = hours * 10;

                }
            }
            return cost;
        }

        public static TimeSpan TimeParked(Vehicle vehicle)
        {
            DateTime dateArrived = new DateTime();


            using(SqlConnection connect = Connection.ConnectToDataBase())
            {
                connect.Open();
                using (SqlCommand cmd = new SqlCommand($@"SELECT Booking_Date_Time
                                                FROM Parking_Slot_Reservation
                                                WHERE Regnumber = '{vehicle.Regnumber}'"))
                {
                    cmd.Connection = connect;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            dateArrived = rdr.GetDateTime(rdr.GetOrdinal("Booking_Date_Time"));
                        }
                    }
                }
            }
           
            DateTime dateNow = DateTime.Now;
            TimeSpan diffrence = dateNow - dateArrived;
            diffrence = StripMilliseconds(diffrence);
            return diffrence;
        }

        public static TimeSpan StripMilliseconds(TimeSpan time)
        {
            return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
        }
    }
}
