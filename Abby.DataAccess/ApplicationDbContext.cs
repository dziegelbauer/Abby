using Abby.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Abby.DataAccess;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<FoodType> FoodTypes { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
}