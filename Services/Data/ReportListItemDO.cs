namespace FindMyFamilies.Data {

    public class ReportListItemDO : FindListItemDO {
        public ReportListItemDO() {
        } 

        public ReportListItemDO(string reportId, string reportDate, string reportFile, string id, string fullName, string records, string researchtype, string generation) {
            this.reportId = reportId;
            this.reportDate = reportDate;
            this.reportFile = reportFile;
            this.id = id;
            this.fullName = fullName;
            this.records = records;
            this.researchType = researchtype;
            this.generation = generation;
        } 

        public string reportId { get; set; }
        public string reportDate { get; set; }
        public string reportFile { get; set; }
        public string records { get; set; }
        public string researchType { get; set; }
        public string generation {get; set;}
	}
}
