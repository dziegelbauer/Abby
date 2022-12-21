using System.Security.Claims;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Customer.Cart;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    
    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
    public double CartTotal { get; set; }

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        CartTotal = 0;
    }
    
    public void OnGet()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null)
        {
            ShoppingCartList =
                _unitOfWork.ShoppingCartRepository.GetAll(filter: u => u.ApplicationUserId == claim.Value, 
                    includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");

            foreach (var cartItem in ShoppingCartList)
            {
                CartTotal += (cartItem.MenuItem.Price * cartItem.Count);
            }
        }
    }

    public IActionResult OnPostPlus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);

        return RedirectToPage("/Customer/Cart/Index");
    }
    
    public IActionResult OnPostMinus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);

        if (cart.Count > 1)
        {
            _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);
        }
        else
        {
            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();
        }

        return RedirectToPage("/Customer/Cart/Index");
    }
    
    public IActionResult OnPostRemove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCartRepository.Remove(cart);
        _unitOfWork.Save();

        return RedirectToPage("/Customer/Cart/Index");
    }
}