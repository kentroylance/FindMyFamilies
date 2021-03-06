// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Conclusion {

  /// <remarks>
  ///  A relationship between two or more persons.
  /// </remarks>
  /// <summary>
  ///  A relationship between two or more persons.
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="Relationship")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="Relationship")]
  [System.Xml.Serialization.XmlRootAttribute(Namespace="http://gedcomx.org/v1/",ElementName="relationship")]
  public partial class Relationship : Gx.Conclusion.Subject {

    private string _type;
    private Gx.Common.ResourceReference _person1;
    private Gx.Common.ResourceReference _person2;
    private System.Collections.Generic.List<Gx.Conclusion.Fact> _facts;
    private System.Collections.Generic.List<Gx.Records.Field> _fields;
    /// <summary>
    ///  The type of this relationship.
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
    ///  Convenience property for treating Type as an enum. See Gx.Types.RelationshipTypeQNameUtil for details on getter/setter functionality.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute]
    public Gx.Types.RelationshipType KnownType {
      get {
        return Gx.Types.RelationshipTypeQNameUtil.ConvertFromKnownQName(this._type);
      }
      set {
        this._type = Gx.Types.RelationshipTypeQNameUtil.ConvertToKnownQName(value);
      }
    }
    /// <summary>
    ///  A reference to a person in the relationship. The name &quot;person1&quot; is used only to distinguish it from
    ///  the other person in this relationship and implies neither order nor role. When the relationship type
    ///  implies direction, it goes from &quot;person1&quot; to &quot;person2&quot;.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="person1",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="person1")]
    public Gx.Common.ResourceReference Person1 {
      get {
        return this._person1;
      }
      set {
        this._person1 = value;
      }
    }
    /// <summary>
    ///  A reference to a person in the relationship. The name &quot;person2&quot; is used only to distinguish it from
    ///  the other person in this relationship and implies neither order nor role. When the relationship type
    ///  implies direction, it goes from &quot;person1&quot; to &quot;person2&quot;.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="person2",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="person2")]
    public Gx.Common.ResourceReference Person2 {
      get {
        return this._person2;
      }
      set {
        this._person2 = value;
      }
    }
    /// <summary>
    ///  The fact conclusions for the relationship.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="fact",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="fact")]
    public System.Collections.Generic.List<Gx.Conclusion.Fact> Facts {
      get {
        return this._facts;
      }
      set {
        this._facts = value;
      }
    }
    /// <summary>
    ///  The references to the record fields being used as evidence.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="field",Namespace="http://gedcomx.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="field")]
    public System.Collections.Generic.List<Gx.Records.Field> Fields {
      get {
        return this._fields;
      }
      set {
        this._fields = value;
      }
    }
  }
}  
