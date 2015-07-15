namespace FindMyFamilies.Data {

    public class AnalyzeListItemDO {
        public AnalyzeListItemDO() {
        } 
        public AnalyzeListItemDO(string name, string clue, int helpers) {
            Name = name;
            Clue = Clue;
            Helpers = helpers;
        } 

		public string Name { get; set; }
		public string Clue { get; set; }
        public int Helpers { get; set; }
	}
}
