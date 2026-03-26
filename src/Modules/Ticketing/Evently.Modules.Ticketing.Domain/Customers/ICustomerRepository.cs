namespace Evently.Modules.Ticketing.Domain.Customers;

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(Guid customerId, CancellationToken cancellationToken = default);

    void Insert(Customer customer);
}
