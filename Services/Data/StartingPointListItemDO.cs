using System;

namespace FindMyFamilies.Data {

    public class StartingPointListItemDO {
        public StartingPointListItemDO() {
        } 

        public StartingPointListItemDO(string id, string fullname, string gender, string reasons) {
            this.Id = id;
            this.Fullname = fullname;
            this.Gender = gender;
            this.Reasons = reasons;
        } 
		public string Id { get; set; }
		public string Gender { get; set; }
		public string Fullname { get; set; }

        public String Reasons {
            get;
            set;
        }

        public int Count {
            get;
            set;
        }

	}
}
