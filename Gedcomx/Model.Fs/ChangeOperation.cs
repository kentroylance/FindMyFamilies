// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs.Tree {

  /// <remarks>
  ///  The set of operations applicable to FamilySearch data.
  /// </remarks>
  /// <summary>
  ///  The set of operations applicable to FamilySearch data.
  /// </summary>
  public enum ChangeOperation {

    /// <summary>
    ///  Unspecified enum value.
    /// </summary>
    [System.Xml.Serialization.XmlEnumAttribute(Name="__NULL__")]
    [System.Xml.Serialization.SoapEnumAttribute(Name="__NULL__")]
    NULL,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    Create,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    Read,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    Update,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    Delete,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    Merge,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    Unmerge,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    Restore
  }

  /// <remarks>
  /// Utility class for converting to/from the QNames associated with ChangeOperation.
  /// </remarks>
  /// <summary>
  /// Utility class for converting to/from the QNames associated with ChangeOperation.
  /// </summary>
  public static class ChangeOperationQNameUtil {

    /// <summary>
    /// Get the known ChangeOperation for a given QName. If the QName isn't a known QName, ChangeOperation.NULL will be returned.
    /// </summary>
    public static ChangeOperation ConvertFromKnownQName(string qname) {
      if (qname != null) {
        if ("http://familysearch.org/v1/Create".Equals(qname)) {
          return ChangeOperation.Create;
        }
        if ("http://familysearch.org/v1/Read".Equals(qname)) {
          return ChangeOperation.Read;
        }
        if ("http://familysearch.org/v1/Update".Equals(qname)) {
          return ChangeOperation.Update;
        }
        if ("http://familysearch.org/v1/Delete".Equals(qname)) {
          return ChangeOperation.Delete;
        }
        if ("http://familysearch.org/v1/Merge".Equals(qname)) {
          return ChangeOperation.Merge;
        }
        if ("http://familysearch.org/v1/Unmerge".Equals(qname)) {
          return ChangeOperation.Unmerge;
        }
        if ("http://familysearch.org/v1/Restore".Equals(qname)) {
          return ChangeOperation.Restore;
        }
      }
      return ChangeOperation.NULL;
    }

    /// <summary>
    /// Convert the known ChangeOperation to a QName. If ChangeOperation.NULL, an ArgumentException will be thrown.
    /// </summary>
    public static string ConvertToKnownQName(ChangeOperation known) {
      switch (known) {
        case ChangeOperation.Create:
          return "http://familysearch.org/v1/Create";
        case ChangeOperation.Read:
          return "http://familysearch.org/v1/Read";
        case ChangeOperation.Update:
          return "http://familysearch.org/v1/Update";
        case ChangeOperation.Delete:
          return "http://familysearch.org/v1/Delete";
        case ChangeOperation.Merge:
          return "http://familysearch.org/v1/Merge";
        case ChangeOperation.Unmerge:
          return "http://familysearch.org/v1/Unmerge";
        case ChangeOperation.Restore:
          return "http://familysearch.org/v1/Restore";
        default:
          throw new System.ArgumentException("No known QName for: " + known, "known");
      }
    }
  }
}
