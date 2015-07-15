using System.Collections.Generic;

namespace Model.Pd {
    public class Gedcomx {
        public List<Person> persons { get; set; }
        public List<Relationship> relationships { get; set; }
        public List<SourceDescription> sourceDescriptions { get; set; }
        public string type { get; set; }
    }
}