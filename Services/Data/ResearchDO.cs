using System;
using System.Collections;
using System.Collections.Generic;
using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class ResearchDO {
        private int _ReportId;
        private int _SearchCriteria;
        private int _GapInChildren;
        private int _Generation;
        private string _ResearchType;
        private string _PersonId;
        private string _CurrentPersonId;
        private SummaryDO _Summary = new SummaryDO();
        private string _ReportPath;
        private int _RetrievedRecords;
        private int _AgeLimit;
        private int _ValidationId;
        private int _RowsReturned;
        private int _StartAt;
        private bool _Refresh;
        private bool _BornBetween18101850;
        private string _PersonName;
        private bool _IncludePossibleDuplicates;
        private bool _IncludePossibleMatches;
        private string _ReportTitle;
        private bool _AddChildren;

        public ResearchDO() {
            _Generation = 2;
            _GapInChildren = 4;
            _AgeLimit = 18;
            _ResearchType = Constants.RESEARCH_TYPE_ANCESTORS;
            _RowsReturned = 10;


        }

        public string PersonId {
            get {
                return _PersonId;
            }
            set {
                _PersonId = value;
            }
        }

        public string ResearchType {
            get {
                return _ResearchType;
            }
            set {
                _ResearchType = value;
            }
        }

        public int GapInChildren {
            get {
                return _GapInChildren;
            }
            set {
                _GapInChildren = value;
            }
        }

        public int ReportId {
            get {
                return _ReportId;
            }
            set {
                _ReportId = value;
            }
        }

        public int Generation {
            get {
                return _Generation;
            }
            set {
                _Generation = value;
            }
        }

        public SummaryDO Summary {
            get {
                return _Summary;
            }
            set {
                _Summary = value;
            }
        }

        public string ReportPath {
            get {
                return _ReportPath;
            }
            set {
                _ReportPath = value;
            }
        }

        public int SearchCriteria {
            get {
                return _SearchCriteria;
            }
            set {
                _SearchCriteria = value;
            }
        }

        public int RetrievedRecords {
            get {
                return _RetrievedRecords;
            }
            set {
                _RetrievedRecords = value;
            }
        }

        public int AgeLimit {
            get {
                return _AgeLimit;
            }
            set {
                _AgeLimit = value;
            }
        }

        public int RowsReturned {
            get {
                return _RowsReturned;
            }
            set {
                _RowsReturned = value;
            }
        }

        public int ValidationId {
            get {
                return _ValidationId;
            }
            set {
                _ValidationId = value;
            }
        }

        public int StartAt {
            get {
                return _StartAt;
            }
            set {
                _StartAt = value;
            }
        }

        public string CurrentPersonId {
            get {
                return _CurrentPersonId;
            }
            set {
                _CurrentPersonId = value;
            }
        }

        public bool Refresh {
            get {
                return _Refresh;
            }
            set {
                _Refresh = value;
            }
        }

        public bool BornBetween18101850 {
            get {
                return _BornBetween18101850;
            }
            set {
                _BornBetween18101850 = value;
            }
        }

        public string PersonName {
            get {
                return _PersonName;
            }
            set {
                _PersonName = value;
            }
        }

        public bool IncludePossibleDuplicates {
            get {
                return _IncludePossibleDuplicates;
            }
            set {
                _IncludePossibleDuplicates = value;
            }
        }

        public bool IncludePossibleMatches {
            get {
                return _IncludePossibleMatches;
            }
            set {
                _IncludePossibleMatches = value;
            }
        }

        public bool AddChildren {
            get {
                return _AddChildren;
            }
            set {
                _AddChildren = value;
            }
        }

        public string ReportTitle {
            get {
                return _ReportTitle;
            }
            set {
                _ReportTitle = value;
            }
        }


    }
}