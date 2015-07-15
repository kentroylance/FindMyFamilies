namespace FindMyFamilies.Data {

    public class DateListItemDO {
        public DateListItemDO(string name) {
            Name = name;
        } 
		public string Name { get; set; }
		public string BirthDate { get; set; }
		public string DeathDate { get; set; }
		public string MarriageDate { get; set; }

	}
}
