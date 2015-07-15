using System;
using System.Collections.Generic;
using FindMyFamilies.BusinessObject;
using FindMyFamilies.Data;
using Gx;
using Gx.Conclusion;

namespace FindMyFamilies.Services {

    /// <summary>
    ///     Purpose: Services Facade Class for AncestryServices
    /// </summary>
    public class AncestryServices {

        private static AncestryServices instance;
        private static readonly object syncLock = new object();

        private AncestryServices() {
        }

        public static AncestryServices Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new AncestryServices();
                    }

                    return instance;
                }
            }
        }

        public Gedcomx GetPersonAncestryWithSpouse(String personId, String spouseId, String generations, ref SessionDO session) {
            return new AncestryBO().GetPersonAncestryWithSpouse(personId, spouseId, generations, ref session);
        }
    }

}