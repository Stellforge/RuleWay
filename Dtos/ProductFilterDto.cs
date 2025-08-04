namespace RuleWay.ProductApi.Dtos
{
    public class ProductFilterDto
    {
        public string? Search { get; set; }

        public int? MinStock { get; set; }

        public int? MaxStock { get; set; }
    }
}
