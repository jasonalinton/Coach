using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coach.Data.Model;

namespace Coach.Site.Controllers.Planner
{
    public class GoalsController : Controller
    {
        private coachdevEntities db = new coachdevEntities();

        // GET: Goals
        public ActionResult Index()
        {
            return View(db.goals.ToList());
        }

        // GET: Goals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            goal goal = db.goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // GET: Goals/Create
        public ActionResult Create()
        {

            ViewBag.InventoryItems = db.inventoryitems.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();

            ViewBag.Todos = db.todoes.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();

            return View();
        }

        // POST: Goals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,text,InventoryItemIDs,TodoIDs,description,position,json,misc,isVisible,isActive,createdAt,updatedAt")] goal goal)
        {
            if (ModelState.IsValid)
            {
                db.goals.Add(goal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goal);
        }

        // GET: Goals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            goal goal = db.goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }

            ViewBag.InventoryItems = db.inventoryitems.ToList().Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = (goal.inventoryitem_goal.Select(y => y.idInventoryItem).ToList().Contains(x.id))
            }).ToList();

            ViewBag.Todos = db.todoes.ToList().Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = (goal.goal_todo.Select(y => y.idTodo).ToList().Contains(x.id))
            }).ToList();

            return View(goal);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,text,InventoryItemIDs,TodoIDs,description,position,json,misc,isVisible,isActive,createdAt,updatedAt")] goal goal)
        {
            if (ModelState.IsValid)
            {
                var originalGoal = db.goals.SingleOrDefault(x => x.id == goal.id);
                if (originalGoal != null)
                {
                    foreach (var inventoryItemID in goal.InventoryItemIDs)
                    {
                        var inventoryitem_goal = db.inventoryitem_goal.SingleOrDefault(x => x.idGoal == goal.id && x.idInventoryItem == inventoryItemID);
                        if (inventoryitem_goal == null)
                        {
                            inventoryitem_goal = new inventoryitem_goal
                            {
                                idGoal = goal.id,
                                idInventoryItem = inventoryItemID
                            };

                            db.Entry(inventoryitem_goal).State = EntityState.Added;
                            db.inventoryitem_goal.Add(inventoryitem_goal);
                        }
                    }

                    foreach (var todoID in goal.TodoIDs)
                    {
                        var goal_todo = db.goal_todo.SingleOrDefault(x => x.idGoal == goal.id && x.idTodo == todoID);
                        if (goal_todo == null)
                        {
                            goal_todo = new goal_todo
                            {
                                idGoal = goal.id,
                                idTodo = todoID
                            };

                            db.Entry(goal_todo).State = EntityState.Added;
                            db.goal_todo.Add(goal_todo);
                        }
                    }

                    db.Entry(originalGoal).CurrentValues.SetValues(goal);
                    db.Entry(originalGoal).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(goal);
        }

        // GET: Goals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            goal goal = db.goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            goal goal = db.goals.Find(id);
            db.goals.Remove(goal);
            db.SaveChanges();
            return RedirectToAction("Index");
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
