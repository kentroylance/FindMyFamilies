using System;

namespace FindMyFamilies.Data {

    public class StartingPointListItemDO {
        public StartingPointListItemDO() {
        } 

        public StartingPointListItemDO(string name, string reasons) {
            this.Name = name;
            this.Reasons = reasons;
        } 
		public string Name { get; set; }

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
