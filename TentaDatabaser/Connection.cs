using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace TentaDatabaser
{
    class Connection
    {//
        private const string connectionString = "Data source=(local);Initial Catalog=ParkingLotDatabase;Integrated Security=SSPI";

        public static SqlConnection ConnectToDataBase()
        {
            return new SqlConnection(connectionString);
        }
    }
}
