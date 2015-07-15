using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FindMyFamilies.Data;
using FindMyFamilies.Util;
using FindMyFamilies.Services;
using Gx;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minimod.PrettyPrint;
using Formatting = System.Xml.Formatting;

namespace FindMyFamilies.Services.Test {

    /// <summary>
    ///     Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1 {

        private Logger logger = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext {
            get;
            set;
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestInitialize]
        public void Initialize() {
            // and one of these too! 
        }

        [TestMethod]
        public void TestMethod1() {
//            ReportDO inputData = new ReportDO();
//    		ICollection outputData = PersonServices.Instance.ReadReportsAll(inputData);

            logger.Error("test");
            SessionDO session = ServiceManager.Instance.Authenticate("tuf000204920", "1234pass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");
            if (session.Authenticated) {
//                List<Person> persons1 = SerDvices.GetPersonAncestryWithSpouse("KW71-97V", "KW71-75P", ref session); //"KW71-SGH");
//                SearchAncestryDO searchAncestryDO = new SearchAncestryDO();
//                searchAncestryDO.PersonId = "KW71-97V";
//                searchAncestryDO.Generation = 9;
//               ServiceManager.GetPersonAncestryValidations(ref searchAncestryDO, ref session); //"KW71-SGH");
                Dictionary<string, PersonDO> gedcomx = null; //ServiceManager.GetPersonAncestry("KW71-97V", ref session); //"KW71-SGH");
//                Gedcomx gedcomx = Services.GetSpouses("KW71-758", ref session); //"KW71-SGH");
//                FamilySearchPlatform gedcomx = Services.GetChildParentRelationships("KW71-758", ref session); //"KW71-SGH");

//                List<Person> persons = Services.GetChildren("KW71-97V", ref session); //"KW71-SGH");
//                List<Person> persons1 = Services.GetPersonDescendancyWithSpouse("KW71-97V", "KW71-75P", ref session); //"KW71-SGH");
//                System.Diagnostics.Debug.WriteLine(persons1[0].PrettyPrint().ToString());
//                Gedcomx gedcomx = Services.GetPerson("KW71-756", ref session);
//               Person person = Services.GetPersonWithDetails("KW71-756", ref session);
//               Person person = Services.GetPersonWithRelationships("KW71-756", ref session);
//               Gedcomx gedcomx = ServiceManager.GetCurrentPerson(ref session);
//                Debug.WriteLine(session.Response.Content.PrettyPrint());
                foreach (var person in gedcomx) {
//                    Debug.WriteLine(person.Value.PrettyPrint());
//                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(person.Value, (Newtonsoft.Json.Formatting) Formatting.Indented));
//                    var jsonString = JsonConvert.SerializeObject(person, (Newtonsoft.Json.Formatting) Formatting.Indented, new JsonConverter[] {new StringEnumConverter()});
                    string test = "";
                }
//               System.Diagnostics.Debug.WriteLine(session.Response.PrettyPrint().ToString());

//               System.Diagnostics.Debug.WriteLine(session.Response.PrettyPrint().ToString());
//               System.Diagnostics.Debug.WriteLine(persons1.PrettyPrint().ToString());
//               System.Diagnostics.Debug.WriteLine(person.PrettyPrint().ToString());

//                string personDetailsLink = persons1[0].Identifiers[0].Value;
//                string personId = persons1[0].Id;
//                string name = persons1[0].Display.Name;

                string test1 = "";
//                string name = person.Display.Name;
//                Services.Person person1 = persons1[0];
//                String id = person1.Id;
//
//                String firstName = person1.Display.AscendancyNumber;
//                NamePart namePart = person1.Names[0].NameForms[0].Parts[0];
//                Link link = person1.Links[0];
//                System.Collections.Generic.List<Person> persons = api.GetParentRelationships("KW71-97V"); //"KW71-SGH");
//                System.Collections.Generic.List<Person> persons2 = api.GetAncestryQuery("KW71-97V"); //"KW71-SGH");
//                System.Collections.Generic.List<SourceDescription> sources = api.GetPersonSources("KW71-97V");
//                Person person = api.GetPerson("KW71-97V").Resource;
//                String gender = person.Gender.KnownType.ToString();
                //person.Names.
                //Assert.IsTrue(person.Names.Count > 0);
            }

            //
            // TODO: Add test logic here
            //
        }
    }

}