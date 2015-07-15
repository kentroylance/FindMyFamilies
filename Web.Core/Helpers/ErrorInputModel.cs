using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyFamiles.Web.Helpers {
    public class ErrorInputModel {
        public string ErrorText {
            get;
            set;
        }

        public string Url {
            get;
            set;
        }

        public int? LineNumber {
            get;
            set;
        }

        public override string ToString() {
            return string.Format("Error Text: {0}, Url: {1}, Line Number: {2}", this.ErrorText, this.Url, (LineNumber.HasValue ? LineNumber.Value.ToString() : "Unkown"));
        }
    }
}