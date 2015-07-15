using System;
using System.Collections.Generic;
using ProtoBuf;

namespace FindMyFamilies.Data {

    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class DateDO {

        public DateDO() {
        }

        public string Id {
            get;
            set;
        }

        public string Fullname {
            get;
            set;
        }

        public string ProblemType {
            get;
            set;
        }

        public string BirthDate {
            get;
            set;
        }

        public string MarriageDate {
            get;
            set;
        }

        public string DeathDate {
            get;
            set;
        }

        public bool DateFlag {
            get;
            set;
        }

        public string BirthPlace {
            get;
            set;
        }

        public string DeathPlace {
            get;
            set;
        }

        public string MarriagePlace {
            get;
            set;
        }

        public bool PlaceFlag {
            get;
            set;
        }


    }
}