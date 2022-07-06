namespace ConsumingAPI.models
{
    public class CountryViewModel
    {
        public Name name{ get; set; }
        
        public string Region { get; set; }

        public string Flag { get; set; }

        public List<string> borders { get; set; }


    }

    public class Name 
    {
        public string common { get; set; }

        public string official { get; set; }

    }


}