using System.Security.Claims;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace AbbyWeb.Pages.Customer.Cart;

[Authorize]
[BindProperties]
public class SummaryModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
    public OrderHeader OrderHeader { get; set; }
    public SummaryModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        OrderHeader = new();
    }
    public void OnGet()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null)
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(
                filter: u => u.ApplicationUserId == claim.Value,
                includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");
            
            ApplicationUser applicationUser =
                _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == claim.Value);

            OrderHeader.PickUpDate = DateTime.Today;
            OrderHeader.PickUpTime = DateTime.Now.AddHours(1);
            OrderHeader.PickUpName = $"{applicationUser.FirstName} {applicationUser.LastName}";
            OrderHeader.PhoneNumber = applicationUser.PhoneNumber;

            foreach (var cartItem in ShoppingCartList)
            {
                OrderHeader.OrderTotal += (cartItem.MenuItem.Price * cartItem.Count);
            }
        }
    }

    public IActionResult OnPost()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null)
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(
                filter: u => u.ApplicationUserId == claim.Value,
                includeProperties: "MenuItem,MenuItem.FoodType,MenuItem.Category");
            
            foreach (var cartItem in ShoppingCartList)
            {
                OrderHeader.OrderTotal += (cartItem.MenuItem.Price * cartItem.Count);
            }

            OrderHeader.Status = SD.StatusPending;
            OrderHeader.OrderDate = DateTime.Now;
            OrderHeader.UserId = claim.Value;
            OrderHeader.PickUpTime = Convert.ToDateTime(
                $"{OrderHeader.PickUpDate.ToShortDateString()} {OrderHeader.PickUpTime.ToShortTimeString()}");
            
            _unitOfWork.OrderHeaderRepository.Add(OrderHeader);
            _unitOfWork.Save();

            foreach (var item in ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    Count = item.Count,
                    MenuItemId = item.MenuItemId,
                    OrderId = OrderHeader.Id,
                    Name = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                };
                _unitOfWork.OrderDetailRepository.Add(orderDetail);
            }
            _unitOfWork.Save();
            
            var domain = "http://localhost:5002";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Mode = "payment",
                SuccessUrl = domain + $"/Customer/Cart/OrderConfirmation?id={OrderHeader.Id}",
                CancelUrl = domain + "/Customer/Cart/Index",
            };

            foreach (var item in ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(item.MenuItem.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = item.MenuItem.Name
                        },
                    },
                    Quantity = item.Count,
                };
                
                options.LineItems.Add(sessionLineItem);
            }
            
            
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);

            OrderHeader.SessionId = session.Id;
            OrderHeader.PaymentIntentId = session.PaymentIntentId;
            _unitOfWork.Save();
            return new StatusCodeResult(303);
        }

        return Page();
    }
}