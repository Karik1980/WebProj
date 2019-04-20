using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;
using WebProject.Data;
using System.Data.Entity;

namespace WebProject.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
       
        // GET: Articel
        Context context = new Context();
        public ActionResult Index(int? categ,string name,int page=1)
        {
            int pagesize = 3;
            ViewBag.pagecount =(int) Math.Ceiling((double)(context.Articles.Count() / pagesize));
            var articles = context.Articles.
                            OrderBy(a => a.Id).
                            Skip((page - 1) * pagesize).
                            Take(pagesize).ToList();

           var searchcateg = context.Categories.ToList();

            ViewBag.categ = new SelectList(searchcateg, "Id", "Name");


         //   IQueryable<Article> articles = context.Articles;
            if (!String.IsNullOrEmpty(name))
            {
                articles = context.Articles.Where(a => a.Name.Contains(name)).ToList();
            }
            if (categ!=null)
            {
                articles = context.Articles.Where(a => a.Categories.Any(c => c.Id == categ)).ToList();
            }
         
            
        
            return View(articles);
        }
        [HttpGet]
        public ActionResult Add()
        {
            var categories = context.Categories.ToList();
            ViewBag.categories = categories;
            var tags = context.Tags.ToList();
            ViewBag.tags = tags;
            return View();
        }
        [HttpPost]
        public ActionResult Add(Article article,List<int> Categories,List<int> Tags,HttpPostedFileBase imagefile)
        {
            if(imagefile!=null)
            {
                string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imagefile.FileName);
                imagefile.SaveAs(Server.MapPath("~/Images/") + filename);
                article.Image = filename;
            }
            article.Date = DateTime.Now;

        
    
                context.Articles.Add(article);
                context.SaveChanges();
            
              return  RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var article = context.Articles.Find(Id);
            var categories = context.Categories.ToList();
            ViewBag.categories = categories;
            var tags = context.Tags.ToList();
            ViewBag.tags = tags;
    
            return View(article);
        }
        [HttpPost]
        public ActionResult Edit(Article article, List<int> Categories, List<int> Tags)
        {
            Article newarticle = context.Articles.Find(article.Id);
            newarticle.Name = article.Name;
            newarticle.Description = article.Description;
           
            newarticle.Image = article.Image;
            newarticle.Date = article.Date;
            newarticle.ViewCount = article.ViewCount;
            newarticle.Categories.Clear();
            if (Categories != null)
            {
                foreach (var c in context.Categories.Where(ca => Categories.Contains(ca.Id)).ToList())
                   newarticle.Categories.Add(c);
            }
       //     context.Entry(article).State = EntityState.Modified;
            if(Tags != null)
            {
                foreach (var t in context.Tags.Where(tag => Tags.Contains(tag.Id)).ToList())
                    newarticle.Tags.Add(t);
            }
            context.Entry(newarticle).State = EntityState.Modified;
            context.SaveChanges();
        
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            var article = context.Articles.Find(Id);
            if (article != null)
            {
                context.Articles.Remove(article);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            context.comments.Add(comment);
            context.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Page(int Id)
        {
            var article = context.Articles.FirstOrDefault(a => a.Id == Id);
            article.ViewCount++;
            
           
            if(article==null)
            {
                return HttpNotFound();
            }
            context.SaveChanges();
            return View(article);
        }
    }

}
