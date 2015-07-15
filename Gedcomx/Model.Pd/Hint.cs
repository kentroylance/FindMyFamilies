using System.Collections.Generic;

namespace Model.Pd {
    public class Hint {
        public int results { get; set; }
        public Links links { get; set; }
        public List<Entry> entries { get; set; }
    }
}