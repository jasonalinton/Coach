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
    public class InventoryItemsController : Controller
    {
        private coachdevEntities db = new coachdevEntities();

        // GET: InventoryItems
        public ActionResult Index()
        {
            return View(db.inventoryitems.ToList());
        }

        // GET: InventoryItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inventoryitem inventoryitem = db.inventoryitems.Find(id);
            if (inventoryitem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryitem);
        }

        // GET: InventoryItems/Create
        public ActionResult Create()
        {

            ViewBag.Goals = db.goals.Select(x => new SelectListItem
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

        // POST: InventoryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,text,GoalIDs,TodoIDs,position,logPosition,description,json,misc,isActive,createdAt,updatedAt,guid")] inventoryitem inventoryitem)
        {
            if (ModelState.IsValid)
            {
                db.inventoryitems.Add(inventoryitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventoryitem);
        }

        // GET: InventoryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inventoryitem inventoryitem = db.inventoryitems.Find(id);
            if (inventoryitem == null)
            {
                return HttpNotFound();
            }

            ViewBag.Goals = db.goals.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = (inventoryitem.inventoryitem_goal.Select(y => y.idGoal).ToList().Contains(x.id))
            }).ToList();

            var todoIDs = new List<int>();
            foreach (var idList in inventoryitem.inventoryitem_goal.Select(y => y.goal.goal_todo.Select(z => z.idTodo)).ToList())
            {
                foreach (var todoID in idList)
                    todoIDs.Add(todoID);
            }

            ViewBag.Todos = db.todoes.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = todoIDs.Contains(x.id)
            }).ToList();
            return View(inventoryitem);
        }

        // POST: InventoryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,text,GoalIDs,TodoIDs,position,logPosition,description,json,misc,isActive,createdAt,updatedAt,guid")] inventoryitem inventoryitem)
        {
            if (ModelState.IsValid)
            {
                var originalInventoryItem = db.inventoryitems.SingleOrDefault(x => x.id == inventoryitem.id);
                if (originalInventoryItem != null)
                {
                    foreach (var todoID in inventoryitem.TodoIDs)
                    {
                        var inventoryitem_todo = db.inventoryitem_todo.SingleOrDefault(x => x.idInventoryItem == inventoryitem.id && x.idTodo == todoID);
                        if (inventoryitem_todo == null)
                        {
                            inventoryitem_todo = new inventoryitem_todo
                            {
                                idInventoryItem = inventoryitem.id,
                                idTodo = todoID
                            };

                            db.Entry(inventoryitem_todo).State = EntityState.Added;
                            db.inventoryitem_todo.Add(inventoryitem_todo);
                        }
                    }

                    foreach (var goalID in inventoryitem.GoalIDs)
                    {
                        var inventoryitem_goal = db.inventoryitem_goal.SingleOrDefault(x => x.idInventoryItem == inventoryitem.id && x.idGoal == goalID);
                        if (inventoryitem_goal == null)
                        {
                            inventoryitem_goal = new inventoryitem_goal
                            {
                                idInventoryItem = inventoryitem.id,
                                idGoal = goalID
                            };

                            db.Entry(inventoryitem_goal).State = EntityState.Added;
                            db.inventoryitem_goal.Add(inventoryitem_goal);
                        }
                    }

                    db.Entry(originalInventoryItem).CurrentValues.SetValues(inventoryitem);
                    db.Entry(originalInventoryItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(inventoryitem);
        }

        // GET: InventoryItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inventoryitem inventoryitem = db.inventoryitems.Find(id);
            if (inventoryitem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryitem);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inventoryitem inventoryitem = db.inventoryitems.Find(id);
            db.inventoryitems.Remove(inventoryitem);
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
