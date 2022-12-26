using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages
{
    public class OrderListModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Customer/Home/Index");
        }
    }
}