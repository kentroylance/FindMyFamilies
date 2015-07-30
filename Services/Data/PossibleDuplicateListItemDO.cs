namespace FindMyFamilies.Data {

    public class PossibleDuplicateListItemDO : FindListItemDO {
        public PossibleDuplicateListItemDO() {
        } 

        public PossibleDuplicateListItemDO(string link, string title, string results, string duplicates) {
            this.Link = link;
            this.Title = title;
            this.Results = Results;
            this.Duplicates = duplicates;
        } 

        public string Link { get; set; }
        public string Title { get; set; }
        public string Results { get; set; }
        public string Duplicates { get; set; }
        public int Count {get; set;}
	}
}
