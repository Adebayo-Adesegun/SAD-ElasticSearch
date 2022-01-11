

using Nest;

namespace SAD_ElasticSearch.Core.Models
{
    public class PropertyModel
    {
        public Prop Property { get; set; }
    }

    public class Prop
    {
        public int PropertyID { get; set; }

        [Text(Analyzer = "smart-analyzer", Name = nameof(Name))]
        public string Name { get; set; }

        [Text(Analyzer = "smart-analyzer", Name = nameof(FormerName))]
        public string FormerName { get; set; }

        [Text(Analyzer = "smart-analyzer", Name = nameof(StreetAddress))]
        public string StreetAddress { get; set; }

        [Text(Analyzer = "smart-analyzer", Name = nameof(City))]
        public string City { get; set; }

        [Text(Analyzer = "smart-analyzer", Name = nameof(Market))]
        public string Market { get; set; }

        [Text(Analyzer = "smart-analyzer", Name = nameof(State))]
        public string State { get; set; }

        public float Lat { get; set; }
        public float Lng { get; set; }
    }

}
