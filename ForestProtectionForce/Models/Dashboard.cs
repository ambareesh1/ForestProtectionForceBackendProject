namespace ForestProtectionForce.Models
{
    public class Dashboard
    {
        public List<BoxModel>? boxModels { get; set; }
        public List<Chart>? charts { get; set; }
        public List<Baseline>? baseline { get; set; }
    }

    public class BoxModel
    {
        public string? Name { get; set; }
        public int? Count { get; set; }
    }

    public class Chart
    {
        public string name { get; set; }
        public string xaxis { get; set; }
        public string yaxis { get; set; }
    }

   
}
