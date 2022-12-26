﻿using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace AbbyWeb.Pages.Customer.Cart;

public class OrderConfirmationModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty]
    public int OrderId { get; set; }

    public OrderConfirmationModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet(int id)
    {
        OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(u => u.Id == id);
        if (orderHeader.SessionId != null)
        {
            var service = new SessionService();

            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderHeader.Status = SD.StatusSubmitted;
                _unitOfWork.Save();
            }
        }

        List<ShoppingCart> shoppingCarts =
            _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == orderHeader.UserId).ToList();
        _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
        _unitOfWork.Save();

        OrderId = id;
    }
}