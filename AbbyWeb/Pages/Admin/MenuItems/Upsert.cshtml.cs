using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AbbyWeb.Pages.Admin.MenuItems;

[BindProperties]
public class UpsertModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public MenuItem MenuItem { get; set; }
    public IEnumerable<SelectListItem> CategoryList { get; set; }
    public IEnumerable<SelectListItem> FoodTypeList { get; set; }

    public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        MenuItem = new MenuItem();
    }
    
    public void OnGet(int? id)
    {
        CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(i => new SelectListItem()
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });
        
        FoodTypeList = _unitOfWork.FoodTypeRepository.GetAll().Select(i => new SelectListItem()
        {
            Text = i.Name,
            Value = i.Id.ToString()
        });

        if (id != null)
        {
            MenuItem = _unitOfWork.MenuItemRepository.GetFirstOrDefault(u => u.Id == id);
        }
    }

    public async Task<IActionResult> OnPost()
    {
        var webRootPath = _webHostEnvironment.WebRootPath;
        var files = HttpContext.Request.Form.Files;

        if (MenuItem.Id == 0) // Create
        {
            if (files.Any())
            {
                string imageFileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\menuitems");
                var extension = Path.GetExtension(files[0].FileName);

                await using (var fileStream =
                       new FileStream(Path.Combine(uploads, imageFileName + extension), FileMode.Create))
                {
                    await files[0].CopyToAsync(fileStream);
                }
                
                MenuItem.Image = @"\images\menuitems\" + imageFileName + extension;
            }

            _unitOfWork.MenuItemRepository.Add(MenuItem);
            _unitOfWork.Save();
        }
        else // Update
        {
            if (files.Any())
            {
                var objFromDb = _unitOfWork.MenuItemRepository.GetFirstOrDefault(u => u.Id == MenuItem.Id);
                
                string imageFileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\menuitems");
                var extension = Path.GetExtension(files[0].FileName);

                await using (var fileStream = new FileStream(Path.Combine(uploads, imageFileName + extension), FileMode.Create))
                {
                    await files[0].CopyToAsync(fileStream);
                }

                System.IO.File.Delete(webRootPath + objFromDb.Image);
                
                MenuItem.Image = @"\images\menuitems\" + imageFileName + extension;
            }
            
            _unitOfWork.MenuItemRepository.Update(MenuItem);
            _unitOfWork.Save();
        }
        
        return RedirectToPage("/Admin/MenuItems/Index");
    }
}