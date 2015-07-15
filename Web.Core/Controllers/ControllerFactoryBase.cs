using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FindMyFamilies.Util;

namespace FindMyFamilies.Web.Controllers {

    public class ControllerFactoryBase : System.Web.Mvc.DefaultControllerFactory {
        private Logger logger = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType) {
            if (controllerType == null) {
                string values = "";
                foreach (var value in requestContext.RouteData.Values) {
                    values = values + value.Value + "\\";
                }
                logger.Error("Either reference or file is missing " + values);

                return new ControllerBase();
            }
            return base.GetControllerInstance(requestContext, controllerType);
        }

    }
}