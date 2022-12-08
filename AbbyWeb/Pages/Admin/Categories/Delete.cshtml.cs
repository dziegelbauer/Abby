using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Categories;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    public Category Category { get; set; }

    public DeleteModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet(int id)
    {
        Category = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);
    }

    public async Task<IActionResult> OnPost()
    {
        _unitOfWork.CategoryRepository.Remove(Category);
        _unitOfWork.Save();
        TempData["success"] = $"Category: {Category.Name} deleted successfully";
        return RedirectToPage("/Admin/Categories/Index");
    }
}