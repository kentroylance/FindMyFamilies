using System;
using System.Collections.Generic;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class SummaryDO {
        private int _GapInSiblings;
        private int _SiblingsBornSameYear;   //  they are either twins, or this was a mistake.  Look for a gap as well; which, might explain where the mistake is.
        private int _NoSpouse;
        private int _LastNameDifferentThanParents;
        private int _OnlyOneChild;
        private int _SiblingsWithSameName;
        private int _LostFemale;  // A female born in " + person.BirthYear + ", between 1830 and 1904, and has no spouse
        private int _DeceasedLivedBefore1905;
        private int _BirthAfterMothersDeath;
        private int _DeathBeforeMarriage;
        private int _LastChild4YearsMother40;
        private int _MarriedToEarly;
        private int _NoSpouseNoChildren;
        private int _NoSpouseOnlyOneChild;
        private int _NoChildren;

        private String _Summary;

        public SummaryDO() {
        }

        public int GapInSiblings {
            get {
                return _GapInSiblings;
            }
            set {
                _GapInSiblings = value;
            }
        }

        public int SiblingsBornSameYear {
            get {
                return _SiblingsBornSameYear;
            }
            set {
                _SiblingsBornSameYear = value;
            }
        }

        public int NoSpouse {
            get {
                return _NoSpouse;
            }
            set {
                _NoSpouse = value;
            }
        }

        public int LastNameDifferentThanParents {
            get {
                return _LastNameDifferentThanParents;
            }
            set {
                _LastNameDifferentThanParents = value;
            }
        }

        public int OnlyOneChild {
            get {
                return _OnlyOneChild;
            }
            set {
                _OnlyOneChild = value;
            }
        }

        public int NoChildren {
            get {
                return _NoChildren;
            }
            set {
                _NoChildren = value;
            }
        }

        public int NoSpouseNoChildren {
            get {
                return _NoSpouseNoChildren;
            }
            set {
                _NoSpouseNoChildren = value;
            }
        }

        public int NoSpouseOnlyOneChild {
            get {
                return _NoSpouseOnlyOneChild;
            }
            set {
                _NoSpouseOnlyOneChild = value;
            }
        }

        public int SiblingsWithSameName {
            get {
                return _SiblingsWithSameName;
            }
            set {
                _SiblingsWithSameName = value;
            }
        }

        public int LostFemale {
            get {
                return _LostFemale;
            }
            set {
                _LostFemale = value;
            }
        }

        public string Summary {
            get {
                return _Summary;
            }
            set {
                _Summary = value;
            }
        }

        public int DeceasedLivedBefore1905 {
            get {
                return _DeceasedLivedBefore1905;
            }
            set {
                _DeceasedLivedBefore1905 = value;
            }
        }

        public int MarriedToEarly {
            get {
                return _MarriedToEarly;
            }
            set {
                _MarriedToEarly = value;
            }
        }

        public int LastChild4YearsMother40 {
            get {
                return _LastChild4YearsMother40;
            }
            set {
                _LastChild4YearsMother40 = value;
            }
        }

        public int DeathBeforeMarriage {
            get {
                return _DeathBeforeMarriage;
            }
            set {
                _DeathBeforeMarriage = value;
            }
        }

        public int BirthAfterMothersDeath {
            get {
                return _BirthAfterMothersDeath;
            }
            set {
                _BirthAfterMothersDeath = value;
            }
        }
    }
}