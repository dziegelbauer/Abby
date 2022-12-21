using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Customer.Cart;

[Authorize]
public class SummaryModel : PageModel
{
    public void OnGet()
    {
        
    }
}