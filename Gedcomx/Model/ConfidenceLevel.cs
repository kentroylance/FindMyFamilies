// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Types {

  /// <remarks>
  ///  Enumeration of levels of confidence.
  /// </remarks>
  /// <summary>
  ///  Enumeration of levels of confidence.
  /// </summary>
  public enum ConfidenceLevel {

    /// <summary>
    ///  Unspecified enum value.
    /// </summary>
    [System.Xml.Serialization.XmlEnumAttribute(Name="__NULL__")]
    [System.Xml.Serialization.SoapEnumAttribute(Name="__NULL__")]
    NULL,

    /// <summary>
    ///   High of confidence.
    /// </summary>
    High,

    /// <summary>
    ///   Medium of confidence.
    /// </summary>
    Medium,

    /// <summary>
    ///   Low of confidence.
    /// </summary>
    Low,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    OTHER
  }

  /// <remarks>
  /// Utility class for converting to/from the QNames associated with ConfidenceLevel.
  /// </remarks>
  /// <summary>
  /// Utility class for converting to/from the QNames associated with ConfidenceLevel.
  /// </summary>
  public static class ConfidenceLevelQNameUtil {

    /// <summary>
    /// Get the known ConfidenceLevel for a given QName. If the QName isn't a known QName, ConfidenceLevel.OTHER will be returned.
    /// </summary>
    public static ConfidenceLevel ConvertFromKnownQName(string qname) {
      if (qname != null) {
        if ("http://gedcomx.org/High".Equals(qname)) {
          return ConfidenceLevel.High;
        }
        if ("http://gedcomx.org/Medium".Equals(qname)) {
          return ConfidenceLevel.Medium;
        }
        if ("http://gedcomx.org/Low".Equals(qname)) {
          return ConfidenceLevel.Low;
        }
      }
      return ConfidenceLevel.OTHER;
    }

    /// <summary>
    /// Convert the known ConfidenceLevel to a QName. If ConfidenceLevel.OTHER, an ArgumentException will be thrown.
    /// </summary>
    public static string ConvertToKnownQName(ConfidenceLevel known) {
      switch (known) {
        case ConfidenceLevel.High:
          return "http://gedcomx.org/High";
        case ConfidenceLevel.Medium:
          return "http://gedcomx.org/Medium";
        case ConfidenceLevel.Low:
          return "http://gedcomx.org/Low";
        default:
          throw new System.ArgumentException("No known QName for: " + known, "known");
      }
    }
  }
}
