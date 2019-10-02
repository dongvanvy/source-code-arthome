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
    public class BigplanController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/Bigplan
        public IQueryable<tbl_bigplan> Gettbl_bigplan()
        {
            return db.tbl_bigplan;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_bigplan))]
        [Route("api/BigPlan/SyncClient/{date}/{user}")]
        public IQueryable<tbl_bigplan> SyncClienttbl_bigplan(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_bigplan.Where(n => (n.BigPlan_SyncFlag >= dt || n.Bigplan_deleteFlag >= dt) && n.Us_id == user);
        }
        // GET: api/Bigplan/5
        [ResponseType(typeof(tbl_bigplan))]
        public IHttpActionResult Gettbl_bigplan(string id)
        {
            tbl_bigplan tbl_bigplan = db.tbl_bigplan.Find(id);
            if (tbl_bigplan == null)
            {
                return NotFound();
            }

            return Ok(tbl_bigplan);
        }

        // PUT: api/Bigplan/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_bigplan(string id, tbl_bigplan tbl_bigplan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_bigplan.Bigplan_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_bigplan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_bigplanExists(id))
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

        // POST: api/Bigplan
        [ResponseType(typeof(tbl_bigplan))]
        public IHttpActionResult Posttbl_bigplan(tbl_bigplan tbl_bigplan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_bigplan.Add(tbl_bigplan);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_bigplanExists(tbl_bigplan.Bigplan_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_bigplan.Bigplan_id }, tbl_bigplan);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_bigplan))]
        [Route("api/BigPlan/SyncServer")]
        public IHttpActionResult Posttbl_bigplan(List<tbl_bigplan> tbl_bigplan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_bigplan)
            {
                if (tbl_bigplanExists(item.Bigplan_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_bigplan.Add(item);
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return NotFound();
            }
            return Ok(tbl_bigplan);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_bigplan.Bigplan_id }, tbl_bigplan);
        }
        // DELETE: api/Bigplan/5
        [ResponseType(typeof(tbl_bigplan))]
        public IHttpActionResult Deletetbl_bigplan(string id)
        {
            tbl_bigplan tbl_bigplan = db.tbl_bigplan.Find(id);
            if (tbl_bigplan == null)
            {
                return NotFound();
            }

            db.tbl_bigplan.Remove(tbl_bigplan);
            db.SaveChanges();

            return Ok(tbl_bigplan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_bigplanExists(string id)
        {
            return db.tbl_bigplan.Count(e => e.Bigplan_id == id) > 0;
        }
    }
}