using Newtonsoft.Json;

namespace External.Product.Core.UseCases.Product.GetProducts
{
    public class GetProductsModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Capacity { get; set; }
        [JsonProperty("capacity GB")]
        public int CapacityGB { get; set; }
        [JsonProperty("Case Size")]
        public string CaseSize { get; set; }
        public string Color { get; set; }
        [JsonProperty("CPU model")]
        public string CPUModel { get; set; }
        public string Description { get; set; }
        public string Generation { get; set; }
        [JsonProperty("Hard disk size")]
        public string HardDiskSize { get; set; }
        public double? Price { get; set; }
        [JsonProperty("Screen size")]
        public double? ScreenSize { get; set; }
        [JsonProperty("Strap Colour")]
        public string StrapColour { get; set; }
        public int? Year { get; set; }
    }
}
