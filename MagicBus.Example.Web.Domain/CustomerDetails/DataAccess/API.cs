using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;

namespace MagicBus.Example.Web.Domain.CustomerDetails.DataAccess
{
    public static class API
    {
        public static List<CustomerDetails> GetAllCustomers()
        {
            var sql = "SELECT * FROM customerDetails";
            var customers = new List<CustomerDetails>();

            using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["customerDetails"].ConnectionString))
            {
                var command = new SqlCeCommand(sql, connection);
                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(MapRow(reader));
                }

                reader.Close();
            }
            
            return customers;
        }

        private static CustomerDetails MapRow(IDataRecord reader)
        {
            return new CustomerDetails
                {
                    CustomerId = Guid.Parse(reader[0].ToString()),
                    FirstName = reader[1].ToString(),
                    LastName = reader[2].ToString()
                };
        }
    }
}
