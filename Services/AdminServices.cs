using System;
using System.Collections;
using System.Collections.Generic;
using FindMyFamilies.BusinessObject;
using FindMyFamilies.Data;
using findmyfamilies.Services;
using Gx;
using Gx.Fs;

namespace FindMyFamilies.Services {

    /// <summary>
    ///     Purpose: Services Facade Class for AdminServices
    /// </summary>
    public class AdminServices : AdminServicesBase {

        private static AdminServices instance;
        private static readonly object syncLock = new object();

        private AdminServices() {
        }

        public static AdminServices Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new AdminServices();
                    }

                    return instance;
                }
            }
        }

        /// <summary>
		/// Purpose: Subscibe to monthly news letter
		/// </summary>
		/// <param name = "string">string email</param>
		public virtual EmailDO SubscribeEmail(string email, SessionDO session) {
			return EmailBO.SubscribeEmail(email, session);
		}



	}
}
