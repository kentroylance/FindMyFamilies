using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using FindMyFamilies.Data;
using FindMyFamilies.Exceptions;
using FindMyFamilies.Util;
using Gx.Atom;
using Gx.Links;
using RestSharp;
using Tavis.UriTemplates;

namespace FindMyFamilies.Helper {
    /// <summary>
    ///   Purpose: Helper class for Rest functionality
    /// </summary>
    public class RestHelper {
        private static readonly Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);
        private static RestHelper instance;
        private static readonly object syncLock = new object();
        private readonly Dictionary<string, RestClient> clients = new Dictionary<string, RestClient>();
        private RestClient sourceClient;
        private string sourcePath;

        private RestHelper() {
            Initialize(Constants.FAMILY_SEARCH_SYSTEM, Constants.DISCOVERY_PATH);
        }

        public static RestHelper Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new RestHelper();
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        ///   The links that describe the API.
        /// </summary>
        /// <value>
        ///   The links.
        /// </value>
        public Dictionary<string, Link> Links {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the expiration of this descriptor.
        /// </summary>
        /// <value>
        ///   The expiration.
        /// </value>
        public DateTime Expiration {
            get;
            set;
        }

        /// <summary>
        ///   Whether this descriptor is expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if expired; otherwise, <c>false</c>.
        /// </value>
        public bool Expired {
            get {
                return DateTime.Now > Expiration;
            }
        }

        public void Initialize(string host, string discoveryPath) {
            Initialize(new RestClient(host), discoveryPath);
        }

        private void Initialize(RestClient client, string discoveryPath) {
            var request = new RestRequest();
            request.Resource = discoveryPath;
            request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);

            var now = DateTime.Now;

            try {
                var response = client.Execute<Feed>(request);
                if (!InvalidResponse(response)) {
                    var feed = response.Data;
                    var expiration = DateTime.MaxValue;
                    foreach (var header in response.Headers) {
                        if ("cache-control".Equals(header.Name.ToLowerInvariant())) {
                            var cacheControl = CacheControl.Parse(header.Value.ToString());
                            expiration = now.AddSeconds(cacheControl.MaxAge);
                        }
                    }

                    Expiration = expiration;
                    Links = BuildLinkLookup(feed != null ? feed.Links : null);

                    sourceClient = client;
                    sourcePath = discoveryPath;
                    clients.Add(client.BaseUrl, client);
                }
            } catch (Exception e) {
                string message = "Failed to intialize/create request for [" + discoveryPath + "]. Error: " + e.ToString();
                logger.Error(message, e);
                throw new RestException(message, e);
            }
        }

        public RestRequest GetRequest(string rel, ref SessionDO session, ref RestClient restClient) {
            return GetRequest(rel, null, ref session, ref restClient);
        }

        public RestRequest GetRequest(string rel, Dictionary<string, string> templateVariables, ref SessionDO session, ref RestClient restClient) {
            //            logger.Info("Entered GetRequest");
            RestRequest request = null;
            Link link;
            string requestPath = null;
            string targetIri = null;
            session.Clear();

            try {
                if ((rel.ToLower().IndexOf("template") > -1) || (rel.IndexOf("oauth.net") > -1)) {
                    if (Links.TryGetValue(rel, out link)) {
                        targetIri = link.Href;
                    } else {
                        var errorMessage = "Failed to get link for [" + rel + "].  Uri string may be invalid";
                        logger.Error(errorMessage);
                        session.ErrorMessage = errorMessage;
                    }
                } else {
                    targetIri = rel;
                }
                //                logger.Info("rel = " + rel);
                //                logger.Info("targetIri = " + targetIri);

                if (!Strings.IsEmpty(targetIri)) {
                    Uri uri;
                    if (TryUriResolution(targetIri, out uri)) {
                        var uriValue = uri.ToString();
                        //                        logger.Info("uri.ToString() = " + uri.ToString());
                        //                        logger.Info("uriValue = " + uriValue);
                        if (uri.IsAbsoluteUri) {
                            var splitHostIndex = uriValue.IndexOf(uri.Host) + uri.Host.Length;
                            restClient = GetClient(uriValue.Substring(0, splitHostIndex));
                            requestPath = uriValue.Length > splitHostIndex + 1 ? uriValue.Substring(splitHostIndex + 1) : "/";
                        } else {
                            restClient = sourceClient;
                            requestPath = uriValue;
                        }
                        if (templateVariables != null) {
                            var template = new UriTemplate(requestPath);
                            foreach (var entry in templateVariables) {
                                template.SetParameter(entry.Key, entry.Value);
                            }
                            requestPath = template.Resolve();
                        }
                        request = new RestRequest(requestPath);
                        session.RequestUri = restClient.BaseUrl + "/" + requestPath;
                        //                        logger.Info("requestPath = " + requestPath);
                        //                        logger.Info("request = " + request.Resource.ToString());
                    }
                }
            } catch (Exception e) {
                string message = "Failed to create request for [" + rel + "]. requestPath = [" + requestPath + "]  Error: "  + e.ToString();
                logger.Error(message, e);
                throw new RestException(message, e);
            }

            return request;
        }

        private RestClient GetClient(string baseUri) {
            RestClient client;
            if (!clients.TryGetValue(baseUri, out client)) {
                client = new RestClient(baseUri);
                clients.Add(baseUri, client);
            }
            return client;
        }

        /// <summary>
        ///   Builds the link lookup table.
        /// </summary>
        /// <returns>
        ///   The link lookup.
        /// </returns>
        /// <param name='links'>
        ///   The links to initialize.
        /// </param>
        private Dictionary<string, Link> BuildLinkLookup(List<Link> links) {
            var lookup = new Dictionary<string, Link>();
            if (links != null) {
                foreach (var link in links) {
                    if (link != null && link.Rel != null) {
                        lookup.Add(link.Rel, link);
                    }
                }
            }
            return lookup;
        }

        /// <summary>
        ///   Tries to resolve a relative or absolute URI.
        /// </summary>
        /// <returns>
        ///   <c>true</c>, if URI resolution was successful, <c>false</c> otherwise.
        /// </returns>
        /// <param name='relativeOrAbsoluteUri'>
        ///   Relative or absolute URI.
        /// </param>
        /// <param name='result'>
        ///   Result.
        /// </param>
        private bool TryUriResolution(string relativeOrAbsoluteUri, out Uri result) {
            //logger.Info("relativeOrAbsoluteUri = " + relativeOrAbsoluteUri);
            if (!Uri.TryCreate(relativeOrAbsoluteUri, UriKind.Absolute, out result)) {
                if (Uri.TryCreate(new Uri(GetBaseUrl(relativeOrAbsoluteUri)), relativeOrAbsoluteUri, out result)) {
                    return true;
                }
            } else {
                return true;
            }

            return false;
        }

        private string GetBaseUrl(string relativeOrAbsoluteUri) {
            var baseUrl = sourceClient.BaseUrl;
            if (relativeOrAbsoluteUri.IndexOf("reservation") > -1) {
                if (baseUrl.IndexOf("beta") < 0) {
                    baseUrl = "https://api.familysearch.org";
                }
            }

            return baseUrl;
        }

        /// <summary>
        ///   Refresh this descriptor using the specified client.
        /// </summary>
        /// <param name='client'>
        ///   The client.
        /// </param>
        public bool Refresh() {
            try {
                Initialize(sourceClient, sourcePath);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static bool InvalidResponse(IRestResponse response) {
            var session = new SessionDO();
            return InvalidResponse(response, ref session);
        }

        public static bool InvalidResponse(IRestResponse response, ref SessionDO session) {
            var invalidResponse = false;
            session.Message = null;
            session.ResponseMessage = null;
            session.MessageType = null;

            session.StatusCode = response.StatusCode.ToString();
            session.ResponseStatus = response.StatusCode.ToString();
            if ((response.ResponseStatus != ResponseStatus.Completed) || (response.ErrorException != null) || response.StatusCode < HttpStatusCode.OK || response.StatusCode >= HttpStatusCode.MultipleChoices || response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                    session.ResponseMessage = "Unauthorized call to FamilySearch Rest Service";
                    session.MessageType = Constants.MESSAGE_TYPE_ERROR;
                    invalidResponse = true;
                } else if (response.StatusCode.Equals(HttpStatusCode.BadRequest)) {
                    session.ResponseMessage = "Bad request made to FamilySearch Rest Service";
                    session.MessageType = Constants.MESSAGE_TYPE_ERROR;
                    invalidResponse = true;
                } else if (!string.IsNullOrEmpty(response.ErrorMessage)) {
                    session.ResponseMessage = response.ErrorMessage;
                    session.MessageType = Constants.MESSAGE_TYPE_ERROR;
                    invalidResponse = true;
                } else {
                    foreach (var header in response.Headers) {
                        if (header.Name.Equals("Warning")) {
                            session.ResponseMessage = header.Value + "; ";
                            session.MessageType = Constants.MESSAGE_TYPE_WARNING;
                            break;
                        }
                    }
                    if (!invalidResponse) {
                        session.ResponseMessage = response.ErrorMessage;
                        session.MessageType = Constants.MESSAGE_TYPE_ERROR;
                        invalidResponse = true;
                    }
                }

                if (!invalidResponse && response.ResponseUri != null) {
                    var uri = response.ResponseUri.ToString();
                    if (uri != null) {
                        if ((uri.IndexOf('{') > -1) && (response.StatusCode == HttpStatusCode.NotFound) && (uri.IndexOf("access_token") < 0)) {
                            var parameter = uri.Substring(uri.IndexOf('{'), (uri.IndexOf('}') - uri.IndexOf('{') + 1));
                            session.ResponseMessage = response.StatusDescription + ": Missing data in parameter: " + parameter + " for Uri: " + response.ResponseUri;
                            session.MessageType = Constants.MESSAGE_TYPE_ERROR;
                            invalidResponse = true;
                        } else {
                            session.ResponseMessage = response.StatusCode + " for Uri: " + response.ResponseUri;
                            session.MessageType = Constants.MESSAGE_TYPE_ERROR;
                            invalidResponse = true;
                        }
                    }
                }
            }

            return invalidResponse;
        }
    }
}