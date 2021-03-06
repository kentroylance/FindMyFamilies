///////////////////////////////////////////////////////////////////////////
// Description: AncestryServices Class
// generated by Generator v1.0.1635.8359 Final on: Wednesday, June 23, 2004, 5:41:55 AM
///////////////////////////////////////////////////////////////////////////

using System;
using FindMyFamilies.Data;
using FindMyFamilies.DataAccess;
using Gx;
using Gx.Conclusion;

namespace FindMyFamilies.BusinessObject {

    /// <summary>
    ///     Purpose: Business Object Class for Family
    /// </summary>
    public class FamilyBO {

        public FamilyDO GetFamily(String personId, ref SessionDO session) {
            var person = new PersonBO();
            Gedcomx spouses = person.GetSpouses(personId, ref session);
            FamilyDO family = populateFamilyWithSpouses(spouses);
            return family;
        }

        public FamilyDO populateFamily(GedcomxPerson persons) {
            var family = new FamilyDO();
            var personDAO = new PersonDAO();
            foreach (Person spouse in persons.Persons) {
                var person = personDAO.GetPerson(spouse);
                family.PersonDOs.Add(person);
            }
            return family;
        }

        public FamilyDO populateFamilyWithSpouses(Gedcomx spouses) {
            var family = new FamilyDO();
            var personDAO = new PersonDAO();
            foreach (Person spouse in spouses.Persons) {
                var person = personDAO.GetPerson(spouse);
                if ((family.Husband == null) && person.IsMale) {
                    family.Husband = person;
                } else if (family.Wife == null) {
                    family.Wife = person;
                }

//                foreach (var name in spouse.Names) {
//                    foreach (var nameForm in name.NameForms) {
//                        person.Fullname = nameForm.FullText;
//                        foreach (var part in nameForm.Parts) {
//                            if (part.Type.Equals("http://gedcomx.org/Given")) {
//                                person.Firstname = part.value;
//                            } else if (part.Type.Equals("http://gedcomx.org/Surname")) {
//                                person.Lastname = part.value;
//                            }
//                        }
//                    }
//                }
//                foreach (Fact fact in spouse.Facts) {
//                    if (fact.Type.Equals("http://gedcomx.org/Birth")) {
//                        if (!Strings.IsEmpty(fact.Date.Original)) {
//                            if (fact.Date.Original.Length > 4) {
//                                person.BirthDate = DateProblems.ConvertToDate(fact.Date.Original);
//                            } else if (fact.Date.Original.Length == 4) {
//                                person.BirthYear = Convert.ToInt32(fact.Date.Original);
//                            }
//                        }
//                        if (fact.Place != null) {
//                            person.BirthPlace = fact.Place.Original;
//                        }
//                    } else if (fact.Type.Equals("http://gedcomx.org/Death")) {
//                        if (!Strings.IsEmpty(fact.Date.Original)) {
//                            if (fact.Date.Original.Length > 4) {
//                                person.DeathDate = DateProblems.ConvertToDate(fact.Date.Original);
//                            } else if (fact.Date.Original.Length == 4) {
//                                person.DeathYear = Convert.ToInt32(fact.Date.Original);
//                            }
//                        }
//                        if (fact.Place != null) {
//                            person.DeathPlace = fact.Place.Original;
//                        }
//                    }
//                }
                family.PersonDOs.Add(person);
            }

//            foreach (Relationship relationship in spouses.Relationships) {
//                foreach (Fact fact in relationship.Facts) {
//                    if (fact.Equals(FactType.Marriage)) {
//                        family.Husband.MarriageDate = DateProblems.ConvertToDate(fact.Date.Original);
//                        family.Husband.MarriagePlace = fact.Place.Original;
//                    }
//                }
////                if (relationship.Type.Equals(RelationshipType.Couple)) {
////                    
////                }
//            }
            return family;
        }

    }

}