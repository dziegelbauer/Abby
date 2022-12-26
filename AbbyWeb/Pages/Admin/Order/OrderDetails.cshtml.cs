using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Models.ViewModel;
using Abby.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace AbbyWeb.Pages.Admin.Order;

public class OrderDetails : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    [BindProperty]
    public OrderDetailVM OrderDetailVm { get; set; }

    public OrderDetails(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        OrderDetailVm = new();
    }
    
    public void OnGet(int id)
    {
        OrderDetailVm.OrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(filter: u => u.Id == id, 
            includeProperties: "ApplicationUser");
        OrderDetailVm.OrderDetailsList = _unitOfWork.OrderDetailRepository.GetAll(filter: u => u.OrderId == id, 
            includeProperties: "MenuItem");
    }
    
    public IActionResult OnPostOrderCancel(int id)
    {
        _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusCancelled);
        _unitOfWork.Save();

        return RedirectToPage("OrderList");
    }
    
    public IActionResult OnPostOrderRefund(int id)
    {
        var orderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(u => u.Id == id);
        
        var options = new RefundCreateOptions
        {
            Reason = RefundReasons.RequestedByCustomer,
            PaymentIntent = orderHeader.PaymentIntentId,
        };

        var service = new RefundService();
        Refund _ = service.Create(options);
        
        _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusRefunded);
        _unitOfWork.Save();

        return RedirectToPage("OrderList");
    }
    
    public IActionResult OnPostOrderComplete(int id)
    {
        _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusCompleted);
        _unitOfWork.Save();

        return RedirectToPage("OrderList");
    }
}