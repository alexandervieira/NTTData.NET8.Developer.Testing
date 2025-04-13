namespace Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs
{
    public class OrderProductsList
    {
        public Guid OrderId { get; set; }
        public ICollection<Item> Items { get; set; } = [];
    }

    public class Item
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
