using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using FindMyFamiles.Services.Data;
using FindMyFamilies.Data;
using FindMyFamilies.Helper;
using FindMyFamilies.Services;
using log4net;

namespace FindMyFamilies.Web.Controllers {
    public class PersonController : ControllerBase {
        private readonly ILog Logger = LogManager.GetLogger(typeof (PersonController));

        [HttpGet]
        public ActionResult DisplayPerson(RetrieveFamilySearchDO retrieveDo) {
            session = GetSession();
            var personInfo = new PersonInfoDO();
            var researchDO = new ResearchDO();
            if (Request.IsAjaxRequest()) {
                try {
                    researchDO.CurrentPersonId = PersonId;
                    researchDO.PersonId = retrieveDo.PersonId;
                    researchDO.Generation = 1;
                    personInfo.IncludeMaidenName = retrieveDo.IncludeMaidenName;
                    personInfo.IncludeMiddleName = retrieveDo.IncludeMiddleName;
                    personInfo.IncludePlace = retrieveDo.IncludePlace;
                    personInfo.YearRange = retrieveDo.YearRange;
                    personInfo.Person = Service.GetPersonInformation(researchDO, ref session);
                    checkAuthentication();
                } catch (Exception e) {
                    return new HttpStatusCodeResult(401, e.Message);
                }
            }

            if (personInfo.Person == null) {
                Logger.Debug("RetrievePersonInfo personInfo == null. ");
            }

            return PartialView("~/Views/Person/PersonUrls.cshtml", personInfo);
        }

        [HttpGet]
        public ActionResult FindPerson() {
            //            session = GetSession();
            //            var personInfo = new PersonInfoDO();
            //            var research = new ResearchDO();
            //            if (Request.IsAjaxRequest()) {
            //            }

            return PartialView("~/Views/Person/FindPerson.cshtml");
        }

        public ActionResult PersonInfo() {
            return PartialView("~/Views/Person/PersonInfo.cshtml");
        }

        [HttpGet]
        public ActionResult GetPersonInfo(string personId) {
            PersonDO person = GetPersonInCache(personId);

            var personInfoDO = new PersonInfoDO();

            return PartialView("~/Views/PersonInfo.cshtml", personInfoDO);
        }

        [HttpGet]
        public ActionResult DisplayPerson111() {
            session = GetSession();
            PersonDO person = session.CurrentPerson;

            return PartialView("~/Views/Person/DisplayPerson.cshtml", person);
        }

        [HttpGet]
        public JsonResult GetAncestorList(string personId) {
            var researchDO = new ResearchDO();
            researchDO.PersonId = personId;
            return Json(GetAncestors(researchDO), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getAncestorsBornBetween18101850(ResearchDO researchDO) {
            SessionDO session = GetSession();
            if (string.IsNullOrEmpty(researchDO.PersonId)) {
                researchDO.PersonId = PersonId;
            }
            List<SelectListItemDO> ancestorList = Service.getAncestorsBornBetween18101850(researchDO, ref session);
            var ancestors = new AncestorsDO();
            ancestors.Ancestors = ancestorList;

            if (ancestorList.Count > 0) {
                int count = ancestorList.Count;
                var random = new Random();
                int randomNumber = random.Next(0, count);
                SelectListItemDO item = ancestorList[randomNumber];
                ancestors.Id = item.id;
                ancestors.Text = item.text;
            }

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAncestorByPersonId(string personId) {
            personId = personId.ToUpper();
            var ancestors = new AncestorsDO();
            SessionDO session = GetSession();
            PersonDO person = Service.GetPerson(personId, ref session);
            checkAuthentication();
            if ((person != null) && !person.IsEmpty) {
                ancestors.Ancestors.Add(new SelectListItemDO(person.Id, person.Id + " - " + person.Fullname));
                ancestors.Id = person.Id;
                ancestors.Text = person.Id + " - " + person.Fullname;
            }

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FindPersons(string personId, string firstName, string lastName, string gender, string birthYear, string deathYear) {
            SessionDO session = GetSession();
            personId = personId.ToUpper();
            var personDO = new PersonDO();
            if (!string.IsNullOrEmpty(personId)) {
                personDO = Service.GetPerson(personId, ref session);
            } else {
                personDO.Id = personId;
                personDO.Firstname = firstName;
                personDO.Lastname = lastName;
                if (!string.IsNullOrEmpty(birthYear)) {
                    personDO.BirthYear = Convert.ToInt16(birthYear);
                }
                if (!string.IsNullOrEmpty(deathYear)) {
                    personDO.DeathYear = Convert.ToInt16(deathYear);
                }
                personDO.Gender = gender.ToUpper();
            }
            List<FindListItemDO> persons = Service.FindPersons(personDO, ref session);
            //            checkAuthentication();
            //            if ((person != null) && !person.IsEmpty) {
            //                ancestors.Ancestors.Add(new SelectListItemDO(person.Id, person.Id + " - " + person.Fullname));
            //                ancestors.Id = person.Id;
            //                ancestors.Text = person.Id + " - " + person.Fullname;
            //            }

            return Json(persons, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAncestors(ResearchDO researchDO) {
            SessionDO session = GetSession();
            if (string.IsNullOrEmpty(researchDO.PersonId)) {
                researchDO.PersonId = PersonId;
            }
            List<SelectListItemDO> ancestorList = PersonServices.Instance.GetAncestorsForPersonInfo(researchDO, ref session);
            checkAuthentication();
            var ancestors = new AncestorsDO();
            ancestors.Ancestors = ancestorList;

            if ((ancestorList != null) && ancestorList.Count > 0) {
                SelectListItemDO item = ancestorList[0];
                ancestors.Id = item.id;
                ancestors.Text = item.text;
            }

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDescendants(string personId) {
            SessionDO session = GetSession();
            var researchDO = new ResearchDO();
            if (string.IsNullOrEmpty(personId)) {
                personId = PersonId;
            }
            researchDO.PersonId = personId;
            researchDO.Generation = 2;
            List<SelectListItemDO> descendantList = PersonServices.Instance.GetDescendantsForPersonInfo(researchDO, ref session);
            checkAuthentication();

            var descendants = new DescendantsDO();
            descendants.Descendants = descendantList;

            if (descendantList.Count > 0) {
                SelectListItemDO item = descendantList[0];
                descendants.Id = item.id;
                descendants.Text = item.text;
            }

            return Json(descendants, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RefreshDescendantsForPersonInfo(string personId) {
            SessionDO session = GetSession();
            var researchDO = new ResearchDO();
            if (string.IsNullOrEmpty(personId)) {
                personId = PersonId;
            }
            researchDO.PersonId = personId;
            researchDO.Generation = 7;
            researchDO.Refresh = true;
            List<SelectListItemDO> ancestors = PersonServices.Instance.GetAncestorsForPersonInfo(researchDO, ref session);
            checkAuthentication();

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }
    }
}