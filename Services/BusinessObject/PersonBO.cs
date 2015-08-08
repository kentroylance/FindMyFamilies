using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using FindMyFamilies.Data;
using FindMyFamilies.DataAccess;
using FindMyFamilies.Helper;
using FindMyFamilies.Services;
using FindMyFamilies.Util;
using Gx.Fs;
using Model.Pd;
using ProtoBuf;
using Fact = Gx.Conclusion.Fact;
using Gedcomx = Gx.Gedcomx;
using Person = Gx.Conclusion.Person;

namespace FindMyFamilies.BusinessObject {
    /// <summary>
    ///   Purpose: Services Facade Class for PersonServices
    /// </summary>
    public class PersonBO {
        private static readonly PersonDAO personDAO = new PersonDAO();
        private readonly Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

        public Gedcomx GetPersonDescendancyWithSpouse(string personId, string spouseId, string generations, ref SessionDO session) {
            var gedcomx = personDAO.GetPersonDescendancyWithSpouse(personId, spouseId, generations, ref session);

            return gedcomx;
        }

        public Gedcomx GetCurrentPerson(ref SessionDO session) {
            var gedcomx = personDAO.GetCurrentPerson(ref session);

            return gedcomx;
        }

        public bool IsChurchMember(ref SessionDO session) {
            return personDAO.IsChurchMember(ref session);
        }

        public Gedcomx GetPersonWithDetails(String personId, ref SessionDO session) {
            var gedcomx = personDAO.GetPersonWithDetails(personId, ref session);

            return gedcomx;
        }

        public Gedcomx GetPersonWithRelationships(String personId, ref SessionDO session) {
            var gedcomx = personDAO.GetPersonWithRelationships(personId, ref session);

            return gedcomx;
        }

        public PersonDO GetPerson(Person person) {
            return personDAO.GetPerson(person);
        }

        public PersonDO GetPerson(String personId, ref SessionDO session) {
            if (!string.IsNullOrEmpty(personId)) {
                personId = personId.ToUpper();
            }

            PersonDO personDO = null;

            if (!session.Error) {
                var gedcomx = personDAO.GetPerson(personId, ref session);

                if ((gedcomx != null) && (gedcomx.Persons != null)) {
                    foreach (var person in gedcomx.Persons) {
                        person.Display.AscendancyNumber = "0";
                        personDO = personDAO.GetPerson(person);
                        break;
                    }
                }
            }

            return personDO;
        }

        public Gedcomx GetSpouses(String personId, ref SessionDO session) {
            var gedcomx = personDAO.GetSpouses(personId, ref session);

            return gedcomx;
        }

        public Gedcomx GetChildren(String personId, ref SessionDO session) {
            var gedcomx = personDAO.GetChildren(personId, ref session);

            return gedcomx;
        }

        public PersonDO GetPersonInformation(ResearchDO researchDO, ref SessionDO session) {
            var persons = new Dictionary<string, PersonDO>();
            SpouseRelationship spouseRelationship = null;
            var person = getPersonWithParents(researchDO.PersonId, ref persons, ref session);
            persons = GetDescendants(ref researchDO, ref session, true);
            if ((persons.Count < 1) || !persons.ContainsKey(person.Id)) {
                persons.Add(person.Id, person);
                if (!person.Father.IsEmpty) {
                    persons.Add(person.Father.Id, person.Father);
                }
                if (!person.Mother.IsEmpty) {
                    persons.Add(person.Mother.Id, person.Mother);
                }
                spouseRelationship = personDAO.GetSpouses(person.Id, ref session);
            } else {
                if (persons[person.Id].Father.IsEmpty && !person.Father.IsEmpty) {
                    persons[person.Id].Father = person.Father;
                }
                if (persons[person.Id].Mother.IsEmpty && !person.Mother.IsEmpty) {
                    persons[person.Id].Mother = person.Mother;
                }
                if (persons[person.Id].HasSpouseLink) {
                    spouseRelationship = personDAO.GetSpouses(person.Id, ref session);
                }
            }

            if (persons.Count > 1) {
                if ((spouseRelationship != null) && (spouseRelationship.Persons.Count > 0)) {
                    PersonDO spouseDO = null;
                    foreach (var spouseGx in spouseRelationship.Persons) {
                        if (!persons.ContainsKey(spouseGx.Id)) {
                            spouseDO = personDAO.GetPerson(spouseGx);
                            persons.Add(spouseDO.Id, spouseDO);
                            persons[person.Id].Spouses.Add(spouseDO.Id, persons[spouseDO.Id]);
                            var childrenRelationship = personDAO.GetChildren(spouseDO.Id, ref session);
                            //                            if ((childrenRelationship == null) && (!persons[person.Id].HasChildren)) {
                            //                                childrenRelationship = personDAO.GetChildren(person.Id, ref session);
                            //                            }
                            if (childrenRelationship != null) {
                                if (childrenRelationship.Persons != null) {
                                    foreach (var childGx in childrenRelationship.Persons) {
                                        AddPerson(childGx, persons, 0);
                                        persons[childGx.Id].Mother = persons[spouseDO.Id];
                                        persons[spouseDO.Id].Children.Add(childGx.Id, persons[childGx.Id]);
                                    }
                                }
                            }
                        } else {
                            if (!persons[person.Id].Spouses.ContainsKey(spouseGx.Id)) {
                                persons[person.Id].Spouses.Add(spouseGx.Id, persons[spouseGx.Id]);
                                if (persons[person.Id].Spouse.IsEmpty) {
                                    persons[person.Id].Spouse = persons[spouseGx.Id];
                                }
                            }
                        }
                    }
                }
            }

            return persons[person.Id];
        }

        public List<FindListItemDO> FindPersons(PersonDO personDO, ref SessionDO session) {
            return personDAO.FindPersons(personDO, ref session);
        }

        public List<SelectListItemDO> getAncestorsBornBetween18101850(ResearchDO researchDO, ref SessionDO session) {
            List<SelectListItemDO> ancestors = null;

            if (session.Error) {
                return ancestors;
            }
            var ancestorList = new Dictionary<string, SelectListItemDO>();
            var persons = new Dictionary<string, PersonDO>();
            var children = new Dictionary<string, PersonDO>();

            if (!String.IsNullOrEmpty(researchDO.PersonId)) {
                var person = GetPerson(researchDO.PersonId, ref session);
                AddPerson(person, persons, 0);
                person = getParents(researchDO.PersonId, ref persons, ref session);

                var fatherAncestors = personDAO.GetAncestors(person.Father.Id, researchDO.Generation.ToString(), ref session);
                if ((fatherAncestors != null) && (fatherAncestors.Persons.Count > 0)) {
                    foreach (var fatherAncestor in fatherAncestors.Persons) {
                        var personDO = personDAO.GetPerson(fatherAncestor);
                        if ((personDO.BirthYear >= 1810) && (personDO.BirthYear <= 1850)) {
                            if (!ancestorList.ContainsKey(personDO.Id)) {
                                ancestorList.Add(personDO.Id, new SelectListItemDO(personDO.Id, personDO.Fullname + " (" + personDO.LifeSpan + ") - " + personDO.Id));
                            }
                        }
                    }
                }

                var motherAncestors = personDAO.GetAncestors(person.Mother.Id, researchDO.Generation.ToString(), ref session);
                if ((motherAncestors != null) && (motherAncestors.Persons.Count > 0)) {
                    foreach (var motherAncestor in motherAncestors.Persons) {
                        var personDO = personDAO.GetPerson(motherAncestor);
                        if ((personDO.BirthYear >= 1810) && (personDO.BirthYear <= 1850)) {
                            if (!ancestorList.ContainsKey(personDO.Id)) {
                                ancestorList.Add(personDO.Id, new SelectListItemDO(personDO.Id, personDO.Fullname + " (" + personDO.LifeSpan + ") - " + personDO.Id));
                            }
                        }
                    }
                }
                ancestors = ancestorList.Select(z => new SelectListItemDO {id = z.Value.id, text = z.Value.text}).ToList();
                ancestors = ancestors.OrderBy(o => o.text).ToList();
                //                   SetAncestorsInCache(personId, ancestors);
            }

            return ancestors;
        }

        public List<SelectListItemDO> GetAncestorsForPersonInfo(ResearchDO researchDO, ref SessionDO session) {
            List<SelectListItemDO> ancestors = null;

            if (session.Error) {
                return ancestors;
            }

            if (researchDO.BornBetween18101850) {
                ancestors = getAncestorsBornBetween18101850(researchDO, ref session);
            } else {
                var ancestorList = new Dictionary<string, SelectListItemDO>();
                var persons = new Dictionary<string, PersonDO>();
                var children = new Dictionary<string, PersonDO>();

                //if (!parents.ContainsKey(personGx.Id)) {
                if (!String.IsNullOrEmpty(researchDO.PersonId)) {
                    var person = GetPerson(researchDO.PersonId, ref session);
                    AddPerson(person, persons, 0);
                    person = getParents(researchDO.PersonId, ref persons, ref session);

                    var fatherAncestors = personDAO.GetAncestors(person.Father.Id, researchDO.Generation.ToString(), ref session);
                    if ((fatherAncestors != null) && (fatherAncestors.Persons.Count > 0)) {
                        foreach (var fatherAncestor in fatherAncestors.Persons) {
                            var personDO = personDAO.GetPerson(fatherAncestor);
                            if (!ancestorList.ContainsKey(personDO.Id)) {
                                ancestorList.Add(personDO.Id, new SelectListItemDO(personDO.Id, personDO.Fullname + " - " + personDO.Id));
                            }
                        }
                    }

                    var motherAncestors = personDAO.GetAncestors(person.Mother.Id, researchDO.Generation.ToString(), ref session);
                    if ((motherAncestors != null) && (motherAncestors.Persons.Count > 0)) {
                        foreach (var motherAncestor in motherAncestors.Persons) {
                            var personDO = personDAO.GetPerson(motherAncestor);
                            if (!ancestorList.ContainsKey(personDO.Id)) {
                                ancestorList.Add(personDO.Id, new SelectListItemDO(personDO.Id, personDO.Fullname + " - " + personDO.Id));
                            }
                        }
                    }
                    ancestors = ancestorList.Select(z => new SelectListItemDO {id = z.Value.id, text = z.Value.text}).ToList();
                    ancestors = ancestors.OrderBy(o => o.text).ToList();
                    //                   SetAncestorsForPersonInfoInCache(ResearchDO.PersonId, ancestors);
                    //               }
                }
            }
            return ancestors;
        }

        public List<SelectListItemDO> GetDescendantsForPersonInfo(ResearchDO researchDO, ref SessionDO session) {
            var descendants = new List<SelectListItemDO>();
            if (session.Error) {
                return descendants;
            }

            var gedcomx = personDAO.GetDescendants(researchDO.PersonId, researchDO.Generation.ToString(), ref session);
            if ((gedcomx != null) && (gedcomx.Persons.Count > 0)) {
                foreach (var person in gedcomx.Persons) {
                    if (!person.Id.Equals(researchDO.PersonId)) {
                        descendants.Add(new SelectListItemDO(person.Id, person.Display.Name + " - " + person.Id));
                    }
                }
            }
            descendants = descendants.OrderBy(o => o.text).ToList();
            return descendants;
        }

        public Dictionary<string, PersonDO> GetAncestors(ref ResearchDO researchDO, ref SessionDO session) {
            var persons = new Dictionary<string, PersonDO>();

            if (session.Error) {
                return persons;
            }

            var personsPedigree = new Dictionary<string, PersonDO>();
            var motherPersons = new Dictionary<string, PersonDO>();
            var children = new Dictionary<string, PersonDO>();

            var person = getPersonWithParents(researchDO.PersonId, ref persons, ref session);
            string generationValue = (researchDO.Generation - 1).ToString();
            researchDO.PersonName = person.Fullname;

            if (!person.Father.IsEmpty && !person.Mother.IsEmpty) {
                var ancestors = personDAO.GetPersonAncestryWithSpouse(person.Father.Id, person.Mother.Id, generationValue, ref session);
                if ((ancestors != null) && (ancestors.Persons != null)) {
                    PersonDO personDO = null;
                    try {
                        for (var i = ancestors.Persons.Count - 1; i > -1; i--) {
                            personDO = personDAO.GetPerson(ancestors.Persons[i]);
                            if ((personDO != null) && !personDO.IsEmpty && !persons.ContainsKey(personDO.Id)) {
                                persons.Add(personDO.Id, personDO);
                                personsPedigree.Add((string.IsNullOrEmpty(personDO.AscendancyNumber) ? personDO.DescendancyNumber : personDO.AscendancyNumber), personDO);
                            }
                            //                       PopulatePerson(personDO.Id, ref persons, ref session, Constants.RESEARCH_TYPE_ANCESTORS);
                            //                        logger.Info("PopulatePerson = " + personDO.Fullname);
                        }
                    } catch (Exception e) {
                        logger.Error("GetAncestors " + e.Message, e);
                        throw;
                    }

                    try {
                        foreach (var personDo in persons) {
                            var ascen = personDo.Value.AscendancyNumber;
                            var descen = personDo.Value.DescendancyNumber;
                            if (!string.IsNullOrEmpty(personDo.Value.AscendancyNumber)) {
                                var parentAscNo = (Convert.ToInt16(personDo.Value.AscendancyNumber) + Convert.ToInt16(personDo.Value.AscendancyNumber)).ToString();
                                if (personsPedigree.ContainsKey(parentAscNo)) {
                                    var parent = personsPedigree[parentAscNo];
                                    if (parent.IsFemale) {
                                        persons[personDo.Value.Id].Mother = parent;
                                    } else {
                                        persons[personDo.Value.Id].Father = parent;
                                    }
                                }
                                parentAscNo = (Convert.ToInt16(personDo.Value.AscendancyNumber) + Convert.ToInt16(personDo.Value.AscendancyNumber) + 1).ToString();
                                if (personsPedigree.ContainsKey(parentAscNo)) {
                                    var parent = personsPedigree[parentAscNo];
                                    if (parent.IsFemale) {
                                        persons[personDo.Value.Id].Mother = persons[parent.Id];
                                    } else {
                                        persons[personDo.Value.Id].Father = persons[parent.Id];
                                        ;
                                    }
                                }
                            } else {
                                if (personDo.Value.DescendancyNumber != null) {
                                    if (personDo.Value.DescendancyNumber.IndexOf("-S") > -1) {
                                        var spousePedigree = personDo.Value.DescendancyNumber.Replace("-S", "");
                                        if (personsPedigree.ContainsKey(spousePedigree)) {
                                            persons[personDo.Value.Id].Spouse = persons[personsPedigree[spousePedigree].Id];
                                            persons[personsPedigree[spousePedigree].Id].Spouse = persons[personDo.Value.Id].Spouse;
                                        }
                                    } else {
                                        var parentPedigree = personDo.Value.DescendancyNumber.Substring(0, personDo.Value.DescendancyNumber.IndexOf("."));
                                        var parentAscNo = (Convert.ToInt16(parentPedigree) + Convert.ToInt16(parentPedigree)).ToString();
                                        if (personsPedigree.ContainsKey(parentAscNo)) {
                                            var parent = personsPedigree[parentAscNo];
                                            persons[parent.Id].Children.Add(personDo.Value.Id, persons[personDo.Value.Id]);
                                            persons[parent.Spouse.Id].Children.Add(personDo.Value.Id, persons[personDo.Value.Id]);
                                        }
                                    }
                                }
                            }
                        }
                    } catch (Exception e) {
                        logger.Error("GetAncestors " + e.Message, e);

                        throw;
                    }

                    try {
                        persons[person.Father.Id].Spouse = persons[person.Mother.Id];
                        persons[person.Mother.Id].Spouse = persons[person.Father.Id];

                        foreach (var relationship in ancestors.Relationships) {
                            if (relationship.Type.Equals("http://gedcomx.org/Couple")) {
                                persons[relationship.Person1.ResourceId].Spouse = persons[relationship.Person2.ResourceId];
                                persons[relationship.Person2.ResourceId].Spouse = persons[relationship.Person1.ResourceId];
                                if (researchDO.AddChildren) {
                                    addChildrenAncestry(relationship.Person1.ResourceId, ref persons, ref session);
                                }
                            }
                        }
                    } catch (Exception e) {
                        logger.Error("GetAncestors " + e.Message, e);
                        throw;
                    }
                }
            }

            return persons;
        }

        private void addChildrenAncestry(string id, ref Dictionary<string, PersonDO> persons, ref SessionDO session) {
            if (session.Error) {
                return;
            }

            var childrenRelationship = personDAO.GetChildren(id, ref session);
            if (childrenRelationship != null) {
                if (childrenRelationship.Persons != null) {
                    foreach (var personGx in childrenRelationship.Persons) {
                        AddPerson(personGx, persons, 0);
                    }
                }
                if (childrenRelationship.Relationships != null) {
                    foreach (var relationship in childrenRelationship.Relationships) {
                        if ((relationship.Type != null) && relationship.Type.Equals("http://gedcomx.org/ParentChild")) {
                            if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                if (persons[relationship.Person1.ResourceId].IsMale) {
                                    persons[relationship.Person2.ResourceId].Father = persons[relationship.Person1.ResourceId];
                                    persons[relationship.Person2.ResourceId].Father.Children.Add(relationship.Person2.ResourceId, persons[relationship.Person2.ResourceId]);
                                } else {
                                    persons[relationship.Person2.ResourceId].Mother = persons[relationship.Person1.ResourceId];
                                    persons[relationship.Person2.ResourceId].Mother.Children.Add(relationship.Person2.ResourceId, persons[relationship.Person2.ResourceId]);
                                }
                            }
                        }
                    }
                }
            }
        }

        public Dictionary<string, PersonDO> GetDescendants(ref ResearchDO researchDO, ref SessionDO session, bool populate) {
            logger.Info("Entered GetDescendants");
 
            if (session.Error) {
                return new Dictionary<string, PersonDO>();
            }
            var descendants = personDAO.GetDescendants(researchDO.PersonId, researchDO.Generation.ToString(), ref session);
            if (session.Error) {
                return new Dictionary<string, PersonDO>();
            }
            //            logger.Info("PersonDAO().GetDescendants = " + ((gedcomx == null) ? "0" : gedcomx.Persons.Count().ToString()));
            var persons = new Dictionary<string, PersonDO>();
            var personsPedigree = new Dictionary<string, PersonDO>();

            if ((descendants != null) && (descendants.Persons != null)) {
                PersonDO personDO = null;
                for (var i = descendants.Persons.Count - 1; i > -1; i--) {
                    personDO = personDAO.GetPerson(descendants.Persons[i]);
                    persons.Add(personDO.Id, personDO);
                    personsPedigree.Add((string.IsNullOrEmpty(personDO.AscendancyNumber) ? personDO.DescendancyNumber : personDO.AscendancyNumber), personDO);
                    //                       PopulatePerson(personDO.Id, ref persons, ref session, Constants.RESEARCH_TYPE_ANCESTORS);
                    logger.Info("PopulatePerson = " + personDO.Fullname);
                }

                foreach (var relationship in descendants.Relationships) {
                    if (relationship.Type.Equals("http://gedcomx.org/Couple")) {
                        persons[relationship.Person1.ResourceId].Spouse = persons[relationship.Person2.ResourceId];
                        persons[relationship.Person1.ResourceId].Spouses.Add(relationship.Person2.ResourceId, persons[relationship.Person2.ResourceId]);
                        persons[relationship.Person2.ResourceId].Spouse = persons[relationship.Person1.ResourceId];
                        persons[relationship.Person2.ResourceId].Spouses.Add(relationship.Person1.ResourceId, persons[relationship.Person1.ResourceId]);
                    }
                }

                var parent1 = new PersonDO();
                var parent2 = new PersonDO();
                foreach (var personDesc in persons) {
                    //                    if (personDesc.Value.DescendancyNumber.Equals("1.1")) {
                    //                            string test = "";
                    //                       }
                    if ((personDesc.Value.DescendancyNumber.IndexOf(".") > 0) && !(personDesc.Value.DescendancyNumber.IndexOf("-S") > -1)) {
                        var position = personDesc.Value.DescendancyNumber.LastIndexOf(".");
                        var key = personDesc.Value.DescendancyNumber.Substring(0, position);
                        if (personsPedigree.ContainsKey(key)) {
                            parent1 = personsPedigree[key];
                            persons[parent1.Id].Children.Add(personDesc.Value.Id, persons[personDesc.Value.Id]);
                            if (parent1.IsMale) {
                                persons[personDesc.Value.Id].Father = persons[parent1.Id];
                            } else {
                                persons[personDesc.Value.Id].Mother = persons[parent1.Id];
                            }
                        }
                        if (personsPedigree.ContainsKey(key + "-S")) {
                            parent2 = personsPedigree[key + "-S"];
                            persons[parent2.Id].Children.Add(personDesc.Value.Id, persons[personDesc.Value.Id]);
                            if (parent2.IsMale) {
                                persons[personDesc.Value.Id].Father = persons[parent2.Id];
                            } else {
                                persons[personDesc.Value.Id].Mother = persons[parent2.Id];
                            }
                        }
                    }
                }
                if (researchDO.Generation > 2) {
                    foreach (var personGen3 in personsPedigree) {
                        if ((personGen3.Value.Children.Count < 1) && personGen3.Value.HasChildrenLink) {
                            var childrenRelationship = personDAO.GetChildren(personGen3.Value.Id, ref session);
                            if (childrenRelationship != null) {
                                if (childrenRelationship.Persons != null) {
                                    foreach (var personGx in childrenRelationship.Persons) {
                                        AddPerson(personGx, persons, 0);
                                    }
                                }
                                if (childrenRelationship.Relationships != null) {
                                    foreach (var relationship in childrenRelationship.Relationships) {
                                        if ((relationship.Type != null) && relationship.Type.Equals("http://gedcomx.org/ParentChild")) {
                                            if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                                if (persons[relationship.Person1.ResourceId].IsMale) {
                                                    persons[relationship.Person2.ResourceId].Father = persons[relationship.Person1.ResourceId];
                                                } else {
                                                    persons[relationship.Person2.ResourceId].Mother = persons[relationship.Person1.ResourceId];
                                                }
                                                AddChildren(persons[relationship.Person2.ResourceId], persons[relationship.Person1.ResourceId].Children, 0);
                                                if (!persons[relationship.Person1.ResourceId].Spouse.IsEmpty) {
                                                    persons[relationship.Person1.ResourceId].Spouse.Children = persons[relationship.Person1.ResourceId].Children;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return persons;
        }

        //        //  deep scan
        //        public Dictionary<string, PersonDO> GetPersonAncestry1(String personId, ref SessionDO session) {
        //            var persons = new Dictionary<string, PersonDO>();
        //            int levels = 3;
        //            int levelsProcessed = 0;
        //            Gedcomx currentPersonGx = personDAO.GetPerson(personId, ref session);
        //
        //            //Gedcomx currentPersonGx = personDAO.GetCurrentPerson(ref session);
        //            PersonDO currentPerson = null;
        //            foreach (Person person in currentPersonGx.Persons) {
        //                AddPerson(person, persons);
        //            }
        //            if (currentPerson != null) {
        //                PopulatePerson(currentPerson.Id, ref persons, ref session, Constants.RESEARCH_TYPE_ANCESTORS);
        //                var ancestryDao = new AncestryDAO();
        //                GedcomxPerson ancestry = ancestryDao.GetPersonAncestry(currentPerson.Id, "2", ref session); //currentPerson.Id,  ("KW71-97V"
        //                PersonDO person = null;
        //                if ((ancestry != null) && (ancestry.Persons != null) && (ancestry.Persons.Count > 0)) {
        //                    for (int i = ancestry.Persons.Count - 1; i > -1; i--) {
        //                        Boolean populated = false;
        //                        if (persons.ContainsKey(ancestry.Persons[i].Id)) {
        //                            populated = persons[ancestry.Persons[i].Id].Populated;
        //                        }
        //                        if (!populated) {
        //                            AddPerson(ancestry.Persons[i], persons);
        //                            PopulatePerson(person.Id, ref persons, ref session, Constants.RESEARCH_TYPE_ANCESTORS);
        //                            logger.Info("Processed: " + person.Fullname);
        //                            persons = null;
        //
        //                            var ResearchDO = new ResearchDO();
        //                            validate(0, persons[person.Id], ref ResearchDO);
        //                        }
        //                    }
        //                }
        //            }
        //            return persons;
        //        }

        public List<AnalyzeListItemDO> GetAnalyzeData(FindCluesInputDO findCluesInputDo, ref SessionDO session) {
            AnalyzeListItemDO analyzeListItemDO = new AnalyzeListItemDO();

            var analyzeListItems = new List<AnalyzeListItemDO>();

            if (session.Error) {
                return analyzeListItems;
            }

            try {
                var persons = new Dictionary<string, PersonDO>();
                if (findCluesInputDo.ReportId > 0) {
                    persons = GetPersonsFromReport(findCluesInputDo.ReportId);
                    if (persons.Count > 0) {
                        var id = 1;
                        foreach (var personDO in persons) {
                            validate(ref id, personDO.Value.Id, persons, findCluesInputDo, ref analyzeListItems);
                            id++;
                        }
                    }
                }
            } catch (Exception e) {
                logger.Error("GetAnalyzeData " + e.Message, e);
                throw;
            }
            return analyzeListItems;
        }

        public void GetPersonAncestryValidations(ref ResearchDO researchDO, ref SessionDO session) {
            if (session.Error) {
                return;
            }

            try {
                var persons = new Dictionary<string, PersonDO>();
                if (session.Action.Equals(Constants.ACTION_RETRIEVE) || session.Action.Equals(Constants.ACTION_RETRIEVE_ANALYZE)) {
                    string personName = researchDO.PersonName;
                    persons = getPersons(ref researchDO, ref session);
                }
                if (session.Action.Equals(Constants.ACTION_ANALYZE) || session.Action.Equals(Constants.ACTION_RETRIEVE_ANALYZE)) {
                    ValidateData(ref researchDO, ref session, persons);
                }
            } catch (Exception e) {
                logger.Error("GetPersonAncestryValidations action = " + session.Action + "  " + e.Message, e);
                throw;
            }
        }

        private Dictionary<string, PersonDO> getPersons(ref ResearchDO researchDO, ref SessionDO session) {
            var persons = new Dictionary<string, PersonDO>();
            if (session.Error) {
                return persons;
            }

            if (researchDO.ResearchType.Equals(Constants.RESEARCH_TYPE_ANCESTORS)) {
                RetrieveAncestorsData(ref researchDO, ref session, ref persons);
                //                    AssignAscendancy(ref ResearchDO, ref session, ref persons);
            } else if (researchDO.ResearchType.Equals(Constants.RESEARCH_TYPE_DESCENDANTS)) {
                RetrieveDescendantsData(ref researchDO, ref session, ref persons);
            }

            if (researchDO.ReportId < 1) {
                storeData(ref researchDO, ref session, ref persons);
            }

            return persons;
        }

        public List<DateListItemDO> GetDates(DateInputDO datesInputDO, ref SessionDO session) {
            var dates = new Dictionary<string, string>();
            var dateListItems = new List<DateListItemDO>();
            if (session.Error) {
                return dateListItems;
            }

            ResearchDO researchDO = new ResearchDO();
            researchDO.PersonId = datesInputDO.PersonId;
            researchDO.ResearchType = datesInputDO.ResearchType;
            researchDO.Generation = datesInputDO.Generation;
            researchDO.ReportId = datesInputDO.ReportId;
            researchDO.PersonName = datesInputDO.PersonName;

            var ancestors = getPersons(ref researchDO, ref session);

            if ((ancestors != null) && (ancestors.Count > 0)) {
                foreach (var ancestor in ancestors) {
                    if (!ancestor.Value.Living && !dates.ContainsKey(ancestor.Value.Id)) {
                        dates.Add(ancestor.Value.Id, "");
                        DateListItemDO dateListItemDO = GetDatesListItem(datesInputDO, ancestor.Value);
                        if (dateListItemDO != null) {
                            dateListItems.Add(GetDatesListItem(datesInputDO, ancestor.Value));
                        }
                    }
                }
            }
            return dateListItems;
        }

        private DateListItemDO GetDatesListItem(DateInputDO datesInputDO, PersonDO personDO) {
            DateListItemDO listItem = null;
            string birthDate = ValidateDate(datesInputDO, personDO.BirthYear, personDO.BirthDate, "birth", personDO.BirthDateString);
            string deathDate = ValidateDate(datesInputDO, personDO.DeathYear, personDO.DeathDate, "death", personDO.DeathDateString);
            string marriageDate = ValidateDate(datesInputDO, personDO.MarriageYear, personDO.MarriageDate, "marriage", personDO.MarriageDateString);

            if (!string.IsNullOrEmpty(birthDate)) {
                listItem = new DateListItemDO();
                listItem.BirthDate = birthDate;
            }
            if (!string.IsNullOrEmpty(deathDate)) {
                if (listItem == null) {
                    listItem = new DateListItemDO();
                }
                listItem.DeathDate = deathDate;
            }
            if (!string.IsNullOrEmpty(marriageDate)) {
                if (listItem == null) {
                    listItem = new DateListItemDO();
                }
                listItem.MarriageDate = marriageDate;
            }
            if (listItem != null) {
                FindListItemDO findListItemDO = (FindListItemDO) listItem;
                personDAO.PopulateFindListItem(personDO, ref findListItemDO);
            }
            return listItem;
        }

        private string ValidateDate(DateInputDO dateInputDO, int eventYear, DateTime? eventDate, string eventType, string eventString) {
            string validateDate = null;
            if ((eventDate == null) && !string.IsNullOrEmpty(eventString)) {
                try {
                    eventDate = Dates.GetDateTime(eventString.Trim());
                    if (eventYear < 1) {
                        eventYear = eventDate.Value.Year;
                    }
                } catch (Exception) {
                    if (dateInputDO.Invalid) {
                        validateDate = "Invalid Format~" + eventString;
                    }
                }
            }
            if (eventYear == 0) {
                if (dateInputDO.Empty) {
                    validateDate = "Blank";
                }
            } else if (eventDate == null) {
                if (!string.IsNullOrEmpty(eventString)) {
                    var date = eventString.Trim().ToLower();
                    if (date.IndexOf("abt") > -1) {
                        validateDate = eventString;
                    } else if (date.IndexOf("about") > -1) {
                        validateDate = eventString;
                    } else if (date.IndexOf("aft") > -1) {
                        validateDate = eventString;
                    } else if (date.IndexOf("bef") > -1) {
                        validateDate = eventString;
                    } else if (date.IndexOf("<") > -1) {
                        validateDate = eventString;
                    } else if (date.IndexOf(">") > -1) {
                        validateDate = eventString;
                    } else if (date.IndexOf(">") > -1) {
                        validateDate = eventString;
                    } else if (date.IndexOf("death") > -1) {
                        validateDate = eventString;
                    }
                    if (!string.IsNullOrEmpty(validateDate) && dateInputDO.Invalid) {
                        validateDate = "Invalid~" + validateDate;
                    }
                } else {
                    if (dateInputDO.Incomplete) {
                        validateDate = "Missing Month & Day~" + eventYear; // Incomplete
                    }
                }
            }
            return validateDate;
        }

        public List<PlaceListItemDO> GetPlaces(PlaceInputDO placesInputDO, ref SessionDO session) {
            var places = new Dictionary<string, string>();

            var placeListItems = new List<PlaceListItemDO>();
            if (session.Error) {
                return placeListItems;
            }


            ResearchDO researchDO = new ResearchDO();
            researchDO.PersonId = placesInputDO.PersonId;
            researchDO.ResearchType = placesInputDO.ResearchType;
            researchDO.Generation = placesInputDO.Generation;
            researchDO.ReportId = placesInputDO.ReportId;
            researchDO.PersonName = placesInputDO.PersonName;

            var ancestors = getPersons(ref researchDO, ref session);

            if ((ancestors != null) && (ancestors.Count > 0)) {
                foreach (var ancestor in ancestors) {
                    if (!ancestor.Value.Living && !places.ContainsKey(ancestor.Value.Id)) {
                        places.Add(ancestor.Value.Id, "");
                        PlaceListItemDO placeListItemDO = GetPlacesListItem(placesInputDO, ancestor.Value);
                        if (placeListItemDO != null) {
                            placeListItems.Add(GetPlacesListItem(placesInputDO, ancestor.Value));
                        }
                    }
                }
            }
            return placeListItems;
        }

        private PlaceListItemDO GetPlacesListItem(PlaceInputDO placesInputDO, PersonDO personDO) {
            PlaceListItemDO listItem = null;
            string birthPlace = ValiplacePlace(placesInputDO, personDO.BirthPlace, "birth");
            string deathPlace = ValiplacePlace(placesInputDO, personDO.DeathPlace, "death");
            string marriagePlace = ValiplacePlace(placesInputDO, personDO.MarriagePlace, "marriage");

            if (!string.IsNullOrEmpty(birthPlace)) {
                listItem = new PlaceListItemDO();
                listItem.BirthPlace = birthPlace;
            }
            if (!string.IsNullOrEmpty(deathPlace)) {
                if (listItem == null) {
                    listItem = new PlaceListItemDO();
                }
                listItem.DeathPlace = deathPlace;
            }
            if (!string.IsNullOrEmpty(marriagePlace)) {
                if (listItem == null) {
                    listItem = new PlaceListItemDO();
                }
                listItem.MarriagePlace = marriagePlace;
            }
            if (listItem != null) {
                FindListItemDO findListItemDO = (FindListItemDO) listItem;
                personDAO.PopulateFindListItem(personDO, ref findListItemDO);
            }
            return listItem;
        }

        private string ValiplacePlace(PlaceInputDO placeInputDO, string eventPlace, string eventType) {
            string valiplacePlace = null;
            if (string.IsNullOrEmpty(eventPlace)) {
                valiplacePlace = "Blank";
            }
            return valiplacePlace;
        }

        //  "DEAD"
        //  marriage place but no marriage date
        //        private List<DatesListItemDO> getDateProblems1(DatesInputDO datesInputDO, ref SessionDO session) {
        //            var datesDO = new DatesDO();
        //            if (!personDO.Living && !datesDO.ContainsKey(personDO.Id + "-1") && !datesDO.ContainsKey(personDO.Id + "-2")) {
        //                if (personDO.BirthYear == 0) {
        //                    datesPlacesDO.DateFlag = true;
        //                    datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                } else if (personDO.BirthDate == null) {
        //                    if (!string.IsNullOrEmpty(personDO.BirthDateString)) {
        //                        var date = personDO.BirthDateString.Trim().ToLower();
        //                        if (date.IndexOf("abt") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                        } else if (date.IndexOf("about") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                        } else if (date.IndexOf("aft") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                        } else if (date.IndexOf("bef") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                        } else if (date.IndexOf("<") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                        } else if (date.IndexOf(">") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                        } else if (date.IndexOf(">") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.BirthDate = personDO.BirthDateString;
        //                        }
        //                    } else {
        //                        datesPlacesDO.DateFlag = true;
        //                        datesPlacesDO.BirthDate = "Blank";
        //                        ;
        //                    }
        //                }
        //                if (personDO.DeathYear == 0) {
        //                    datesPlacesDO.DateFlag = true;
        //                    datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                } else if (personDO.DeathDate == null) {
        //                    if (!string.IsNullOrEmpty(personDO.DeathDateString)) {
        //                        var date = personDO.DeathDateString.Trim().ToLower();
        //                        if (date.IndexOf("abt") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                        } else if (date.IndexOf("about") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                        } else if (date.IndexOf("<") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                        } else if (date.IndexOf("aft") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                        } else if (date.IndexOf("bef") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                        } else if (date.IndexOf("dead") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                        } else if (date.IndexOf(">") > -1) {
        //                            datesPlacesDO.DateFlag = true;
        //                            datesPlacesDO.DeathDate = personDO.DeathDateString;
        //                        }
        //                    } else {
        //                        datesPlacesDO.DateFlag = true;
        //                        datesPlacesDO.DeathDate = "Blank";
        //                    }
        //                }
        //                if (datesPlacesDO.DateFlag) {
        //                    datesPlacesDO.Id = personDO.Id;
        //                    datesPlacesDO.ProblemType = "Date";
        //                    datesPlacesDO.Fullname = personDO.Fullname;
        //                    datesPlaces.Add(personDO.Id + "-1", datesPlacesDO);
        //                }
        //
        //                datesPlacesDO = new DatesPlacesDO();
        //                if (!personDO.IsPlaceValid(personDO.BirthPlace)) {
        //                    datesPlacesDO.PlaceFlag = true;
        //                    datesPlacesDO.BirthPlace = string.IsNullOrEmpty(personDO.BirthPlace) ? "Blank" : personDO.BirthPlace;
        //                } else if (string.IsNullOrEmpty(personDO.BirthPlace)) {
        //                    datesPlacesDO.PlaceFlag = true;
        //                    datesPlacesDO.BirthPlace = "Blank";
        //                }
        //                if (!personDO.IsPlaceValid(personDO.DeathPlace)) {
        //                    datesPlacesDO.PlaceFlag = true;
        //                    datesPlacesDO.DeathPlace = string.IsNullOrEmpty(personDO.DeathPlace) ? "Blank" : personDO.DeathPlace;
        //                } else if (string.IsNullOrEmpty(personDO.DeathPlace)) {
        //                    datesPlacesDO.PlaceFlag = true;
        //                    datesPlacesDO.DeathPlace = "Blank";
        //                }
        //                if (!personDO.IsPlaceValid(personDO.MarriagePlace)) {
        //                    datesPlacesDO.PlaceFlag = true;
        //                    datesPlacesDO.MarriagePlace = string.IsNullOrEmpty(personDO.MarriagePlace) ? "Blank" : personDO.MarriagePlace;
        //                } else if (string.IsNullOrEmpty(personDO.MarriagePlace)) {
        //                    datesPlacesDO.PlaceFlag = true;  
        //                    datesPlacesDO.MarriagePlace = "Blank";
        //                }
        //
        //                if (datesPlacesDO.PlaceFlag) {
        //                    datesPlacesDO.Id = personDO.Id;
        //                    datesPlacesDO.Fullname = personDO.Fullname;
        //                    datesPlacesDO.ProblemType = "Place";
        //                    datesPlaces.Add(datesPlacesDO.Id + "-2", datesPlacesDO);
        //                }
        //            }
        //        }

        //        public List<DatesPlacesListItemDO> GetDatesPlaces(DatesPlacesInputDO datesPlacesInputDO, ref SessionDO session) {
        ////            var datesPlaces = new Dictionary<string, DatesPlacesDO>();
        ////            var persons = new Dictionary<string, PersonDO>();
        ////            var children = new Dictionary<string, PersonDO>();
        ////
        ////            if (!String.IsNullOrEmpty(ResearchDO.PersonId)) {
        ////                var person = GetPerson(ResearchDO.PersonId, ref session);
        ////                ResearchDO.PersonName = person.Fullname;
        ////                if (ResearchDO.ResearchType.Equals("Descendants")) {
        ////                    persons = GetDescendants(ref ResearchDO, ref session, false);
        ////                    if ((persons != null) && (persons.Count > 0)) {
        ////                        foreach (var personDO in persons.Values) {
        ////                            processDatePlace(personDO, ref datesPlaces);
        ////                        }
        ////                        ResearchDO.DatesPlaces = new List<DatesPlacesDO>(datesPlaces.Values);
        ////                    }
        ////                } else {
        ////                    AddPerson(person, persons, 0);
        ////                    person = getParents(ResearchDO.PersonId, ref persons, ref children, ref session);
        ////
        ////                    var fatherAncestors = personDAO.GetAncestors(person.Father.Id, ResearchDO.Generation.ToString(), ref session);
        ////                    if ((fatherAncestors != null) && (fatherAncestors.Persons.Count > 0)) {
        ////                        foreach (var fatherAncestor in fatherAncestors.Persons) {
        ////                            var personDO = personDAO.GetPerson(fatherAncestor);
        ////                            processDatePlace(personDO, ref datesPlaces);
        ////                        }
        ////                    }
        ////
        ////                    var motherAncestors = personDAO.GetAncestors(person.Mother.Id, ResearchDO.Generation.ToString(), ref session);
        ////                    if ((motherAncestors != null) && (motherAncestors.Persons.Count > 0)) {
        ////                        foreach (var motherAncestor in motherAncestors.Persons) {
        ////                            var personDO = personDAO.GetPerson(motherAncestor);
        ////                            processDatePlace(personDO, ref datesPlaces);
        ////                        }
        ////                    }
        ////                    ResearchDO.DatesPlaces = new List<DatesPlacesDO>(datesPlaces.Values);
        ////                    ResearchDO.RetrievedRecords = ResearchDO.DatesPlaces.Count;
        ////                }
        ////            }
        //            return null;
        //        }

        public OrdinanceListItemDO GetOrdinanceListItem(OrdinanceDO ordinance, PersonDO personDO) {
            OrdinanceListItemDO listItem = new OrdinanceListItemDO();
            FindListItemDO findListItemDO = (FindListItemDO) listItem;
            personDAO.PopulateFindListItem(personDO, ref findListItemDO);

            if (ordinance.Baptism != null) {
                listItem.Status += "<p>Baptism: <b>" + ordinance.Baptism.status;
                if (!ordinance.Baptism.completed) {
                    if (ordinance.Baptism.reservable) {
                        listItem.Status += " - Reservable"; // + ordinance.Baptism.reservable;
                    } else {
                        listItem.Status += " - " + ordinance.Baptism.reservable;
                    }
                }
                listItem.Status += "</b></p>";
            }
            if (ordinance.Confirmation != null) {
                listItem.Status += "<p>Confirmation: <b>" + ordinance.Confirmation.status;
                if (!ordinance.Confirmation.completed) {
                    listItem.Status += " - Reservable"; // + ordinance.Confirmation.reservable;
                } else {
                    listItem.Status += " - " + ordinance.Confirmation.reservable;
                }
                listItem.Status += "</b></p>";
            }
            if (ordinance.Initiatory != null) {
                listItem.Status += "<p>Initiatory: <b>" + ordinance.Initiatory.status;
                if (!ordinance.Initiatory.completed) {
                    listItem.Status += " - Reservable"; // + ordinance.Initiatory.reservable;
                } else {
                    listItem.Status += " - " + ordinance.Initiatory.reservable;
                }
                listItem.Status += "</b></p>";
            }
            if (ordinance.Endowment != null) {
                listItem.Status += "<p>Endowment: <b>" + ordinance.Endowment.status;
                if (!ordinance.Endowment.completed) {
                    listItem.Status += " - Reservable"; // + ordinance.Endowment.reservable;
                } else {
                    listItem.Status += " - " + ordinance.Endowment.reservable;
                }
                listItem.Status += "</b></p>";
            }
            if (ordinance.SealedToParent != null) {
                listItem.Status += "<p>Sealed To Parent: <b>" + ordinance.SealedToParent.status;
                if (!ordinance.SealedToParent.completed) {
                    listItem.Status += " - Reservable"; // + ordinance.SealedToParent.reservable;
                } else {
                    listItem.Status += " - " + ordinance.SealedToParent.reservable;
                }
                listItem.Status += "</b></p>";
            }
            if (ordinance.SealedToSpouse != null) {
                listItem.Status += "<p>Sealed To Spouse: <b>" + ordinance.SealedToSpouse.status;
                if (!ordinance.SealedToSpouse.completed) {
                    listItem.Status += " - Reservable"; // + ordinance.SealedToSpouse.reservable;
                } else {
                    listItem.Status += " - " + ordinance.SealedToSpouse.reservable;
                }
                listItem.Status += "</b></p>";
            } else {
                if (ordinance.Baptism.status.Equals("Not Needed")) {
                    listItem.Status += "<p>No Spouse</p>";
                }
            }
            return listItem;
        }

        public string GetOrdinanceInfo(OrdinanceDO ordinance) {
            string ordinanceInfo = "";
            if (ordinance.Baptism != null) {
                if (!ordinance.Baptism.completed) {
                    ordinanceInfo += "<p>&nbsp;&nbsp;&nbsp;Baptism: <b>" + ordinance.Baptism.status + " Reservable: " + ordinance.Baptism.reservable + "</b></p>";
                }
            }
            if (ordinance.Confirmation != null) {
                if (!ordinance.Confirmation.completed) {
                    ordinanceInfo += "<p>&nbsp;&nbsp;&nbsp;Confirmation: <b>" + ordinance.Confirmation.status + " Reservable: " + ordinance.Confirmation.reservable + "</b></p>";
                }
            }
            if (ordinance.Initiatory != null) {
                if (!ordinance.Initiatory.completed) {
                    ordinanceInfo += "<p>&nbsp;&nbsp;&nbsp;Initiatory: <b>" + ordinance.Initiatory.status + " Reservable: " + ordinance.Initiatory.reservable + "</b></p>";
                }
            }
            if (ordinance.Endowment != null) {
                if (!ordinance.Endowment.completed) {
                    ordinanceInfo += "<p>&nbsp;&nbsp;&nbsp;Endowment: <b>" + ordinance.Endowment.status + " Reservable: " + ordinance.Endowment.reservable + "</b></p>";
                }
            }
            if (ordinance.SealedToParent != null) {
                if (!ordinance.SealedToParent.completed) {
                    ordinanceInfo += "<p>&nbsp;&nbsp;&nbsp;SealedToParent: <b>" + ordinance.SealedToParent.status + " Reservable: " + ordinance.SealedToParent.reservable + "</b></p>";
                }
            }
            if (ordinance.SealedToSpouse != null) {
                if (!ordinance.SealedToSpouse.completed) {
                    ordinanceInfo += "<p>&nbsp;&nbsp;&nbsp;SealedToSpouse: <b>" + ordinance.SealedToSpouse.status + " Reservable: " + ordinance.SealedToSpouse.reservable + "</b></p>";
                }
            } else {
                if (ordinance.Baptism.status.Equals("Not Needed")) {
                    ordinanceInfo += "<p>&nbsp;&nbsp;&nbsp;No Spouse</p>";
                }
            }
            return ordinanceInfo;
        }

        public List<OrdinanceListItemDO> GetOrdinances(IncompleteOrdinanceDO incompleteOrdinanceDo, ref SessionDO session) {
            var ordinances = new Dictionary<string, OrdinanceDO>();
            var ordinanceListItems = new List<OrdinanceListItemDO>();
            var personName = "";

            ResearchDO researchDO = new ResearchDO();
            researchDO.PersonId = incompleteOrdinanceDo.Id;
            researchDO.ResearchType = incompleteOrdinanceDo.ResearchType;
            researchDO.Generation = incompleteOrdinanceDo.Generation;
            researchDO.ReportId = incompleteOrdinanceDo.ReportId;
            researchDO.PersonName = incompleteOrdinanceDo.FullName;

            var ancestors = getPersons(ref researchDO, ref session);

            OrdinanceDO ordinance = null;
            if ((ancestors != null) && (ancestors.Count > 0)) {
                foreach (var ancestor in ancestors) {
                    if (!ordinances.ContainsKey(ancestor.Value.Id)) {
                        ordinance = personDAO.GetOrdinances(ancestor.Value, ref session);
                        if ((ordinance != null) && !ancestor.Value.Living) {
                            if ((ordinance.Baptism.reservable) || (ordinance.Confirmation.reservable) || (ordinance.Initiatory.reservable) || (ordinance.Endowment.reservable)) {
                                ordinances.Add(ancestor.Value.Id, ordinance);
                                ordinanceListItems.Add(GetOrdinanceListItem(ordinance, ancestor.Value));
                            } else {
                                if (((ordinance.SealedToParent != null) && ordinance.SealedToParent.reservable) || ((ordinance.SealedToSpouse != null) && ordinance.SealedToSpouse.reservable)) {
                                    ordinances.Add(ancestor.Value.Id, ordinance);
                                    ordinanceListItems.Add(GetOrdinanceListItem(ordinance, ancestor.Value));
                                }
                            }
                        }
                    }
                }
            }
            return ordinanceListItems;
        }

        public HintListItemDO GetHintListItem(HintDO hint, PersonDO personDO) {
            HintListItemDO listItem = new HintListItemDO();
            FindListItemDO findListItemDO = (FindListItemDO) listItem;

            personDAO.PopulateFindListItem(personDO, ref findListItemDO);

            var hints = "";
            if ((hint.Entries != null) && (hint.Entries.Count > 0)) {
                double topScore = 0.0;
                foreach (var entry in hint.Entries) {
                    hints += "<p><a style=\"color: rgb(50,205,50)\" href=\"" + entry.Id + "\" target=\"_tab\">" + entry.Fullname + "</a> Score: " + entry.Score + "</p>";
                    if (entry.Score > topScore) {
                        topScore = entry.Score;
                    }
                    listItem.Count++;

                    //                    foreach (var person in entry.Persons) {
                    //                        hints += "<p><a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.PersonUrl(person.Value.Id) + "\" target=\"_tab\">" + person.Value.Fullname + " - " + person.Value.Id + "</a></p>";
                    //                    }
                }
                listItem.TopScore = topScore;
                listItem.Hints = hints;
            }

            return listItem;
        }

        public List<HintListItemDO> GetHints(HintInputDO hintInputDO, ref SessionDO session) {
            var hints = new Dictionary<string, HintDO>();
            var hintListItems = new List<HintListItemDO>();
            var personName = "";

            ResearchDO researchDO = new ResearchDO();
            researchDO.PersonId = hintInputDO.Id;
            researchDO.ResearchType = hintInputDO.ResearchType;
            researchDO.Generation = hintInputDO.Generation;
            researchDO.ReportId = hintInputDO.ReportId;
            researchDO.PersonName = hintInputDO.FullName;

            var ancestors = getPersons(ref researchDO, ref session);

            HintDO hint = null;
            if ((ancestors != null) && (ancestors.Count > 0)) {
                foreach (var ancestor in ancestors) {
                    if (!hints.ContainsKey(ancestor.Value.Id)) {
                        hint = personDAO.GetHints(ancestor.Value, ref session);
                        if ((hint != null) && (hint.Results > 0)) {
                            hints.Add(ancestor.Value.Id, hint);
                            hintListItems.Add(GetHintListItem(hint, ancestor.Value));
                        }
                    }
                }
            }
            if (hintInputDO.topScore) {
                return hintListItems.OrderByDescending(o => o.TopScore).ToList();
            } else {
                return hintListItems.OrderByDescending(o => o.Count).ToList();
            }
        }

        private StartingPointListItemDO GetStartingPoints(PersonDO personDO, StartingPointInputDO startingPointInputDO, ref SessionDO session) {
            StartingPointListItemDO startingPointListItemDO = new StartingPointListItemDO();
            FindListItemDO findListItemDO = (FindListItemDO) startingPointListItemDO;

            if (!personDO.Living) {
                personDAO.PopulateFindListItem(personDO, ref findListItemDO);
                startingPointListItemDO.Reasons = "";
                if (startingPointInputDO.Born18101850 && ((personDO.BirthYear >= 1810) && (personDO.BirthYear <= 1850))) {
                    startingPointListItemDO.Reasons += "BornBetween1810and1850[" + personDO.BirthYear.ToString() + "]~";
                    startingPointListItemDO.Count++;
                }
                if (startingPointInputDO.LivedInUSA && personDO.BirthYear < 1) {
                    startingPointListItemDO.Reasons += "NoBirthDate[" + personDO.BirthDateString + "]~";
                    startingPointListItemDO.Count++;
                }

                if (startingPointInputDO.LivedInUSA && string.IsNullOrEmpty(personDO.BirthPlace)) {
                    startingPointListItemDO.Reasons += "NoBirthPlace~";
                    startingPointListItemDO.Count++;
                }

                if (startingPointInputDO.LivedInUSA) {
                    if (!string.IsNullOrEmpty(personDO.BirthPlace)) {
                        var birthPlace = personDO.BirthPlace.ToLower();
                        if ((birthPlace.IndexOf("united states") > -1) || (birthPlace.IndexOf("usa") > -1) || (birthPlace.IndexOf("u.s.a") > -1) || (birthPlace.IndexOf("united") > -1) || (birthPlace.IndexOf("states") > -1)) {
                            startingPointListItemDO.Reasons += "BornInUSA[" + personDO.BirthPlace + "]~";
                            startingPointListItemDO.Count++;
                        }
                    }

                    if (!string.IsNullOrEmpty(personDO.DeathPlace)) {
                        var deathPlace = personDO.DeathPlace.ToLower();
                        if ((deathPlace.IndexOf("united states") > -1) || (deathPlace.IndexOf("usa") > -1) || (deathPlace.IndexOf("u.s.a") > -1) || (deathPlace.IndexOf("united") > -1) || (deathPlace.IndexOf("states") > -1)) {
                            startingPointListItemDO.Reasons += "DiedInUSA[" + personDO.DeathPlace + "]~";
                            startingPointListItemDO.Count++;
                        }
                    }
                }

                if (startingPointInputDO.NonMormon || startingPointInputDO.NeedOrdinances) {
                    OrdinanceDO ordinance = personDAO.GetOrdinances(personDO, ref session);
                    if (!session.Error && (ordinance != null) && !personDO.Living) {
                        if ((ordinance.Baptism.reservable) || (ordinance.Confirmation.reservable) || (ordinance.Initiatory.reservable) || (ordinance.Endowment.reservable)) {
                            startingPointListItemDO.Reasons += "IncompleteOrdinances[" + GetOrdinanceInfo(ordinance) + "]~";
                            startingPointListItemDO.Count++;
                        } else {
                            if (((ordinance.SealedToParent != null) && ordinance.SealedToParent.reservable) || ((ordinance.SealedToSpouse != null) && ordinance.SealedToSpouse.reservable)) {
                                startingPointListItemDO.Reasons += "IncompleteOrdinances[" + GetOrdinanceInfo(ordinance) + "]~";
                                startingPointListItemDO.Count++;
                            }
                        }
                        if (ordinance.Baptism.date != null) {
                            if (personDO.DeathYear > 100) {
                                if (ordinance.Baptism.date.normalized.Length > 4) {
                                    DateTime? baptismDate = null;
                                    try {
                                        baptismDate = Dates.GetDateTime(ordinance.Baptism.date.original);
                                    } catch (Exception) {
                                    }
                                    var notNeeded = ordinance.Baptism.status.Equals("Not Needed") ? true : false;
                                    if (startingPointInputDO.NonMormon && ((baptismDate != null) && !notNeeded && baptismDate.Value.Year > personDO.DeathYear)) {
                                        startingPointListItemDO.Reasons += "NonMormon~";
                                        startingPointListItemDO.Count++;
                                    }
                                } else {
                                    var baptismYear = Convert.ToInt16(ordinance.Baptism.date.normalized);

                                    var notNeeded = ordinance.Baptism.status.Equals("Not Needed") ? true : false;
                                    if (startingPointInputDO.NonMormon && ((baptismYear > 100) && !notNeeded && baptismYear > personDO.DeathYear)) {
                                        startingPointListItemDO.Reasons += "NonMormon~";
                                        startingPointListItemDO.Count++;
                                    }
                                }
                            } else {
                                if (ordinance.Baptism.date.normalized.Length > 4) {
                                    DateTime? baptismDate = null;
                                    try {
                                        baptismDate = Dates.GetDateTime(ordinance.Baptism.date.original);
                                    } catch (Exception) {
                                    }
                                    var notNeeded = ordinance.Baptism.status.Equals("Not Needed") ? true : false;
                                    if (startingPointInputDO.NonMormon && ((baptismDate != null) && !notNeeded && baptismDate.Value.Year > personDO.BirthYear + 100)) {
                                        startingPointListItemDO.Reasons += "NonMormon~";
                                        startingPointListItemDO.Count++;
                                    }
                                } else {
                                    var baptismYear = Convert.ToInt16(ordinance.Baptism.date.normalized);

                                    var notNeeded = ordinance.Baptism.status.Equals("Not Needed") ? true : false;
                                    if (startingPointInputDO.NonMormon && ((baptismYear > 100) && !notNeeded && baptismYear > personDO.DeathYear + 100)) {
                                        startingPointListItemDO.Reasons += "NonMormon~";
                                        startingPointListItemDO.Count++;
                                    }
                                }
                            }
                        } else {
                            var notNeeded = ordinance.Baptism.status.Equals("Not Needed") ? true : false;
                            if (!notNeeded) {
                                startingPointListItemDO.Reasons += "NonMormon~";
                                startingPointListItemDO.Count++;
                            }
                        }
                    } else {
                        if (startingPointInputDO.NonMormon) {
                            startingPointListItemDO.Reasons += "NonMormon~";
                            startingPointListItemDO.Count++;
                        }
                    }
                }

                if (!session.Error && startingPointInputDO.Duplicate) {
                    PossibleDuplicateDO possibleDuplicate = personDAO.GetPossibleDuplicates(personDO, ref session);
                    if ((possibleDuplicate != null) && possibleDuplicate.Results > 0) {
                        foreach (var entry in possibleDuplicate.Entries) {
                            if (entry.Score > .5) {
                                startingPointListItemDO.Reasons += "PossibleDuplicate[" + entry.Fullname + " (" + entry.Score + ")]~";
                                startingPointListItemDO.Count++;
                            }
                        }
                    }
                }

                if (!session.Error && startingPointInputDO.Hint) {
                    HintDO hint = personDAO.GetHints(personDO, ref session);
                    if ((hint != null) && hint.Results > 0) {
                        foreach (var entry in hint.Entries) {
                            startingPointListItemDO.Reasons += "Hint[" + entry.Fullname + " (" + entry.Score + ")]~";
                            startingPointListItemDO.Count++;
                        }
                    }
                }
            }

            return startingPointListItemDO;
        }

        public List<StartingPointListItemDO> GetStartingPoints(StartingPointInputDO startingPointInputDO, ref SessionDO session) {
            var startingPoints = new Dictionary<string, StartingPointDO>();
            var startingPointListItems = new List<StartingPointListItemDO>();

            ResearchDO researchDO = new ResearchDO();
            researchDO.PersonId = startingPointInputDO.Id;
            researchDO.ResearchType = startingPointInputDO.ResearchType;
            researchDO.Generation = startingPointInputDO.Generation;
            researchDO.ReportId = startingPointInputDO.ReportId;
            researchDO.PersonName = startingPointInputDO.FullName;

            var ancestors = getPersons(ref researchDO, ref session);
            if (!session.Error) {
                StartingPointListItemDO startingPointListItemDO;
                if ((ancestors != null) && (ancestors.Count > 0)) {
                    foreach (var ancestor in ancestors) {
                        if (!startingPoints.ContainsKey(ancestor.Value.Id)) {
                            startingPointListItemDO = GetStartingPoints(ancestor.Value, startingPointInputDO, ref session);
                            if (!session.Error && (startingPointListItemDO.Reasons != null) && (startingPointListItemDO.Reasons.Length > 0)) {
                                startingPointListItems.Add(startingPointListItemDO);
                            }
                        }
                        if (session.Error) {
                            break;
                        }
                    }
                }
            }

            return startingPointListItems.OrderByDescending(o => o.Count).ToList();
        }

        public PossibleDuplicateListItemDO GetPossibleDuplicateListItem(PossibleDuplicateDO possibleDuplicate, PersonDO personDO) {
            PossibleDuplicateListItemDO listItem = new PossibleDuplicateListItemDO();
            FindListItemDO findListItemDO = (FindListItemDO) listItem;
            personDAO.PopulateFindListItem(personDO, ref findListItemDO);

            listItem.Link = possibleDuplicate.Id;
            //            if (possibleDuplicate.Duplicates) {
            //                listItem.Link = "<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.PossibleDuplicateUrl(possibleDuplicate.Id) + "\" target=\"_tab\">Possible Duplicate</a>&nbsp;";
            //            }
            listItem.Title = possibleDuplicate.Title;
            listItem.Results = possibleDuplicate.Results.ToString();

            var duplicates = "";
            if ((possibleDuplicate.Entries != null) && (possibleDuplicate.Entries.Count > 0)) {
                foreach (var entry in possibleDuplicate.Entries) {
                    duplicates += "<b><p>ID: " + entry.Id + ", Fullname: " + entry.Fullname + ", Score: " + entry.Score + "</p></b>";
                    foreach (var person in entry.Persons) {
                        duplicates += "<p><a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.PersonUrl(person.Value.Id) + "\" target=\"_tab\">" + person.Value.Fullname + " - " + person.Value.Id + "</a></p>";
                        listItem.Count++;
                    }
                }
            }
            listItem.Duplicates = duplicates;

            return listItem;
        }

        public List<PossibleDuplicateListItemDO> GetPossibleDuplicates(PossibleDuplicateInputDO possibleDuplicateInputDO, ref SessionDO session) {
            var possibleDuplicates = new Dictionary<string, PossibleDuplicateDO>();
            var possibleDuplicateListItems = new List<PossibleDuplicateListItemDO>();
            var personName = "";

            ResearchDO researchDO = new ResearchDO();
            researchDO.PersonId = possibleDuplicateInputDO.PersonId;
            researchDO.ResearchType = possibleDuplicateInputDO.ResearchType;
            researchDO.Generation = possibleDuplicateInputDO.Generation;
            researchDO.ReportId = possibleDuplicateInputDO.ReportId;
            researchDO.PersonName = possibleDuplicateInputDO.PersonName;

            var ancestors = getPersons(ref researchDO, ref session);

            if (!session.Error) {
                PossibleDuplicateDO possibleDuplicate = null;
                if ((ancestors != null) && (ancestors.Count > 0)) {
                    foreach (var ancestor in ancestors) {
                        if (!possibleDuplicates.ContainsKey(ancestor.Value.Id)) {
                            possibleDuplicate = personDAO.GetPossibleDuplicates(ancestor.Value, ref session);
                            if (session.Error) {
                                break;
                            }
                            if ((possibleDuplicate != null) && (possibleDuplicate.Results > 0)) {
                                if ((possibleDuplicateInputDO.IncludePossibleDuplicates && possibleDuplicate.Duplicates) || (possibleDuplicateInputDO.IncludePossibleMatches && possibleDuplicate.Matches)) {
                                    possibleDuplicates.Add(ancestor.Value.Id, possibleDuplicate);
                                    possibleDuplicateListItems.Add(GetPossibleDuplicateListItem(possibleDuplicate, ancestor.Value));
                                }
                            }
                        }
                    }
                }
            }
            return possibleDuplicateListItems.OrderByDescending(o => o.Count).ToList();
            ;
        }

        private void AssignAscendancy(ref ResearchDO researchDO, ref SessionDO session, ref Dictionary<string, PersonDO> persons) {
            var totalCount = persons.Count;
            for (var i = totalCount - 1; i > -1; i--) {
                var id = persons.Keys.ElementAt(i);
                if (!persons[id].Populated && !persons[id].Living) {
                    //                    PopulatePerson(persons[id].Id, ref persons, ref session, ResearchDO.ResearchType);
                    //logger.Info("Processed: " + person.Fullname);
                    persons[id].Descendants = true;
                } else if (!persons[id].Living) {
                    persons[id].Ancestors = true;
                }
            }

            //  Assign assendancy to each person
            if ((persons != null) && (persons.Count > 0)) {
                var currentPerson = persons[researchDO.CurrentPersonId];
                //                currentPerson.AscendancyNumber = 1;
                if (currentPerson.HasSpouse) {
                    foreach (var spouseDo in currentPerson.Spouses) {
                        persons[spouseDo.Value.Id].AscendancyNumber = currentPerson.AscendancyNumber + 1;
                    }
                }
                if (currentPerson.HasParents) {
                    foreach (var parentDo in currentPerson.Parents) {
                        persons[parentDo.Value.Id].AscendancyNumber = currentPerson.AscendancyNumber + 1;
                        persons[parentDo.Value.Id].DirectLine = true;
                    }
                }
                bool foundAsendancyNumberZero;
                do {
                    foundAsendancyNumberZero = false;
                    foreach (var personDo in persons) {
                        //                        if (personDo.Value.Ancestor && personDo.Value.AscendancyNumber > 0) {
                        //                            if (currentPerson.HasParents) {
                        //                                foreach (var parentDo in personDo.Value.Parents) {
                        //                                    if (persons[parentDo.Value.Id].AscendancyNumber == 0) {
                        //                                        persons[parentDo.Value.Id].AscendancyNumber = personDo.Value.AscendancyNumber + 1;
                        //                                        persons[parentDo.Value.Id].DirectLine = true;
                        //                                        break;
                        //                                    }
                        //                                }
                        //                            }
                        //                        } else {
                        //                            if (personDo.Value.Ancestor) {
                        //                                foundAsendancyNumberZero = true;
                        //                            }
                        //                        }
                    }
                } while (foundAsendancyNumberZero);
            }
        }

        public void ValidateData(ref ResearchDO researchDO, ref SessionDO session) {
            ValidateData(ref researchDO, ref session, null);
        }

        public void ValidateData(ref ResearchDO researchDO, ref SessionDO session, Dictionary<string, PersonDO> persons) {
            var analyzeListItems = new List<ValidationDO>();
            var person = GetPerson(researchDO.PersonId, ref session);
            researchDO.PersonName = person.Fullname;

            string reportFilePath = null;
            if (session.Action.Equals(Constants.ACTION_ANALYZE)) {
                persons = GetPersonsFromReport(researchDO.ReportId);
                //                ResearchDO.ReportTitle = report.ReportTitle;
            } else if (session.Action.Equals(Constants.ACTION_RETRIEVE_ANALYZE)) {
                persons = GetPersonsFromReport(session.ReportFilePath);
            }
            if ((persons != null) && (persons.Count > 0)) {
                var id = 1;
                foreach (var personDo in persons) {
                    //                    validate(ref id, personDo.Value.Id, ref persons, null, ref ResearchDO);
                    id++;
                }
            }
        }

        public Dictionary<string, PersonDO> GetPersonsFromReport(int reportId) {
            string reportFilePath = null;
            var report = new ReportDO();
            report.ReportId = reportId;
            report = PersonServices.Instance.ReadReport(report);
            if (report != null) {
                reportFilePath = report.ReportFile;
            }
            return GetPersonsFromReport(reportFilePath);
        }

        public Dictionary<string, PersonDO> GetPersonsFromReport(string reportFilePath) {
            Dictionary<string, PersonDO> persons = null;
            if (!string.IsNullOrEmpty(reportFilePath)) {
                if (File.Exists(reportFilePath)) {
                    using (var file = File.OpenRead(reportFilePath)) {
                        var personsDO = Serializer.Deserialize<PersonsDO>(file);
                        persons = personsDO.Persons;
                    }
                }
            }
            return persons;
        }

        public PersonDO getPersonWithParents(string personId, ref Dictionary<string, PersonDO> persons, ref SessionDO session) {
            PersonDO personDO = null;

            if (session.Username.Equals(personId)) {
                var personFile = FilePathHelper.GetTempPath() + "/" + personId + "_PersonWithParents.bin";
                if (!string.IsNullOrEmpty(personFile)) {
                    if (File.Exists(personFile)) {
                        using (var file = File.OpenRead(personFile)) {
                            try {
                                personDO = Serializer.Deserialize<PersonDO>(file);
                            } catch (Exception) {
                                file.Close();
                                if (File.Exists(personFile)) {
                                    File.Delete(personFile);
                                }
                            }
                        }
                    }
                }
            }
            if ((personDO == null) || ((personDO != null) && personDO.IsEmpty)) {
                personDO = storePersonWithParents(personId, ref persons, ref session);
            }
            return personDO;
        }

        public PersonDO storePersonWithParents(string personId, ref Dictionary<string, PersonDO> persons, ref SessionDO session) {
            PersonDO personDO = null;
            if (!string.IsNullOrEmpty(personId)) {
                personDO = GetPerson(personId, ref session);
                AddPerson(personDO, persons, 0);
                personDO = getParents(personId, ref persons, ref session);
                if (session.Username.Equals(personId)) {
                    storePersonWithParents(personDO, ref persons, ref session);
                }
            }
            return personDO;
        }

        public void storePersonWithParents(PersonDO personDO, ref Dictionary<string, PersonDO> persons, ref SessionDO session) {
            if ((personDO != null) && !personDO.IsEmpty) {
                var personFile = FilePathHelper.GetTempPath() + "/" + personDO.Id + "_PersonWithParents.bin";
                if (File.Exists(personFile)) {
                    File.Delete(personFile);
                }

                using (var file = File.Create(personFile)) {
                    Serializer.Serialize(file, personDO);
                }
            }
        }

        public void GetAncestorsDescendantsData(ref ResearchDO researchDO, ref SessionDO session, ref Dictionary<string, PersonDO> persons) {
            var children = new Dictionary<string, PersonDO>();
            var levelsProcessed = 0;

            if (!String.IsNullOrEmpty(researchDO.PersonId)) {
                var ancestryDao = new AncestryDAO();
                var person = getParents(researchDO.PersonId, ref persons, ref session);

                if (!session.Error && (researchDO.Generation > 1) && (person != null) && ((person.Father != null) && !person.Father.IsEmpty)) {
                    var ancestryFather = ancestryDao.GetPersonAncestry(person.Father.Id, researchDO.Generation.ToString(), ref session);
                    //                    foreach (Person person1 in ancestryFather.Persons) {
                    //                        logger.Info(person1.Display.Name);
                    //                    }
                    if (!session.Error && (ancestryFather != null) && (ancestryFather.Persons != null) && (ancestryFather.Persons.Count > 0)) {
                        for (var i = ancestryFather.Persons.Count - 1; i > -1; i--) {
                            if (ancestryFather.Persons[i].Id.Equals("KW71-FN3")) {
                                var test = "";
                            }

                            var populated = false;
                            if ((persons != null) && (persons.Count > 0) && persons.ContainsKey(ancestryFather.Persons[i].Id)) {
                                populated = persons[ancestryFather.Persons[i].Id].Populated;
                            }
                            if (!populated) {
                                AddPerson(ancestryFather.Persons[i], persons, 0); //person.AscendancyNumber);
                                PopulatePerson(ancestryFather.Persons[i].Id, ref persons, ref session, researchDO.ResearchType);
                            }
                        }
                    }
                }

                if ((researchDO.Generation > 1) && (person != null) && ((person.Mother != null) && !person.Mother.IsEmpty)) {
                    var ancestryMother = ancestryDao.GetPersonAncestry(person.Mother.Id, researchDO.Generation.ToString(), ref session);
                    //                    foreach (Person person1 in ancestryMother.Persons) {
                    //                        logger.Info(person1.Display.Name);
                    //                    }
                    if (!session.Error && (ancestryMother != null) && (ancestryMother.Persons != null) && (ancestryMother.Persons.Count > 0)) {
                        for (var i = ancestryMother.Persons.Count - 1; i > -1; i--) {
                            //                            if (ancestryMother.Persons[i].Id.Equals("KW71-FN3")) {
                            //                                string test = "";
                            //                            }

                            var populated = false;
                            if ((persons != null) && (persons.Count > 0) && persons.ContainsKey(ancestryMother.Persons[i].Id)) {
                                populated = persons[ancestryMother.Persons[i].Id].Populated;
                            }
                            if (!populated) {
                                AddPerson(ancestryMother.Persons[i], persons, 0); //person.AscendancyNumber);
                                PopulatePerson(ancestryMother.Persons[i].Id, ref persons, ref session, researchDO.ResearchType);
                            }
                        }
                    }
                }

                //                if (ResearchDO.ResearchType.Equals(Constants.RESEARCH_TYPE_DESCENDANTS)) {
                var totalCount = persons.Count;
                for (var i = totalCount - 1; i > -1; i--) {
                    var id = persons.Keys.ElementAt(i);
                    if (!persons[id].Populated && !persons[id].Living) {
                        PopulatePerson(persons[id].Id, ref persons, ref session, researchDO.ResearchType);
                        logger.Info("Processed: " + person.Fullname);
                        persons[id].Descendants = true;
                    } else if (!persons[id].Living) {
                        persons[id].Ancestors = true;
                    }
                }
                //                }
                foreach (var personDo in persons) {
                    if (personDo.Value.HasParents) {
                        foreach (var parentDo in personDo.Value.Parents) {
                            //                            if (persons[parentDo.Value.Id].AscendancyNumber > 0) {
                            //                                persons[personDo.Value.Id].AscendancyNumber = persons[parentDo.Value.Id].AscendancyNumber;
                            //                                break;
                            //                            }
                        }
                    }
                }

                //  Assign assendancy to each person
                if ((persons != null) && (persons.Count > 0)) {
                    var currentPerson = persons[researchDO.PersonId];
                    //                    currentPerson.AscendancyNumber = 1;
                    if (currentPerson.HasParents) {
                        foreach (var parentDo in currentPerson.Parents) {
                            persons[parentDo.Value.Id].AscendancyNumber = currentPerson.AscendancyNumber + 1;
                        }
                    }
                    foreach (var personDo in persons) {
                        //                        if (personDo.Value.AscendancyNumber > 0) {
                        //                            if (currentPerson.HasParents) {
                        //                                foreach (var parentDo in personDo.Value.Parents) {
                        //                                    persons[parentDo.Value.Id].AscendancyNumber = personDo.Value.AscendancyNumber + 1;
                        //                                    persons[parentDo.Value.Id].DirectLine = true;
                        //                                }
                        //                            }
                        //                        }
                    }
                    foreach (var personDo in persons) {
                        //                        if (personDo.Value.AscendancyNumber == 0) {
                        //                            if (personDo.Value.HasParents) {
                        //                                foreach (var parentDo in personDo.Value.Parents) {
                        //                                    if (persons[parentDo.Value.Id].AscendancyNumber > 0) {
                        //                                        persons[personDo.Value.Id].AscendancyNumber = persons[parentDo.Value.Id].AscendancyNumber - 1;
                        //                                        // could this be a cousin
                        //                                        break;
                        //                                    }
                        //                                }
                        //                            }
                        //                        }
                    }
                }
            }
        }

        public void RetrieveDescendantsData(ref ResearchDO researchDO, ref SessionDO session, ref Dictionary<string, PersonDO> persons) {
            if (!String.IsNullOrEmpty(researchDO.PersonId)) {
                if (researchDO.ReportId > 0) {
                    persons = GetPersonsFromReport(researchDO.ReportId);
                } else {
                    persons = GetDescendants(ref researchDO, ref session, true);
                }
                if ((persons != null) && (persons.Count > 0)) {
                    researchDO.RetrievedRecords = persons.Count;
                }
            }
        }

        public void RetrieveAncestorsData(ref ResearchDO researchDO, ref SessionDO session, ref Dictionary<string, PersonDO> persons) {
            if (!String.IsNullOrEmpty(researchDO.PersonId)) {
                if (researchDO.ReportId > 0) {
                    persons = GetPersonsFromReport(researchDO.ReportId);
                } else {
                    persons = GetAncestors(ref researchDO, ref session);
                }
                if ((persons != null) && (persons.Count > 0)) {
                    researchDO.RetrievedRecords = persons.Count;
                }
            }
        }

        private int storeData(ref ResearchDO researchDO, ref SessionDO session, ref Dictionary<string, PersonDO> persons) {
            if (persons.Count > 0) {
                try {
                    var report = new ReportDO();
                    report.ReportType = "Research";
                    report.ReportBy = session.Username;
                    report.ReportDate = Dates.TodaysDateTime();
                    report.ReportDescription = (!string.IsNullOrEmpty(researchDO.ReportTitle) ? researchDO.ReportTitle + ". " : "") + "Name: " + researchDO.PersonId + " - " + researchDO.PersonName + ", Date:  " + report.ReportDate.ToString(Constants.DATETIME_FORMAT_HM) + ", Research Type: " + researchDO.ResearchType + ",  Generations: " + researchDO.Generation + ", Records: " + persons.Count;
                    report.ReportFile = FilePathHelper.GetTempPath() + "/" + researchDO.CurrentPersonId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_Persons.bin";
                    PersonServices.Instance.CreateReport(report);
                    researchDO.ReportId = report.ReportId;
                    if (File.Exists(report.ReportFile)) {
                        File.Delete(report.ReportFile);
                    }

                    using (var file = File.Create(report.ReportFile)) {
                        var personsDO = new PersonsDO();
                        personsDO.Persons = persons;
                        Serializer.Serialize(file, personsDO);
                    }
                } catch (Exception e) {
                    session.Error = true;
                    session.ErrorMessage = e.Message;
                    logger.Error("storeData: " + e.Message, e);

                    throw;
                }
            }
            return persons.Count;
        }

        public PersonDO getParents(string id, ref Dictionary<string, PersonDO> persons, ref SessionDO session) {
            var personDo = persons[id];

            var parentRelationship = personDAO.GetParents(id, ref session);
            if (!session.Error && parentRelationship != null) {
                if (parentRelationship.Persons != null) {
                    foreach (var personGx in parentRelationship.Persons) {
                        if (!id.Equals(personGx.Id)) {
                            AddParent(personGx, persons[id].Parents, 0);
                            AddPerson(personGx, persons, 0);
                            persons[id].DirectLine = true;
                        }
                    }
                }
                if (parentRelationship.Relationships != null) {
                    foreach (var relationship in parentRelationship.Relationships) {
                        if (!persons[id].HasSpouse && relationship.Type.Equals("http://gedcomx.org/Couple")) {
                            var isMarried = false;
                            Fact marriageFact = null;
                            foreach (var fact in relationship.Facts) {
                                if ((fact.Type != null) && fact.Type.Equals("http://gedcomx.org/Marriage")) {
                                    marriageFact = fact;
                                    break;
                                }
                            }
                            if (persons[id].Spouses.Count > 0) {
                                if (marriageFact != null) {
                                    if (relationship.Person1.ResourceId.Equals(relationship.Id)) {
                                        if (persons[id].Spouses.ContainsKey(relationship.Person2.ResourceId)) {
                                            persons[id].Spouses[relationship.Person2.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                            persons[id].Spouses[relationship.Person2.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                            persons[id].Spouse = persons[id].Spouses[relationship.Person2.ResourceId];
                                            persons[id].DirectLine = true;
                                        }
                                    } else {
                                        if (persons[id].Spouses.ContainsKey(relationship.Person1.ResourceId)) {
                                            persons[id].Spouses[relationship.Person1.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                            persons[id].Spouses[relationship.Person1.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                            persons[id].Spouse = persons[id].Spouses[relationship.Person1.ResourceId];
                                            persons[id].DirectLine = true;
                                        }
                                    }
                                } else {
                                    if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                        persons[relationship.Person1.ResourceId].Spouse = persons[relationship.Person2.ResourceId];
                                        persons[relationship.Person2.ResourceId].Spouse = persons[relationship.Person1.ResourceId];
                                        persons[relationship.Person1.ResourceId].DirectLine = true;
                                        persons[relationship.Person2.ResourceId].DirectLine = true;
                                    }
                                }
                            }
                        } else if (relationship.Type.Equals("http://gedcomx.org/ParentChild")) {
                            if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                AddChildren(persons[relationship.Person2.ResourceId], persons[relationship.Person1.ResourceId].Children, 0);
                                if (persons[relationship.Person1.ResourceId].IsMale) {
                                    persons[relationship.Person2.ResourceId].Father = persons[relationship.Person1.ResourceId];
                                } else {
                                    persons[relationship.Person2.ResourceId].Mother = persons[relationship.Person1.ResourceId];
                                }
                            }
                        }
                    }
                }
            }
            return personDo;
        }

        public void PopulatePerson(string id, ref Dictionary<string, PersonDO> persons, ref SessionDO session, string include) {
            var person = persons[id];

            if (persons[id] != null) {
                var spouseRelationship = personDAO.GetSpouses(id, ref session);
                if (!session.Error && spouseRelationship != null) {
                    if (spouseRelationship.Persons != null) {
                        foreach (var personGx in spouseRelationship.Persons) {
                            if (!id.Equals(personGx.Id)) {
                                AddPerson(personGx, persons, 0);
                                persons[personGx.Id].DirectLine = true;
                                AddSpouse(persons[personGx.Id], persons[id].Spouses, 0);
                                AddSpouse(persons[id], persons[personGx.Id].Spouses, 0);
                            }
                        }
                        if (spouseRelationship.Relationships.Count > 1) {
                            var test = "";
                        }
                    }
                    if (spouseRelationship.Relationships != null) {
                        foreach (var relationship in spouseRelationship.Relationships) {
                            if ((relationship.Type != null) && relationship.Type.Equals("http://gedcomx.org/Couple")) {
                                var isMarried = false;
                                Fact marriageFact = null;
                                foreach (var fact in relationship.Facts) {
                                    if ((fact.Type != null) && fact.Type.Equals("http://gedcomx.org/Marriage")) {
                                        marriageFact = fact;
                                        break;
                                    }
                                }
                                if (persons[id].Spouses.Count > 0) {
                                    if (persons[id].Spouses.Count > 1) {
                                        var test = "";
                                    }

                                    if (marriageFact != null) {
                                        if (relationship.Person1.ResourceId.Equals(id)) {
                                            if (persons[id].Spouses.ContainsKey(relationship.Person2.ResourceId)) {
                                                persons[id].Spouses[relationship.Person2.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                                persons[id].Spouses[relationship.Person2.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                                if (persons[id].Spouse.IsEmpty) {
                                                    persons[id].Spouse = persons[id].Spouses[relationship.Person2.ResourceId];
                                                }
                                            }
                                        } else {
                                            if (persons[id].Spouses.ContainsKey(relationship.Person1.ResourceId)) {
                                                persons[id].Spouses[relationship.Person1.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                                persons[id].Spouses[relationship.Person1.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                                if (persons[id].Spouse.IsEmpty) {
                                                    persons[id].Spouse = persons[id].Spouses[relationship.Person1.ResourceId];
                                                }
                                            }
                                        }
                                    } else {
                                        if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                            if (persons[relationship.Person1.ResourceId].Spouse.IsEmpty) {
                                                persons[relationship.Person1.ResourceId].Spouse = persons[relationship.Person2.ResourceId];
                                            }
                                            if (persons[relationship.Person2.ResourceId].Spouse.IsEmpty) {
                                                persons[relationship.Person2.ResourceId].Spouse = persons[relationship.Person1.ResourceId];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var childrenRelationship = personDAO.GetChildren(id, ref session);
                if (!session.Error && childrenRelationship != null) {
                    if (childrenRelationship.Persons != null) {
                        foreach (var personGx in childrenRelationship.Persons) {
                            AddPerson(personGx, persons, 0);
                        }
                    }
                    if (childrenRelationship.Relationships != null) {
                        foreach (var relationship in childrenRelationship.Relationships) {
                            if ((relationship.Type != null) && relationship.Type.Equals("http://gedcomx.org/Couple")) {
                                var isMarried = false;
                                Fact marriageFact = null;
                                foreach (var fact in relationship.Facts) {
                                    if ((fact.Type != null) && fact.Type.Equals("http://gedcomx.org/Marriage")) {
                                        marriageFact = fact;
                                        break;
                                    }
                                }
                                if (persons[id].Spouses.Count > 0) {
                                    if (marriageFact != null) {
                                        if (relationship.Person1.ResourceId.Equals(relationship.Id)) {
                                            if (persons[id].Spouses.ContainsKey(relationship.Person2.ResourceId)) {
                                                persons[id].Spouses[relationship.Person2.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                                persons[id].Spouses[relationship.Person2.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                                if (persons[id].Spouse.IsEmpty) {
                                                    persons[id].Spouse = persons[id].Spouses[relationship.Person2.ResourceId];
                                                }
                                            }
                                        } else {
                                            if (persons[id].Spouses.ContainsKey(relationship.Person1.ResourceId)) {
                                                persons[id].Spouses[relationship.Person1.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                                persons[id].Spouses[relationship.Person1.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                                if (persons[id].Spouse.IsEmpty) {
                                                    persons[id].Spouse = persons[id].Spouses[relationship.Person1.ResourceId];
                                                }
                                            }
                                        }
                                    } else {
                                        if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                            if (persons[relationship.Person1.ResourceId].Spouse.IsEmpty) {
                                                persons[relationship.Person1.ResourceId].Spouse = persons[relationship.Person2.ResourceId];
                                            }
                                            if (persons[relationship.Person2.ResourceId].Spouse.IsEmpty) {
                                                persons[relationship.Person2.ResourceId].Spouse = persons[relationship.Person1.ResourceId];
                                            }
                                        }
                                    }
                                }
                            } else if ((relationship.Type != null) && relationship.Type.Equals("http://gedcomx.org/ParentChild")) {
                                if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                    if (persons[relationship.Person1.ResourceId].IsMale) {
                                        persons[relationship.Person2.ResourceId].Father = persons[relationship.Person1.ResourceId];
                                    } else {
                                        persons[relationship.Person2.ResourceId].Mother = persons[relationship.Person1.ResourceId];
                                    }
                                    AddChildren(persons[relationship.Person2.ResourceId], persons[relationship.Person1.ResourceId].Children, 0);
                                }
                            }
                        }
                    }
                }

                var parentRelationship = personDAO.GetParents(id, ref session);
                if (!session.Error && parentRelationship != null) {
                    if (parentRelationship.Persons != null) {
                        foreach (var personGx in parentRelationship.Persons) {
                            if (!id.Equals(personGx.Id)) {
                                AddParent(personGx, persons[id].Parents, 0);
                                AddPerson(personGx, persons, 0);
                                persons[id].DirectLine = true;
                            }
                        }
                    }
                    if (parentRelationship.Relationships != null) {
                        foreach (var relationship in parentRelationship.Relationships) {
                            if (!persons[id].HasSpouse && (relationship.Type != null) && relationship.Type.Equals("http://gedcomx.org/Couple")) {
                                var isMarried = false;
                                Fact marriageFact = null;
                                foreach (var fact in relationship.Facts) {
                                    if ((fact.Type != null) && fact.Type.Equals("http://gedcomx.org/Marriage")) {
                                        marriageFact = fact;
                                        break;
                                    }
                                }
                                if (persons[id].Spouses.Count > 0) {
                                    if (marriageFact != null) {
                                        if (relationship.Person1.ResourceId.Equals(relationship.Id)) {
                                            if (persons[id].Spouses.ContainsKey(relationship.Person2.ResourceId)) {
                                                persons[id].Spouses[relationship.Person2.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                                persons[id].Spouses[relationship.Person2.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                                persons[id].Spouse = persons[id].Spouses[relationship.Person2.ResourceId];
                                                persons[id].DirectLine = true;
                                            }
                                        } else {
                                            if (persons[id].Spouses.ContainsKey(relationship.Person1.ResourceId)) {
                                                persons[id].Spouses[relationship.Person1.ResourceId].MarriageDate = personDAO.GetMarriageDate(marriageFact);
                                                persons[id].Spouses[relationship.Person1.ResourceId].MarriagePlace = personDAO.GetMarriagePlace(marriageFact);
                                                persons[id].Spouse = persons[id].Spouses[relationship.Person1.ResourceId];
                                                persons[id].DirectLine = true;
                                            }
                                        }
                                    } else {
                                        if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                            persons[relationship.Person1.ResourceId].Spouse = persons[relationship.Person2.ResourceId];
                                            persons[relationship.Person2.ResourceId].Spouse = persons[relationship.Person1.ResourceId];
                                            persons[relationship.Person1.ResourceId].DirectLine = true;
                                            persons[relationship.Person2.ResourceId].DirectLine = true;
                                        }
                                    }
                                }
                            } else if ((relationship.Type != null) && relationship.Type.Equals("http://gedcomx.org/ParentChild")) {
                                if (persons.ContainsKey(relationship.Person1.ResourceId) && persons.ContainsKey(relationship.Person2.ResourceId)) {
                                    AddChildren(persons[relationship.Person2.ResourceId], persons[relationship.Person1.ResourceId].Children, 0);
                                    if (persons[relationship.Person1.ResourceId].IsMale) {
                                        persons[relationship.Person2.ResourceId].Father = persons[relationship.Person1.ResourceId];
                                    } else {
                                        persons[relationship.Person2.ResourceId].Mother = persons[relationship.Person1.ResourceId];
                                    }
                                }
                            }
                        }
                    }
                }
                persons[id].Populated = true;
                if (persons[id].IsFemale) {
                    if (!persons[id].Father.IsEmpty) {
                        if (!persons[id].Lastname.Equals(persons[id].Father.Lastname)) {
                            persons[id].UsingMaidenName = false;
                        }
                    }
                }
                logger.Info("Processed: " + persons[id].Fullname);
            }
        }

        private void AddChildren(Person personGx, Dictionary<string, PersonDO> children, int asendancyNumber) {
            if (!children.ContainsKey(personGx.Id)) {
                var personDo = personDAO.GetPerson(personGx);
                if (personDo != null) {
                    //                    if (personDo.AscendancyNumber == 0) {
                    //                        personDo.AscendancyNumber = asendancyNumber - 1;
                    //                    }
                    children.Add(personGx.Id, personDo);
                }
            }
        }

        private void AddChildren(PersonDO personDo, Dictionary<string, PersonDO> children, int asendancyNumber) {
            if (!children.ContainsKey(personDo.Id)) {
                //                if (personDo.AscendancyNumber == 0) {
                //                    personDo.AscendancyNumber = asendancyNumber - 1;
                //                }
                children.Add(personDo.Id, personDo);
            }
        }

        private void AddParent(Person personGx, Dictionary<string, PersonDO> parents, int asendancyNumber) {
            if (!parents.ContainsKey(personGx.Id)) {
                var personDo = personDAO.GetPerson(personGx);
                if (personDo != null) {
                    //                    if (personDo.AscendancyNumber == 0) {
                    //                        personDo.AscendancyNumber = asendancyNumber + 1;
                    //                    }
                    parents.Add(personGx.Id, personDo);
                }
            }
        }

        private void AddParent(PersonDO personDo, Dictionary<string, PersonDO> parents, int asendancyNumber) {
            if (!parents.ContainsKey(personDo.Id)) {
                //                if (personDo.AscendancyNumber == 0) {
                //                    personDo.AscendancyNumber = asendancyNumber + 1;
                //                }
                parents.Add(personDo.Id, personDo);
            }
        }

        private void AddSpouse(Person personGx, Dictionary<string, PersonDO> spouses, int asendancyNumber) {
            try {
                if (!spouses.ContainsKey(personGx.Id)) {
                    var personDo = personDAO.GetPerson(personGx);
                    if (personDo != null) {
                        //                        if (personDo.AscendancyNumber == 0) {
                        //                            personDo.AscendancyNumber = asendancyNumber;
                        //                        }
                        spouses.Add(personGx.Id, personDo);
                    }
                }
            } catch (Exception e) {
                logger.Info("personGx.Id = " + personGx.Id);
                logger.Error("Error adding spouse: " + e.Message, e);
                throw;
            }
        }

        private void AddSpouse(PersonDO personDo, Dictionary<string, PersonDO> spouses, int asendancyNumber) {
            if (!spouses.ContainsKey(personDo.Id)) {
                //                if (personDo.AscendancyNumber == 0) {
                //                    personDo.AscendancyNumber = asendancyNumber;
                //                }
                spouses.Add(personDo.Id, personDo);
            }
        }

        private void AddPerson(Person personGx, Dictionary<string, PersonDO> persons, int asendancyNumber) {
            if (!persons.ContainsKey(personGx.Id)) {
                var personDo = personDAO.GetPerson(personGx);
                if (personDo != null) {
                    //                    if (personDo.AscendancyNumber == 0) {
                    //                        personDo.AscendancyNumber = asendancyNumber;
                    //                    }
                    persons.Add(personGx.Id, personDo);
                }
            }
        }

        private void AddPerson(PersonDO personDo, Dictionary<string, PersonDO> persons, int asendancyNumber) {
            if (!persons.ContainsKey(personDo.Id)) {
                //                if (personDo.AscendancyNumber == 0) {
                //                    personDo.AscendancyNumber = asendancyNumber;
                //                }
                persons.Add(personDo.Id, personDo);
            }
        }

        private bool MeetsCriteria(FindCluesInputDO findCluesInputDo, PersonDO personDo, int validation) {
            var meetsCriteria = false;
            if ((findCluesInputDo.SearchCriteria == 0) || (findCluesInputDo.SearchCriteria == validation)) {
                meetsCriteria = true;
            }
            return meetsCriteria;
        }

        public void validate(ref int id, string personId, Dictionary<string, PersonDO> persons, FindCluesInputDO findCluesInputDo, ref List<AnalyzeListItemDO> analyzeListItems) {
            AnalyzeListItemDO analyzeListItemDO = new AnalyzeListItemDO();
            FindListItemDO findListItemDO = (FindListItemDO) analyzeListItemDO;

            var person = persons[personId];
            personDAO.PopulateFindListItem(person, ref findListItemDO);
            //            analyzeListItemDO.Name = person.Id + "~" + person.Fullname;
            if (person.Id.Equals("KWCN-2VW")) {
                string test = "";
            }
            var criteriaId = 2;
            try {
                if (!person.Living) {
                    if (MeetsCriteria(findCluesInputDo, person, 2) && (person.DeathYear == 0) && (person.BirthYear > 1849) && (person.BirthYear < 1941)) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.CriteriaId = 2;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        analyzeListItemDO.clue = "Person's death date is \"Deceased\" and was born between 1850 and 1940";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if ((MeetsCriteria(findCluesInputDo, person, 2) && ((person.BirthYear > 1849) && (person.BirthYear < 1941)) && (person.IsFemale) && (person.DeathYear < 1) && (!person.HasSpouse))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.CriteriaId = 2;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        analyzeListItemDO.clue = "Female child with no spouse and no death date, lived between (between 1850 and 1940)";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if (MeetsCriteria(findCluesInputDo, person, 4) && ((person.HasSpouse) && (!person.HasChildrenLink))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.CriteriaId = 4;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        criteriaId = 4;
                        analyzeListItemDO.clue = "Person has a spouse" + (person.YearsLived > 0 ? ", lived " + person.YearsLived + " years" : "") + ", and no children.";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if (MeetsCriteria(findCluesInputDo, person, 5) && ((person.HasSpouse) && (person.NumberOfChildren == 1) && ((person.YearsLived > 0) && person.YearsLived > findCluesInputDo.AgeLimit))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.CriteriaId = 5;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        criteriaId = 5;
                        analyzeListItemDO.clue = "Person has a spouse" + (person.YearsLived > 0 ? ", lived " + person.YearsLived + " years" : "") + (((person.YearsLived > 0) && person.YearsLived > findCluesInputDo.AgeLimit) ? ", lived longer than " + findCluesInputDo.AgeLimit + " years" : "") + ", and only one child.";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if (MeetsCriteria(findCluesInputDo, person, 6) && ((!person.HasSpouse) && (!person.HasChildrenLink) && ((person.YearsLived > 0) && person.YearsLived > findCluesInputDo.AgeLimit))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.CriteriaId = 6;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        criteriaId = 6;
                        analyzeListItemDO.clue = "Person has no spouse " + (person.YearsLived > 0 ? ", lived " + person.YearsLived + " years" : "") + (((person.YearsLived > 0) && person.YearsLived > findCluesInputDo.AgeLimit) ? ", lived longer than " + findCluesInputDo.AgeLimit + " years" : "") + ", and no children.";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if (MeetsCriteria(findCluesInputDo, person, 7) && ((!person.HasSpouse) && (person.NumberOfChildren == 1))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.CriteriaId = 7;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        criteriaId = 7;
                        analyzeListItemDO.clue = "Person has no spouse " + (person.YearsLived > 0 ? "lived " + person.YearsLived + " years " : "") + ", and only one child.";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if (MeetsCriteria(findCluesInputDo, person, 10) && (person.BirthYear > 1000) && (!person.Mother.IsEmpty && ((person.Mother.DeathYear > 1000) && (person.BirthYear > person.Mother.DeathYear)))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.CriteriaId = 10;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        criteriaId = 10;
                        analyzeListItemDO.clue = "Person's birth year " + person.BirthYear + " is after mother's death year " + person.Mother.DeathYear + ".";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if (MeetsCriteria(findCluesInputDo, person, 11) && ((person.DeathYear > 1000) && (person.MarriageYear > 1000) && (person.DeathYear < person.MarriageYear))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.CriteriaId = 11;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        criteriaId = 11;
                        analyzeListItemDO.clue = "Person's death year " + person.DeathYear + " is earlier than the marriage year " + person.MarriageYear + ".";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }
                    if (MeetsCriteria(findCluesInputDo, person, 12) && (person.IsFemale && ((person.NumberOfChildren > 0) && (person.LastChild.BirthYear > 1000) && (person.YearsLived > 39) && ((person.LastChild.BirthYear + 35) < (person.BirthYear + 40))))) {
                        //                    analyzeListItemDO.Id = id;
                        //                    analyzeListItemDO.PersonId = person.Id;
                        //                    analyzeListItemDO.FullName = person.Fullname;
                        //                    analyzeListItemDO.CriteriaId = 12;
                        //                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                    analyzeListItemDO.Male = person.IsMale;
                        criteriaId = 12;
                        analyzeListItemDO.clue = "Last child was born 4 or more years before turning 40.";
                        analyzeListItemDO.helpers = criteriaId;
                        analyzeListItems.Add(analyzeListItemDO);
                    }

                    //        private int _LastChild4YearsMother40;
                    //        private int _MarriedToEarly;

                    if (person.IsFemale && (person.Children.Count > 1)) {
                        //                    var children = new Dictionary<string, PersonDO>(person.Children);
                        //                    var sortedChildren = new Dictionary<string, PersonDO>();
                        //                    foreach (var child in person.Children) {
                        //                        PersonDO youngest = getYoungest(ref children);
                        //                        sortedChildren.Add(youngest.Id, youngest);
                        //                    }
                        //                    persons[person.Id].Children = sortedChildren;
                        //                    List<KeyValuePair<string, PersonDO>> childrenList = sortedChildren.ToList();
                        var validatedGap = false;
                        for (var i = 0; i < person.SortedListOfChildren().Count; i++) {
                            if (i > 0) {
                                var child = person.SortedListOfChildren()[i - 1];
                                var nextChild = person.SortedListOfChildren()[i];
                                if (((child.BirthYear > 1000) && (nextChild.BirthYear > 1000)) && (Math.Abs(child.BirthYear - nextChild.BirthYear) >= findCluesInputDo.GapInChildren)) {
                                    var gap = Math.Abs(child.BirthYear - nextChild.BirthYear);
                                    if (gap < 30) {
                                        var problem = gap + " year gap between " + child.Fullname + " and " + nextChild.Fullname;
                                        var problem1 = gap + " year gap between " + nextChild.Fullname + " and " + child.Fullname;
                                        if (MeetsCriteria(findCluesInputDo, person, 3) && (!isDuplicateValidation(problem, analyzeListItems) && !isDuplicateValidation(problem1, analyzeListItems))) {
                                            //                                        analyzeListItemDO.Id = id;
                                            //                                        analyzeListItemDO.PersonId = person.Id;
                                            //                                        analyzeListItemDO.CriteriaId = 3;
                                            //                                        analyzeListItemDO.FullName = person.Fullname;
                                            //                                        analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                                            //                                        analyzeListItemDO.DirectLine = person.DirectLine;
                                            //                                        analyzeListItemDO.Male = person.IsMale;
                                            criteriaId = 3;
                                            analyzeListItemDO.clue = problem + "";
                                            analyzeListItemDO.helpers = criteriaId;
                                            analyzeListItems.Add(analyzeListItemDO);
                                            id++;
                                            validatedGap = true;
                                        }
                                    }
                                    break;
                                }
                            }
                            if (validatedGap) {
                                id--;
                            }
                        }

                        //                    foreach (var child in person.Children) {
                        //                        foreach (var nextChild in person.Children) {
                        //                            if (MeetsCriteria(FindCluesInputDO, 8) &&
                        //                                (!child.Value.Id.Equals(nextChild.Value.Id) && (child.Value.BirthYear == nextChild.Value.BirthYear))) {
                        //                                string problem = child.Value.Fullname + " and " + nextChild.Value.Fullname + " are born the same year " +
                        //                                                 child.Value.BirthYear + ", are they twins?, or could this be a duplicate?";
                        //                                string problem1 = nextChild.Value.Fullname + " and " + child.Value.Fullname + " are born the same year " +
                        //                                                  child.Value.BirthYear + ", are they twins?, or could this be a duplicate?";
                        //                                if (!isDuplicateValidation(problem, analyzeListItems) &&
                        //                                    !isDuplicateValidation(problem1, analyzeListItems)) {
                        //                                    validation = new ValidationDO();
                        //                                    analyzeListItemDO.Id = person.Id;
                        //                                    analyzeListItemDO.FullName = person.Fullname;
                        //                                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                                    analyzeListItemDO.Male = person.IsMale;
                        //                                    analyzeListItemDO.Problem = problem;
                        //                                    analyzeListItemDO.FamilySearchDetailsLink = person.PersonUrl;
                        //                                    analyzeListItems.Add(analyzeListItemDO);
                        //                                    ResearchDO.Summary.SiblingsBornSameYear++;
                        //                                    logger.Error(analyzeListItemDO.Id + " - " + analyzeListItemDO.Problem);
                        //                                }
                        //                                break;
                        //                            }
                        //                        }
                        //                    }

                        //                    foreach (var child in person.Children) {
                        //                        foreach (var nextChild in person.Children) {
                        //                            //  This could also apply to having the same first name
                        //                            if (MeetsCriteria(FindCluesInputDO, 9) &&
                        //                                (!child.Value.Id.Equals(nextChild.Value.Id) && (child.Value.Firstname.Equals(nextChild.Value.Firstname)))) {
                        //                                string problem = child.Value.Fullname + " and " + nextChild.Value.Fullname +
                        //                                                 " have the same first name, could this be a duplicate?";
                        //                                string problem1 = nextChild.Value.Fullname + " and " + child.Value.Fullname +
                        //                                                  " have the same first name, could this be a duplicate?";
                        //                                if (!isDuplicateValidation(problem, analyzeListItems) &&
                        //                                    !isDuplicateValidation(problem1, analyzeListItems)) {
                        //                                    validation = new ValidationDO();
                        //                                    analyzeListItemDO.Id = person.Id;
                        //                                    analyzeListItemDO.FullName = person.Fullname;
                        //                                    analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                                    analyzeListItemDO.DirectLine = person.DirectLine;
                        //                                    analyzeListItemDO.Male = person.IsMale;
                        //                                    analyzeListItemDO.Problem = problem;
                        //                                    analyzeListItemDO.FamilySearchDetailsLink = person.PersonUrl;
                        //                                    analyzeListItems.Add(analyzeListItemDO);
                        //                                    ResearchDO.Summary.SiblingsWithSameName++;
                        //                                    logger.Error(analyzeListItemDO.Id + " - " + analyzeListItemDO.Problem);
                        //                                }
                        //                                break;
                        //                            }
                        //                        }
                        //                    }

                        //foreach (var child in person.Children) {
                        //                        if (!child.Value.Father.IsEmpty && !child.Value.Lastname.Equals(child.Value.Father.Lastname) &&
                        //                            (!child.Value.Mother.IsEmpty && !child.Value.Lastname.Equals(child.Value.Mother.Lastname))) {
                        //                            string problem = "The last name for " + child.Value.Fullname + " is different than both parents last names (Father: " +
                        //                                             child.Value.Father.Fullname + ", Mother: " + child.Value.Mother.Fullname + ")";
                        //                            if (MeetsCriteria(FindCluesInputDO, 10) && (!isDuplicateValidation(problem, analyzeListItems))) {
                        //                                validation = new ValidationDO();
                        //                                analyzeListItemDO.Id = id;
                        //                                analyzeListItemDO.PersonId = person.Id;
                        //                                analyzeListItemDO.FullName = person.Fullname;
                        //                                analyzeListItemDO.CriteriaId = 10;
                        //                                analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                                analyzeListItemDO.DirectLine = person.DirectLine;
                        //                                analyzeListItemDO.Male = person.IsMale;
                        //                                analyzeListItemDO.Problem = problem;
                        //                                analyzeListItems.Add(analyzeListItemDO);
                        //                                ResearchDO.Summary.LastNameDifferentThanParents++;
                        //                                logger.Error(analyzeListItemDO.Id + " - " + analyzeListItemDO.Problem);
                        //                            }
                        //                        }
                        //                        if ((child.Value.BirthYear > 0) && (child.Value.Mother != null) && (child.Value.BirthYear > child.Value.Mother.DeathYear)) {
                        //                            string problem = "Birth year " + child.Value.BirthYear + " is later than Mother's death year." + child.Value.Mother.DeathYear;
                        //                            if (MeetsCriteria(FindCluesInputDO, person, 11) && (!isDuplicateValidation(problem, analyzeListItems))) {
                        //                                validation = new ValidationDO();
                        //                                analyzeListItemDO.Id = id;
                        //                                analyzeListItemDO.PersonId = person.Id;
                        //                                analyzeListItemDO.FullName = person.Fullname;
                        //                                analyzeListItemDO.CriteriaId = 11;
                        //                                analyzeListItemDO.AscendancyNumber = person.AscendancyNumber;
                        //                                analyzeListItemDO.DirectLine = person.DirectLine;
                        //                                analyzeListItemDO.Male = person.IsMale;
                        //                                analyzeListItemDO.Problem = problem;
                        //                                analyzeListItems.Add(analyzeListItemDO);
                        //                                ResearchDO.Summary.LastNameDifferentThanParents++;
                        //                                logger.Error(analyzeListItemDO.Id + " - " + analyzeListItemDO.Problem);
                        //                            }
                        //                        }
                        //}
                    }
                }
            } catch (Exception e) {
                throw e;
            }

            //                      if ((person.BirthYear > 1870) && (person.BirthYear < 1880)) {

            //            //  6. Search if no spouse and lived longer than 20 years
            //            if (person.Spouse.IsEmpty && ((person.DeathYear - person.BirthYear) > 20)) {
            //                logger.Error("no spouse and lived longer than 20 years - " + person.Fullname + "    " + person.BirthYear + "    " +
            //                    person.LinkToFamilyTreeDetail);
            //            }
            //
            //            //  2. If not married, but a couple
            //            if ((!person.IsMarried) && (!person.Spouse.IsEmpty)) {
            //                logger.Error("not married, but a couple - " + person.Fullname + "    " + person.BirthYear + "    " + person.LinkToFamilyTreeDetail);
            //            } else if ((!person.IsMarried) && (person.Spouse.IsEmpty)) {
            //                logger.Error("not married, with no spouse, and lived " + (person.DeathYear - person.BirthYear) + " years - " + person.Fullname +
            //                    "    " + person.BirthYear + "    " + person.LinkToFamilyTreeDetail);
            //            }
        }

        private bool isDuplicateValidation(string clue, List<AnalyzeListItemDO> analyzeListItems) {
            var isDuplicate = false;

            foreach (var analyzeListItemDO in analyzeListItems) {
                if (analyzeListItemDO.clue.Equals(clue)) {
                    isDuplicate = true;
                }
            }

            return isDuplicate;
        }

        public FamilySearchPlatform GetChildParentRelationships(String personId, ref SessionDO session) {
            //     FamilySearchPlatform familySearchPlatform = personDAO.GetChildParentRelationships(personId, ref session);
            //FamilyDO familyDO = new FamilyBO().populateFamilyWithSpouses(gedcomx);

            return null; //familySearchPlatform;
        }
    }
}