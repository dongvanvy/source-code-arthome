using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using APIFORMOBILEAPPWORKING.Models;

namespace APIFORMOBILEAPPWORKING.Controllers
{
    public class MenuController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/Menu
        public IQueryable<tbl_menu> Gettbl_menu()
        {
            return db.tbl_menu;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_menu))]
        [Route("api/Menu/SyncClient/{date}/{user}")]
        public IQueryable<tbl_menu> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            string role = db.tbl_user.SingleOrDefault(n => n.Us_id == user).Us_Role.ToString();
            return db.tbl_menu.Where(n => n.Menu_Role.Contains(role) && (n.Menu_DeleteFlag > dt || n.Menu_SyncFlag > dt));
        }
        // GET: api/Menu/5
        [ResponseType(typeof(tbl_menu))]
        public IHttpActionResult Gettbl_menu(int id)
        {
            tbl_menu tbl_menu = db.tbl_menu.Find(id);
            if (tbl_menu == null)
            {
                return NotFound();
            }

            return Ok(tbl_menu);
        }

        // PUT: api/Menu/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_menu(int id, tbl_menu tbl_menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_menu.Menu_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_menu).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_menuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Menu
        [ResponseType(typeof(tbl_menu))]
        public IHttpActionResult Posttbl_menu(tbl_menu tbl_menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_menu.Add(tbl_menu);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbl_menu.Menu_id }, tbl_menu);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_menu))]
        [Route("api/Menu/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_menu> tbl_menu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_menu)
            {
                if (tbl_menuExists(item.Menu_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_menu.Add(item);
                }
            }
            db.SaveChanges();
            return Ok(tbl_menu);
        }
        // DELETE: api/Menu/5
        [ResponseType(typeof(tbl_menu))]
        public IHttpActionResult Deletetbl_menu(int id)
        {
            tbl_menu tbl_menu = db.tbl_menu.Find(id);
            if (tbl_menu == null)
            {
                return NotFound();
            }

            db.tbl_menu.Remove(tbl_menu);
            db.SaveChanges();

            return Ok(tbl_menu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_menuExists(int id)
        {
            return db.tbl_menu.Count(e => e.Menu_id == id) > 0;
        }
    }
}