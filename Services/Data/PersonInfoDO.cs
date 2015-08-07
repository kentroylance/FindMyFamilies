using FindMyFamilies.Data;

namespace FindMyFamiles.Services.Data
{
    public class PersonInfoDO
    {
        public string Name { get; set; }
        public string PersonId { get; set; }
        public string Message { get; set; }
        public PersonDO Person { get; set; }
        public bool IncludeMaidenName { get; set; }
        public bool IncludeMiddleName { get; set; }
        public bool IncludePlace { get; set; }
        public int YearRange { get; set; }
        public int Generation { get; set; }
        public string PersonType { get; set; }
        public string errorMessage { get; set; }
    }
}

