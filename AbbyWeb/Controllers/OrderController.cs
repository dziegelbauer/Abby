using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Abby.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbbyWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Get(string? status = null)
    {
        var orderList = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser");

        switch (status)
        {
            case "cancelled":
                orderList = orderList.Where(u => u.Status == SD.StatusCancelled || u.Status == SD.StatusRejected);
                break;
            case "completed":
                orderList = orderList.Where(u => u.Status == SD.StatusCompleted);
                break;
            case "ready":
                orderList = orderList.Where(u => u.Status == SD.StatusReady);
                break;
            case "inProcess":
                orderList = orderList.Where(u => u.Status == SD.StatusInProcess);
                break;
        }

        return Json(new { data = orderList });
    }
}