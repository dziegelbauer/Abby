using Abby.DataAccess;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    public FoodType FoodType { get; set; }

    public DeleteModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet(int id)
    {
        FoodType = _unitOfWork.FoodTypeRepository.GetFirstOrDefault(u => u.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        _unitOfWork.FoodTypeRepository.Remove(FoodType);
        _unitOfWork.Save();
        TempData["success"] = $"Category: {FoodType.Name} deleted successfully";
        return RedirectToPage("/Admin/FoodTypes/Index");
    }
}