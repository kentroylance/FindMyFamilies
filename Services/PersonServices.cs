using System;
using System.Collections;
using System.Collections.Generic;
using FindMyFamilies.BusinessObject;
using FindMyFamilies.Data;
using FindMyFamilies.DataAccess;
using Gx.Conclusion;
using findmyfamilies.Services;
using Gx;
using Gx.Fs;

namespace FindMyFamilies.Services {

    /// <summary>
    ///     Purpose: Services Facade Class for PersonServices
    /// </summary>
    public class PersonServices : PersonServicesBase {

        private static PersonServices instance;
        private static readonly object syncLock = new object();
        private static readonly PersonBO personBO = new PersonBO();


        private PersonServices() {
        }

        public static PersonServices Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new PersonServices();
                    }

                    return instance;
                }
            }
        }

        public Gedcomx GetPersonDescendancyWithSpouse(string personId, string spouseId, string generations, ref SessionDO session) {
            return personBO.GetPersonDescendancyWithSpouse(personId, spouseId, generations, ref session);
        }

        public Gedcomx GetCurrentPerson(ref SessionDO session) {
            return personBO.GetCurrentPerson(ref session);
        }

        public List<FindListItemDO> FindPersons(PersonDO personDO, ref SessionDO session) {
            return personBO.FindPersons(personDO, ref session);
        }

        public Gedcomx GetPersonWithDetails(String personId, ref SessionDO session) {
            return personBO.GetPersonWithDetails(personId, ref session);
        }

        public Gedcomx GetPersonWithRelationships(String personId, ref SessionDO session) {
            return personBO.GetPersonWithRelationships(personId, ref session);
        }

        public PersonDO GetPerson(String pid, ref SessionDO session) {
            return personBO.GetPerson(pid, ref session);
        }

        public PersonDO GetPerson(Person person) {
            return personBO.GetPerson(person);
        }

        public bool IsChurchMember(ref SessionDO session) {
            return personBO.IsChurchMember(ref session);
        }

        public List<SelectListItemDO> getAncestorsBornBetween18101850(ResearchDO researchDO, ref SessionDO session) {
            return personBO.getAncestorsBornBetween18101850(researchDO, ref session);
        }

        public Gedcomx GetSpouses(String pid, ref SessionDO session) {
            return personBO.GetSpouses(pid, ref session);
        }

        public PersonDO GetPersonInformation(ResearchDO researchDO, ref SessionDO session) {
            return personBO.GetPersonInformation(researchDO, ref session);
        }

//        public ArrayList GetAncestorsList(ResearchDO ResearchDO, ref SessionDO session) {
//            return personBO.GetAncestorsList(ResearchDO, ref session);
//        }

        public List<SelectListItemDO> GetAncestorsForPersonInfo(ResearchDO researchDO, ref SessionDO session) {
            return personBO.GetAncestorsForPersonInfo(researchDO, ref session);
        }

        public List<SelectListItemDO> GetDescendantsForPersonInfo(ResearchDO researchDO, ref SessionDO session) {
            return personBO.GetDescendantsForPersonInfo(researchDO, ref session);
        }

        public Gedcomx GetChildren(String pid, ref SessionDO session) {
            return personBO.GetChildren(pid, ref session);
        }

//        public Dictionary<string, PersonDO> GetPersonAncestry(String personId, ref SessionDO session) {
//            return personBO.GetPersonAncestry(personId, ref session);
//        }

        public void GetPersonAncestryValidations(ref ResearchDO researchDO, ref SessionDO session) {
            personBO.GetPersonAncestryValidations(ref researchDO, ref session);
        }

        public List<DateListItemDO> GetDates(DateInputDO dateInputDO, ref SessionDO session) {
           return personBO.GetDates(dateInputDO, ref session);
        }

        public List<PlaceListItemDO> GetPlaces(PlaceInputDO placeInputDO, ref SessionDO session) {
           return personBO.GetPlaces(placeInputDO, ref session);
        }

        public List<OrdinanceListItemDO> GetOrdinances(IncompleteOrdinanceDO incompleteOrdinanceDo, ref SessionDO session) {
            return personBO.GetOrdinances(incompleteOrdinanceDo, ref session);
        }

        public List<HintListItemDO> GetHints(HintInputDO hintInputDO, ref SessionDO session) {
            return personBO.GetHints(hintInputDO, ref session);
        }

        public List<AnalyzeListItemDO> GetAnalyzeData(FindCluesInputDO findCluesInputDo, ref SessionDO session) {
            return personBO.GetAnalyzeData(findCluesInputDo, ref session);
        }

        public List<StartingPointListItemDO> GetStartingPoints(StartingPointInputDO startingPointInputDO, ref SessionDO session) {
            return personBO.GetStartingPoints(startingPointInputDO, ref session);
        }

        public List<PossibleDuplicateListItemDO> GetPossibleDuplicates(PossibleDuplicateInputDO possibleDuplicateInputDO, ref SessionDO session) {
            return personBO.GetPossibleDuplicates(possibleDuplicateInputDO, ref session);
        }


        public FamilySearchPlatform GetChildParentRelationships(String personId, ref SessionDO session) {
            return personBO.GetChildParentRelationships(personId, ref session);
        }

   		/// <summary>
		/// Purpose: Read Reports by Report By
		/// </summary>
		/// <returns>Returns Report data objects by page</returns>
		/// <param name = "reportDO">ReportDO reportDO</param>
		public ICollection ReadReportsByReport(ReportDO reportDO) {
			return ReportBO.ReadReportsByReportBy(reportDO);
		}


	}
}
