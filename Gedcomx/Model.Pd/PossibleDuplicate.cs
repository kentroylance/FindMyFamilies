using System.Collections.Generic;

namespace Model.Pd {
    public class PossibleDuplicate {
        public string id { get; set; }
        public int results { get; set; }
        public int index { get; set; }
        public Links links { get; set; }
        public string title { get; set; }
        public List<Entry> entries { get; set; }
    }
}