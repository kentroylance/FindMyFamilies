using System.Collections;
using System.EnterpriseServices.Internal;

namespace FindMyFamilies.Data {

    public class ResultDO {

        public ResultDO() {
        } 
		public IList list { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string errorMessage { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
        public string reportId { get; set; }
        public string reportFile { get; set; }
	}
}
