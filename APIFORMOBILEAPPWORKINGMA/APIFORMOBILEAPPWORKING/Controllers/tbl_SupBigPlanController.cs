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
    public class tbl_SupBigPlanController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/tbl_SupBigPlan
        public IQueryable<tbl_SupBigPlan> Gettbl_SupBigPlan()
        {
            return db.tbl_SupBigPlan;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_SupBigPlan))]
        [Route("api/SupBigPlan/SyncClient/{date}/{user}")]
        public IQueryable<tbl_SupBigPlan> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_SupBigPlan.Where(n => n.SupBigPlan_SupUsername.Contains(user) && (n.SupBigPlan_DeleteFlag > dt || n.SupBigPlan_SyncFlag > dt));
        }
        // GET: api/tbl_SupBigPlan/5
        [ResponseType(typeof(tbl_SupBigPlan))]
        public IHttpActionResult Gettbl_SupBigPlan(string id)
        {
            tbl_SupBigPlan tbl_SupBigPlan = db.tbl_SupBigPlan.Find(id);
            if (tbl_SupBigPlan == null)
            {
                return NotFound();
            }

            return Ok(tbl_SupBigPlan);
        }

        // PUT: api/tbl_SupBigPlan/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_SupBigPlan(string id, tbl_SupBigPlan tbl_SupBigPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_SupBigPlan.SupBigPlan_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_SupBigPlan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_SupBigPlanExists(id))
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

        // POST: api/tbl_SupBigPlan
        [ResponseType(typeof(tbl_SupBigPlan))]
        public IHttpActionResult Posttbl_SupBigPlan(tbl_SupBigPlan tbl_SupBigPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_SupBigPlan.Add(tbl_SupBigPlan);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_SupBigPlanExists(tbl_SupBigPlan.SupBigPlan_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_SupBigPlan.SupBigPlan_id }, tbl_SupBigPlan);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_SupBigPlan))]
        [Route("api/SupBigPlan/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_SupBigPlan> tbl_SupBigPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_SupBigPlan)
            {
                if (tbl_SupBigPlanExists(item.SupBigPlan_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_SupBigPlan.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Ok(tbl_SupBigPlan);
        }
        // DELETE: api/tbl_SupBigPlan/5
        [ResponseType(typeof(tbl_SupBigPlan))]
        public IHttpActionResult Deletetbl_SupBigPlan(string id)
        {
            tbl_SupBigPlan tbl_SupBigPlan = db.tbl_SupBigPlan.Find(id);
            if (tbl_SupBigPlan == null)
            {
                return NotFound();
            }

            db.tbl_SupBigPlan.Remove(tbl_SupBigPlan);
            db.SaveChanges();

            return Ok(tbl_SupBigPlan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_SupBigPlanExists(string id)
        {
            return db.tbl_SupBigPlan.Count(e => e.SupBigPlan_id == id) > 0;
        }
    }
}