namespace Abby.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    IFoodTypeRepository FoodTypeRepository { get; }
    IMenuItemRepository MenuItemRepository { get; }
    
    IShoppingCartRepository ShoppingCartRepository { get; }
    void Save();
}