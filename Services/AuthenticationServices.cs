using System.Net;
using FindMyFamilies.BusinessObject;
using FindMyFamilies.Data;

namespace FindMyFamilies.Services {

    /// <summary>
    ///     Purpose: Services Facade Class for AuthenticationServices
    /// </summary>
    public class AuthenticationServices {
        private static AuthenticationServices instance;
        private static readonly object syncLock = new object();

        private AuthenticationServices() {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public static AuthenticationServices Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new AuthenticationServices();
                    }

                    return instance;
                }
            }
        }

        public SessionDO Authenticate(string username, string password, string clientId) {
            return new AuthenticationBO().Authenticate(username, password, clientId, null);
        }

        public SessionDO Authenticate(string username, string password, string clientId, string clientSecret) {
            return new AuthenticationBO().Authenticate(username, password, clientId, clientSecret);
        }
    }

}