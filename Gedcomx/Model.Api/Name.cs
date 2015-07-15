using System.Collections.Generic;

namespace Model.Api {
    public class Name {
        public string fullText {
            get;
            set;
        }

        public object script {
            get;
            set;
        }

        public List<Piece> pieces {
            get;
            set;
        }
    }
}