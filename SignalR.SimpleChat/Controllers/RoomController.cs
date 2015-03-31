using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalR.Chat.Models;
using System.IO;

namespace SignalR.Chat.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private SignalRChatRepository repo = new SignalRChatRepository();

        //
        // GET: /Room/

        public ViewResult Index()
        {
            return View(repo.GetAllRoomsOnline());
        }

        //
        // GET: /Room/Details/5

        public ViewResult Enter(Guid id)
        {
            return View(repo.GetRoomViewModel(id));
        }

        // This action handles the form POST and the upload
        //[HttpPost]
        //public ActionResult Enter(HttpPostedFileBase file)
        //{
        //    // Verify that the user selected a file
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        // extract only the fielname
        //        var fileName = Path.GetFileName(file.FileName);
        //        // store the file inside ~/App_Data/uploads folder
        //        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
        //        file.SaveAs(path);
        //    }
        //    // redirect back to the index action to show the form once again
        //    return RedirectToAction("Index");
        //}

        //
        // GET: /Room/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Room/Create

        [HttpPost]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                repo.AddRoom(room);
                return RedirectToAction("Enter", new { id = room.RoomId });  
            }

            return View(room);
        }
        
        //
        // GET: /Room/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            return View(repo.GetRoom(id));
        }

        //
        // POST: /Room/Edit/5

        [HttpPost]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                repo.ModifyRoom(room);
                return RedirectToAction("Index");
            }
            return View(room);
        }

        //
        // GET: /Room/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            return View(repo.GetRoom(id));
        }

        //
        // POST: /Room/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            repo.DeleteRoom(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            repo.DisposeDb();
            base.Dispose(disposing);
        }
    }
}