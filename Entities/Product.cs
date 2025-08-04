namespace RuleWay.ProductApi.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public bool IsLive => Category != null && StockQuantity >= Category.MinimumStock;
    }
}
