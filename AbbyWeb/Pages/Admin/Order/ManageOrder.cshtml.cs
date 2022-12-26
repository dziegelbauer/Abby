using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Models.ViewModel;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Order;

[Authorize(Roles = $"{SD.ManagerRole},{SD.KitchenRole}")]
public class ManageOrderModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public List<OrderDetailVM> OrderDetailVms { get; set; }

    public ManageOrderModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public void OnGet()
    {
        OrderDetailVms = new();

        IEnumerable<OrderHeader> orderHeaders =
            _unitOfWork.OrderHeaderRepository.GetAll(u =>
                u.Status == SD.StatusSubmitted || u.Status == SD.StatusInProcess);

        foreach (var order in orderHeaders)
        {
            OrderDetailVms.Add(new OrderDetailVM()
            {
                OrderHeader = order,
                OrderDetailsList = _unitOfWork.OrderDetailRepository.GetAll(u => u.OrderId == order.Id),
            });
        }

        
    }

    public IActionResult OnPostOrderInProcess(int id)
    {
        _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusInProcess);
        _unitOfWork.Save();

        return RedirectToPage("ManageOrder");
    }
    
    public IActionResult OnPostOrderReady(int id)
    {
        _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusReady);
        _unitOfWork.Save();

        return RedirectToPage("ManageOrder");
    }
    
    public IActionResult OnPostOrderCancel(int id)
    {
        _unitOfWork.OrderHeaderRepository.UpdateStatus(id, SD.StatusCancelled);
        _unitOfWork.Save();

        return RedirectToPage("ManageOrder");
    }
}