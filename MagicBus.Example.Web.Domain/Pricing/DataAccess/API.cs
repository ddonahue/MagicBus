using System;
using System.Configuration;
using System.Data.SqlServerCe;

namespace MagicBus.Example.Web.Domain.Pricing.DataAccess
{
    public static class API
    {
        public static string GetCustomerStatus(Guid customerId)
        {
            var sql = "SELECT Status FROM customerStatus WHERE CustomerId = @CustomerId";

            using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["pricing"].ConnectionString))
            {
                connection.Open();

                var command = new SqlCeCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);
                var status = command.ExecuteScalar();

                return status != null ? status.ToString() : "(New Customer)";
            }
        }
    }
}
