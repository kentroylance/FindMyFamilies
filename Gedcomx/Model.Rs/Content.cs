// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Atom {

  /// <remarks>
  ///  
  /// </remarks>
  /// <summary>
  ///  
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2005/Atom",TypeName="Content")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://www.w3.org/2005/Atom",TypeName="Content")]
  public sealed partial class Content {

    private string _type;
    private Gx.Gedcomx _gedcomx;
    /// <summary>
    ///  The type of the content.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="type")]
    [System.Xml.Serialization.SoapAttributeAttribute(AttributeName="type")]
    public string Type {
      get {
        return this._type;
      }
      set {
        this._type = value;
      }
    }
    /// <summary>
    ///  The genealogical data associated with this entry.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="gedcomx",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="gedcomx")]
    public Gx.Gedcomx Gedcomx {
      get {
        return this._gedcomx;
      }
      set {
        this._gedcomx = value;
      }
    }
  }
}  
