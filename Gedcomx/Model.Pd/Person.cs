using System.Collections.Generic;

namespace Model.Pd {
    public class Person {
        public string id { get; set; }
        public Links2 links { get; set; }
        public bool living { get; set; }
        public Gender gender { get; set; }
        public List<Name> names { get; set; }
        public List<Fact> facts { get; set; }
        public Display display { get; set; }
        public string href { get; set; }
    }
}