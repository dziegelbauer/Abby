using Abby.DataAccess.Repository.IRepository;
using Abby.Models;

namespace Abby.DataAccess.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _db;
    
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Category category)
    {
        var objFromDb = _db.Categories.FirstOrDefault(u => u.Id == category.Id);
        objFromDb.Name = category.Name;
        objFromDb.DisplayOrder = category.DisplayOrder;
    }
}