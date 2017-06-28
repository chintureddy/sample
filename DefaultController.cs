using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication10.Controllers
{
    public class listUserdetails
    {
        public List<UserDetails> listusedils { get; set; }
    }
    public class Edit
    {
        public bool EditNotebody { get; set; }
    }
    public class UserDetails
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string NoteBody { get; set; }
        public bool Edit = false;

        public int count { get; set; }
       
    }
    public class DefaultController : Controller
    {
        // GET: Default
        public static List<UserDetails> userDetails = null;
        public static listUserdetails lstdetails = null;
       
        static DefaultController()
        {
            userDetails = new List<UserDetails>();
            lstdetails = new listUserdetails();
        }
        public ActionResult Index()
        {
            return View(initialloadDetails());
        }
        public listUserdetails initialloadDetails()
        {
            UserDetails ud = new UserDetails();
            ud.Edit = false;
            ud.count=1;
            userDetails.Add(ud);
            var Details = new listUserdetails
            {
                listusedils = userDetails
            };
            return Details;
        }
        public listUserdetails loadDetails()
        {
            var Details = new listUserdetails
            {
                listusedils = userDetails
            };
            return Details;
        }
        public UserDetails update (listUserdetails Details, int index, string text)
        {
            var result = userDetails.Where(s => s.count == index).FirstOrDefault();
            result.NoteBody = text;
            return result;
        }
        public listUserdetails removeDetails(listUserdetails Details, int index)
        { 
            userDetails.RemoveAt(index);
            var urDetails = new listUserdetails
            {
                listusedils = userDetails
            };
            return urDetails;
        }
      
        public ActionResult back(string contents)
        {
            listUserdetails detatils = loadDetails();
            UserDetails ud = new UserDetails();
            Edit edit = new Edit();
            if (contents.Contains("Edit"))
            {
                string inx = contents.Substring(contents.IndexOf('_') + 1, 1);
                var result = userDetails.Where(s => s.count == Convert.ToInt32(inx)).FirstOrDefault();
                return PartialView("View", detatils);
            }
            if(contents.Contains("cancel"))
            {
                string sub = contents.Substring(contents.IndexOf('_') + 1, 1);
                detatils = removeDetails(detatils, Convert.ToInt32(sub));
            }
            ud.UserID = 01;
            ud.UserName = "some";
            ud.NoteBody = contents;
            ud.Edit = false;
            ud.count++;
            userDetails.Add(ud);
            detatils.listusedils = userDetails;

            return PartialView("View", detatils);
        }
    }
}