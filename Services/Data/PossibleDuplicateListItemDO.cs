namespace FindMyFamilies.Data {

    public class PossibleDuplicateListItemDO {
        public PossibleDuplicateListItemDO() {
        } 

        public PossibleDuplicateListItemDO(string name, string link, string title, string results, string duplicates) {
            this.Name = name;
            this.Link = link;
            this.Title = title;
            this.Results = Results;
            this.Duplicates = duplicates;
        } 
		public string Name { get; set; }
		public string Link { get; set; }
        public string Title { get; set; }
        public string Results { get; set; }
        public string Duplicates { get; set; }
        public int Count {get; set;}
	}
}
