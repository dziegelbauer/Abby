using Abby.DataAccess;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories;

[BindProperties]
public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public Category Category { get; set; }

    public DeleteModel(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public void OnGet(int id)
    {
        Category = _db.Categories.Find(id)!;
    }

    public async Task<IActionResult> OnPost()
    {
        _db.Categories.Remove(Category);
        await _db.SaveChangesAsync();
        TempData["success"] = $"Category: {Category.Name} deleted successfully";
        return RedirectToPage("/Categories/Index");
    }
}