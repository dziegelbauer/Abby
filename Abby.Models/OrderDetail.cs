using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Abby.Models;

public class OrderDetail
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public OrderHeader OrderHeader { get; set; }
    
    [Required]
    public int MenuItemId { get; set; }

    [ForeignKey("MenuItemId")]
    public MenuItem MenuItem { get; set; }

    public int Count { get; set; }

    public double Price { get; set; }

    public string Name { get; set; }
}