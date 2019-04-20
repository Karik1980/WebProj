using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Data;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class TagController : Controller
    {
        Context context = new Context();

        public ActionResult Index()
        {
          var tags=  context.Tags.ToList();
            return View(tags);
        }
        [HttpGet]
        public ActionResult AddTag()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTag(Tag tag)
        {
            context.Tags.Add(tag);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}