using Abby.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AbbyWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuItemController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MenuItemController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var menuItemList = _unitOfWork.MenuItemRepository.GetAll("FoodType,Category");
        return Json(new { data = menuItemList });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var objFromDb = _unitOfWork.MenuItemRepository.GetFirstOrDefault(u => u.Id == id);

        var webRootPath = _webHostEnvironment.WebRootPath;
        System.IO.File.Delete(webRootPath + objFromDb.Image);
        
        _unitOfWork.MenuItemRepository.Remove(objFromDb);
        _unitOfWork.Save();
        
        return Json(new
        {
            success = true,
            message = "Delete successful"
        });
    }
}