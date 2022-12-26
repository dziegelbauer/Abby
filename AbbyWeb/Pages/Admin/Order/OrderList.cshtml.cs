using Abby.DataAccess;
using Abby.DataAccess.Repository.IRepository;
using Abby.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Admin.Order;

[Authorize]
public class OrderListModel : PageModel
{
    public void OnGet(string? status = null)
    {

    }
}