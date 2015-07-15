using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyFamiles.Web.Helpers {
    public class ClientErrorException : Exception {
        public ClientErrorException(string message) : base(message) {
        }
    }
}