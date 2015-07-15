namespace FindMyFamilies.Data {

    public class PlaceListItemDO {
        public PlaceListItemDO(string name) {
            Name = name;
        } 
		public string Name { get; set; }
		public string BirthPlace { get; set; }
		public string DeathPlace { get; set; }
		public string MarriagePlace { get; set; }

	}
}
