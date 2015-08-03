namespace FindMyFamilies.Data {

    public class OrdinanceListItemDO : FindListItemDO {
        public OrdinanceListItemDO() {
        } 

        public OrdinanceListItemDO(string status) {
            this.Status = status;
        } 
		public string Status { get; set; }
	}
}
