﻿using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CerberusMultiBranch.Models.Entities.Config;
using CerberusMultiBranch.Models;
using CerberusMultiBranch.Support;

namespace CerberusMultiBranch.Controllers.Config
{
    [Authorize]
    public class ConfigurationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public ActionResult SaveConfigVariable(Variable variable)
        {
            if (ModelState.IsValid)
            {
                if (variable.VariableId == Cons.Zero)
                    db.Variables.Add(variable);
                else
                    db.Entry(variable).State = EntityState.Modified;

                db.SaveChanges();
            }

            var variables = db.Variables.OrderBy(c => c.VariableId);
            return PartialView("_CategoryList", variables);
        }

        [HttpPost]
        
        public ActionResult SaveCategory(int? categoryId, string name)
        {
            
            if (ModelState.IsValid)
            {
                if (categoryId == null)
                {
                    var category = new Category {  Name = name };
                    db.Categories.Add(category);
                }
                else
                {
                    var category = new Category { CategoryId = categoryId.Value, Name = name };
                    db.Entry(category).State = EntityState.Modified;
                }
                db.SaveChanges();
            }

            var categories = db.Categories.OrderBy(c => c.CategoryId);
            return PartialView("_CategoryList", categories);
        }

        [HttpPost]
        
        public ActionResult GetCategories()
        {
            var categories = db.Categories.OrderBy(c => c.CategoryId).ToList();
            return PartialView("_CategoryList", categories);
        }


        [HttpPost]

        public ActionResult SaveBranch(int? branchId, string name)
        {

            if (ModelState.IsValid)
            {
                if (branchId == null)
                {
                    var branch = new Branch { Name = name };
                    db.Branches.Add(branch);
                }
                else
                {
                    var branch = new Branch { BranchId = branchId.Value, Name = name };
                    db.Entry(branch).State = EntityState.Modified;
                }
                db.SaveChanges();
            }

            var branches = db.Branches.OrderBy(c => c.BranchId);
            return PartialView("_BranchList", branches);
        }

        public ActionResult GetBranches()
        {
            var branches = db.Branches.OrderBy(b=> b.BranchId).ToList();
            return PartialView("_BranchList", branches);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
