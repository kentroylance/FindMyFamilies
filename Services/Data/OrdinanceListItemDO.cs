namespace FindMyFamilies.Data {

    public class OrdinanceListItemDO : FindListItemDO {
        public OrdinanceListItemDO() {
        } 

        public OrdinanceListItemDO(string baptism, string confirmation, string initiatory, string endowment, string sealedToParent, string sealedToSpouse) {
            this.Baptism = baptism;
            this.Confirmation = confirmation;
            this.Initiatory = Initiatory;
            this.Endowment = endowment;
            this.SealedToParent = sealedToParent;
            this.SealedToSpouse = sealedToSpouse;
        } 
		public string Baptism { get; set; }
        public string Confirmation { get; set; }
        public string Initiatory { get; set; }
        public string Endowment { get; set; }
        public string SealedToParent { get; set; }
        public string SealedToSpouse { get; set; }
	}
}
