using System;
using System.Collections.Generic;
using FindMyFamilies.Helper;
using FindMyFamilies.Data;
using FindMyFamilies.Util;
using RestSharp;

namespace  FindMyFamilies.DataAccess {

    /// <summary>
    ///     Purpose: Data Access Class for Authenticating at familysearch.org
    /// </summary>
    public class AuthenticationDAO {
        private Logger logger = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SessionDO Authenticate(string username, string password, string clientId) {
            var session = new SessionDO(username, password, clientId);
            return Authenticate(username, password, clientId, null);
        }

        public SessionDO Authenticate(string username, string password, string clientId, string clientSecret) {
            var session = new SessionDO(username, password, clientId, clientSecret);

            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            RestClient restClient = null;
            RestRequest request = RestHelper.Instance.GetRequest(Constants.AUTH2_TOKEN, ref session, ref restClient);
            if (request != null) {
                request.Method = Method.POST;
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", username);
                request.AddParameter("password", password);
                request.AddParameter("client_id", clientId);
                if (!Strings.IsEmpty(clientSecret)) {
                    request.AddParameter("client_secret", clientSecret);
                }
                IRestResponse<Dictionary<string, object>> response = null;
                try {
                    response = restClient.Execute<Dictionary<string, object>>(request); 
                    if (response.ErrorException == null) {
                        Dictionary<string, object> result = response.Data;
                        if ((result != null) && result.ContainsKey("access_token")) {
                            var accessToken = (string) result["access_token"];
                            if (!Strings.IsEmpty(accessToken)) {
                                session.Authenticated = true;
                                session.AccessToken = accessToken;
                            }
                        }
                    }
                } catch (Exception e) {
                    string errorMessage = "Failed to authenticate for [" + Constants.AUTH2_TOKEN + "]. username = [" + username + "]  Error: " +
                        e.Message;
                    logger.Error(errorMessage, e);
                    session.Error = true;
                    session.ErrorMessage = errorMessage;
                } finally {
//                    session.Response = response;
                }
            } else {
//                session.Request = request;
            }
            return session;
        }
    }

}