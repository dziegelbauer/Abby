using System.ComponentModel.DataAnnotations;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Customer.Home;

public class DetailsModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public MenuItem MenuItem { get; set; }
    [Range(1,100)]
    public int Count { get; set; }

    public DetailsModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet(int id)
    {
        MenuItem = _unitOfWork.MenuItemRepository.GetFirstOrDefault(u => u.Id == id, "Category,FoodType");
    }
}