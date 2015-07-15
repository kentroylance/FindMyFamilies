// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs {

  /// <remarks>
  ///  A description of a FamilySearch feature.
  /// </remarks>
  /// <summary>
  ///  A description of a FamilySearch feature.
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="FeatureSet")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://familysearch.org/v1/",TypeName="FeatureSet")]
  public partial class Feature {

    private string _name;
    private string _description;
    private bool? _enabled;
    private bool _enabledSpecified;
    private DateTime? _activationDate;
    private bool _activationDateSpecified;
    /// <summary>
    ///  The name of the feature.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="name",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="name")]
    public string Name {
      get {
        return this._name;
      }
      set {
        this._name = value;
      }
    }
    /// <summary>
    ///  A description of the feature.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="description",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="description")]
    public string Description {
      get {
        return this._description;
      }
      set {
        this._description = value;
      }
    }
    /// <summary>
    ///  Whether the feature is enabled for the current request.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="enabled",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="enabled")]
    public bool Enabled {
      get {
        return this._enabled.GetValueOrDefault();
      }
      set {
        this._enabled = value;
        this._enabledSpecified = true;
      }
    }

    /// <summary>
    ///  Property for the XML serializer indicating whether the "Enabled" property should be included in the output.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute]
    [System.Xml.Serialization.SoapIgnoreAttribute]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool EnabledSpecified {
      get {
        return this._enabledSpecified;
      }
      set {
        this._enabledSpecified = value;
      }
    }

    /// <summary>
    ///  The date that this feature is scheduled to activate permanently.
    /// </summary>
    [System.Xml.Serialization.XmlElementAttribute(ElementName="activationDate",Namespace="http://familysearch.org/v1/")]
    [System.Xml.Serialization.SoapElementAttribute(ElementName="activationDate")]
    public DateTime ActivationDate {
      get {
        return this._activationDate.GetValueOrDefault();
      }
      set {
        this._activationDate = value;
        this._activationDateSpecified = true;
      }
    }

    /// <summary>
    ///  Property for the XML serializer indicating whether the "ActivationDate" property should be included in the output.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute]
    [System.Xml.Serialization.SoapIgnoreAttribute]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public bool ActivationDateSpecified {
      get {
        return this._activationDateSpecified;
      }
      set {
        this._activationDateSpecified = value;
      }
    }

  }
}  
