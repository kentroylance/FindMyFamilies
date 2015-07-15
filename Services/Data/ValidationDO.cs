using System;
using System.Collections.Generic;
using ProtoBuf;

namespace FindMyFamilies.Data {

    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class ValidationDO {

        private int _Id;
        private string _PersonId;
        private int _CriteriaId;
        private string _FullName;
        private bool _Male;
        private string _Problem;
        private bool _DirectLine;
        private int _AsendancyNumber;

        public ValidationDO() {
        }

        public string FullName {
            get {
                return _FullName;
            }
            set {
                _FullName = value;
            }
        }

        public int Id {
            get {
                return _Id;
            }
            set {
                _Id = value;
            }
        }

        public string Problem {
            get {
                return _Problem;
            }
            set {
                _Problem = value;
            }
        }

        public bool Male {
            get {
                return _Male;
            }
            set {
                _Male = value;
            }
        }

        public bool DirectLine {
            get {
                return _DirectLine;
            }
            set {
                _DirectLine = value;
            }
        }

        public int AsendancyNumber {
            get {
                return _AsendancyNumber;
            }
            set {
                _AsendancyNumber = value;
            }
        }

        public string PersonId {
            get {
                return _PersonId;
            }
            set {
                _PersonId = value;
            }
        }

        public int CriteriaId {
            get {
                return _CriteriaId;
            }
            set {
                _CriteriaId = value;
            }
        }
    }
}