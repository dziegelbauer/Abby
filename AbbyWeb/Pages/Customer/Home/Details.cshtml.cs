using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Customer.Home;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    [BindProperty]
    public ShoppingCart ShoppingCart { get; set; }

    public DetailsModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet(int id)
    {
        ShoppingCart = new()
        {
            MenuItem = _unitOfWork.MenuItemRepository.GetFirstOrDefault(u => u.Id == id, "Category,FoodType"),
            MenuItemId = id,
        };
        
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
        ShoppingCart.ApplicationUserId = claim!.Value;

    }
    
    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            ShoppingCart? shoppingCartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(
                    u => u.ApplicationUserId == ShoppingCart.ApplicationUserId &&
                     u.MenuItemId == ShoppingCart.MenuItemId);

            if (shoppingCartFromDb == null)
            {
                _unitOfWork.ShoppingCartRepository.Add(ShoppingCart);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.IncrementCount(ShoppingCart, ShoppingCart.Count);
            }
            
            return RedirectToPage("Index");
        }
        else
        {
            return Page();
        }
    }
}