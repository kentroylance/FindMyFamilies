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
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="Merge")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="Merge")]
  [System.Xml.Serialization.XmlRootAttribute(Namespace="http://familysearch.org/v1/",ElementName="merge")]
  public partial class Merge {

    private System.Collections.Generic.List<Gx.Common.ResourceReference> _resourcesToDelete;
    private System.Collections.Generic.List<Gx.Common.ResourceReference> _resourcesToCopy;
    /// <summary>
    ///  List of resources to remove from the survivor person.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="resourceToDelete",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="resourceToDelete")]
    public System.Collections.Generic.List<Gx.Common.ResourceReference> ResourcesToDelete {
      get {
        return this._resourcesToDelete;
      }
      set {
        this._resourcesToDelete = value;
      }
    }
    /// <summary>
    ///  List of resources to copy from the duplicate person to survivor person.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="resourceToCopy",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="resourceToCopy")]
    public System.Collections.Generic.List<Gx.Common.ResourceReference> ResourcesToCopy {
      get {
        return this._resourcesToCopy;
      }
      set {
        this._resourcesToCopy = value;
      }
    }
  }
}  
