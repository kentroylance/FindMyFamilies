// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Agent {

  /// <remarks>
  ///  An online account for a web application.
  /// </remarks>
  /// <summary>
  ///  An online account for a web application.
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="OnlineAccount")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="OnlineAccount")]
  public partial class OnlineAccount : Gx.Common.ExtensibleData {

    private string _accountName;
    private Gx.Common.ResourceReference _serviceHomepage;
    /// <summary>
    ///  The name associated the holder of this account with the account.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="accountName",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="accountName")]
    public string AccountName {
      get {
        return this._accountName;
      }
      set {
        this._accountName = value;
      }
    }
    /// <summary>
    ///  The homepage of the service that provides this account.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="serviceHomepage",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="serviceHomepage")]
    public Gx.Common.ResourceReference ServiceHomepage {
      get {
        return this._serviceHomepage;
      }
      set {
        this._serviceHomepage = value;
      }
    }
  }
}  
