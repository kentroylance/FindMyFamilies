// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs.Tree {

  /// <remarks>
  ///  
  /// </remarks>
  /// <summary>
  ///  
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="DiscussionReference")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="DiscussionReference")]
  [System.Xml.Serialization.XmlRootAttribute(Namespace="http://familysearch.org/v1/",ElementName="discussion-reference")]
  public sealed partial class DiscussionReference : Gx.Links.HypermediaEnabledData {

    private string _resource;
    /// <summary>
    ///  The URI to the resource.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="resource")]
    [System.Xml.Serialization.SoapAttributeAttribute(AttributeName="resource")]
    public string Resource {
      get {
        return this._resource;
      }
      set {
        this._resource = value;
      }
    }
  }
}  
