using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ControllerBase = FindMyFamilies.Web.Controllers.ControllerBase;

namespace FindMyFamilies.Web {
    public class LogEntriesController : ControllerBase {
        private const int ITEMSPERPAGE = 10;

        public LogReader myLogReader {
            get {
                if (HttpContext.Cache["LogReader"] == null) {
                    HttpContext.Cache["LogReader"] = new LogReader();
                }
                return (LogReader) HttpContext.Cache["LogReader"];
            }
            set {
                HttpContext.Cache["LogReader"] = value;
            }
        }
  
        [System.Web.Http.HttpGet]
        public ActionResult GetEntries(string currentLogFolder, string searchTerm, int page, bool reload) {

            if (string.IsNullOrEmpty(currentLogFolder)) {
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }

            if (reload) {
                myLogReader = new LogReader();
            }

            List<ConfigurationReader.LogPathEntry> logFolders = ConfigurationReader.ReadLogFolders();
            ConfigurationReader.LogPathEntry folderEntry = logFolders.FirstOrDefault(x => x.Name == currentLogFolder);

            if (folderEntry == null) {
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }

            //Load Log Folder (if not already loaded)
            if (myLogReader.LogPathsLoaded.Contains(folderEntry.Path) != true) {
                myLogReader.LoadLogFolder(folderEntry.Name, folderEntry.Path);
            }

            List<LogGrouping> results = myLogReader.GroupedLogEntries.ToList();

            //Apply Search Critera if provided
            if (string.IsNullOrEmpty(searchTerm) != true) {
                results = results.Where(x => x.ErrorMessage.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || x.ErrorDetail.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int pageCount = results.Count / ITEMSPERPAGE;
            if ((results.Count % ITEMSPERPAGE) > 0) {
                pageCount++;
            }

            //Apply Paging
            int skip = (page - 1) * ITEMSPERPAGE;
            results = results.Skip(skip).Take(ITEMSPERPAGE).ToList();

            return Json(new {Items = results, PageCount = pageCount}, JsonRequestBehavior.AllowGet);
        }

        // GET api/logentries/5
        public string Get(int id) {
            return "value";
        }

        // POST api/logentries
        public void Post([FromBody] string value) {
        }

        // PUT api/logentries/5
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/logentries/5
        public void Delete(string id) {
            myLogReader.ClearLogEntriesInGroup(id);
        }

    }
}