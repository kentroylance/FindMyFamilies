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
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.w3.org/2005/Atom",TypeName="ExtensibleElement")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://www.w3.org/2005/Atom",TypeName="ExtensibleElement")]
  public abstract partial class ExtensibleElement : Gx.Atom.CommonAttributes {

    private System.Xml.XmlElement[] _extensionElements;

    /// <summary>
    ///  Custom extension elements.
    /// </summary>
    [System.Xml.Serialization.XmlAnyElementAttribute()]
    public System.Xml.XmlElement[] ExtensionElements {
      get {
        return this._extensionElements;
      }
      set {
        this._extensionElements = value;
      }
    }
  }
}  
