namespace Abby.Models.ViewModel;

public class OrderDetailVM
{
    public OrderHeader OrderHeader { get; set; }
    public IEnumerable<OrderDetail> OrderDetailsList { get; set; }
}