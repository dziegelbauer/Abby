using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Customer.Home;

public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IEnumerable<MenuItem> MenuItemList { get; set; }
    public IEnumerable<Category> CategoryList { get; set; }

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void OnGet()
    {
        MenuItemList = _unitOfWork.MenuItemRepository.GetAll(includeProperties: "Category,FoodType");
        CategoryList = _unitOfWork.CategoryRepository.GetAll(orderby: u => u.OrderBy(c => c.DisplayOrder));
    }
}