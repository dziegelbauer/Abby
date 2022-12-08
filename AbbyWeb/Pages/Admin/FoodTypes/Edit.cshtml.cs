using Abby.DataAccess;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes;

[BindProperties]
public class EditModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    public FoodType FoodType { get; set; }

    public EditModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet(int id)
    {
        FoodType = _unitOfWork.FoodTypeRepository.GetFirstOrDefault(u => u.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.FoodTypeRepository.Update(FoodType);
            _unitOfWork.Save();
            TempData["success"] = $"Category: {FoodType.Name} updated successfully";
            return RedirectToPage("/Admin/FoodTypes/Index");
        }

        return Page();
    }
}