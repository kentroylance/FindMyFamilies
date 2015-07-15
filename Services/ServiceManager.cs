using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FindMyFamilies.Data;
using FindMyFamilies.Util;
using Gx;
using Gx.Conclusion;
using Gx.Fs;

namespace FindMyFamilies.Services {
    public class ServiceManager {
        private static ServiceManager instance;
        private static readonly object syncLock = new object();
        private readonly Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

        private ServiceManager() {
        }

        public static ServiceManager Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new ServiceManager();
                    }

                    return instance;
                }
            }
        }

        public AdminServices Admin {
            get {
                return AdminServices.Instance;
            }
        }

        public AncestryServices Ancestry {
            get {
                return AncestryServices.Instance;
            }
        }

        public AuthenticationServices Authentication {
            get {
                return AuthenticationServices.Instance;
            }
        }

        public PersonServices Person {
            get {
                return PersonServices.Instance;
            }
        }

        public SessionDO Authenticate(string username, string password, string clientId) {
            return AuthenticationServices.Instance.Authenticate(username, password, clientId);
        }

        public Gedcomx GetPersonAncestryWithSpouse(String personId, String spouseId, String generations, ref SessionDO session) {
            return AncestryServices.Instance.GetPersonAncestryWithSpouse(personId, spouseId, generations, ref session);
        }

        //        public Dictionary<string, PersonDO> GetPersonAncestry(String personId, ref SessionDO session) {
        //            return PersonServices.Instance.GetPersonAncestry(personId, ref session);
        //        }

        public void GetPersonAncestryValidations(ref ResearchDO researchDO, ref SessionDO session) {
            PersonServices.Instance.GetPersonAncestryValidations(ref researchDO, ref session);
        }

        public List<FindListItemDO> FindPersons(PersonDO personDO, ref SessionDO session) {
            return PersonServices.Instance.FindPersons(personDO, ref session);
        }


        public List<HintListItemDO> GetHints(HintInputDO hintInputDO, ref SessionDO session) {
            return PersonServices.Instance.GetHints(hintInputDO, ref session);
        }

        public List<OrdinanceListItemDO> GetOrdinances(IncompleteOrdinanceDO incompleteOrdinanceDo, ref SessionDO session) {
            return PersonServices.Instance.GetOrdinances(incompleteOrdinanceDo, ref session);
        }

        public List<AnalyzeListItemDO> GetAnalyzeData(FindCluesInputDO findCluesInputDo, ref SessionDO session) {
            return PersonServices.Instance.GetAnalyzeData(findCluesInputDo, ref session);
        }

        public List<StartingPointListItemDO> GetStartingPoints(StartingPointInputDO startingPointInputDO, ref SessionDO session) {
            return PersonServices.Instance.GetStartingPoints(startingPointInputDO, ref session);
        }

        public List<PossibleDuplicateListItemDO> GetPossibleDuplicates(PossibleDuplicateInputDO possibleDuplicateInputDO, ref SessionDO session) {
           return PersonServices.Instance.GetPossibleDuplicates(possibleDuplicateInputDO, ref session);
        }

        public List<DateListItemDO> GetDates(DateInputDO dateInputDO, ref SessionDO session) {
           return PersonServices.Instance.GetDates(dateInputDO, ref session);
        }

        public List<PlaceListItemDO> GetPlaces(PlaceInputDO placeInputDO, ref SessionDO session) {
           return PersonServices.Instance.GetPlaces(placeInputDO, ref session);
        }

        public PersonDO GetPersonInformation(ResearchDO researchDO, ref SessionDO session) {
            return PersonServices.Instance.GetPersonInformation(researchDO, ref session);
        }

        public Gedcomx GetPersonDescendancyWithSpouse(String personId, String spouseId, string generations, ref SessionDO session) {
            return PersonServices.Instance.GetPersonDescendancyWithSpouse(personId, spouseId, generations, ref session);
        }

        public Gedcomx GetChildren(String personId, ref SessionDO session) {
            return PersonServices.Instance.GetChildren(personId, ref session);
        }

        public Gedcomx GetSpouses(String personId, ref SessionDO session) {
            return PersonServices.Instance.GetSpouses(personId, ref session);
        }

        public Gedcomx GetCurrentPerson(ref SessionDO session) {
            return PersonServices.Instance.GetCurrentPerson(ref session);
        }

        public PersonDO GetPerson(String personId, ref SessionDO session) {
            return PersonServices.Instance.GetPerson(personId, ref session);
        }

        public List<SelectListItemDO> getAncestorsBornBetween18101850(ResearchDO researchDO, ref SessionDO session) {
            return PersonServices.Instance.getAncestorsBornBetween18101850(researchDO, ref session);
        }

        public PersonDO GetPerson(Person person) {
            return PersonServices.Instance.GetPerson(person);
        }

        public Gedcomx GetPersonWithDetails(String personId, ref SessionDO session) {
            return PersonServices.Instance.GetPersonWithDetails(personId, ref session);
        }

        public Gedcomx GetPersonWithRelationships(String personId, ref SessionDO session) {
            return PersonServices.Instance.GetPersonWithRelationships(personId, ref session);
        }

        public FamilySearchPlatform GetChildParentRelationships(String personId, ref SessionDO session) {
            return PersonServices.Instance.GetChildParentRelationships(personId, ref session);
        }

        public bool IsChurchMember(ref SessionDO session) {
            return PersonServices.Instance.IsChurchMember(ref session);
        }

    }
}