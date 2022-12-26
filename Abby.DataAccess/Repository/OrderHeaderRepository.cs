using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

namespace Abby.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{
    private readonly ApplicationDbContext _db;
    
    public OrderHeaderRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(OrderHeader orderHeader)
    {
        _db.OrderHeaders.Update(orderHeader);
    }

    public void UpdateStatus(int id, string status)
    {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

        if (orderFromDb != null)
        {
            orderFromDb.Status = status;
        }
    }
}