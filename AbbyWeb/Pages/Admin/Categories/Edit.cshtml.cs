using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Categories;

[BindProperties]
public class EditModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    public Category Category { get; set; }

    public EditModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet(int id)
    {
        Category = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        if (Category.Name == Category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Category.Name", "The Display Order cannot exactly match the Name");
        }
        
        if (ModelState.IsValid)
        {
            _unitOfWork.CategoryRepository.Update(Category);
            _unitOfWork.Save();
            TempData["success"] = $"Category: {Category.Name} updated successfully";
            return RedirectToPage("/Admin/Categories/Index");
        }

        return Page();
    }
}