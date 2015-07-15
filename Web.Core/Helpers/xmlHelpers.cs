using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace FindMyFamilies.Web
{
    public static class xmlHelpers
    {
        public static string GetValue(this XmlAttribute xAttr)
        {
            return xAttr != null ? xAttr.Value : "";
        }
    }
}