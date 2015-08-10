using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class FeedbackDO {
        public FeedbackDO() {
        }

        public string Bug {
            get;
            set;
        }

        public string FeatureRequest {
            get;
            set;
        }

        public string Other {
            get;
            set;
        }

        public string PersonId {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string Message {
            get;
            set;
        }

        public string Email {
            get;
            set;
        }

    }
}