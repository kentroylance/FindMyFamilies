// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs.Tree {

  /// <remarks>
  ///  Identifiers for a system that might contain match results.
  /// </remarks>
  /// <summary>
  ///  Identifiers for a system that might contain match results.
  /// </summary>
  public enum MatchSystem {

    /// <summary>
    ///  Unspecified enum value.
    /// </summary>
    [System.Xml.Serialization.XmlEnumAttribute(Name="__NULL__")]
    [System.Xml.Serialization.SoapEnumAttribute(Name="__NULL__")]
    NULL,

    /// <summary>
    ///   The FamilySearch Family Tree.
    /// </summary>
    tree,

    /// <summary>
    ///   The FamilySearch Record Set.
    /// </summary>
    records,

    /// <summary>
    ///   The FamilySearch User-Submitted Trees.
    /// </summary>
    lls,

    /// <summary>
    ///   The FamilySearch Temple System.
    /// </summary>
    tss,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    OTHER
  }

  /// <remarks>
  /// Utility class for converting to/from the QNames associated with MatchSystem.
  /// </remarks>
  /// <summary>
  /// Utility class for converting to/from the QNames associated with MatchSystem.
  /// </summary>
  public static class MatchSystemQNameUtil {

    /// <summary>
    /// Get the known MatchSystem for a given QName. If the QName isn't a known QName, MatchSystem.OTHER will be returned.
    /// </summary>
    public static MatchSystem ConvertFromKnownQName(string qname) {
      if (qname != null) {
        if ("http://familysearch.org/v1/tree".Equals(qname)) {
          return MatchSystem.tree;
        }
        if ("http://familysearch.org/v1/records".Equals(qname)) {
          return MatchSystem.records;
        }
        if ("http://familysearch.org/v1/trees".Equals(qname)) {
          return MatchSystem.lls;
        }
        if ("http://familysearch.org/v1/temple".Equals(qname)) {
          return MatchSystem.tss;
        }
      }
      return MatchSystem.OTHER;
    }

    /// <summary>
    /// Convert the known MatchSystem to a QName. If MatchSystem.OTHER, an ArgumentException will be thrown.
    /// </summary>
    public static string ConvertToKnownQName(MatchSystem known) {
      switch (known) {
        case MatchSystem.tree:
          return "http://familysearch.org/v1/tree";
        case MatchSystem.records:
          return "http://familysearch.org/v1/records";
        case MatchSystem.lls:
          return "http://familysearch.org/v1/trees";
        case MatchSystem.tss:
          return "http://familysearch.org/v1/temple";
        default:
          throw new System.ArgumentException("No known QName for: " + known, "known");
      }
    }
  }
}
