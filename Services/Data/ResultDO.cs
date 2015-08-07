using System.Collections;

namespace FindMyFamilies.Data {

    public class ResultDO {

        public ResultDO() {
        } 
		public IList list { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public string errorMessage { get; set; }
	}
}
