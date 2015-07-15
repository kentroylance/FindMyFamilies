using System.Collections.Generic;

namespace Model.Pd {
    public class Search {
        public int results { get; set; }
        public int index { get; set; }
        public Links links { get; set; }
        public string subtitle { get; set; }
        public string title { get; set; }
        public List<Entry> entries { get; set; }
        public List<SearchInfo> searchInfo { get; set; }
    }
}