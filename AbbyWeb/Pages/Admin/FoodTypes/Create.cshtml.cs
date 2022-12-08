using Abby.DataAccess;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.FoodTypes;

[BindProperties]
public class CreateModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    public FoodType FoodType { get; set; }

    public CreateModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.FoodTypeRepository.Add(FoodType);
            _unitOfWork.Save();
            TempData["success"] = $"Category: {FoodType.Name} created successfully";
            return RedirectToPage("/Admin/FoodTypes/Index");
        }

        return Page();
    }
}