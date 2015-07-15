// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs.Discussions {

  /// <remarks>
  ///  
  /// </remarks>
  /// <summary>
  ///  
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="Comment")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="Comment")]
  public partial class Comment : Gx.Links.HypermediaEnabledData {

    private string _text;
    private DateTime? _created;
    private bool _createdSpecified;
    private Gx.Common.ResourceReference _contributor;
    /// <summary>
    ///  The text or &quot;message body&quot; of the comment
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="text",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="text")]
    public string Text {
      get {
        return this._text;
      }
      set {
        this._text = value;
      }
    }
    /// <summary>
    ///  date of creation
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="created",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="created")]
    public DateTime Created {
      get {
        return this._created.GetValueOrDefault();
      }
      set {
        this._created = value;
        this._createdSpecified = true;
      }
    }

    /// <summary>
    ///  Property for the XML serializer indicating whether the "Created" property should be included in the output.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute]
    [System.Xml.Serialization.SoapIgnoreAttribute]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool CreatedSpecified {
      get {
        return this._createdSpecified;
      }
      set {
        this._createdSpecified = value;
      }
    }

    /// <summary>
    ///  contributor of comment
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="contributor",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="contributor")]
    public Gx.Common.ResourceReference Contributor {
      get {
        return this._contributor;
      }
      set {
        this._contributor = value;
      }
    }
  }
}  
