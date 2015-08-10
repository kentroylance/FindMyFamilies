using System;
using FindMyFamilies.Util;
using ProtoBuf;

namespace FindMyFamilies.Data {
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields, AsReferenceDefault = true)]
    public class SessionDO {
        private bool _Authenticated;
        private string _ErrorMessage;
        private string _MessageType;
        private string _RedirectUri;


        public SessionDO() {
            ResetExpiration();
        }

        public SessionDO(string accessToken, string errorMessage) {
            AccessToken = accessToken;
            ErrorMessage = errorMessage;
            Language = Constants.LANGUAGE_DEFAULT;
            ResetExpiration();
        }

        public SessionDO(string username, string password, string clientId) {
            Username = username;
            Password = password;
            ClientId = clientId;
            Language = Constants.LANGUAGE_DEFAULT;
            ResetExpiration();
        }

        public SessionDO(string username, string password, string clientId, string clientSecret) {
            ClientId = clientId;
            Username = username;
            Password = password;
            ClientSecret = clientSecret;
            Language = Constants.LANGUAGE_DEFAULT;
            ResetExpiration();
        }

        public bool Authenticated {
            get {
                _Authenticated = true;
                if (!string.IsNullOrEmpty(ResponseStatus) && (ResponseStatus.IndexOf("Unauthorized") > -1)) {
                    _Authenticated = false;
                } else if (!string.IsNullOrEmpty(ErrorMessage) && (ErrorMessage.IndexOf("Unauthorized") > -1)) {
                    _Authenticated = false;
                } else if (Expired) {
                    _Authenticated = false;
                } else if (string.IsNullOrEmpty(AccessToken)) {
                    _Authenticated = false;
                }

                return _Authenticated;
            }
            set {
                _Authenticated = value;
            }
        }

        public string AuthCode {
            get;
            set;
        }

        public string ServerPath {
            get;
            set;
        }

        public string ClientId {
            get;
            set;
        }

        public string RedirectUri {
            get;
            set;
        }

        public string RequestUri {
            get;
            set;
        }

        public string ClientSecret {
            get;
            set;
        }

        public string Username {
            get;
            set;
        }

        public string Password {
            get;
            set;
        }

        public string AccessToken {
            get;
            set;
        }

        public string SourcePath {
            get;
            set;
        }

        public DateTime? TokenHourExpire {
            get;
            set;
        }

        public DateTime? Token24HourExpire {
            get;
            set;
        }

        /// <summary>
        ///     Whether this descriptor is expired.
        /// </summary>
        /// <value>
        ///     <c>true</c> if expired; otherwise, <c>false</c>.
        /// </value>
        public bool Expired {
            get {
                bool expired = false;
                DateTime now = DateTime.Now;
                if ((now > TokenHourExpire) || (now > Token24HourExpire)) {
                    expired = true;
                }
                return expired;
            }
        }

        public bool Error {
            get;
            set;
        }

        public string ErrorMessage {
            get {
                return _ErrorMessage;
            }
            set {
                if (Strings.IsEmpty(value)) {
                    _ErrorMessage = value;
                    Error = false;
                } else {
                    _ErrorMessage = _ErrorMessage + value + "\n";
                    Error = true;
                }
            }
        }

        public PersonDO CurrentPerson {
            get;
            set;
        }

        public string Language {
            get;
            set;
        }

        public string LogMessage {
            get;
            set;
        }

        public string ResponseMessage {
            get;
            set;
        }

        public string ResponseData {
            get;
            set;
        }

        public string Message {
            get;
            set;
        }

        public string MessageType {
            get {
                return _MessageType;
            }
            set {
                _MessageType = value;
                if (!string.IsNullOrEmpty(value) && value.Equals(Constants.MESSAGE_TYPE_ERROR)) {
                    Error = true;
                }
            }
        }

        public string ReportFilePath {
            get;
            set;
        }

        public string Action {
            get;
            set;
        }

        public string StatusCode {
            get;
            set;
        }

        public string ResponseStatus {
            get;
            set;
        }

        public string DisplayName {
            get;
            set;
        }

        public void ResetExpiration() {
            TokenHourExpire = DateTime.Now.AddMinutes(60);
            Token24HourExpire = DateTime.Now.AddHours(24);
        }

        public void ResetHourExpiration() {
            TokenHourExpire = DateTime.Now.AddMinutes(60);
        }

        public void Clear() {
            Message = null;
            MessageType = null;
            ErrorMessage = null;
            Error = false;
        }
    }
}