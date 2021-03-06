// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Types {

  /// <remarks>
  ///  Enumeration of document types.
  /// </remarks>
  /// <summary>
  ///  Enumeration of document types.
  /// </summary>
  public enum DocumentType {

    /// <summary>
    ///  Unspecified enum value.
    /// </summary>
    [System.Xml.Serialization.XmlEnumAttribute(Name="__NULL__")]
    [System.Xml.Serialization.SoapEnumAttribute(Name="__NULL__")]
    NULL,

    /// <summary>
    ///   The document is an abstract of a record or document.
    /// </summary>
    Abstract,

    /// <summary>
    ///   The document is a translation of a record or document.
    /// </summary>
    Translation,

    /// <summary>
    ///   The document is a transcription (full or partial) of a record or document.
    /// </summary>
    Transcription,

    /// <summary>
    ///   The document is an analysis done by a researcher, often used as a genealogical proof statement.
    /// </summary>
    Analysis,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    OTHER
  }

  /// <remarks>
  /// Utility class for converting to/from the QNames associated with DocumentType.
  /// </remarks>
  /// <summary>
  /// Utility class for converting to/from the QNames associated with DocumentType.
  /// </summary>
  public static class DocumentTypeQNameUtil {

    /// <summary>
    /// Get the known DocumentType for a given QName. If the QName isn't a known QName, DocumentType.OTHER will be returned.
    /// </summary>
    public static DocumentType ConvertFromKnownQName(string qname) {
      if (qname != null) {
        if ("http://gedcomx.org/Abstract".Equals(qname)) {
          return DocumentType.Abstract;
        }
        if ("http://gedcomx.org/Translation".Equals(qname)) {
          return DocumentType.Translation;
        }
        if ("http://gedcomx.org/Transcription".Equals(qname)) {
          return DocumentType.Transcription;
        }
        if ("http://gedcomx.org/Analysis".Equals(qname)) {
          return DocumentType.Analysis;
        }
      }
      return DocumentType.OTHER;
    }

    /// <summary>
    /// Convert the known DocumentType to a QName. If DocumentType.OTHER, an ArgumentException will be thrown.
    /// </summary>
    public static string ConvertToKnownQName(DocumentType known) {
      switch (known) {
        case DocumentType.Abstract:
          return "http://gedcomx.org/Abstract";
        case DocumentType.Translation:
          return "http://gedcomx.org/Translation";
        case DocumentType.Transcription:
          return "http://gedcomx.org/Transcription";
        case DocumentType.Analysis:
          return "http://gedcomx.org/Analysis";
        default:
          throw new System.ArgumentException("No known QName for: " + known, "known");
      }
    }
  }
}
