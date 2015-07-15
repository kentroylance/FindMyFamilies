using System;
using System.Collections.Generic;
using System.Net;
using FindMyFamilies.Data;
using FindMyFamilies.Helper;
using FindMyFamilies.Util;
using Gx;
using RestSharp;

namespace FindMyFamilies.DataAccess {
    /// <summary>
    ///     Purpose: Data Access Class for accessing Ancestry Information from familysearch.org
    /// </summary>
    public class AncestryDAO {
        //        GET /platform/throttled
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx TestThrottle(ref SessionDO session) {
            Gedcomx gedcomx = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }
            RestClient restClient = null;
            RestRequest request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_THROTTLE, ref session, ref restClient);

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                IRestResponse<Gedcomx> response = restClient.Execute<Gedcomx>(request);
                if (response.StatusCode == (HttpStatusCode) 404) {
                    string test = "";
                }

                string content = response.Content;

                IRestResponse<Gedcomx> response2 = restClient.Execute<Gedcomx>(request);
                if (response.StatusCode != (HttpStatusCode) 429) {
                    string test = "";
                }
                IRestResponse<Gedcomx> response3 = restClient.Execute<Gedcomx>(request);
                if (response.StatusCode != (HttpStatusCode) 429) {
                    string test = "";
                }

                //response.ResponseStatus != ResponseStatus.Completed

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!session.Error & (response.Data.Persons == null || response.Data.Persons.Count < 1)) {
                        session.ErrorMessage = "Error getting person from the server";
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ErrorMessage = "The API descriptor doesn't have a link to the GEDCOM X person resource.";
            }

            return gedcomx;
        }

        //        GET /platform/tree/ancestry?person=PM12-345&spouse=PW12-345
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetPersonAncestryWithSpouse(String personId, String spouseId, String generations, ref SessionDO session) {
            Gedcomx gedcomx = null;
            if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(spouseId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var variables = new Dictionary<string, string>();
                variables.Add(Constants.TEMPLATE_ID_PERSON, personId);
                variables.Add(Constants.TEMPLATE_ID_PERSON_SPOUSE, spouseId);
                variables.Add(Constants.TEMPLATE_ID_GENERATIONS, generations);
                RestClient restClient = null;
                RestRequest request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_ANCESTRY_WITH_SPOUSE, variables, ref session, ref restClient);

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    IRestResponse<Gedcomx> response = restClient.Execute<Gedcomx>(request);

                    //response.ResponseStatus != ResponseStatus.Completed

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!session.Error & (response.Data.Persons == null || response.Data.Persons.Count < 1)) {
                            session.ErrorMessage = "Error getting person from the server";
                        }
                        //                    session.Response = response;
                    } else {
                        gedcomx = response.Data;
                    }
                } else {
                    session.ErrorMessage = "The API descriptor doesn't have a link to the GEDCOM X person resource.";
                }
            }
            return gedcomx;
        }

        //        GET /platform/tree/descendancy?person=PM12-345&spouse=PW12-345
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public GedcomxPerson GetPersonDescendancy(String personId, String spouseId, String generations, ref SessionDO session) {
            GedcomxPerson gedcomx = null;
            if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(spouseId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var variables = new Dictionary<string, string>();
                variables.Add(Constants.TEMPLATE_ID_PERSON, personId);
                variables.Add(Constants.TEMPLATE_ID_PERSON_SPOUSE, spouseId);
                variables.Add(Constants.TEMPLATE_ID_GENERATIONS, generations);
                RestClient restClient = null;
                RestRequest request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_DESCENDANTS, variables, ref session, ref restClient);

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    IRestResponse<GedcomxPerson> response = restClient.Execute<GedcomxPerson>(request);

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!session.Error & (response.Data.Persons == null || response.Data.Persons.Count < 1)) {
                            session.ErrorMessage = "Error getting person from the server";
                        }
                        //                    session.Response = response;
                    } else {
                        gedcomx = response.Data;
                    }
                } else {
                    session.ErrorMessage = "The API descriptor doesn't have a link to the GEDCOM X person resource.";
                }
            }
            return gedcomx;
        }

        //        GET /platform/tree/descendancy?person=PM12-345&spouse=PW12-345
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public GedcomxPerson GetPersonDescendancy(String personId, String generations, ref SessionDO session) {
            GedcomxPerson gedcomx = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var variables = new Dictionary<string, string>();
                variables.Add(Constants.TEMPLATE_ID_PERSON, personId);
                // variables.Add(Constants.TEMPLATE_ID_GENERATIONS, generations);
                RestClient restClient = null;
                RestRequest request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_DESCENDANTS, variables, ref session, ref restClient);

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    IRestResponse<GedcomxPerson> response = restClient.Execute<GedcomxPerson>(request);

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!session.Error & (response.Data.Persons == null || response.Data.Persons.Count < 1)) {
                            session.ErrorMessage = "Error getting person from the server";
                        }
                        //                    session.Response = response;
                    } else {
                        gedcomx = response.Data;
                    }
                } else {
                    session.ErrorMessage = "The API descriptor doesn't have a link to the GEDCOM X person resource.";
                }
            }
            return gedcomx;
        }

        //        GET /platform/tree/ancestry?person={pid}&personDetails&marriageDetails
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public GedcomxPerson GetPersonAncestry(String personId, String generations, ref SessionDO session) {
            GedcomxPerson gedcomx = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var variables = new Dictionary<string, string>();
                variables.Add(Constants.TEMPLATE_ID_PERSON, personId);
                int generationParameter = 0;
                if (Convert.ToInt32(generations) > 0) {
                    generationParameter = Convert.ToInt32(generations) - 1;
                }
                variables.Add(Constants.TEMPLATE_ID_GENERATIONS, generationParameter.ToString());
                RestClient restClient = null;
                RestRequest request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_ANCESTRY, variables, ref session, ref restClient);

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    IRestResponse<GedcomxPerson> response = restClient.Execute<GedcomxPerson>(request);

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!session.Error & (response.Data.Persons == null || response.Data.Persons.Count < 1)) {
                            session.ErrorMessage = "Error getting person ancestry from the server";
                        }
                        //                    session.Response = response;
                    } else {
                        gedcomx = response.Data;
                    }
                    //                session.Response = response;
                } else {
                    session.ErrorMessage = "The API descriptor doesn't have a link to the GEDCOM X person ancestry resource.";
                }
            }
            return gedcomx;
        }
    }
}