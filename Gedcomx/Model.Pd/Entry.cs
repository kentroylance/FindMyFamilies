using System.Collections.Generic;

namespace Model.Pd {
    public class Entry {
        public Content content { get; set; }
        public string id { get; set; }
        public double score { get; set; }
        public int confidence { get; set; }
        public Links3 links { get; set; }
        public object published { get; set; }
        public string title { get; set; }
        public List<MatchInfo> matchInfo { get; set; }
    }
}