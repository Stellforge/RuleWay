namespace RuleWay.ProductApi.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int MinimumStock { get; set; }

        public ICollection<Product>? Products { get; set; }

    }
}
