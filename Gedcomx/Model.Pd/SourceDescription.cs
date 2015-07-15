using System.Collections.Generic;

namespace Model.Pd {
    public class SourceDescription
    {
        public List<Title> titles { get; set; }
        public string resourceType { get; set; }
        public Identifiers identifiers { get; set; }
    }
}