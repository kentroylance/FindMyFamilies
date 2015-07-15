using System;
using System.Collections.Generic;
using System.Linq;
using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class PersonDO {
        private int _BirthYear;
        private Dictionary<string, PersonDO> _children;
        private int _DeathYear;
        private PersonDO _father;
        private string _Firstname;
        private PersonDO _mother;
        private Dictionary<string, PersonDO> _parents;
        private List<PersonDO> _SortedChildren;
        private List<PersonDO> _SortedSpouses;
        private PersonDO _spouse;
        private Dictionary<string, PersonDO> _spouses;
        private int _YearsLived;
        private int _yearsMarried;

        public PersonDO() {
            UsingMaidenName = true;
        }

        public Dictionary<string, PersonDO> Spouses {
            get {
                if (_spouses == null) {
                    _spouses = new Dictionary<string, PersonDO>();
                }
                return _spouses;
            }
            set {
                _spouses = value;
            }
        }

        public Dictionary<string, PersonDO> Parents {
            get {
                if (_parents == null) {
                    _parents = new Dictionary<string, PersonDO>();
                }
                return _parents;
            }
            set {
                _parents = value;
            }
        }

        public PersonDO Mother {
            get {
                if (_mother == null) {
                    _mother = new PersonDO();
                }
                return _mother;
            }
            set {
                _mother = value;
            }
        }

        public PersonDO Father {
            get {
                if (_father == null) {
                    _father = new PersonDO();
                }
                return _father;
            }
            set {
                _father = value;
            }
        }

        public bool Populated {
            get;
            set;
        }

        public bool IsEmpty {
            get {
                return (Id == null);
            }
        }

        public Dictionary<string, PersonDO> Children {
            get {
                if (_children == null) {
                    _children = new Dictionary<string, PersonDO>();
                }
                return _children;
            }
            set {
                _children = value;
            }
        }

        public PersonDO Spouse {
            get {
                if (_spouse == null) {
                    _spouse = new PersonDO();
                }
                return _spouse;
            }
            set {
                _spouse = value;
            }
        }

        public string MarriedSpouseId {
            get;
            set;
        }

        public bool IsMale {
            get {
                return (Gender.Equals(Constants.MALE));
            }
        }

        public bool IsFemale {
            get {
                return (Gender.Equals(Constants.FEMALE));
            }
        }

        public int YearsMarried {
            get {
                if (Spouse != null) {
                    if (IsMarried) {
                        var deathYear = 0;
                        if ((DeathYear > 0) && (Spouse.DeathYear > 0)) {
                            if (DeathYear <= Spouse.DeathYear) {
                                deathYear = DeathYear;
                            } else {
                                deathYear = Spouse.DeathYear;
                            }
                            _yearsMarried = Math.Abs(deathYear - MarriageDate.Value.Year);
                        }
                    }
                }
                return _yearsMarried;
            }
        }

        public PersonDO FirstChild {
            get {
                var earliestDate = 0;
                PersonDO youngest = null;
                // find the youngest first
                if (HasChildren) {
                    foreach (var child in Children) {
                        if (earliestDate == 0) {
                            earliestDate = child.Value.BirthYear;
                            youngest = child.Value;
                        } else if (child.Value.BirthYear < earliestDate) {
                            earliestDate = child.Value.BirthYear;
                            youngest = child.Value;
                        }
                    }
                }
                return youngest;
            }
        }

        public PersonDO LastChild {
            get {
                var latestDate = 0;
                PersonDO oldest = null;
                // find the oldest child
                if (HasChildren) {
                    foreach (var child in Children) {
                        if (latestDate == 0) {
                            latestDate = child.Value.BirthYear;
                            oldest = child.Value;
                        } else if (child.Value.BirthYear > latestDate) {
                            latestDate = child.Value.BirthYear;
                            oldest = child.Value;
                        }
                    }
                }
                return oldest;
            }
        }

        // this doesn't include years before the first child, or if another child was born before this child
        public int YearsAsCoupleSinceFirstChild {
            get {
                var yearsAsCouple = 0;
                if ((Spouse != null) && (HasChildren)) {
                    var firstChild = FirstChild;
                    if ((firstChild != null) && (firstChild.BirthYear > 0)) {
                        var mother = (!IsMale ? this : Spouse);
                        var father = (IsMale ? this : Spouse);

                        //                        int yearsMotherLivedAfterFirstChild = Math.Abs(firstChild.BirthYear - mother.DeathYear);
                        //                        int yearsFatherLivedAfterFirstChild = Math.Abs(firstChild.BirthYear - father.DeathYear);
                        //                        int fathersAgeWhenFirstChildBorn = Math.Abs(father.BirthYear + firstChild.BirthYear);
                        //                        int mothersAgeWhenFirstChildBorn = Math.Abs(mother.BirthYear + firstChild.BirthYear);

                        var deathYearOfSpouseWhoDiedFirst = (DeathYear <= Spouse.DeathYear ? DeathYear : Spouse.DeathYear);
                        yearsAsCouple = Math.Abs(firstChild.BirthYear - deathYearOfSpouseWhoDiedFirst);
                    }
                }
                return yearsAsCouple;
            }
        }

        //man 1960 - 2010     50 years
        //women 1965 -  2020  55 years
        //
        //
        //Death year of who died first - 2010 - (1960 + 20) = 30 years
        public int YearsAsCouple {
            get {
                var yearsAsCouple = 0;
                if ((Spouse != null) && (DeathYear > 0) && (Spouse.DeathYear > 0)) {
                    var deathYearOfSpouseWhoDiedFirst = (DeathYear <= Spouse.DeathYear ? DeathYear : Spouse.DeathYear);
                    var birthYearOfMale = (IsMale ? this : Spouse).BirthYear;
                    if (birthYearOfMale > 0) {
                        yearsAsCouple = Math.Abs((birthYearOfMale + 20) - deathYearOfSpouseWhoDiedFirst);
                    }
                }
                return yearsAsCouple;
            }
        }

        public string BirthDeathDateCombined {
            get {
                var birthDeathDateCombined = "";
                if ((BirthYear > 0) && (DeathYear > 0)) {
                    birthDeathDateCombined = "(" + BirthYear + " - " + DeathYear + ")";
                } else if ((BirthYear > 0) && (DeathYear == 0) && (!Living)) {
                    birthDeathDateCombined = "(" + BirthYear + " - Deceased)";
                } else if ((BirthYear > 0) && (DeathYear == 0) && (Living)) {
                    birthDeathDateCombined = "(" + BirthYear + " - Living)";
                } else if ((BirthYear == 0) && (DeathYear > 0)) {
                    birthDeathDateCombined = "(? - " + DeathYear + ")";
                    //                } else {
                    //                    birthDeathDateCombined = "(? - ?)";
                }
                return birthDeathDateCombined;
            }
        }

        public int YearsLived {
            get {
                if ((BirthYear > 0) && (DeathYear > 0)) {
                    _YearsLived = Math.Abs(DeathYear - BirthYear);
                }
                return _YearsLived;
            }
        }

        public int NumberOfChildren {
            get {
                return Children.Count;
            }
        }

        public bool IsMarried {
            get {
                return (MarriageDate != null);
            }
        }

        public bool HasSpouse {
            get {
                bool spouse = false;
                if ((Spouses.Count == 0)) {
                    if ((Spouse != null) && !Spouse.IsEmpty) {
                        Spouses.Add(Spouse.Id, Spouse);
                    }
                }
                if (Spouses.Count > 0) {
                    spouse = true;
                } else if (HasSpouseLink) {
                    spouse = true;
                }
                return spouse;
            }
        }

        public bool HasChildren {
            get {
                bool children = false;
                if (Children.Count > 0) {
                    children = true;
                } else if (HasChildrenLink) {
                    children = true;
                }
                return children;
            }
        }

        public bool HasParents {
            get {
                bool parents = false;
                if ((Parents.Count == 0)) {
                    if ((Mother != null) && !Mother.IsEmpty) {
                        Parents.Add(Mother.Id, Mother);
                    }
                    if ((Father != null) && !Father.IsEmpty) {
                        Parents.Add(Father.Id, Father);
                    }
                }
                if (Parents.Count > 0) {
                    parents = true;
                } else if (HasParentsLink) {
                    parents = true;
                }
                return parents;
            }
        }

        public int DeathYear {
            get {
                if ((_DeathYear == 0) && (DeathDate != null)) {
                    _DeathYear = DeathDate.Value.Year;
                }
                return _DeathYear;
            }
            set {
                _DeathYear = value;
            }
        }

        public int BirthYear {
            get {
                if ((_BirthYear == 0) && (BirthDate != null)) {
                    _BirthYear = BirthDate.Value.Year;
                }
                return _BirthYear;
            }
            set {
                _BirthYear = value;
            }
        }

        public int MarriageYear {
            get {
                var _MarriageYear = 0;
                if (MarriageDate != null) {
                    _MarriageYear = MarriageDate.Value.Year;
                }
                return _MarriageYear;
            }
        }

        public DateTime? DeathDate {
            get;
            set;
        }

        public DateTime? BirthDate {
            get;
            set;
        }

        public DateTime? MarriageDate {
            get;
            set;
        }

        public string MarriagePlace {
            get;
            set;
        }

        public string Fullname {
            get;
            set;
        }

        public bool HasSpouseLink {
            get;
            set;
        }

        public bool HasSpousesLink {
            get;
            set;
        }

        public bool HasChildrenLink {
            get;
            set;
        }

        public bool HasParentsLink {
            get;
            set;
        }

        public bool HasMultipleParentsLink {
            get;
            set;
        }

        public string Firstname {
            get {
                return _Firstname;
            }
            set {
                if (!string.IsNullOrEmpty(value)) {
                    var nameParts = value.Split(' ');
                    if ((nameParts != null) && (nameParts.Length > 1)) {
                        string firstname = null;
                        var middlename = "";
                        foreach (var namePart in nameParts) {
                            if (string.IsNullOrEmpty(firstname)) {
                                firstname = namePart;
                            } else {
                                middlename = middlename + namePart + " ";
                            }
                        }
                        _Firstname = firstname;
                        Middlename = middlename.Trim();
                    } else {
                        _Firstname = value;
                    }
                }
            }
        }

        public string Middlename {
            get;
            set;
        }

        public string Lastname {
            get;
            set;
        }

        public string BirthPlace {
            get;
            set;
        }

        public string Id {
            get;
            set;
        }

        public string Gender {
            get;
            set;
        }

        public bool Living {
            get;
            set;
        }

        public string DeathPlace {
            get;
            set;
        }

        public string DescendancyNumber {
            get;
            set;
        }

        public string AscendancyNumber {
            get;
            set;
        }

        public bool DirectLine {
            get;
            set;
        }

        public bool Ancestors {
            get;
            set;
        }

        public bool Descendants {
            get;
            set;
        }

        public string DeathDateString {
            get;
            set;
        }

        public string BirthDateString {
            get;
            set;
        }

        public string MarriageDateString {
            get;
            set;
        }

        public string LifeSpan {
            get;
            set;
        }

        public bool UsingMaidenName {
            get;
            set;
        }

        public List<PersonDO> SortedChildren {
            get {
                if (_SortedChildren == null) {
                    _SortedChildren = SortedListOfChildren();
                }
                return _SortedChildren;
            }
            set {
                _SortedChildren = value;
            }
        }

        public List<PersonDO> SortedSpouses {
            get {
                if (_SortedSpouses == null) {
                    _SortedSpouses = SortedListOfSpouses();
                }
                return _SortedSpouses;
            }
            set {
                _SortedSpouses = value;
            }
        }

        public PersonDO GetFather() {
            PersonDO father = null;
            foreach (var person in Parents) {
                if (person.Value.IsMale) {
                    father = person.Value;
                    break;
                }
            }
            return father;
        }

        public PersonDO GetMother() {
            PersonDO mother = null;
            foreach (var person in Parents) {
                if (person.Value.IsFemale) {
                    mother = person.Value;
                    break;
                }
            }
            return mother;
        }

        public List<PersonDO> SortedListOfChildren() {
            var sortedChildren = new List<PersonDO>(Children.Values);
            return sortedChildren.OrderBy(o => o.BirthYear).ToList();
        }

        public List<PersonDO> SortedListOfSpouses() {
            var sortedSpouses = new List<PersonDO>(Spouses.Values);
            return sortedSpouses.OrderBy(o => o.BirthYear).ToList();
        }

        public List<PersonDO> SortedListOfSpousesAndChildren() {
            var sortedSpouses = new List<PersonDO>(Children.Values);
            foreach (var spouse in sortedSpouses) {
                spouse.SortedListOfChildren();
            }
            return sortedSpouses.OrderBy(o => o.BirthYear).ToList();
        }

        public string City(string place) {
            var city = "";

            if (!string.IsNullOrEmpty(place)) {
                var placeParts = place.Split(',');
                if (placeParts.Length > 0) {
                    city = placeParts[0];
                }
            }

            return city;
        }

        public string State(string place) {
            var state = "";

            if (!string.IsNullOrEmpty(place)) {
                var placeParts = place.Split(',');
                if (placeParts.Length > 1) {
                    state = placeParts[1];
                }
            }

            return state;
        }

        public string County(string place) {
            var county = "";

            if (!string.IsNullOrEmpty(place)) {
                var placeParts = place.Split(',');
                if (placeParts.Length > 2) {
                    county = placeParts[2];
                }
            }

            return county;
        }

        public string Country(string place) {
            var country = "";

            if (!string.IsNullOrEmpty(place)) {
                var placeParts = place.Split(',');
                place = place.ToLower();
                if (place.Equals("united states") || place.Equals("us") || place.Equals("u.s.")) {
                    if (placeParts.Length > 2) {
                        country = placeParts[3];
                    }
                } else if (place.Equals("england")) {
                    if (placeParts.Length > 1) {
                        country = placeParts[2];
                    }
                } else {
                    if (placeParts.Length > 0) {
                        country = placeParts[placeParts.Length - 1];
                    }
                }
            }

            return country;
        }

        public bool IsPlaceValid(string place) {
            var verified = false;
            if (!string.IsNullOrEmpty(place)) {
                var placeParts = place.Split(',');
                if (placeParts.Length > 2) {
                    verified = true;
                }
            }

            return verified;
        }
    }
}