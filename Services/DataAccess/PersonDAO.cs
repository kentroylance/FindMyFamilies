using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using FindMyFamilies.Data;
using FindMyFamilies.Exceptions;
using FindMyFamilies.Helper;
using FindMyFamilies.Util;
using Gx;
using Gx.Fs;
using Gx.Fs.Tree;
using Gx.Types;
using Model.Api;
using Model.Pd;
using RestSharp;
using Fact = Gx.Conclusion.Fact;
using Gedcomx = Gx.Gedcomx;
using Person = Gx.Conclusion.Person;

namespace FindMyFamilies.DataAccess {
    /// <summary>
    ///   Purpose: Data Access Class for accessing Person Information from familysearch.org
    /// </summary>
    public class PersonDAO {
        private static readonly bool[] _specialCharacters;
        private static readonly bool[] _alphaCharacters;

        private readonly Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

        static PersonDAO() {
            _specialCharacters = new bool[35535];
            _alphaCharacters = new bool[35535];
            //            for (char c = '0'; c <= '9'; c++) {
            //                _lookup[c] = true;
            //            }
            for (char c = 'A'; c <= 'Z'; c++) {
                _alphaCharacters[c] = true;
            }
            for (char c = 'a'; c <= 'z'; c++) {
                _alphaCharacters[c] = true;
            }
            _specialCharacters['.'] = true;
            _specialCharacters['_'] = true;
            //            _specialCharacters['/'] = true;
            //            _specialCharacters['\\'] = true;
            _specialCharacters['<'] = true;
            _specialCharacters['>'] = true;
        }

        //        GET /platform/tree/descendancy?person=PM12-345&spouse=PW12-345&personDetails&marriageDetails
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetPersonDescendancyWithSpouse(string personId, string spouseId, string generations, ref SessionDO session) {
            Gedcomx gedcomx = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
            inputs.Add(Constants.TEMPLATE_ID_PERSON_SPOUSE, spouseId);
            inputs.Add(Constants.TEMPLATE_ID_GENERATIONS, generations);

            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_DESCENDANCY_WITH_SPOUSE_MARRIAGE_DETAILS, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while retrieving person descendancy with spouse from FamilySearch.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));

                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while retrieving person descendancy with spouse from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieve person descendancy with spouse from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error retrieving person descendancy with spouse from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        } else if (response.Data.Persons == null) {
                            session.ResponseData = "Person descendancy with spouse is null";
                        } else if (response.Data != null && response.Data.Persons != null && response.Data.Persons.Count < 1) {
                            session.ResponseData = "Person descendancy with spouse is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the GEDCOM X person resource.";
                session.ErrorMessage = "Received error retrieving person descendancy with spouse from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, null, session, new StackTrace(true).GetFrames(), null);
            }

            return gedcomx;
        }

        //        GET /platform/tree/descendancy?person=PM12-345&spouse=PW12-345&personDetails&marriageDetails
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetPersonAncestryWithSpouse(string personId, string spouseId, string generations, ref SessionDO session) {
            Gedcomx gedcomx = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
            inputs.Add(Constants.TEMPLATE_ID_PERSON_SPOUSE, spouseId);
            inputs.Add(Constants.TEMPLATE_ID_GENERATIONS, generations);

            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_ANCESTRY_WITH_SPOUSE_MARRIAGE_DETAILS, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while retrieving person ancestry with spouse from FamilySearch.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));

                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while retrieving person ancestry with spouse from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieve person ancestry with spouse from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error retrieving person ancestry with spouse from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        } else if (response.Data != null && response.Data.Persons == null) {
                            session.ResponseData = "Person ancestry with spouse is null";
                        } else if (response.Data != null && response.Data.Persons != null && response.Data.Persons.Count < 1) {
                            session.ResponseData = "Person ancestry with spouse is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_ANCESTRY_WITH_SPOUSE_MARRIAGE_DETAILS;
                session.ErrorMessage = "Received error retrieving person ancestry with spouse from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            return gedcomx;
        }

        //        GET /platform/tree/descendancy?person=PM12-345&spouse=PW12-345&personDetails&marriageDetails
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetDescendancy(string personId, string spouseId, ref SessionDO session) {
            Gedcomx gedcomx = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_DESCENDANTS, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while retrieving descendants from FamilySearch.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));

                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while retrieving descendants from FamilySearch";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieve descendants from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error retrieving descendants from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        } else if (response.Data != null && response.Data.Persons == null) {
                            session.ResponseData = "Descendants is null";
                        } else if (response.Data != null && response.Data.Persons != null && response.Data.Persons.Count < 1) {
                            session.ResponseData = "Descendants is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_DESCENDANTS;
                session.ErrorMessage = "Received error retrieving descendants from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }
            return gedcomx;
        }

        //GET /platform/tree/persons/PPPJ-MYZ
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public OrdinanceDO GetOrdinances(PersonDO personDO, ref SessionDO session) {
            Reservation reservation = null;
            if (!string.IsNullOrEmpty(personDO.Id)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personDO.Id);
                inputs.Add(Constants.TEMPLATE_ID_ACCESS_TOKEN, session.AccessToken);

                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<Reservation> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_TEMPLE, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving ordinance info from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", "application/json");
                    //                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));

                    try {
                        response = restClient.Execute<Reservation>(request);
                    } catch (Exception e) {
                        var message = "Failed to execute REST request while retrieving ordinance info from FamilySearch.";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.persons == null)))) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve ordinance info from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving ordinance info from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data != null && response.Data.persons == null) {
                                session.ResponseData = "Ordinance info is null";
                            } else if (response.Data != null && response.Data.persons != null && response.Data.persons.count < 1) {
                                session.ResponseData = "Ordinance info is empty";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        reservation = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_TEMPLE;
                    session.ErrorMessage = "Received error retrieving ordinance info from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }

            OrdinanceDO ordinanceDO = null;
            if (reservation != null) {
                ordinanceDO = getOrdinance(reservation);
                ordinanceDO.Id = personDO.Id;
                ordinanceDO.Fullname = personDO.Fullname;
            }

            return ordinanceDO;
        }

        //GET /platform/tree/persons/12345/matches
        //Accept: application/atom+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public PossibleDuplicateDO GetPossibleDuplicates(PersonDO personDO, ref SessionDO session) {
            PossibleDuplicate possibleDuplicate = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_PERSON, personDO.Id);
            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<PossibleDuplicate> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_DUPLICATES, inputs, ref session, ref restClient);
            } catch (Exception e) {
                logger.Error(e.Message, e);
                var message = "Failed to create REST request while retrieving possible duplicate info from FamilySearch";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }


            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));

                try {
                    response = restClient.Execute<PossibleDuplicate>(request);
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    string message = "Failed to execute REST request while retrieving possible duplicate info from FamilySearch";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                logger.Error("StatusCode: " + response.StatusCode);
                logger.Error("Data: " + response.Data);
                logger.Error("ErrorMessage: " + session.ErrorMessage);
                logger.Error("ResponseData: " + session.ResponseData);

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieve possible duplicates from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error retrieving possible duplicates from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        } else if (response.Data == null) {
                            session.ResponseData = "Possible duplicates info is null";
                        } else if (response.Data.entries != null && Convert.ToInt16(response.Data.results) < 1) {
                            session.ResponseData = "Possible duplicates info is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    possibleDuplicate = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_DUPLICATES;
                session.ErrorMessage = "Received error retrieving possible duplicate info from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            PossibleDuplicateDO possibleDuplicateDO = null;
            if (possibleDuplicate != null) {
                possibleDuplicateDO = getPossibleDuplicate(possibleDuplicate);
                possibleDuplicateDO.Id = personDO.Id;
                possibleDuplicateDO.Fullname = personDO.Fullname;
            }

            return possibleDuplicateDO;
        }

        //GET tree-data/search/by-id/LH8P-DMT
        //Accept: application/atom+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public List<FindListItemDO> FindPersons(PersonDO personDO, ref SessionDO session) {
            Search search = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            //            inputs.Add(Constants.TEMPLATE_ID_QUERY, "fatherSurname:Heaton~+spouseSurname:Cox~+surname:Heaton~+givenName:Israel~+birthPlace:~+deathDate:~+deathPlace:~+spouseGivenName:~+motherGivenName:~+motherSurname:~+gender:Male~+birthDate:~+fatherGivenName:~&count=10");
            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Search> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_SEARCH, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while finding persons from FamilySearch.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            //            request.Resource =  request.Resource + "q=givenName:John surname:Smith gender:male birthDate:\"30 June 1900\"";
            request.Resource = request.Resource + "q=";
            bool given = false;
            if (!string.IsNullOrEmpty(personDO.Firstname)) {
                request.Resource += "givenName:\"" + personDO.Firstname;
                given = true;
            }
            if (!string.IsNullOrEmpty(personDO.Middlename)) {
                request.Resource += " " + personDO.Middlename + "\" ";
            } else if (given) {
                request.Resource += "\" ";
            }
            if (!string.IsNullOrEmpty(personDO.Lastname)) {
                request.Resource += "surname:\"" + personDO.Lastname + "\" ";
            }
            if (personDO.BirthYear > 0) {
                request.Resource += "birthDate:" + personDO.BirthYear + " ";
            }
            if (personDO.DeathYear > 0) {
                request.Resource += "deathDate:" + personDO.DeathYear + " ";
            }
            if (personDO.IsMale) {
                request.Resource += "gender:male";
            } else {
                request.Resource += "gender:female";
            }

            //            request.Resource += "birthDate:1920" + "\u2011" + "1923"; // + "~";
            //            request.Resource += "deathDate:2004";

            request.Method = Method.GET;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));

            try {
                response = restClient.Execute<Search>(request);
            } catch (Exception e) {
                string message = "Failed to execute REST request while finding persons from FamilySearch";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (RestHelper.InvalidResponse(response, ref session)) {
                if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                    if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                        session.ErrorMessage = "Unauthorized to find persons from FamilySearch";
                    } else {
                        session.ErrorMessage = "Received error finding persons from FamilySearch";
                    }
                    if (response.Data == null) {
                        session.ResponseData = "Data is Null";
                    } else if (response.Data.entries == null) {
                        session.ResponseData = "Find person data is null";
                    } else if (response.Data.entries != null && response.Data.entries.Count < 1) {
                        session.ResponseData = "Find person data is empty";
                    }
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            } else {
                search = response.Data;
            }

            var persons = new Dictionary<string, PersonDO>();
            var findPersons = new List<FindListItemDO>();
            if (search != null) {
                if ((search.entries != null) && (search.entries.Count > 0)) {
                    foreach (var entry in search.entries) {
                        persons = new Dictionary<string, PersonDO>();
                        var findListItemDO = new FindListItemDO();
                        findListItemDO.id = entry.id;
                        if ((entry.content != null) && (entry.content.gedcomx != null)) {
                            if ((entry.content.gedcomx.persons != null) && (entry.content.gedcomx.persons.Count > 0)) {
                                foreach (var person in entry.content.gedcomx.persons) {
                                    var personEntryDO = GetPersonFromSearch(person);
                                    if (personEntryDO != null) {
                                        AddPerson(personEntryDO, persons);
                                    }
                                }
                            }
                            if ((entry.content.gedcomx.relationships != null) && (entry.content.gedcomx.relationships.Count > 0)) {
                                foreach (var relationship in entry.content.gedcomx.relationships) {
                                    updateRelationships(findListItemDO.id, relationship, ref persons);
                                }
                            }
                        }
                        if (persons.ContainsKey(findListItemDO.id)) {
                            var findPersonDO = persons[findListItemDO.id];
                            PopulateFindListItem(findPersonDO, ref findListItemDO);
                        }
                        //                        findListItemDO.Score = entry.score;
                        //                        findListItemDO.Confidence = entry.confidence;

                        findPersons.Add(findListItemDO);
                    }
                }
            }

            return findPersons;
        }

        public void PopulateFindListItem(PersonDO findPersonDO, ref FindListItemDO findListItemDO) {
            findListItemDO.id = findPersonDO.Id;
            findListItemDO.fullName = findPersonDO.Fullname;
            findListItemDO.firstName = findPersonDO.Firstname;
            findListItemDO.middleName = findPersonDO.Middlename;
            findListItemDO.lastName = findPersonDO.Lastname;
            var birthDate = Dates.GetDateTime(findPersonDO.BirthDateString);
            if ((birthDate != null) && (birthDate.Value.Year != 1)) {
                findListItemDO.birthYear = birthDate.Value.Year.ToString();
            } else {
                findListItemDO.birthYear = findPersonDO.BirthDateString;
            }
            findListItemDO.birthYear = (findListItemDO.birthYear == null) ? "" : findListItemDO.birthYear;
            findListItemDO.gender = (findPersonDO.IsMale) ? "Male" : "Female";
            var deathDate = Dates.GetDateTime(findPersonDO.DeathDateString);
            if ((deathDate != null) && (deathDate.Value.Year != 1)) {
                findListItemDO.deathYear = deathDate.Value.Year.ToString();
            } else {
                findListItemDO.deathYear = findPersonDO.DeathDateString;
            }
            findListItemDO.deathYear = (findListItemDO.deathYear == null) ? "" : findListItemDO.deathYear;
            findListItemDO.birthPlace = findPersonDO.BirthPlace;
            findListItemDO.deathPlace = findPersonDO.DeathPlace;
            if (!findPersonDO.Father.IsEmpty) {
                findListItemDO.fatherName = findPersonDO.Father.Fullname;
            }
            if (!findPersonDO.Mother.IsEmpty) {
                findListItemDO.motherName = findPersonDO.Mother.Fullname;
            }
            if (!findPersonDO.Spouse.IsEmpty) {
                findListItemDO.spouseName = findPersonDO.Spouse.Fullname;
                findListItemDO.spouseGender = (findPersonDO.Spouse.IsMale) ? "Male" : "Female";
            }
            //                        findListItemDO.Score = entry.score;
            //                        findListItemDO.Confidence = entry.confidence;
        }

        private void AddPerson(PersonDO personDo, Dictionary<string, PersonDO> persons) {
            if (!persons.ContainsKey(personDo.Id)) {
                persons.Add(personDo.Id, personDo);
            }
        }

        public void updateRelationships(string personID, Model.Pd.Relationship relationoship, ref Dictionary<string, PersonDO> persons) {
            if (relationoship.type.Equals("http://gedcomx.org/Couple")) {
                if (persons.ContainsKey(relationoship.person1.resourceId) && persons.ContainsKey(relationoship.person2.resourceId)) {
                    persons[personID].Spouse = persons[relationoship.person2.resourceId];
                }
            } else if (relationoship.type.Equals("http://gedcomx.org/ParentChild")) {
                if (persons.ContainsKey(relationoship.person1.resourceId) && persons.ContainsKey(relationoship.person2.resourceId)) {
                    if (persons[relationoship.person1.resourceId].IsMale) {
                        persons[personID].Father = persons[relationoship.person1.resourceId];
                    } else {
                        persons[personID].Mother = persons[relationoship.person1.resourceId];
                    }
                }
            }
        }

        public PersonDO GetPersonFromSearch(Model.Pd.Person person) {
            PersonDO personDO = null;
            try {
                if (!string.IsNullOrEmpty(person.display.name)) {
                    personDO = new PersonDO();
                    personDO.Id = person.id;
                    if (person.gender != null) {
                        if (person.gender.type.Equals("http://gedcomx.org/Male")) {
                            personDO.Gender = Constants.MALE;
                        } else {
                            personDO.Gender = Constants.FEMALE;
                        }
                    }
                    personDO.Living = person.living;
                    personDO.BirthDateString = person.display.birthDate;
                    personDO.DeathDateString = person.display.deathDate;
                    personDO.BirthPlace = person.display.birthPlace;

                    personDO.Fullname = person.display.name;
                    var names = person.display.name.Split(' ');
                    if (names.Length == 2) {
                        personDO.Firstname = names[0];
                        personDO.Lastname = names[1];
                    } else if (names.Length == 3) {
                        personDO.Firstname = names[0];
                        personDO.Middlename = names[1];
                        personDO.Lastname = names[2];
                    } else if (names.Length > 3) {
                        personDO.Firstname = names[0];
                        var middle = "";
                        for (var i = 1; i < names.Length - 1; i++) {
                            middle = middle + " " + names[i];
                        }
                        personDO.Middlename = names[1];
                        personDO.Lastname = names[names.Length - 1];
                    }

                    personDO.DeathPlace = person.display.deathPlace;
                }
            } catch (Exception e) {
                logger.Error("Cannot Convert Person GX to Person: " + e.Message, e);
                throw e;
            }
            return personDO;
        }

        //GET /tree-data/record-matches/{pid}{?access_token}
        //Accept: application/atom+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public HintDO GetHints(PersonDO personDO, ref SessionDO session) {
            Hint hint = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_PERSON, personDO.Id);
            inputs.Add(Constants.TEMPLATE_ID_COLLECTION, "https://familysearch.org/platform/collections/records");
            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Hint> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_HINTS, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while retrieving hints from FamilySearch.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));

                try {
                    response = restClient.Execute<Hint>(request);
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    string message = "Failed to execute REST request while retrieving hints from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                logger.Error("StatusCode: " + response.StatusCode);
                logger.Error("Data: " + response.Data);
                logger.Error("ErrorMessage: " + session.ErrorMessage);
                logger.Error("ResponseData: " + session.ResponseData);

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieve hints from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is null";
                        } else if ((response.Data.results == null) || (response.Data.entries == null)) {
                            session.ResponseData = "Hints entries are null";
                        } else if (response.Data.results != null && ((response.Data.entries != null) && response.Data.entries.Count < 1)) {
                            session.ResponseData = "Hints is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    hint = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_HINTS;
                session.ErrorMessage = "Received error retrieving hints from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            HintDO hintDO = null;
            if (hint != null) {
                hintDO = getHint(hint);
                hintDO.Id = personDO.Id;
                hintDO.Fullname = personDO.Fullname;
            }

            return hintDO;
        }

        public HintDO getHint(Hint hint) {
            var hintDO = new HintDO();

            if (hint != null) {
                //                hintDO.Id = hint.id;
                //                hintDO.Title = hint.title;
                hintDO.Results = hint.results;
                if ((hint.results > 0) && (hint.entries != null)) {
                    foreach (var entry in hint.entries) {
                        HintEntryDO hintEntry = new HintEntryDO();
                        hintEntry.Fullname = entry.title;
                        hintEntry.Score = entry.score;
                        hintDO.Score = entry.score;
                        if (entry.score > 0) {
                            hintDO.Duplicates = true;
                        } else {
                            hintDO.Matches = true;
                        }
                        hintEntry.Id = entry.id;
                        if (entry.content.gedcomx.persons != null) {
                            foreach (var person2 in entry.content.gedcomx.persons) {
                                PersonDO person = new PersonDO();
                                person.Id = person2.id;
                                person.Fullname = person2.display.name;
                                hintEntry.Persons.Add(person2.id, person);
                            }
                        }
                        hintDO.Entries.Add(hintEntry);
                    }
                }
            }

            return hintDO;
        }

        public PossibleDuplicateDO getPossibleDuplicate(PossibleDuplicate possibleDuplicate) {
            var possibleDuplicateDO = new PossibleDuplicateDO();

            if (possibleDuplicate != null) {
                possibleDuplicateDO.Id = possibleDuplicate.id;
                possibleDuplicateDO.Title = possibleDuplicate.title;
                possibleDuplicateDO.Results = possibleDuplicate.results;
                if ((possibleDuplicate.results > 0) && (possibleDuplicate.entries != null)) {
                    foreach (var entry in possibleDuplicate.entries) {
                        var possibleDuplicateEntry = new PossibleDuplicateEntryDO();
                        possibleDuplicateEntry.Fullname = entry.title;
                        possibleDuplicateEntry.Score = entry.score;
                        possibleDuplicateDO.Score = entry.score;
                        if (entry.score > 0) {
                            possibleDuplicateDO.Duplicates = true;
                        } else {
                            possibleDuplicateDO.Matches = true;
                        }
                        possibleDuplicateEntry.Id = entry.id;
                        if (entry.content.gedcomx.persons != null) {
                            foreach (var person2 in entry.content.gedcomx.persons) {
                                var person = new PersonDO();
                                person.Id = person2.id;
                                person.Fullname = person2.display.name;
                                possibleDuplicateEntry.Persons.Add(person2.id, person);
                            }
                        }
                        possibleDuplicateDO.Entries.Add(possibleDuplicateEntry);
                    }
                }
            }

            return possibleDuplicateDO;
        }

        //GET /platform/tree/current-person
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetCurrentPerson(ref SessionDO session) {
            Gedcomx gedcomx = null;
            logger.Enter("PersonDAO.GetCurrentPerson");

            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_ACCESS_TOKEN, session.AccessToken);

            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_CURRENT, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while retrieving retrieving current person from FamilySearch.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
//                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while retrieving current person from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieve current person from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error retrieving current person from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        } else if (response.Data.Persons == null) {
                            session.ResponseData = "Current person is null";
                        } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                            session.ResponseData = "Current person is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_CURRENT;
                session.ErrorMessage = "Received error retrieving current person from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            return gedcomx;
        }

        public void LogError(string message, ref SessionDO session) {
            logger.Error(message + ": " + session.MessageType + ": " + session.ResponseMessage);
            session.Error = true;
            session.ErrorMessage = message;
        }

        //GET /platform/tree/current-person
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public bool IsChurchMember(ref SessionDO session) {
            var isChurchMember = true;
            logger.Enter("PersonDAO.IsChurchMember");

            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            //            inputs.Add(Constants.TEMPLATE_ID_ACCESS_TOKEN, session.AccessToken);

            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_ACCESS_TO_ORDINANCES, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while trying to determine if church member.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while trying to determine if church member.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to determine if church member from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error while trying to determine if church member.";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    if (response.StatusCode == HttpStatusCode.Forbidden) {
                        isChurchMember = false;
                    }
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_ACCESS_TO_ORDINANCES;
                session.ErrorMessage = "Received error to determine if church member from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            return isChurchMember;
        }

        //DELETE /cis-web/oauth2/v3/token?access_token=2YoTnFdFEjr1zCsicMWpAA
        //Content-Type: */*
        public Gedcomx Logout(ref SessionDO session) {
            Gedcomx gedcomx = null;

            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_ACCESS_TOKEN, session.AccessToken);

            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_INVALIDATE_TOKEN, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while trying to log out from FamilySearch.";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.DELETE;
                request.AddHeader("Accept", "");
                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while trying to log out from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to logout from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error while trying to log out from FamilySearch.";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_INVALIDATE_TOKEN;
                session.ErrorMessage = "Received error logging out from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            return gedcomx;
        }

        /// platform/tree/ancestry?person=PS11-234&personDetails
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetPersonWithDetails(string personId, ref SessionDO session) {
            Gedcomx gedcomx = null;
            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
            inputs.Add(Constants.TEMPLATE_ID_ACCESS_TOKEN, session.AccessToken);

            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_WITH_DETAILS, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while retrieving person with details from FamilySearch";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_GEDCOM);
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while retrieving person with details from FamilySearch";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieve person with details from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error retrieving person with details from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        } else if (response.Data.Persons == null) {
                            session.ResponseData = "Current person is null";
                        } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                            session.ResponseData = "Current person is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_WITH_DETAILS;
                session.ErrorMessage = "Received error retrieving person with details from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            return gedcomx;
        }

        ////platform/tree/persons-with-relationships?person=PW8J-MZ0
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetPersonWithRelationships(string personId, ref SessionDO session) {
            Gedcomx gedcomx = null;

            if (RestHelper.Instance.Expired) {
                RestHelper.Instance.Refresh();
            }

            var inputs = new Dictionary<string, string>();
            inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
            inputs.Add(Constants.TEMPLATE_ID_ACCESS_TOKEN, session.AccessToken);

            RestClient restClient = null;
            RestRequest request = null;
            IRestResponse<Gedcomx> response = null;

            try {
                request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_WITH_RELATIONSHIPS, inputs, ref session, ref restClient);
            } catch (Exception e) {
                var message = "Failed to create REST request while retrieving person with relationships from FamilySearch";
                LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                throw new DataAccessException(message, e, session);
            }

            if (request != null) {
                request.Method = Method.GET;
                request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                try {
                    response = restClient.Execute<Gedcomx>(request);
                } catch (Exception e) {
                    string message = "Failed to execute REST request while retrieving person with relationships from FamilySearch";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (RestHelper.InvalidResponse(response, ref session)) {
                    if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                        if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                            session.ErrorMessage = "Unauthorized to retrieving person with relationships from FamilySearch";
                        } else {
                            session.ErrorMessage = "Received error retrieving person with relationships from FamilySearch";
                        }
                        if (response.Data == null) {
                            session.ResponseData = "Data is Null";
                        } else if (response.Data.Persons == null) {
                            session.ResponseData = "Current person is null";
                        } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                            session.ResponseData = "Current person is empty";
                        }
                        LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                        throw new DataAccessException(session.ErrorMessage, null, session);
                    }
                } else {
                    gedcomx = response.Data;
                }
            } else {
                session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_WITH_RELATIONSHIPS;
                session.ErrorMessage = "Received error retrieving person with relationships from FamilySearch";
                session.ResponseData = "Null";
                LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                throw new DataAccessException(session.ErrorMessage, null, session);
            }

            return gedcomx;
        }

        public string RemoveAlphaCharacters(string str) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }
            logger.Info("RemoveAlphaCharacters before = " + str);

            var buffer = new char[str.Length];
            int index = 0;
            foreach (char c in str) {
                if (_alphaCharacters[c]) {
                    buffer[index] = c;
                    index++;
                }
            }
            var result = new string(buffer, 0, index);
            logger.Info("RemoveAlphaCharacters after = " + result);
            return result;
        }

        public string RemoveSpecialCharacters(string str) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }
            //                logger.Info("RemoveSpecialCharacters before = " + str);
            //                str = str.ToLower();
            str = str.Trim();
            //                if (str.IndexOf("abt") > -1) {
            //                    str = str.Replace("abt", "");
            //                } else if (str.IndexOf("about") > -1) {
            //                    str = str.Replace("about", "");
            //                } else if (str.IndexOf("dead") > -1) {
            //                    str = str.Replace("dead", "");
            //                }
            //                logger.Info("RemoveSpecialCharacters between = " + str);
            foreach (char c in str) {
                if (_specialCharacters[c]) {
                    str = str.Replace(c.ToString(), "");
                }
            }
            logger.Info("RemoveSpecialCharacters after = " + str);
            return str;
        }

        private void SetDeathDate(ref PersonDO personDO, Gx.Conclusion.Person person) {
            var deathDate = RemoveSpecialCharacters(person.Display.DeathDate);
            if (!string.IsNullOrEmpty(deathDate)) {
                if (deathDate.Length == 4) {
                    int valueParsed;
                    if (Int32.TryParse(deathDate, NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                        personDO.DeathYear = valueParsed;
                    } else {
                        personDO.DeathDateString = deathDate;
                    }
                } else {
                    personDO.DeathDate = Dates.GetDateTime(deathDate);
                }
                if ((personDO.DeathYear < 2) && (personDO.DeathDate.Value.Year > 1)) {
                    personDO.DeathYear = personDO.DeathDate.Value.Year;
                }

                if ((personDO.DeathDate == null) || (personDO.DeathYear < 2) || ((personDO.DeathDate != null) && (personDO.DeathDate.Value.Year < 2))) {
                    if ((deathDate.IndexOf("/") == 4) || (deathDate.IndexOf("\\") == 4)) {
                        int valueParsed;
                        if (Int32.TryParse(deathDate.Substring(0, 4), NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                            personDO.DeathYear = valueParsed;
                        } else {
                            personDO.DeathDateString = deathDate;
                        }
                    }
                    if ((personDO.DeathDate == null) || (personDO.DeathYear < 2) || ((personDO.DeathDate != null) && (personDO.DeathDate.Value.Year < 2))) {
                        if (deathDate.Length > 3) {
                            int valueParsed;
                            if (Int32.TryParse(Strings.Right(deathDate, 4), NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                                personDO.DeathYear = valueParsed;
                            } else {
                                personDO.DeathDateString = deathDate;
                            }
                        }
                    } else {
                        if (personDO.DeathYear < 2) {
                            personDO.DeathDateString = deathDate;
                            string deathYearFact = GetFactPart("DeathYear", person);
                            deathYearFact = RemoveSpecialCharacters(deathYearFact);
                            if (!string.IsNullOrEmpty(deathYearFact)) {
                                if (deathYearFact.Length == 4) {
                                    int valueParsed;
                                    if (Int32.TryParse(deathYearFact, NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                                        personDO.DeathYear = valueParsed;
                                    } else {
                                        personDO.DeathDateString = deathYearFact;
                                    }
                                } else {
                                    personDO.DeathDate = Dates.GetDateTime(deathYearFact);
                                }
                            }
                            //                string deathDatePart = GetFactPart("DeathDate", person);
                            //                
                            //                logger.Info("deathDatePart = " + deathDatePart);
                            //                if (!string.IsNullOrEmpty(deathDatePart) && (deathDatePart.Length > 4)) {
                            //                    personDO.DeathDate = DateProblems.GetDateTime(deathDatePart);
                            //                }
                        }
                    }
                }
            }
            logger.Info(personDO.DeathYear.ToString());
        }

        private void SetBirthDate(ref PersonDO personDO, Person person) {
            string birthDate = RemoveSpecialCharacters(person.Display.BirthDate);
            if (!string.IsNullOrEmpty(birthDate)) {
                if (birthDate.Length == 4) {
                    int valueParsed;
                    if (Int32.TryParse(birthDate, NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                        personDO.BirthYear = valueParsed;
                    } else {
                        personDO.BirthDateString = birthDate;
                    }
                } else {
                    try {
                        personDO.BirthDate = Dates.GetDateTime(birthDate);
                    } catch (Exception) {
                        personDO.DeathDate = null;
                    }
                }
                if ((personDO.BirthYear < 2) && (personDO.BirthDate.Value.Year > 1)) {
                    personDO.BirthYear = personDO.BirthDate.Value.Year;
                }

                if ((personDO.BirthDate == null) || (personDO.BirthYear < 2) || ((personDO.BirthDate != null) && (personDO.BirthDate.Value.Year < 2))) {
                    if ((birthDate.IndexOf("/") == 4) || (birthDate.IndexOf("\\") == 4)) {
                        int valueParsed;
                        if (Int32.TryParse(birthDate.Substring(0, 4), NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                            personDO.BirthYear = valueParsed;
                        } else {
                            personDO.BirthDateString = birthDate;
                        }
                    }
                    if ((personDO.BirthDate == null) || (personDO.BirthYear < 2) || ((personDO.BirthDate != null) && (personDO.BirthDate.Value.Year < 2))) {
                        if (birthDate.Length > 3) {
                            int valueParsed;
                            if (Int32.TryParse(Strings.Right(birthDate, 4), NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                                personDO.BirthYear = valueParsed;
                            } else {
                                personDO.BirthDateString = birthDate;
                            }
                        }
                    } else {
                        logger.Info(personDO.BirthYear.ToString());
                        if (personDO.BirthYear < 2) {
                            personDO.BirthDateString = birthDate;
                            string birthYearFact = GetFactPart("BirthYear", person);
                            birthYearFact = RemoveSpecialCharacters(birthYearFact);
                            if (!string.IsNullOrEmpty(birthYearFact)) {
                                if (birthYearFact.Length == 4) {
                                    int valueParsed;
                                    if (Int32.TryParse(birthYearFact, NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                                        personDO.BirthYear = valueParsed;
                                    } else {
                                        personDO.BirthDateString = birthYearFact;
                                    }
                                } else {
                                    personDO.BirthDate = Dates.GetDateTime(birthYearFact);
                                }
                            }
                            //                string birthDatePart = GetFactPart("BirthDate", person);
                            //                
                            //                logger.Info("birthDatePart = " + birthDatePart);
                            //                if (!string.IsNullOrEmpty(birthDatePart) && (birthDatePart.Length > 4)) {
                            //                    personDO.BirthDate = DateProblems.GetDateTime(birthDatePart);
                            //                }
                        }
                    }
                }
            }
            logger.Info(personDO.BirthYear.ToString());
        }

        private void SetMarriageDate(ref PersonDO personDO, Gx.Conclusion.Person person) {
            var marriageDate = RemoveSpecialCharacters(person.Display.MarriageDate);
            if (!string.IsNullOrEmpty(marriageDate)) {
                personDO.MarriageDate = Dates.GetDateTime(marriageDate);
                logger.Info(personDO.MarriageYear.ToString());
                if ((personDO.MarriageDate == null) || (personDO.MarriageYear < 2) || ((personDO.MarriageDate != null) && (personDO.MarriageDate.Value.Year < 2))) {
                    logger.Info(personDO.MarriageYear.ToString());
                    if (personDO.MarriageYear < 2) {
                        personDO.MarriageDateString = marriageDate;
                        var marriageYearFact = GetFactPart("MarriageYear", person);
                        marriageYearFact = RemoveSpecialCharacters(marriageYearFact);
                        if (!string.IsNullOrEmpty(marriageYearFact)) {
                            if (marriageYearFact.Length == 4) {
                                //                                int valueParsed;
                                //                                if (Int32.TryParse(marriageYearFact, NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                                //                                    personDO.MarriageYear = valueParsed; 
                                //                                } else {
                                personDO.MarriageDateString = marriageYearFact;
                                //                                }
                            } else {
                                personDO.MarriageDate = Dates.GetDateTime(marriageYearFact);
                            }
                        }
                        //                string marriageDatePart = GetFactPart("MarriageDate", person);
                        //                
                        //                logger.Info("marriageDatePart = " + marriageDatePart);
                        //                if (!string.IsNullOrEmpty(marriageDatePart) && (marriageDatePart.Length > 4)) {
                        //                    personDO.MarriageDate = DateProblems.GetDateTime(marriageDatePart);
                        //                }
                    }
                }
            }
        }

        public OrdinanceDO getOrdinance(Reservation reservation) {
            var ordinanceDO = new OrdinanceDO();

            if (reservation != null) {
                ordinanceDO.statusCode = reservation.statusCode;
                ordinanceDO.statusMessage = reservation.statusMessage;

                if ((reservation.persons != null) && (reservation.persons.person != null) && (reservation.persons.person.Count > 0)) {
                    ordinanceDO.Baptism = reservation.persons.person[0].baptism;
                    ordinanceDO.Confirmation = reservation.persons.person[0].confirmation;
                    ordinanceDO.Endowment = reservation.persons.person[0].endowment;
                    ordinanceDO.Initiatory = reservation.persons.person[0].initiatory;
                    if ((reservation.persons.person[0].sealingToParents != null) && (reservation.persons.person[0].sealingToParents.Count > 0)) {
                        ordinanceDO.SealedToParent = reservation.persons.person[0].sealingToParents[0];
                    }
                    if ((reservation.persons.person[0].sealingToSpouse != null) && (reservation.persons.person[0].sealingToSpouse.Count > 0)) {
                        ordinanceDO.SealedToSpouse = reservation.persons.person[0].sealingToSpouse[0];
                    }
                }
            }

            return ordinanceDO;
        }

        public PersonDO GetPerson(Gx.Conclusion.Person person) {
            //            if (person.Id.Equals("KW71-756")) {
            //                string test = "";
            //
            //            }

            PersonDO personDO = null;
            try {
                if (!string.IsNullOrEmpty(person.Display.Name)) {
                    personDO = new PersonDO();
                    if (!person.Living && person.Links != null && (person.Display.AscendancyNumber != null || person.Display.DescendancyNumber != null)) {
                        foreach (var link in person.Links) {
                            if (link.Rel.Equals("additional-spouse-relationships")) {
                                personDO.HasSpousesLink = true;
                                personDO.HasSpouseLink = true;
                            } else if (link.Rel.Equals("child-relationships")) {
                                personDO.HasChildrenLink = true;
                                personDO.HasSpouseLink = true;
                            } else if (link.Rel.Equals("additional-parent-relationships")) {
                                personDO.HasMultipleParentsLink = true;
                                personDO.HasParentsLink = true;
                            } else if (link.Rel.Equals("ancestry")) {
                                personDO.HasParentsLink = true;
                            } else if (link.Rel.Equals("descendancy")) {
                                personDO.HasChildrenLink = true;
                                personDO.HasSpouseLink = true;
                            }
                        }
                    }

                    personDO.Id = person.Id;
                    //            if (ascendancyNumber > 0) {
                    //                personDO.AscendancyNumber = ascendancyNumber;
                    //            }
                    if (person.Gender != null) {
                        if (person.Gender.KnownType == GenderType.Male) {
                            personDO.Gender = Constants.MALE;
                        } else {
                            personDO.Gender = Constants.FEMALE;
                        }
                    }
                    personDO.AscendancyNumber = person.Display.AscendancyNumber;
                    personDO.DescendancyNumber = person.Display.DescendancyNumber;
                    personDO.Living = person.Living;
                    // before 31 October 1759
                    //about 28 may 1791
                    //September 1821
                    // march 1817
                    //about 1846
                    //RemoveSpecialCharacters before = 1876 / 1876  2014-07-18 07:26:47,990 INFO [FindMyFamilies.Helper.RestHelper]: RemoveSpecialCharacters after = 1876 / 1876
                    personDO.LifeSpan = person.Display.Lifespan;

                    //                    if ((!string.IsNullOrEmpty(personDO.LifeSpan) && (personDO.LifeSpan.IndexOf("Living") > -1)) || string.IsNullOrEmpty(person.Display.DeathDate)) {
                    //                        personDO.Living = true;
                    //                    } else {
                    //                        personDO.Living = false;
                    //                    }

                    var birthDate = person.Display.BirthDate;
                    var deathDate = person.Display.DeathDate;
                    var lifeSpan = person.Display.Lifespan.ToLower();
                    if (!string.IsNullOrEmpty(lifeSpan)) {
                        int valueParsed;
                        var dash = false;
                        if (lifeSpan.IndexOf("living") > -1) {
                            personDO.BirthDateString = person.Display.BirthDate;
                        } else {
                            dash = (lifeSpan.IndexOf("-") == 4);

                            if (dash && int.TryParse(lifeSpan.Substring(0, 4), NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                                personDO.BirthYear = valueParsed;
                            } else {
                                personDO.BirthDateString = person.Display.BirthDate;
                            }
                        }

                        if (!person.Living) {
                            if (lifeSpan.IndexOf("deceased") > -1) {
                                personDO.DeathDateString = person.Display.DeathDate;
                            } else if (!person.Living && dash && (lifeSpan.Length == 9) && (lifeSpan.IndexOf("deceased") < 0) && (lifeSpan.IndexOf("living") < 0)) {
                                if (int.TryParse(lifeSpan.Substring(5, 4), NumberStyles.Any, CultureInfo.CurrentCulture, out valueParsed)) {
                                    personDO.DeathYear = valueParsed;
                                }
                            }
                            //                            else if (!person.Living && (personDO.DeathYear == 0) && dash && (lifeSpan.Length > 5) && (person.Display.DeathDate == null)) {
                            //                                personDO.DeathDateString = lifeSpan.Substring(5, lifeSpan.Length - 5);
                            //                                //                        } else if (!person.Living) {
                            //                                //                            personDO.DeathDateString = person.Display.DeathDate;
                            //                            }
                        }
                    }
                    personDO.BirthDateString = person.Display.BirthDate;
                    personDO.DeathDateString = person.Display.DeathDate;
                    personDO.MarriageDateString = person.Display.MarriageDate;
                    //                    SetBirthDate(ref personDO, person);
                    //                    if (personDO.LifeSpan.IndexOf("Deceased") < 0) {
                    //                        SetDeathDate(ref personDO, person);
                    //                    }
                    //                    SetMarriageDate(ref personDO, person);
                    //    personDO.MarriageDateString = person.Display.MarriageDate;
                    personDO.BirthPlace = person.Display.BirthPlace;
                    //                    if (!Strings.IsEmpty(person.Display.BirthPlace)) {
                    //                        personDO.BirthPlace = GetFactPart("BirthPlace", person);
                    //                    }

                    personDO.MarriagePlace = person.Display.MarriagePlace;
                    //                    if (!Strings.IsEmpty(person.Display.MarriagePlace)) {
                    //                        personDO.MarriagePlace = GetFactPart("MarriagePlace", person);
                    //                    }
                    personDO.Fullname = person.Display.Name;
                    string[] names = person.Display.Name.Split(new[] {' '});
                    if (names.Length == 2) {
                        personDO.Firstname = names[0];
                        personDO.Lastname = names[1];
                    } else if (names.Length == 3) {
                        personDO.Firstname = names[0];
                        personDO.Middlename = names[1];
                        personDO.Lastname = names[2];
                    } else if (names.Length > 3) {
                        personDO.Firstname = names[0];
                        var middle = "";
                        for (var i = 1; i < names.Length - 1; i++) {
                            middle = middle + " " + names[i];
                        }
                        personDO.Middlename = names[1];
                        personDO.Lastname = names[names.Length - 1];
                    }

                    personDO.DeathPlace = person.Display.DeathPlace;

                    //                    if (!Strings.IsEmpty(person.Display.DeathPlace)) {
                    //                        personDO.DeathPlace = GetFactPart("DeathPlace", person);
                    //                    }

                    //            if (person.Id.Equals("KW71-756")) {
                    //                bool deceased = personDO.Deceased;
                    //
                    //            }
                } else {
                    personDO = new PersonDO();
                    personDO.Id = person.Id;
                    if (person.Gender != null) {
                        if (person.Gender.KnownType == GenderType.Male) {
                            personDO.Gender = Constants.MALE;
                        } else {
                            personDO.Gender = Constants.FEMALE;
                        }
                    }
                    personDO.Living = person.Living;
                    personDO.LifeSpan = person.Display.Lifespan;
                    personDO.Fullname = "Unknown Name";
                    personDO.Firstname = "Unknown";
                    personDO.Lastname = "Name";
                }
            } catch (Exception e) {
                logger.Error("Cannot Convert PersonGX to Person: " + e.Message, e);
                throw e;
            }
            return personDO;
        }

        public DateTime? GetMarriageDate(Fact fact) {
            DateTime? marriageDate = null;
            var marriageDatePart = GetFactPart("MarriageDate", fact);
            if (!Strings.IsEmpty(marriageDatePart) && (marriageDatePart.Length > 4)) {
                marriageDate = Dates.GetDateTime(marriageDatePart);
            }
            return marriageDate;
        }

        public string GetMarriagePlace(Fact fact) {
            return GetFactPart("MarriagePlace", fact);
        }

        //        private string GetNamePart(string keyPart, Person person) {
        //            string namepart = "";
        //            if ((person.Display != null) && keyPart.Equals("Fullname")) {
        //                namepart = person.Display.Name;
        //            }
        //            foreach (Name name in person.Names) {
        //                foreach (NameForm nameForm in name.NameForms) {
        //                    if (keyPart.Equals("Fullname")) {
        //                        if (string.IsNullOrEmpty(namepart)) {
        //                            namepart = nameForm.FullText;
        //                        }
        //                        break;
        //                    }
        //                    foreach (Part part in nameForm.Parts) {
        //                        if (part.Type.Equals("http://gedcomx.org/Given")) {
        //                            if (keyPart.Equals("Firstname")) {
        //                                namepart = part.value;
        //                                break;
        //                            }
        //                        } else if (part.Type.Equals("http://gedcomx.org/Surname")) {
        //                            if (keyPart.Equals("Lastname")) {
        //                                namepart = part.value;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //
        //            return namepart;
        //        }

        private string GetFactPart(string keyPart, Fact fact) {
            var factPart = "";
            if (fact.Type.Equals("http://gedcomx.org/Birth") && (keyPart.Equals("BirthDate") || keyPart.Equals("BirthYear") || keyPart.Equals("BirthPlace"))) {
                if ((fact.Date != null) && !Strings.IsEmpty(fact.Date.Original)) {
                    if (fact.Date.Original.Length > 4) {
                        if (keyPart.Equals("BirthDate")) {
                            factPart = fact.Date.Original;
                        }
                    } else if (fact.Date.Original.Length == 4) {
                        if (keyPart.Equals("BirthYear")) {
                            factPart = fact.Date.Original;
                        }
                    }
                } else if (fact.Date == null) {
                    factPart = "";
                }
                if ((fact.Place != null) && fact.Place != null) {
                    if (keyPart.Equals("BirthPlace")) {
                        factPart = fact.Place.Original;
                    }
                }
            } else if (fact.Type.Equals("http://gedcomx.org/Death") && (keyPart.Equals("DeathDate") || keyPart.Equals("DeathYear") || keyPart.Equals("DeathPlace"))) {
                if ((fact.Date != null) && !Strings.IsEmpty(fact.Date.Original)) {
                    if (fact.Date.Original.Length > 4) {
                        if (keyPart.Equals("DeathDate")) {
                            factPart = fact.Date.Original;
                        }
                    } else if (fact.Date.Original.Length == 4) {
                        if (keyPart.Equals("DeathYear")) {
                            factPart = fact.Date.Original;
                        }
                    }
                }
                if ((fact.Place != null) && fact.Place != null) {
                    if (keyPart.Equals("DeathPlace")) {
                        factPart = fact.Place.Original;
                    }
                }
            } else if (fact.Type.Equals("http://gedcomx.org/Marriage") && (keyPart.Equals("MarriageDate") || keyPart.Equals("MarriageYear") || keyPart.Equals("MarriagePlace"))) {
                if ((fact.Date != null) && !Strings.IsEmpty(fact.Date.Original)) {
                    if (fact.Date.Original.Length > 4) {
                        if (keyPart.Equals("MarriageDate")) {
                            factPart = fact.Date.Original;
                        }
                    } else if ((fact.Date != null) && fact.Date.Original.Length == 4) {
                        if (keyPart.Equals("MarriageYear")) {
                            factPart = fact.Date.Original;
                        }
                    }
                }
                if ((fact.Place != null) && fact.Place != null) {
                    if (keyPart.Equals("MarriagePlace")) {
                        factPart = fact.Place.Original;
                    }
                }
            }
            return factPart;
        }

        private string GetFactPart(string keyPart, Gx.Conclusion.Person person) {
            var factPart = "";
            foreach (var fact in person.Facts) {
                if (fact.Type.Equals("http://gedcomx.org/Birth") && (keyPart.Equals("BirthDate") || keyPart.Equals("BirthYear") || keyPart.Equals("BirthPlace"))) {
                    if ((fact.Date != null) && !Strings.IsEmpty(fact.Date.Original)) {
                        if (fact.Date.Original.Length > 4) {
                            if (keyPart.Equals("BirthDate")) {
                                factPart = fact.Date.Original;
                                break;
                            }
                        } else if (fact.Date.Original.Length == 4) {
                            if (keyPart.Equals("BirthYear")) {
                                factPart = fact.Date.Original;
                                break;
                            }
                        }
                    } else if (fact.Date == null) {
                        factPart = "";
                    }
                    if ((fact.Place != null) && fact.Place != null) {
                        if (keyPart.Equals("BirthPlace")) {
                            factPart = fact.Place.Original;
                            break;
                        }
                    }
                } else if (fact.Type.Equals("http://gedcomx.org/Death") && (keyPart.Equals("DeathDate") || keyPart.Equals("DeathYear") || keyPart.Equals("DeathPlace"))) {
                    if ((fact.Date != null) && !Strings.IsEmpty(fact.Date.Original)) {
                        if (fact.Date.Original.Length > 4) {
                            if (keyPart.Equals("DeathDate")) {
                                factPart = fact.Date.Original;
                                break;
                            }
                        } else if (fact.Date.Original.Length == 4) {
                            if (keyPart.Equals("DeathYear")) {
                                factPart = fact.Date.Original;
                                break;
                            }
                        }
                        factPart = RemoveSpecialCharacters(factPart);
                    }
                    if ((fact.Place != null) && fact.Place != null) {
                        if (keyPart.Equals("DeathPlace")) {
                            factPart = fact.Place.Original;
                            break;
                        }
                    }
                } else if (fact.Type.Equals("http://gedcomx.org/Marriage") && (keyPart.Equals("MarriageDate") || keyPart.Equals("MarriageYear") || keyPart.Equals("MarriagePlace"))) {
                    if ((fact.Date != null) && !Strings.IsEmpty(fact.Date.Original)) {
                        if (fact.Date.Original.Length > 4) {
                            if (keyPart.Equals("MarriageDate")) {
                                factPart = fact.Date.Original;
                                break;
                            }
                        } else if ((fact.Date != null) && fact.Date.Original.Length == 4) {
                            if (keyPart.Equals("MarriageYear")) {
                                factPart = fact.Date.Original;
                                break;
                            }
                        }
                    }
                    if ((fact.Place != null) && fact.Place != null) {
                        if (keyPart.Equals("MarriagePlace")) {
                            factPart = fact.Place.Original;
                            break;
                        }
                    }
                }
            }

            return factPart;
        }

        //GET /platform/tree/persons/PPPJ-MYZ
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public Gedcomx GetPerson(string personId, ref SessionDO session) {
            Gedcomx gedcomx = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personId.Trim());

                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<Gedcomx> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving person from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    try {
                        response = restClient.Execute<Gedcomx>(request);
                    } catch (Exception e) {
                        string message = "Failed to execute REST request while retrieving person from FamilySearch";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve person from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving person from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data.Persons == null) {
                                session.ResponseData = "Current person is null";
                            } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                                session.ResponseData = "Current person is empty";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        gedcomx = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON;
                    session.ErrorMessage = "Received error retrieving person from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }
            return gedcomx;
        }

        //GET /platform/tree/persons/{pid}/spouses
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        // https://familysearch.org/developers/docs/api/tree/Spouses_of_a_Person_resource
        public SpouseRelationship GetSpouses(string personId, ref SessionDO session) {
            SpouseRelationship spouseRelationship = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);

                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<SpouseRelationship> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_SPOUSES, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving spouses relationships from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    try {
                        response = restClient.Execute<SpouseRelationship>(request);
                    } catch (Exception e) {
                        string message = "Failed to execute REST request while retrieving spouses relationships from FamilySearch";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve spouse relationships from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving spouses relationships from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data.Persons == null) {
                                session.ResponseData = "Spouse Relationship is null";
                            } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                                session.ResponseData = "Spouse Relationship is empty";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        spouseRelationship = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_SPOUSES;
                    session.ErrorMessage = "Received error retrieving spouses relationships from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }
            return spouseRelationship;
        }

        //        GET /platform/tree/persons/pid-3/parents
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        // https://familysearch.org/developers/docs/api/tree/Parents_of_a_person_resource
        public SpouseRelationship GetParents(string personId, ref SessionDO session) {
            SpouseRelationship parentRelationship = null;

            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);

                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<SpouseRelationship> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_PARENTS, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving parent relationships from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    try {
                        response = restClient.Execute<SpouseRelationship>(request);
                    } catch (Exception e) {
                        string message = "Failed to execute REST request while retrieving parent relationships from FamilySearch";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve parent relationships from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving parent relationships from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data.Persons == null) {
                                session.ResponseData = "Parent Relationship is null";
                            } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                                session.ResponseData = "Parent Relationship is empty";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        parentRelationship = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_PARENTS;
                    session.ErrorMessage = "Received error retrieving parent relationships from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }
            return parentRelationship;
        }

        //GET /platform/tree/persons/{pid}/children
        //Accept: application/x-gedcomx-v1+xml
        //Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        // https://familysearch.org/developers/docs/api/tree/Parents_of_a_person_resource
        public SpouseRelationship GetChildren(string personId, ref SessionDO session) {
            SpouseRelationship childrenRelationship = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);

                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<SpouseRelationship> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_CHILDREN, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving children relationships from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    try {
                        response = restClient.Execute<SpouseRelationship>(request);
                    } catch (Exception e) {
                        string message = "Failed to execute REST request while retrieving child relationships from FamilySearch";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve child relationships from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving child relationships from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data.Persons == null) {
                                session.ResponseData = "child Relationship is null";
                            } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                                session.ResponseData = "Child Relationship is empty";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        childrenRelationship = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_CHILDREN;
                    session.ErrorMessage = "Received error retrieving children relationships from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }
            return childrenRelationship;
        }

        //        GET /platform/tree/persons/PPP0-MP1/fs-child-relationships
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public ChildAndParentsRelationship GetChildParentRelationships(string personId, ref SessionDO session) {
            ChildAndParentsRelationship childAndParentsRelationship = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<ChildAndParentsRelationship> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_CHILD_PARENT_RELATIONSHIPS, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving children relationships from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    try {
                        response = restClient.Execute<ChildAndParentsRelationship>(request);
                    } catch (Exception e) {
                        string message = "Failed to execute REST request while retrieving child relationships from FamilySearch";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)))) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve child relationships from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving child relationships from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data.Mother == null) {
                                session.ResponseData = "Child Parent Relationship is null";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        childAndParentsRelationship = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_CHILD_PARENT_RELATIONSHIPS;
                    session.ErrorMessage = "Received error retrieving children relationships from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }
            return childAndParentsRelationship;
        }

        //        GET /platform/tree/ancestry?person=PM12-345&spouse=PW12-345
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public GedcomxPerson GetAncestors(string personId, string generations, ref SessionDO session) {
            GedcomxPerson gedcomx = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
                inputs.Add(Constants.TEMPLATE_ID_GENERATIONS, generations);

                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<GedcomxPerson> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_ANCESTRY, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving ancestors from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    try {
                        response = restClient.Execute<GedcomxPerson>(request);
                    } catch (Exception e) {
                        string message = "Failed to execute REST request while retrieving ancestors from FamilySearch";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error && (response.Data == null || response.Data.Persons == null)))) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve ancestors from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving ancestors from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data.Persons == null) {
                                session.ResponseData = "Ancestors is null";
                            } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                                session.ResponseData = "Ancestors is empty";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        gedcomx = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the template: " + Constants.TEMPLATE_PERSON_ANCESTRY;
                    session.ErrorMessage = "Received error retrieving ancestors from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }

            return gedcomx;
        }

        //        GET /platform/tree/ancestry?person=PM12-345&spouse=PW12-345
        //        Accept: application/x-fs-v1+xml
        //        Authorization: Bearer YOUR_ACCESS_TOKEN_HERE
        public GedcomxPerson GetDescendants(string personId, string generations, ref SessionDO session) {
            GedcomxPerson gedcomx = null;
            if (!string.IsNullOrEmpty(personId)) {
                if (RestHelper.Instance.Expired) {
                    RestHelper.Instance.Refresh();
                }

                if (generations.Equals("3")) {
                    generations = "2";
                }
                var inputs = new Dictionary<string, string>();
                inputs.Add(Constants.TEMPLATE_ID_PERSON, personId);
                inputs.Add(Constants.TEMPLATE_ID_GENERATIONS, generations);

                RestClient restClient = null;
                RestRequest request = null;
                IRestResponse<GedcomxPerson> response = null;

                try {
                    request = RestHelper.Instance.GetRequest(Constants.TEMPLATE_PERSON_DESCENDANTS, inputs, ref session, ref restClient);
                } catch (Exception e) {
                    var message = "Failed to create REST request while retrieving descendants from FamilySearch.";
                    LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                    throw new DataAccessException(message, e, session);
                }

                if (request != null) {
                    request.Method = Method.GET;
                    request.AddHeader("Accept", Constants.MEDIA_TYPE_XML);
                    request.AddHeader("Authorization", string.Format("Bearer {0}", session.AccessToken));
                    try {
                        response = restClient.Execute<GedcomxPerson>(request);
                    } catch (Exception e) {
                        string message = "Failed to execute REST request while retrieving ordinance info from FamilySearch.";
                        LogRestError(message, inputs, request, response, session, new StackTrace(true).GetFrames(), e);
                        throw new DataAccessException(message, e, session);
                    }

                    if (RestHelper.InvalidResponse(response, ref session)) {
                        if (!response.StatusCode.Equals(HttpStatusCode.NoContent) && (session.Error || (!session.Error & (response.Data == null)) || response.Data.Persons == null)) {
                            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                                session.ErrorMessage = "Unauthorized to retrieve descendants from FamilySearch";
                            } else {
                                session.ErrorMessage = "Received error retrieving descendants from FamilySearch";
                            }
                            if (response.Data == null) {
                                session.ResponseData = "Data is Null";
                            } else if (response.Data.Persons == null) {
                                session.ResponseData = "Persons is null";
                            } else if (response.Data.Persons != null && response.Data.Persons.Count < 1) {
                                session.ResponseData = "Persons is empty";
                            }
                            LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                            throw new DataAccessException(session.ErrorMessage, null, session);
                        }
                    } else {
                        gedcomx = response.Data;
                    }
                } else {
                    session.ResponseMessage = "The API descriptor doesn't have a link to the GEDCOM X person resource.";
                    session.ErrorMessage = "Received error retrieving descendants from FamilySearch";
                    session.ResponseData = "Null";
                    LogRestError(session.ErrorMessage, inputs, request, response, session, new StackTrace(true).GetFrames(), null);
                    throw new DataAccessException(session.ErrorMessage, null, session);
                }
            }

            return gedcomx;
        }

        public void LogRestError(string message, Dictionary<string, string> inputs, RestRequest request, IRestResponse response, SessionDO session, StackFrame[] stackFrames, Exception e, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) {
            var sb = new StringBuilder(256);
            sb.AppendLine("Message:  " + message + ((e != null) ? e.ToString() : ""));
            sb.AppendLine("Error Message:  " + session.ErrorMessage);
            sb.AppendLine("Response Message:  " + session.ResponseMessage);
            if ((response != null) && response.StatusCode.Equals(HttpStatusCode.Unauthorized)) {
                sb.AppendLine("Possible Solution:  Token may be invalid or expired.");
            } else if ((session.ResponseMessage != null) && session.ResponseMessage.IndexOf("unexpected token") > -1) {
                sb.AppendLine("Possible Solution:  The template uri is probably invalid, please make sure the uri is correct");
            } else if ((session.ResponseMessage != null) && session.ResponseMessage.IndexOf("FamilySearch Best Practice") > -1) {
                var index = session.ResponseMessage.IndexOf("FamilySearch Best Practice");
                var test = "Possible Solution: " + session.ResponseMessage.Substring(index, session.ResponseMessage.Length - index);
                sb.AppendLine("Possible Solution: " + session.ResponseMessage.Substring(index, session.ResponseMessage.Length - index));
            }
            sb.AppendLine("Message Type:  " + session.MessageType);
            sb.AppendLine("User ID:  " + session.Username);
            sb.AppendLine("User Name:  " + session.DisplayName);
            sb.AppendLine("Class:  " + sourceFilePath);
            sb.AppendLine("Method:  " + memberName);
            sb.AppendLine("Line No.:  " + sourceLineNumber);
            if (inputs != null) {
                sb.AppendLine("Inputs:");
                foreach (var input in inputs) {
                    sb.AppendLine("   " + input.Key + " - " + input.Value);
                }
            }
            if (request != null) {
                sb.AppendLine("Request Resource:  " + request.Resource);
                sb.AppendLine("Request Method:  " + request.Method);
                if (request.Parameters != null) {
                    sb.AppendLine("Parameters:");
                    foreach (var parameter in request.Parameters) {
                        sb.AppendLine("   " + parameter.Name + " - " + parameter.Value);
                    }
                }
            }
            if (response != null) {
                sb.AppendLine("Response Uri:  " + response.ResponseUri);
                sb.AppendLine("Response Status Code:  " + response.StatusCode);
                sb.AppendLine("Response Status:  " + response.ResponseStatus);
                sb.AppendLine("Response Data:  " + session.ResponseData);
                sb.AppendLine("Response Error Message:  " + response.ErrorMessage);
                sb.AppendLine("Response Content Type:  " + response.ContentType);
                var contentLength = response.Content.Length; // or search for "Error Status Report".
                if (contentLength > 480) {
                    contentLength = 480;
                }
                sb.AppendLine("Response Content:  " + response.Content.Substring(0, contentLength));
            }
            sb.AppendLine("Stack Trace:");

            for (var i = 0; i < stackFrames.Length; i++) {
                var frame = stackFrames[i];
                var filename = frame.GetFileName();
                if (!string.IsNullOrEmpty(filename) && (filename.IndexOf("FindMyFamilies") > 0)) {
                    sb.AppendLine(string.Format(frame.GetFileName() + " " + frame.GetMethod() + " " + frame.GetFileLineNumber())); //"{0}:{1}", method.ReflectedType != null ? method.ReflectedType.Name : string.Empty, method.Name));
                } else {
                    break;
                }
            }
            logger.Error(sb.ToString());
        }
    }
}