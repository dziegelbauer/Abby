using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Abby.Models;

public class ShoppingCart
{
    public ShoppingCart()
    {
        Count = 1;
    }
    public int Id { get; set; }
    
    public int MenuItemId { get; set; }
    
    [ForeignKey("MenuItemId")]
    [ValidateNever]
    public MenuItem MenuItem { get; set; }
    
    [Range(1,100)] 
    public int Count { get; set; }
    
    public string ApplicationUserId { get; set; }
    
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}