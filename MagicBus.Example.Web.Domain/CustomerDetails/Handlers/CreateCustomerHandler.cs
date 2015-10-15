using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.Transactions;
using MagicBus.Example.Web.Domain.CustomerDetails.Commands;
using MagicBus.Example.Web.Domain.CustomerDetails.Events;

namespace MagicBus.Example.Web.Domain.CustomerDetails.Handlers
{
    public class CreateCustomerHandler : IHandler<CreateCustomer>
    {
        private readonly IBus bus;

        public CreateCustomerHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(CreateCustomer message)
        {
            const string sql = "INSERT INTO customerDetails (CustomerId, FirstName, LastName) VALUES (@CustomerId, @FirstName, @LastName)";

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["customerDetails"].ConnectionString))
                {
                    connection.Open();

                    var command = new SqlCeCommand(sql, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@CustomerId", message.CustomerId);
                    command.Parameters.AddWithValue("@FirstName", message.FirstName);
                    command.Parameters.AddWithValue("@LastName", message.LastName);

                    command.ExecuteNonQuery();
                }
            }
            bus.Publish(new CustomerCreated { CustomerId = message.CustomerId });
        }
    }
}