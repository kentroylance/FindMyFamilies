using System;
using System.Xml.Serialization;

namespace Model.Api {
    /// <remarks>
    ///     An Ordinance.
    /// </remarks>
    /// <summary>
    ///     An Ordinance.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://api.familysearch.org/reservation/v1/", TypeName = "Ordinance")]
    [SoapType(Namespace = "http://api.familysearch.org/reservation/v1/", TypeName = "Ordinance")]
    public abstract partial class Ordinance {
        private bool? _completed;
        private bool? _readyForTrip;
        private bool? _reservable;
        private string _status;

        /// <summary>
        ///     The city.
        /// </summary>
        [XmlElement(ElementName = "status", Namespace = "http://gedcomx.org/v1/")]
        [SoapElement(ElementName = "status")]
        public string Status {
            get {
                return _status;
            }
            set {
                _status = value;
            }
        }

        [XmlElement(ElementName = "completed", Namespace = "http://api.familysearch.org/reservation/v1/")]
        [SoapElement(ElementName = "completed")]
        public bool Completed {
            get {
                return _completed.GetValueOrDefault();
            }
            set {
                _completed = value;
            }
        }

        [XmlElement(ElementName = "readyForTrip", Namespace = "http://api.familysearch.org/reservation/v1/")]
        [SoapElement(ElementName = "readyForTrip")]
        public bool ReadyForTrip {
            get {
                return _readyForTrip.GetValueOrDefault();
            }
            set {
                _readyForTrip = value;
            }
        }

        [XmlElement(ElementName = "reservable", Namespace = "http://api.familysearch.org/reservation/v1/")]
        [SoapElement(ElementName = "reservable")]
        public bool Reservable {
            get {
                return _reservable.GetValueOrDefault();
            }
            set {
                _reservable = value;
            }
        }

    }

}