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
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="MergeAnalysis")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="MergeAnalysis")]
  [System.Xml.Serialization.XmlRootAttribute(Namespace="http://familysearch.org/v1/",ElementName="mergeAnalysis")]
  public partial class MergeAnalysis {

    private System.Collections.Generic.List<Gx.Common.ResourceReference> _survivorResources;
    private System.Collections.Generic.List<Gx.Common.ResourceReference> _duplicateResources;
    private System.Collections.Generic.List<Gx.Fs.Tree.MergeConflict> _conflictingResources;
    private Gx.Common.ResourceReference _survivor;
    private Gx.Common.ResourceReference _duplicate;
    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="survivorResource",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="survivorResource")]
    public System.Collections.Generic.List<Gx.Common.ResourceReference> SurvivorResources {
      get {
        return this._survivorResources;
      }
      set {
        this._survivorResources = value;
      }
    }
    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="duplicateResource",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="duplicateResource")]
    public System.Collections.Generic.List<Gx.Common.ResourceReference> DuplicateResources {
      get {
        return this._duplicateResources;
      }
      set {
        this._duplicateResources = value;
      }
    }
    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="conflictingResource",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="conflictingResource")]
    public System.Collections.Generic.List<Gx.Fs.Tree.MergeConflict> ConflictingResources {
      get {
        return this._conflictingResources;
      }
      set {
        this._conflictingResources = value;
      }
    }
    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="survivor",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="survivor")]
    public Gx.Common.ResourceReference Survivor {
      get {
        return this._survivor;
      }
      set {
        this._survivor = value;
      }
    }
    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="duplicate",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="duplicate")]
    public Gx.Common.ResourceReference Duplicate {
      get {
        return this._duplicate;
      }
      set {
        this._duplicate = value;
      }
    }
  }
}  
