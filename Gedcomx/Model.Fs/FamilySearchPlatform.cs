// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs {

  /// <remarks>
  ///  &lt;p&gt;The FamilySearch media types define serialization formats that are specific the FamilySearch developer platform. These
  ///  media types are extensions of the &lt;a href=&quot;http://gedcomx.org&quot;&gt;GEDCOM X&lt;/a&gt; media types and provide concepts and data types
  ///  that are specific to FamilySearch and therefore haven't been adopted into a formal, more general specification.&lt;/p&gt;
  /// </remarks>
  /// <summary>
  ///  &lt;p&gt;The FamilySearch media types define serialization formats that are specific the FamilySearch developer platform. These
  ///  media types are extensions of the &lt;a href=&quot;http://gedcomx.org&quot;&gt;GEDCOM X&lt;/a&gt; media types and provide concepts and data types
  ///  that are specific to FamilySearch and therefore haven't been adopted into a formal, more general specification.&lt;/p&gt;
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="FamilySearch")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="FamilySearch")]
  [System.Xml.Serialization.XmlRootAttribute(Namespace="http://familysearch.org/v1/",ElementName="familysearch")]
  public partial class FamilySearchPlatform : Gx.Gedcomx {

    private System.Collections.Generic.List<Gx.Conclusion.Relationship> _relationships;
    private System.Collections.Generic.List<Gx.Conclusion.PlaceDescription> _places;
    private System.Collections.Generic.List<Gx.Fs.Tree.ChildAndParentsRelationship> _childAndParentsRelationships;
    private System.Collections.Generic.List<Gx.Fs.Discussions.Discussion> _discussions;
    private System.Collections.Generic.List<Gx.Fs.Users.User> _users;
    private System.Collections.Generic.List<Gx.Fs.Tree.Merge> _merges;
    private System.Collections.Generic.List<Gx.Fs.Tree.MergeAnalysis> _mergeAnalyses;
    private System.Collections.Generic.List<Gx.Fs.Feature> _features;
    /// <summary>
    ///  The child-and-parents relationships for this data set.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="childAndParentsRelationship",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="childAndParentsRelationship")]
    public System.Collections.Generic.List<Gx.Fs.Tree.ChildAndParentsRelationship> ChildAndParentsRelationships {
      get {
        return this._childAndParentsRelationships;
      }
      set {
        this._childAndParentsRelationships = value;
      }
    }
    /// <summary>
    ///  The discussions included in this data set.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="discussion",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="discussion")]
    public System.Collections.Generic.List<Gx.Fs.Discussions.Discussion> Discussions {
      get {
        return this._discussions;
      }
      set {
        this._discussions = value;
      }
    }
    /// <summary>
    ///  The users included in this genealogical data set.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="user",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="user")]
    public System.Collections.Generic.List<Gx.Fs.Users.User> Users {
      get {
        return this._users;
      }
      set {
        this._users = value;
      }
    }
    /// <summary>
    ///  The merges for this data set.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="merge",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="merge")]
    public System.Collections.Generic.List<Gx.Fs.Tree.Merge> Merges {
      get {
        return this._merges;
      }
      set {
        this._merges = value;
      }
    }
    /// <summary>
    ///  The merge analysis results for this data set.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="mergeAnalysis",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="mergeAnalysis")]
    public System.Collections.Generic.List<Gx.Fs.Tree.MergeAnalysis> MergeAnalyses {
      get {
        return this._mergeAnalyses;
      }
      set {
        this._mergeAnalyses = value;
      }
    }
    /// <summary>
    ///  The set of features defined in the platform API.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="feature",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="feature")]
    public System.Collections.Generic.List<Gx.Fs.Feature> Features {
      get {
        return this._features;
      }
      set {
        this._features = value;
      }
    }

/// <summary>
    ///  The relationships included in this genealogical data set.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="relationship",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="relationship")]
    public System.Collections.Generic.List<Gx.Conclusion.Relationship> Relationships {
      get {
        return this._relationships;
      }
      set {
        this._relationships = value;
      }
    }
    /// <summary>
    ///  The places included in this genealogical data set.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="place",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="place")]
    public System.Collections.Generic.List<Gx.Conclusion.PlaceDescription> Places {
      get {
        return this._places;
      }
      set {
        this._places = value;
      }
    }
  }
}  
