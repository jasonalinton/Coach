using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coach.Data.DataAccess.Items;
using Coach.Data.Model;
using static Coach.Model.Planner.Time.Types;

namespace Coach.Site.Controllers.Planner
{
    public class TodoesController : Controller
    {
        ITodoDAO _todoDAO;
        private coachdevEntities db = new coachdevEntities();

        public TodoesController(ITodoDAO todoDAO)
        {
            _todoDAO = todoDAO;
        }

        // GET: Todoes
        public ActionResult Index()
        {
            return View(db.todoes.ToList());
        }

        // GET: Todoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            todo todo = db.todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // GET: Todoes/Create
        public ActionResult Create()
        {

            ViewBag.InventoryItems = db.inventoryitems.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();

            ViewBag.GoalIDs = db.goals.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();

            ViewBag.ParentIDs = db.todoes.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();

            ViewBag.ChildIDs = db.todoes.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();

            ViewBag.TimeIDs = db.times.ToList().Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();

            ViewBag.RepeatIDs = db.repeats.ToList().Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString()
            }).ToList();


            return View();
        }

        // POST: Todoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,text,InventoryItemIDs,GoalIDs,ParentIDs,ChildIDs,RepeatIDs,TimeIDs,description,points,json,isGroup,isVisible,isActive,createdAt,updatedAt")] todo todo)
        {
            if (ModelState.IsValid)
            {
                todo.inventoryitem_todo.ToList().ForEach(x => x.idTodo = todo.id);
                todo.goal_todo.ToList().ForEach(x => x.idTodo = todo.id);
                todo.todo_parent.ToList().ForEach(x => x.idChild = todo.id);
                todo.todo_child.ToList().ForEach(x => x.idParent = todo.id);
                todo.todo_time.ToList().ForEach(x => x.idTodo = todo.id);
                todo.todo_repeat.ToList().ForEach(x => x.idTodo = todo.id);

                foreach (var inventoryItemID in todo.InventoryItemIDs)
                {
                    var inventoryitem_todo = db.inventoryitem_todo.SingleOrDefault(x => x.idTodo == todo.id && x.idInventoryItem == inventoryItemID);
                    if (inventoryitem_todo == null)
                    {
                        inventoryitem_todo = new inventoryitem_todo
                        {
                            idTodo = todo.id,
                            idInventoryItem = inventoryItemID
                        };

                        db.Entry(inventoryitem_todo).State = EntityState.Added;
                        db.inventoryitem_todo.Add(inventoryitem_todo);
                    }
                }

                foreach (var goalID in todo.GoalIDs)
                {
                    var goal_todo = db.goal_todo.SingleOrDefault(x => x.idTodo == todo.id && x.idGoal == goalID);
                    if (goal_todo == null)
                    {
                        goal_todo = new goal_todo
                        {
                            idTodo = todo.id,
                            idGoal = goalID
                        };

                        db.Entry(goal_todo).State = EntityState.Added;
                        db.goal_todo.Add(goal_todo);
                    }
                }

                foreach (var parentID in todo.ParentIDs)
                {
                    var parent_todo = db.todo_todo.SingleOrDefault(x => x.idChild == todo.id && x.idParent == parentID);
                    if (parent_todo == null)
                    {
                        parent_todo = new todo_todo
                        {
                            idChild = todo.id,
                            idParent = parentID
                        };

                        db.Entry(parent_todo).State = EntityState.Added;
                        db.todo_todo.Add(parent_todo);
                    }
                }

                foreach (var childID in todo.ChildIDs)
                {
                    var child_todo = db.todo_todo.SingleOrDefault(x => x.idParent == todo.id && x.idChild == childID);
                    if (child_todo == null)
                    {
                        child_todo = new todo_todo
                        {
                            idChild = childID,
                            idParent = todo.id
                        };

                        db.Entry(child_todo).State = EntityState.Added;
                        db.todo_todo.Add(child_todo);
                    }
                }

                /* TEMP: IF TODO ISNT MAPPED TO A TIME HARDCODE ONE THAT STARTS AUG 3, 2020 */
                if (todo.TimeIDs.Count == 0)
                    todo.TimeIDs.Add(_todoDAO.HardCodeTimeFor_SUMMERMOVEOUT_Goals());
                foreach (var timeID in todo.TimeIDs)
                {
                    var todo_time = db.todo_time.SingleOrDefault(x => x.idTodo == todo.id && x.idTime == timeID);
                    if (todo_time == null)
                    {
                        todo_time = new todo_time
                        {
                            idTodo = todo.id,
                            idTime = timeID
                        };

                        db.Entry(todo_time).State = EntityState.Added;
                        db.todo_time.Add(todo_time);
                    }
                }

                foreach (var repeatID in todo.RepeatIDs)
                {
                    var todo_repeat = db.todo_repeat.SingleOrDefault(x => x.idTodo == todo.id && x.idRepeat == repeatID);
                    if (todo_repeat == null)
                    {
                        todo_repeat = new todo_repeat
                        {
                            idTodo = todo.id,
                            idRepeat = repeatID
                        };

                        db.Entry(todo_repeat).State = EntityState.Added;
                        db.todo_repeat.Add(todo_repeat);
                    }
                }

                db.todoes.Add(todo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(todo);
        }

        // GET: Todoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            todo todo = db.todoes
                    .Include("inventoryitem_todo.inventoryitem")
                    .Include("goal_todo.goal")
                    .Include("routine_todo.routine")
                    .Include("todo_parent.parent")
                    .Include("todo_child.child")
                    .Include("todo_time.time")
                    .Include("todo_repeat.repeat")
                    .Include("todo_eventtask.eventtask")
                    .ToList()
                    .Find(x => x.id == id);

            
            if (todo == null)
            {
                return HttpNotFound();
            }

            ViewBag.InventoryItems = db.inventoryitems.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = (todo.inventoryitem_todo.Select(y => y.idInventoryItem).ToList().Contains(x.id))
            }).ToList();

            var goalsSelected = todo.goal_todo.Select(y => y.idGoal).ToList();
            ViewBag.Goals = db.goals.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = goalsSelected.Contains(x.id)
            }).ToList();

            var parentsSelected = todo.todo_parent.Select(y => y.idParent).ToList();
            ViewBag.Parents = db.todoes.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = parentsSelected.Contains(x.id)
            }).ToList();

            var childrenSelected = todo.todo_child.Select(y => y.idChild).ToList();
            ViewBag.Children = db.todoes.Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = childrenSelected.Contains(x.id)
            }).ToList();

            ViewBag.Times = db.times.ToList().Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = (todo.todo_time.Select(y => y.idTime).ToList().Contains(x.id))
            }).ToList();

            ViewBag.Repeats = db.repeats.ToList().Select(x => new SelectListItem
            {
                Text = x.text,
                Value = x.id.ToString(),
                Selected = (todo.todo_repeat.Select(y => y.idRepeat).ToList().Contains(x.id))
            }).ToList();

            return View(todo);
        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,text,InventoryItemIDs,GoalIDs,ParentIDs,ChildIDs,RepeatIDs,TimeIDs,description,points,json,isGroup,isVisible,isActive,createdAt,updatedAt")] todo todo)
        {
            if (ModelState.IsValid)
            {
                var originalTodo = db.todoes.SingleOrDefault(x => x.id == todo.id);
                if (originalTodo != null)
                {
                    foreach (var inventoryItemID in todo.InventoryItemIDs)
                    {
                        var inventoryitem_todo = db.inventoryitem_todo.SingleOrDefault(x => x.idTodo == todo.id && x.idInventoryItem == inventoryItemID);
                        if (inventoryitem_todo == null)
                        {
                            inventoryitem_todo = new inventoryitem_todo
                            {
                                idTodo = todo.id,
                                idInventoryItem = inventoryItemID
                            };

                            db.Entry(inventoryitem_todo).State = EntityState.Added;
                            db.inventoryitem_todo.Add(inventoryitem_todo);
                        }
                    }

                    foreach (var goalID in todo.GoalIDs)
                    {
                        var goal_todo = db.goal_todo.SingleOrDefault(x => x.idTodo == todo.id && x.idGoal == goalID);
                        if (goal_todo == null)
                        {
                            goal_todo = new goal_todo
                            {
                                idTodo = todo.id,
                                idGoal = goalID
                            };

                            db.Entry(goal_todo).State = EntityState.Added;
                            db.goal_todo.Add(goal_todo);
                        }
                    }

                    foreach (var parentID in todo.ParentIDs)
                    {
                        var parent_todo = db.todo_todo.SingleOrDefault(x => x.idChild == todo.id && x.idParent == parentID);
                        if (parent_todo == null)
                        {
                            parent_todo = new todo_todo
                            {
                                idChild = todo.id,
                                idParent = parentID
                            };

                            db.Entry(parent_todo).State = EntityState.Added;
                            db.todo_todo.Add(parent_todo);
                        }
                    }

                    foreach (var childID in todo.ChildIDs)
                    {
                        var child_todo = db.todo_todo.SingleOrDefault(x => x.idParent == todo.id && x.idChild == childID);
                        if (child_todo == null)
                        {
                            child_todo = new todo_todo
                            {
                                idChild = childID,
                                idParent = todo.id
                            };

                            db.Entry(child_todo).State = EntityState.Added;
                            db.todo_todo.Add(child_todo);
                        }
                    }

                    ///* TEMP: IF TODO ISNT MAPPED TO A TIME HARDCODE ONE THAT STARTS AUG 3, 2020 */
                    //if (todo.TimeIDs.Count == 0)
                    //    todo.TimeIDs.Add(_todoDAO.HardCodeTimeFor_SUMMERMOVEOUT_Goals());
                    //foreach (var timeID in todo.TimeIDs)
                    //{
                    //    var todo_time = db.todo_time.SingleOrDefault(x => x.idTodo == todo.id && x.idTime == timeID);
                    //    if (todo_time == null)
                    //    {
                    //        todo_time = new todo_time
                    //        {
                    //            idTodo = todo.id,
                    //            idTime = timeID
                    //        };

                    //        db.Entry(todo_time).State = EntityState.Added;
                    //        db.todo_time.Add(todo_time);
                    //    }
                    //}

                    foreach (var repeatID in todo.RepeatIDs)
                    {
                        var todo_repeat = db.todo_repeat.SingleOrDefault(x => x.idTodo == todo.id && x.idRepeat == repeatID);
                        if (todo_repeat == null)
                        {
                            todo_repeat = new todo_repeat
                            {
                                idTodo = todo.id,
                                idRepeat = repeatID
                            };

                            db.Entry(todo_repeat).State = EntityState.Added;
                            db.todo_repeat.Add(todo_repeat);
                        }
                    }

                    db.Entry(originalTodo).CurrentValues.SetValues(todo);
                    db.Entry(originalTodo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(todo);
        }

        // GET: Todoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            todo todo = db.todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            todo todo = db.todoes.Find(id);
            db.todoes.Remove(todo);
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
