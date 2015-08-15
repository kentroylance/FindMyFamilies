namespace FindMyFamilies.Data {

    public class FindListItemDO {
		public string id { get; set; }
		public string firstName { get; set; }
		public string middleName { get; set; }
		public string lastName { get; set; }
		public string fullName { get; set; }
		public string gender { get; set; }
		public string birthYear { get; set; }
		public string deathYear { get; set; }
		public string birthPlace { get; set; }
		public string deathPlace { get; set; }
		public string state { get; set; }
		public string motherName { get; set; }
        public string fatherName { get; set; }
        public string spouseName { get; set; }
        public string spouseGender { get; set; }
        public string errorMessage { get; set; }
	}
}
