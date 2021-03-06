// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Source {

  /// <remarks>
  ///  Represents a source citation.
  /// </remarks>
  /// <summary>
  ///  Represents a source citation.
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="SourceCitation")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="SourceCitation")]
  public partial class SourceCitation : Gx.Links.HypermediaEnabledData {

    private string _lang;
    private Gx.Common.ResourceReference _citationTemplate;
    private System.Collections.Generic.List<Gx.Source.CitationField> _fields;
    private string _value;
    /// <summary>
    ///  The language of the note.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="lang",Namespace="http://www.w3.org/XML/1998/namespace")]
    [System.Xml.Serialization.SoapAttributeAttribute(AttributeName="lang",Namespace="http://www.w3.org/XML/1998/namespace")]
    public string Lang {
      get {
        return this._lang;
      }
      set {
        this._lang = value;
      }
    }
    /// <summary>
    ///  A reference to the citation template for this citation.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="citationTemplate",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="citationTemplate")]
    public Gx.Common.ResourceReference CitationTemplate {
      get {
        return this._citationTemplate;
      }
      set {
        this._citationTemplate = value;
      }
    }
    /// <summary>
    ///  The list of citation fields.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="field",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="field")]
    public System.Collections.Generic.List<Gx.Source.CitationField> Fields {
      get {
        return this._fields;
      }
      set {
        this._fields = value;
      }
    }
    /// <summary>
    ///  A rendering (as a string) of a source citation.  This rendering should be the most complete rendering available.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="value",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="value")]
    public string Value {
      get {
        return this._value;
      }
      set {
        this._value = value;
      }
    }
  }
}  
