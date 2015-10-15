using System;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.Transactions;
using MagicBus.Example.Web.Domain.Pricing.Commands;
using MagicBus.Example.Web.Domain.Pricing.Events;

namespace MagicBus.Example.Web.Domain.Pricing.Handlers
{
    public class CreateCustomerStatusHandler : IHandler<MakeCustomerStandard>, IHandler<UpgradeCustomer>
    {
	    private readonly IBus bus;

	    public CreateCustomerStatusHandler(IBus bus)
	    {
		    this.bus = bus;
	    }

	    public void Handle(MakeCustomerStandard message)
        {
            CreateCustomerStatusRecordIfDoesNotExist(message.CustomerId);
        }

        public void Handle(UpgradeCustomer message)
        {
            CreateCustomerStatusRecordIfDoesNotExist(message.CustomerId);

            string sql = "UPDATE customerStatus SET Status = @Status WHERE CustomerId = @CustomerId";
            RunQuery(sql, message.CustomerId, "Gold");

			bus.Publish(new CustomerUpgraded { CustomerId = message.CustomerId });
        }

        private void CreateCustomerStatusRecordIfDoesNotExist(Guid customerId)
        {
            if (!CustomerStatusRecordDoesNotExist(customerId)) return;

            var sql = "INSERT INTO customerStatus (CustomerId, Status) VALUES (@CustomerId, @Status)";
            RunQuery(sql, customerId, "Standard");
        }

        private static bool CustomerStatusRecordDoesNotExist(Guid customerId)
        {
            const string sql = "SELECT COUNT(CustomerId) FROM customerStatus WHERE CustomerId = @CustomerId";
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["pricing"].ConnectionString))
                {
                    connection.Open();

                    var command = new SqlCeCommand(sql, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    var result = (int)command.ExecuteScalar();
                    return result <= 0;
                }
            }
        }

        private void RunQuery(string sql, Guid customerId, string preferredStatus)
        {
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["pricing"].ConnectionString))
                {
                    connection.Open();

                    var command = new SqlCeCommand(sql, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@Status", preferredStatus);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
