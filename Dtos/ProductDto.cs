namespace RuleWay.ProductApi.Dtos
{
    public class ProductDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }
    }
}
