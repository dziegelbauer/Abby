using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

namespace Abby.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        CategoryRepository = new CategoryRepository(_db);
        FoodTypeRepository = new FoodTypeRepository(_db);
        MenuItemRepository = new MenuItemRepository(_db);
        ShoppingCartRepository = new ShoppingCartRepository(_db);
        OrderHeaderRepository = new OrderHeaderRepository(_db);
        OrderDetailRepository = new OrderDetailRepository(_db);
        ApplicationUserRepository = new ApplicationUserRepository(_db);
    }

    public ICategoryRepository CategoryRepository { get; }
    public IFoodTypeRepository FoodTypeRepository { get; }
    public IMenuItemRepository MenuItemRepository { get; }
    public IShoppingCartRepository ShoppingCartRepository { get; }
    public IOrderHeaderRepository OrderHeaderRepository { get; }
    public IOrderDetailRepository OrderDetailRepository { get; }
    public IApplicationUserRepository ApplicationUserRepository { get; set; }
   
    public void Save()
    {
        _db.SaveChanges();
    }
}