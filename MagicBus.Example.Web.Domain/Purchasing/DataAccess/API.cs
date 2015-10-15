using System;
using System.Configuration;
using System.Data.SqlServerCe;

namespace MagicBus.Example.Web.Domain.Purchasing.DataAccess
{
    public static class API
    {
        public static string GetPurchases(Guid customerId)
        {
            var sql = "SELECT COUNT(PurchaseId) FROM Purchases WHERE CustomerId = @CustomerId";

            using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["purchasing"].ConnectionString))
            {
                connection.Open();

                var command = new SqlCeCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);
                var numberOfPurchases = command.ExecuteScalar();

                return numberOfPurchases != null ? numberOfPurchases.ToString() : "0";
            }
        }
    }
}
