// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs.Discussions {

  /// <remarks>
  ///  A discussion.
  /// </remarks>
  /// <summary>
  ///  A discussion.
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="Discussion")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="Discussion")]
  [System.Xml.Serialization.XmlRootAttribute(Namespace="http://familysearch.org/v1/",ElementName="discussion")]
  public partial class Discussion : Gx.Links.HypermediaEnabledData {

    private string _title;
    private string _details;
    private DateTime? _created;
    private bool _createdSpecified;
    private Gx.Common.ResourceReference _contributor;
    private DateTime? _modified;
    private bool _modifiedSpecified;
    private int? _numberOfComments;
    private bool _numberOfCommentsSpecified;
    private System.Collections.Generic.List<Gx.Fs.Discussions.Comment> _comments;
    /// <summary>
    ///  the one-line summary text
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="title",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="title")]
    public string Title {
      get {
        return this._title;
      }
      set {
        this._title = value;
      }
    }
    /// <summary>
    ///  The text or &quot;message body&quot; of the discussion
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="details",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="details")]
    public string Details {
      get {
        return this._details;
      }
      set {
        this._details = value;
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
    ///  contributor of discussion
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
    /// <summary>
    ///  Date of last modification
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="modified",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="modified")]
    public DateTime Modified {
      get {
        return this._modified.GetValueOrDefault();
      }
      set {
        this._modified = value;
        this._modifiedSpecified = true;
      }
    }

    /// <summary>
    ///  Property for the XML serializer indicating whether the "Modified" property should be included in the output.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute]
    [System.Xml.Serialization.SoapIgnoreAttribute]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool ModifiedSpecified {
      get {
        return this._modifiedSpecified;
      }
      set {
        this._modifiedSpecified = value;
      }
    }

    /// <summary>
    ///  Number of comments
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="numberOfComments",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="numberOfComments")]
    public int NumberOfComments {
      get {
        return this._numberOfComments.GetValueOrDefault();
      }
      set {
        this._numberOfComments = value;
        this._numberOfCommentsSpecified = true;
      }
    }

    /// <summary>
    ///  Property for the XML serializer indicating whether the "NumberOfComments" property should be included in the output.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute]
    [System.Xml.Serialization.SoapIgnoreAttribute]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool NumberOfCommentsSpecified {
      get {
        return this._numberOfCommentsSpecified;
      }
      set {
        this._numberOfCommentsSpecified = value;
      }
    }

    /// <summary>
    ///  The comments on this discussion.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="comment",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="comment")]
    public System.Collections.Generic.List<Gx.Fs.Discussions.Comment> Comments {
      get {
        return this._comments;
      }
      set {
        this._comments = value;
      }
    }
  }
}  
