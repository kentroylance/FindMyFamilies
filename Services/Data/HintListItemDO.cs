namespace FindMyFamilies.Data {

    public class HintListItemDO {
        public HintListItemDO() {
        } 

        public HintListItemDO(string name, string hints) {
            this.Name = name;
            this.Hints = hints;
        } 
		public string Name { get; set; }
        public string Hints { get; set; }
        public int Count { get; set; }
        public double TopScore { get; set; }
	}
}
