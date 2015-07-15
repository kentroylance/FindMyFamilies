using System.Collections.Generic;
using System.Web.Mvc;

namespace FindMyFamilies.Web.Models {
    public class DropDownListViewModel {
        public string SelectedValue {
            get;
            set;
        }

        public IEnumerable<SelectListItem> Items {
            get;
            set;
        }
    }
}