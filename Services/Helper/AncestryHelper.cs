using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using FindMyFamiles.Services.Data;
using FindMyFamilies.Data;
using FindMyFamilies.Services;
using FindMyFamilies.Util;

namespace FindMyFamilies.Helper {
    public class AncestryHelper {
        private static Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

        private static AncestryHelper instance;
        private static readonly object syncLock = new object();
        private static ArrayList searchCriteriaList;

        private AncestryHelper() {
        }

        public static AncestryHelper Instance
        {
            get
            {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new AncestryHelper();
                    }

                    return instance;
                }
            }
        }

        public static string GetAncestryName(bool male, bool directLine, int ascendency) {
            string ancestryName = "";
            if (directLine) {
                if (male) {
                    ancestryName = "Father";
                    if (ascendency > 1) {
                        ancestryName = "Grand " + ancestryName;
                    }
                    if (ascendency > 2) {
                        for (int i = 3; i < ascendency; i++) {
                            ancestryName = "Great " + ancestryName;
                        }
                    }
                } else {
                    ancestryName = "Mother";
                    if (ascendency > 1) {
                        ancestryName = "Grand " + ancestryName;
                    }
                    if (ascendency > 2) {
                        for (int i = 3; i < ascendency; i++) {
                            ancestryName = "Great " + ancestryName;
                        }
                    }
                }
            }
            return ancestryName;
        }

        public static string GetCriteriaInfo(PersonDO person, ValidationDO validation) {
            string criteriaInfo = "";
            switch (validation.CriteriaId) {
                case 1:
                    criteriaInfo = "When there is no death date, or the death date is only “deceased”, it indicates that the person submitting the information did not know much about the person. Census records are available from 1850 to 1940 and could reveal new information, including spouses and children.";
                    break;
                case 2:
                    criteriaInfo = "Searching marriage, census or cemetery records, between 1850 and 1940, might find a spouse and/or children.";
                    break;
                case 3:
                    criteriaInfo = "When the mother is healthy, a child might be born every two years.  Checking census records every ten years might find additional children.  Sometimes the children died, and would not show up on a census record, but might be found in a cemetery record.";
                    break;
                case 4:
                    criteriaInfo = "Most married couples had children, in the 1800s.  With the name of the married couple, the census records and cemetery records can be checked, to see if children are listed.";
                    break;
                case 5:
                    criteriaInfo = "Most married mothers had a child every two years, in the 1800s.  If only one child is listed, the census and cemetery records should be checked to look for other children.";
                    //                    criteriaInfo = "The family search record shows that " + person.Fullname + " has a spouse, but only one child. " +
                    //                                   "This person lived to be # years old, and it shows that they lived with their spouse for # years. " +
                    //                                   "They were a couple long enough that most likely they had more than one child. " +
                    //                                   "What usually happens is that someone will add a new parent to a person, and so that parent will be left with showing only one child.  " +
                    //                                   "They will not continue on to include the rest of the children for that parent, so that only leaves one child. " +
                    //                                   "Even though it is possible there is only one child, in most cases, if they lived long enough, and there is one child, " +
                    //                                   "then most likely there are more missing children. <p></p>" +
                    //                                   "Next step is to find if there are any census records for <persons name> by clicking on the Family Search button below.  " +
                    //                                   "There are several other good web sites that can be researched by selecting any of the buttons below.";
                    break;
                case 6:
                    criteriaInfo = "Most people married in the 1800s.  Do a search of marriage, census and cemetery records to look for a possible spouse and children.";
                    break;
                case 7:
                    criteriaInfo = "Most people married in the 1800s.  Do a search of marriage, census and cemetery records to look for a possible spouse and children.";
                    break;
                case 8:
                    criteriaInfo = "If one child is listed, the person probably married, and might have other children.  Do a search of marriage, census and cemetery records to look for a possible spouse and more children.";
                    break;
                case 9:
                    criteriaInfo = "";
                    break;
                case 10:
                    criteriaInfo = "Check records to determine whether the child’s birth year or the mother’s death year is accurate.  Obviously, one of them is wrong.  The child might have a different mother, or belong to a different family.";
                    break;
                case 11:
                    criteriaInfo = "This is an obvious error.  If the person died before the marriage date, the marriage is wrong.  This requires further searching to determine which is correct, and if the couple was married.";
                    break;
                default:
                    criteriaInfo = "";
                    break;
            }
            return criteriaInfo;
        }

        public static string GetVideoInfo(ValidationDO validation) {
            string videoId = "";
            switch (validation.CriteriaId) {
                case 1:
                    videoId = "";
                    break;
                case 2:
                    videoId = "";
                    break;
                case 3:
                    //  Gap in children
                    videoId = "96324885";
                    break;
                case 4:
                    videoId = "";
                    break;
                case 5:
                    videoId = "";
                    break;
                case 6:
                    videoId = "";
                    break;
                case 7:
                    videoId = "";
                    break;
                case 8:
                    videoId = "";
                    break;
                case 9:
                    videoId = "";
                    break;
                case 10:
                    videoId = "";
                    break;
                case 11:
                    videoId = "";
                    break;
                default:
                    videoId = "";
                    break;
            }
            StringBuilder videoInfo = new StringBuilder();
            if (!string.IsNullOrEmpty(videoId)) {
                //                videoInfo.Append("<li>");
                videoInfo.Append("<a class=\"fancyboxvideo fitVideo\" data-width=\"800\" data-height=\"450\" caption=\"\" href=\"http://player.vimeo.com/video/" + videoId + "?title=0&amp;byline=0&amp;portrait=0\">");
                videoInfo.Append("<button type=\"button\" class=\"btn-u btn-u-sm btn-u-dark\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Video\"><i class=\"fa fa-film\"></i></button>");
                videoInfo.Append("</a>");
                //                videoInfo.Append("<li>");
            }
            return videoInfo.ToString();
        }

        public static string GetTutorialInfo(ValidationDO validation) {
            string tutorialId = "";
            switch (validation.CriteriaId) {
                case 1:
                    tutorialId = "";
                    break;
                case 2:
                    tutorialId = "";
                    break;
                case 3:
                    //  Gap in children
                    //                    tutorialId = "/Content/person.pdf"; //"http://broadcast2.lds.org/elearning/fhd/Community/en/FamilySearch/Descendancy/Easy%20Steps%20to%20Descendancy%20Research.pdf";
                    tutorialId = "/Content/Easy Steps to Descendancy Research.pdf";
                    break;
                case 4:
                    tutorialId = "";
                    break;
                case 5:
                    tutorialId = "";
                    break;
                case 6:
                    tutorialId = "";
                    break;
                case 7:
                    tutorialId = "";
                    break;
                case 8:
                    tutorialId = "";
                    break;
                case 9:
                    tutorialId = "";
                    break;
                case 10:
                    tutorialId = "";
                    break;
                case 11:
                    tutorialId = "";
                    break;
                default:
                    tutorialId = "";
                    break;
            }
            StringBuilder tutorialInfo = new StringBuilder();
            if (!string.IsNullOrEmpty(tutorialId)) {
                //                tutorialInfo.Append("<li>");
                tutorialInfo.Append("<a class=\"fancybox\" data-fancybox-type=\"iframe\" href=\"" + tutorialId + "\">");
                tutorialInfo.Append("<button type=\"button\" class=\"btn-u btn-u-sm btn-u-dark\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Tutorial\"><i class=\"fa fa-book\"></i></button>");
                tutorialInfo.Append("</a>");
                //                tutorialInfo.Append("<li>");
            }
            return tutorialInfo.ToString();
        }

        public static string GetWebInfo(ValidationDO validation) {
            string webId = "";
            switch (validation.CriteriaId) {
                case 1:
                    webId = "";
                    break;
                case 2:
                    webId = "";
                    break;
                case 3:
                    //  Gap in children
                    webId = "https://www.lds.org/callings/temple-and-family-history/family-history-consultants/easy-steps-to-descendancy?lang=eng";
                    break;
                case 4:
                    webId = "";
                    break;
                case 5:
                    webId = "";
                    break;
                case 6:
                    webId = "";
                    break;
                case 7:
                    webId = "";
                    break;
                case 8:
                    webId = "";
                    break;
                case 9:
                    webId = "";
                    break;
                case 10:
                    webId = "";
                    break;
                case 11:
                    webId = "";
                    break;
                default:
                    webId = "";
                    break;
            }
            StringBuilder webInfo = new StringBuilder();
            if (!string.IsNullOrEmpty(webId)) {
                //                webInfo.Append("<li>");
                webInfo.Append("<a class=\"fancybox\" data-fancybox-type=\"iframe\" href=\"" + webId + "\">");
                webInfo.Append("<button type=\"button\" class=\"btn-u btn-u-sm btn-u-dark\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"More Info\"><i class=\"fa fa-file-text-o\"></i></button>");
                webInfo.Append("</a>");
                //                webInfo.Append("<li>");
            }
            return webInfo.ToString();
        }

        public static string PossibleDuplicateUrl(string personId) {
            string url = "";
            if (!string.IsNullOrEmpty(personId)) {
                url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=possibleDuplicates&person=" + personId;
            }
            return url;
        }

        public static string getPersonImage(string gender) {
            if (gender.Equals("MALE")) {
                return "<i class=\"fa fa-user color-dark-blue\"></i>&nbsp;";
            } else {
                return "<i class=\"fa fa-user color-red\"></i>&nbsp;";
            }
        }

        public static string getPersonColor(string gender) {
            if (gender.Equals("MALE")) {
                return "rgb(0,0,255)";
            } else {
                return "rgb(255,0,0)";
            }
        }

        public static string PersonDropDown(PersonDO person, PersonInfoDO personInfo) {
            string url = "<div class=\"btn-group research-dropdown\"><button type=\"button\" data-id=" + person.Id + " class=\"btn btn-default dropdown-btn personAction1\"><span style=\"color: " + getPersonColor(person.Gender) + "\">" + getPersonImage(person.Gender) + person.Fullname + "</span></button><a id=" + person.Id + " class=\"personUrlsAction\" data-id=" + person.Id + " data-firstName=\"" + person.Firstname + "\" data-middleName=\"" + person.Middlename  + "\" data-lastName=\"" + person.Lastname + "\" data-fullName=\"" + person.Fullname + "\" data-fathername=\"" + person.Father.Fullname + "\" data-mothername=\"" + person.Mother.Fullname + "\" + data-spousename=\"" + person.Spouse.Fullname + "\" data-gender=" + person.Gender + " data-birthYear=" + person.BirthYear + " data-deathYear=" + person.DeathYear + " data-birthDate=\"" + person.BirthDateString + "\" data-deathDate=\"" + person.DeathDateString + "\" data-birthPlace=\"" + person.BirthPlace + "\" data-deathPlace=\"" + person.DeathPlace + "\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";

            //            string url = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default\">" +person.Fullname + "(" + person.LifeSpan + ") " + AncestryHelper.GetYearsLived(person) + "</button><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button><ul class=\"dropdown-menu\" role=\"menu\">" +
            //            "<li><a href=\"" + AncestryHelper.AncestryUrl(person, personInfo) + "\" target=\"_blank\"><span class=\"fa fmf16-Ancestry\"></span> Ancestry</a></li>" +
            //            "<li><a href=\"" + AncestryHelper.FindAGraveUrl(person, personInfo) + "\" target=\"_blank\"><span class=\"fa fmf16-FindAGrave\"></span> Find A Grave</a></li>" +
            //            "<li><a href=\"" + AncestryHelper.BillionGravesUrl(person, personInfo) + "\" target=\"_blank\"><span class=\"fa fmf16-BillionGraves\"></span> Billion Graves</a></li>" +
            //            "<li><a href=\"" + AncestryHelper.MyHeritageUrl(person, personInfo) + "\" target=\"_blank\"><span class=\"fa fmf16-FindMyPast\"></span> Find My Past</a></li>" +
            //            "<li><a href=\"" + AncestryHelper.FindMyPastUrl(person, personInfo) + "\" target=\"_blank\"><span class=\"fa fmf16-Google\"></span> Google</a></li>" +
            //            "<li><a href=\"https://puzzilla.org/descendants?id=" + person.Id + "&changeToId=" + person.Id + "&depth=6&ancestorsView=true\" target=\"_blank\"><span class=\"fa fmf16-Puzilla\"></span> Puzilla Ancestors</a></li>" +
            //            "<li><a href=\"https://puzzilla.org/descendants?id=" + person.Id + "\" target=\"_blank\"><span class=\"fa fmf16-Puzilla\"></span> Puzilla Descendants</a></li>" +
            //            "<li><a onclick=\"findPersonsStartingPoint();\" href=\"javascript:void(0);\"><span class=\"fa fmf16-FindMyFamilies\"></span> Starting Point</a></li>" + 
            //            "</ul></div>";

            //@*                @Html.Raw(Model.Person.Fullname + ",  " + Model.Person.LifeSpan + ",  " + AncestryHelper.GetYearsLived(Model.Person) + " - ")*@
            //@*                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.PersonUrl(Model.Person, Model) + "\" target=\"_blank\">Person</a>,&nbsp;")*@
            //@*                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.TreeUrl(Model.Person, Model) + "\" target=\"_blank\">Tree</a>,&nbsp;")*@
            //@*                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.FanChartUrl(Model.Person, Model) + "\" target=\"_blank\">Fan</a>,&nbsp;")*@
            //@*                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.DescendancyUrl(Model.Person, Model) + "\" target=\"_blank\">Descendancy</a>,&nbsp;")*@
            //@*                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.familySearchUrl(Model.Person, Model) + "\" target=\"_blank\">Search</a>")*@

            //                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.AncestryUrl(Model.Person, Model) + "\" target=\"_blank\">Ancestry</a>,&nbsp;")
            //                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.FindAGraveUrl(Model.Person, Model) + "\" target=\"_blank\">Find A Grave</a>,&nbsp;")
            //                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.BillionGravesUrl(Model.Person, Model) + "\" target=\"_blank\">Billion Graves</a>,&nbsp;")
            //                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.MyHeritageUrl(Model.Person, Model) + "\" target=\"_blank\">My Heritage</a>,&nbsp;")
            //                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.FindMyPastUrl(Model.Person, Model) + "\" target=\"_blank\">Find My Past</a>,&nbsp;")
            //                @Html.Raw("<a style=\"color: rgb(50,205,50)\" href=\"" + AncestryHelper.GoogleUrl(Model.Person, Model) + "\" target=\"_blank\">Google</a>")

            return url;
        }

        public static string PersonUrl(PersonDO person) {
            string url = "";
            if (person != null && !person.IsEmpty && (!string.IsNullOrEmpty(person.Id))) {
                url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=ancestor&person=" + person.Id;
            }
            return url;
        }

        public static string PersonUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty && (!string.IsNullOrEmpty(person.Id))) {
                url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=ancestor&person=" + person.Id;
            }
            return url;
        }

        public static string PersonUrl(string personId) {
            string url = "";
            if (!string.IsNullOrEmpty(personId)) {
                url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=ancestor&person=" + personId;
            }
            return url;
        }

        public static string PersonUrl(string personId, PersonInfoDO personInfo) {
            string url = "";
            if (!string.IsNullOrEmpty(personId)) {
                url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=ancestor&person=" + personId;
            }
            return url;
        }

        public static string TreeUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=tree&section=pedigree&person=" + person.Id;
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string FanChartUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=tree&section=fan&person=" + person.Id;
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string DescendancyUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.FAMILY_SEARCH_SYSTEM + "/tree/#view=tree&section=descendancy&person=" + person.Id;
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string FindAGraveUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.FIND_A_GRAVE + "&GSfn=" + person.Firstname + getMiddlename(person, Constants.FIND_A_GRAVE, personInfo) + "&GSln=" + getLastname(person, personInfo) + getBirthYear(person, Constants.FIND_A_GRAVE, personInfo) + getDeathYear(person, Constants.FIND_A_GRAVE, personInfo) + "&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n";
                    //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
                    //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
                    //                return Constants.FIND_A_GRAVE + "&GSfn=" + person.Firstname + "&GSmn=" + person.Middlename + "&GSln=" + person.Lastname + "&GSbyrel=after&GSby=" + (person.BirthYear - 5) + "&GSdyrel=after&GSdy=" + (person.DeathYear - 5) + "&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n";
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string AncestryUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.ANCESTRY + "&gsfn=" + person.Firstname + getMiddlename(person, Constants.ANCESTRY, personInfo) + "&gsln=" + getLastname(person, personInfo) + ((personInfo.YearRange == 0) ? "&msbdy_x=1" : "&msbdy_x=1&msbdp=" + personInfo.YearRange) + getBirthYear(person, Constants.ANCESTRY, personInfo) + getPlace(person, Constants.ANCESTRY, personInfo) + getDeathYear(person, Constants.ANCESTRY, personInfo) + ((personInfo.YearRange == 0) ? "&msddy_x=1" : "&msddy_x=1&msddp=" + personInfo.YearRange) + "&_83004003-n_xcl=" + ((person.IsMale) ? "f" : "m") + "&cp=0&catbucket=rstp&uidh=000gl=42&so=2";
                    //            return Constants.ANCESTRY + "&gsfn=" + person.Firstname + "+" + person.Middlename + "&gsln=" + person.Lastname + "&msbdy=" + person.BirthYear + "&msbpn__ftp=" + ancestryBirthPlace(person) + "&msddy=" + person.DeathYear + "&cpxt=0&catBucket=rstp&uidh=6k4&msbdp=2&msddp=2&_83004003-n_xcl=f&cp=0";
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string GoogleUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.GOOGLE + person.Firstname + getMiddlename(person, Constants.GOOGLE, personInfo) + "+" + getLastname(person, personInfo) + "+" + getBirthYear(person, Constants.GOOGLE, personInfo) + "+" + getDeathYear(person, Constants.GOOGLE, personInfo) + "+" + getPlace(person, Constants.GOOGLE, personInfo);
                    ;
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string BillionGravesUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.BILLION_GRAVES + "given_names=" + person.Firstname + getMiddlename(person, Constants.BILLION_GRAVES, personInfo) + "&family_names=" + getLastname(person, personInfo) + getBirthYear(person, Constants.BILLION_GRAVES, personInfo) + getDeathYear(person, Constants.BILLION_GRAVES, personInfo) + "&year_range=" + personInfo.YearRange + "&lim=0&num=10&action=search&exact=false&phonetic=false&record_type=0&country=0&state=null&county=null";
                    //                return Constants.BILLION_GRAVES + "given_names=" + person.Firstname  + " " + person.Middlename + "&family_names=" + person.Lastname + "&birth_year=" + person.BirthYear + "&death_year=&year_range=5                                                                                                                                     &lim=0&num=10&action=search&exact=false&phonetic=false&record_type=0&country=0&state=null&county=null";
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string MyHeritageUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.MY_HERITAGE + "&qname=Name+fn." + person.Firstname + getMiddlename(person, Constants.MY_HERITAGE, personInfo) + "+ln." + getLastname(person, personInfo) + getBirthYear(person, Constants.MY_HERITAGE, personInfo) + getPlace(person, Constants.MY_HERITAGE, personInfo);
                    //                return Constants.MY_HERITAGE + "&qname=Name+fn." + person.Firstname + "+" + person.Middlename + "+ln." + person.Lastname + "&qbirth=Event+et.birth+ey." +  + person.BirthYear + myheritageBirthPlace(person);
                    //    &qname=Name+fn.Newell+ln.Anderson&qbirth=Event+et.birth+ey.1923&qany%2F1event=Event+et.any+ep.Shelley%2C%2F3Bingham%2C%2F3Idaho%2C%2F3United%2F3States
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        public static string FindMyPastUrl(PersonDO person, PersonInfoDO personInfo) {
            string url = "";
            if (person != null && !person.IsEmpty) {
                try {
                    url = Constants.FIND_MY_PAST + "firstname=" + person.Firstname + getMiddlename(person, Constants.FIND_MY_PAST, personInfo) + "&lastname=" + getLastname(person, personInfo) + getBirthYear(person, Constants.FIND_MY_PAST, personInfo);
                    //                Constants.FIND_MY_PAST + "firstname=" + person.Firstname + "%20" + person.Middlename + "&lastname=" + person.Lastname + "&yearofbirth=" + person.BirthYear + "&yearofbirth_offset=5";
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return url;
        }

        private static string MiddleNameQuote(PersonDO person, PersonInfoDO personInfo) {
            string quote = "";
            if (person != null && !person.IsEmpty && !string.IsNullOrEmpty(person.Middlename) && personInfo.IncludeMiddleName) {
                quote = "%22";
            }
            return quote;
        }

        public static string familySearchUrl(PersonDO person, PersonInfoDO personInfo) {
            string familySearchUrl = "";
            if (person != null && !person.IsEmpty) {
                try {
                    familySearchUrl = Constants.FAMILY_SEARCH_SYSTEM + "/search/record/results?count=20&query=%2Bgivenname%3A" + MiddleNameQuote(person, personInfo) + person.Firstname + getMiddlename(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + MiddleNameQuote(person, personInfo) + "~%20%2Bsurname%3A" + getLastname(person, personInfo) + getPlace(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + getBirthYear(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + getDeathYear(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + "~&treeref=" + person.Id;

                    //https://sandbox.familysearch.org/search/record/results?count=20&query=%2Bgivenname%3ABertha~%20%2Bsurname%3AVevers~%20%2Bbirth_year%3A1872-1876~%20%2Bdeath_year%3A1913-1917~
                    // https://sandbox.familysearch.org/search/record/results?count=20&query=%2Bgivenname%3A%22Anna%20Eliza%22~%20%2Bsurname%3AHomer%20%2Bbirth_place%3A%22Middletown%2C%20Logan%2C%20Illinois%2C%20United%20States%22~%20%2Bbirth_year%3A1841-1845~%20%2Bdeath_year%3A1909-1913~
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }
            return familySearchUrl;
        }

        private static string getLastname(PersonDO person, PersonInfoDO personInfo) {
            string lastname = person.Lastname;

            if ((person != null) && !person.IsEmpty && person.IsFemale && personInfo.IncludeMaidenName) {
                if (!person.Father.IsEmpty && !string.IsNullOrEmpty(person.Father.Lastname)) {
                    lastname = person.Father.Lastname;
                }
                if (!person.Mother.IsEmpty && !string.IsNullOrEmpty(person.Mother.Lastname)) {
                    lastname = person.Mother.Lastname;
                }
            }

            return lastname;
        }

        private static string getMiddlename(PersonDO person, string webSite, PersonInfoDO personInfo) {
            string middlename = "";

            if (person != null && !person.IsEmpty && !string.IsNullOrEmpty(person.Middlename) && personInfo.IncludeMiddleName) {
                if (webSite.Equals(Constants.ANCESTRY)) {
                    middlename = "%20" + person.Middlename;
                } else if (webSite.Equals(Constants.FIND_A_GRAVE)) {
                    middlename = "&GSmn=" + person.Middlename;
                } else if (webSite.Equals(Constants.BILLION_GRAVES)) {
                    middlename = "+" + person.Middlename;
                } else if (webSite.Equals(Constants.MY_HERITAGE)) {
                    middlename = "%2F3" + person.Middlename;
                } else if (webSite.Equals(Constants.FIND_MY_PAST)) {
                    middlename = "%20" + person.Middlename;
                } else if (webSite.Equals(Constants.FAMILY_SEARCH_SYSTEM)) {
                    middlename = "%20" + person.Middlename;
                } else if (webSite.Equals(Constants.GOOGLE)) {
                    middlename = "+" + person.Middlename;
                }
            }

            return middlename;
        }

        private static string getBirthYear(PersonDO person, string webSite, PersonInfoDO personInfo) {
            string birthYear = "";
            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=in&GSby=1874&GSdyrel=in&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n

            if (person != null && !person.IsEmpty && (person.BirthYear > 100)) {
                if (webSite.Equals(Constants.ANCESTRY)) {
                    birthYear = "&MSAV=1&msbdy=" + person.BirthYear;
                } else if (webSite.Equals(Constants.FIND_A_GRAVE)) {
                    if (personInfo.YearRange == 0) {
                        birthYear = "&GSbyrel=in&GSby=" + person.BirthYear;
                    } else {
                        birthYear = "&GSbyrel=after&GSby=" + (person.BirthYear - personInfo.YearRange - 1);
                    }
                } else if (webSite.Equals(Constants.BILLION_GRAVES)) {
                    birthYear = "&birth_year=" + person.BirthYear;
                } else if (webSite.Equals(Constants.MY_HERITAGE)) {
                    birthYear = "&qbirth=Event+et.birth+ey." + +person.BirthYear;
                } else if (webSite.Equals(Constants.FIND_MY_PAST)) {
                    birthYear = "&yearofbirth=" + person.BirthYear + "&yearofbirth_offset=" + personInfo.YearRange;
                } else if (webSite.Equals(Constants.FAMILY_SEARCH_SYSTEM)) {
                    birthYear = "~%20%2Bbirth_year%3A" + (person.BirthYear - personInfo.YearRange) + "-" + (person.BirthYear + personInfo.YearRange);
                } else if (webSite.Equals(Constants.GOOGLE)) {
                    birthYear = person.BirthYear + "";
                }
            }

            return birthYear;
        }

        public static string GetCriteriaSummary(ResearchDO researchDO) {
            var summary = "";
            string generation = researchDO.Generation.ToString();
            if (researchDO.RowsReturned > 0) {
                if ((researchDO.Generation == 0) && !string.IsNullOrEmpty(researchDO.ReportTitle)) {
                    int generationsPos = researchDO.ReportTitle.IndexOf("Generations:");
                    int recordsPos = researchDO.ReportTitle.IndexOf("Records:");
                    int length = (recordsPos - 2) - (generationsPos + 12);
                    generation = researchDO.ReportTitle.Substring(generationsPos + 12, length);
                }
                string startingName = researchDO.PersonName;
                if (!string.IsNullOrEmpty(researchDO.ReportTitle)) {
                    int namePos = researchDO.ReportTitle.IndexOf("Name: ");
                    if (namePos > -1) {
                        int datePos = researchDO.ReportTitle.IndexOf("Date: ");
                        int length = (datePos - 2) - (namePos + 5);
                        startingName = researchDO.ReportTitle.Substring(namePos + 5, length);
                    }
                }
                string researchType = researchDO.ResearchType;
                if (!string.IsNullOrEmpty(researchDO.ReportTitle)) {
                    int researchTypePos = researchDO.ReportTitle.IndexOf("Research Type: ");
                    if (researchTypePos > -1) {
                        int generationos = researchDO.ReportTitle.IndexOf("Generations: ");
                        int length = (generationos - 3) - (researchTypePos + 15);
                        researchType = researchDO.ReportTitle.Substring(researchTypePos + 15, length);
                    }
                }

                summary = "<div class=\"col-md-12\"><p>" + "<span style=\"font-size: 15px; color: rgb(0,133,193)\">Found <b> + ResearchDO.Validations.Count + </b> clues by researching <b>" + generation.Trim() + "</b> generations of <b>" + researchType + "</b> starting from <b>" + startingName + "</b>, using the research criteria of <b>" + GetSearchCriteria(researchDO.SearchCriteria) + "</b>" + (!string.IsNullOrEmpty(researchDO.ReportTitle) ? ".<br>Retrieved Data: <b>" + researchDO.ReportTitle + "</b>" : "") + ".</span></p></div><br>";
            }
            return summary;
        }

        public static string GetDatesPlacesSummary(ResearchDO researchDO) {
            var summary = "";
            if (researchDO.RowsReturned > 0) {
                summary = "<div class=\"col-md-12\"><p>" + "<span style=\"font-size: 15px; color: rgb(0,0,255)\">Discovered <b>" + researchDO.RetrievedRecords + "</b> problems with dates and places by researching <b>" + researchDO.Generation + "</b> generations of <b>" + researchDO.ResearchType + "</b> starting from <b>" + researchDO.PersonName + " - " + researchDO.PersonId + "</b>.</span></p></div><br>";
            }
            return summary;
        }

        private static string getDeathYear(PersonDO person, string webSite, PersonInfoDO personInfo) {
            string deathYear = "";

            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=in&GSby=1874&GSdyrel=in&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n

            if (person != null && !person.IsEmpty && (person.DeathYear > 100)) {
                if (webSite.Equals(Constants.ANCESTRY)) {
                    deathYear = "&msddy=" + person.DeathYear;
                } else if (webSite.Equals(Constants.FIND_A_GRAVE)) {
                    if (personInfo.YearRange == 0) {
                        deathYear = "&GSdyrel=in&GSdy=" + person.DeathYear;
                    } else {
                        deathYear = "&GSdyrel=before&GSdy=" + (person.DeathYear + personInfo.YearRange + 1);
                    }
                } else if (webSite.Equals(Constants.BILLION_GRAVES)) {
                    deathYear = "&death_year=" + (person.DeathYear);
                } else if (webSite.Equals(Constants.MY_HERITAGE)) {
                } else if (webSite.Equals(Constants.FIND_MY_PAST)) {
                } else if (webSite.Equals(Constants.FAMILY_SEARCH_SYSTEM)) {
                    deathYear = "~%20%2Bdeath_year%3A" + +(person.DeathYear - personInfo.YearRange) + "-" + (person.DeathYear + personInfo.YearRange);
                } else if (webSite.Equals(Constants.GOOGLE)) {
                    deathYear = "" + person.DeathYear;
                }
            }

            return deathYear;
        }

        private static string getPlace(PersonDO person, string webSite, PersonInfoDO personInfo) {
            string place = "";

            if (person != null && !person.IsEmpty && !string.IsNullOrEmpty(person.BirthPlace) && personInfo.IncludePlace) {
                bool isUnitedStates = false;
                try {
                    if (webSite.Equals(Constants.ANCESTRY)) {
                        place = "&msbpn__ftp="; //&msbpn__ftp=Othello%2C+Adams%2C+Washington%2C+USA
                        string[] birthPlaceParts = person.BirthPlace.Split(',');

                        foreach (string birthPlacePart in birthPlaceParts) {
                            if (birthPlacePart == " United States") {
                                place += "United%20States";
                                isUnitedStates = true;
                            } else {
                                place += birthPlacePart.Trim() + "%2C";
                            }
                        }
                        if (isUnitedStates == false) {
                            place = place.Substring(0, place.Length - 3);
                        }
                    } else if (webSite.Equals(Constants.FIND_A_GRAVE)) {
                        place = person.BirthPlace;
                    } else if (webSite.Equals(Constants.BILLION_GRAVES)) {
                        place = person.BirthPlace;
                    } else if (webSite.Equals(Constants.MY_HERITAGE)) {
                        place = "&qany%2F1event=Event+et.any+ep.";
                        string[] birthPlaceParts = person.BirthPlace.Split(',');
                        foreach (string birthPlacePart in birthPlaceParts) {
                            if (birthPlacePart == " United States") {
                                place += "United%2F3States";
                                isUnitedStates = true;
                            } else {
                                place += birthPlacePart.Trim() + "%2C%2F3";
                            }
                        }
                        if (isUnitedStates == false) {
                            place = place.Substring(0, place.Length - 7);
                        }
                        //                        int unitedStatesPos = place.ToLower().IndexOf("united");
                        //                        if ((unitedStatesPos > -1) && (place.ToLower().IndexOf("states") > -1)) {
                        //                            place = place.Substring(0, unitedStatesPos);
                        //                            place += "United%2F3States";
                        //                        } else {
                        //                            place = place.Substring(0, place.Length - 7);
                        //                        }
                    } else if (webSite.Equals(Constants.FIND_MY_PAST)) {
                        place = person.BirthPlace;
                    } else if (webSite.Equals(Constants.FAMILY_SEARCH_SYSTEM)) {
                        string[] birthPlaceParts = person.BirthPlace.Split(',');
                        place = "~%20%2Bbirth_place%3A%22";
                        foreach (var birthPlacePart in birthPlaceParts) {
                            place += birthPlacePart.Trim() + "%2C%20";
                        }
                        place = place.Substring(0, place.Length - 6) + "%22";
                    } else if (webSite.Equals(Constants.GOOGLE)) {
                        place = person.BirthPlace;
                    }
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }

            return place;
        }

        public static string GetYearsLived(PersonDO person) {
            var yearsLived = "";
            if (person != null && !person.IsEmpty) {
                try {
                    if (!person.Living && (person.YearsLived > 0)) {
                        yearsLived = " Lived " + person.YearsLived + " years";
                    }
                } catch (Exception e) {
                    logger.Error(e.Message, e);
                    throw e;
                }
            }

            return yearsLived;
        }

        public static string NameColor(PersonDO person) {
            var nameColor = "0,0,255";

            if (person != null && !person.IsEmpty) {
                if (person.IsFemale && !person.UsingMaidenName) {
                    nameColor = "255,0,0";
                }
            }

            return nameColor;
        }

        public static string GetSearchCriteria(int index) {
            var searchCriteria = "";
            ArrayList list = (ArrayList)GetSearchCriteriaList().list;
            foreach (ListItemDO listItem in list) {
                if (listItem.ValueMember.Equals(index.ToString())) {
                    searchCriteria = listItem.DisplayMember;
                }
            }
            return searchCriteria;
        }

        public static ResultDO GetSearchCriteriaList() {
            var result = new ResultDO();
            if (searchCriteriaList == null) {
                searchCriteriaList = new ArrayList();
                searchCriteriaList.Add(new ListItemDO("0", "All"));
                searchCriteriaList.Add(new ListItemDO("1", "No death date, or \"deceased\", and lived between 1850 to 1940."));
                searchCriteriaList.Add(new ListItemDO("2", "A female child with no spouse and no death date, and lived between 1850 and 1940."));
                searchCriteriaList.Add(new ListItemDO("3", "Gap between children"));
                searchCriteriaList.Add(new ListItemDO("4", "Couples with no children, might have missing children"));
                searchCriteriaList.Add(new ListItemDO("5", "Couples with only one child and lived longer than 20 years"));
                searchCriteriaList.Add(new ListItemDO("6", "Person has no spouse and lived longer than 20 years"));
                searchCriteriaList.Add(new ListItemDO("7", "Person has no spouse and no children"));
                searchCriteriaList.Add(new ListItemDO("8", "Person has no spouse and only one child"));
                //                searchCriteriaList.Add(new ListItemDO("9", "Last name is different than parents last name"));
                searchCriteriaList.Add(new ListItemDO("10", "Child's birth year is after mother's death year"));
                searchCriteriaList.Add(new ListItemDO("11", "Death year is before the marriage year"));
                searchCriteriaList.Add(new ListItemDO("12", "Last child was born 4 or more years before the mother turned 40"));
                result.list = searchCriteriaList;
                //                searchCriteriaList.Add(new ListItemDO("13", "Married too early"));
            }

            return result;
        }
    }
}