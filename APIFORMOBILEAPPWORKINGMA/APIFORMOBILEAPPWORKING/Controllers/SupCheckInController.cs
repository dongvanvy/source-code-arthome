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
    public class SupCheckInController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/SupCheckIn
        public IQueryable<tbl_SupCheckIn> Gettbl_SupCheckIn()
        {
            return db.tbl_SupCheckIn;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_SupCheckIn))]
        [Route("api/SupCheckIn/SyncClient/{date}/{user}")]
        public IQueryable<tbl_SupCheckIn> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var lst = db.tbl_SupBigPlan.Where(n => n.Us_id == user).Select(n => n.SupBigPlan_id).ToList();
            return db.tbl_SupCheckIn.Where(n => lst.Contains(n.SupBigPlan_id) && (n.SupCheckIn_DeleteFlag > dt || n.SupCheckIn_SyncFlag > dt));
        }
        // GET: api/SupCheckIn/5
        [ResponseType(typeof(tbl_SupCheckIn))]
        public IHttpActionResult Gettbl_SupCheckIn(string id)
        {
            tbl_SupCheckIn tbl_SupCheckIn = db.tbl_SupCheckIn.Find(id);
            if (tbl_SupCheckIn == null)
            {
                return NotFound();
            }

            return Ok(tbl_SupCheckIn);
        }

        // PUT: api/SupCheckIn/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_SupCheckIn(string id, tbl_SupCheckIn tbl_SupCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_SupCheckIn.SupCheckIn_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_SupCheckIn).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_SupCheckInExists(id))
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

        // POST: api/SupCheckIn
        [ResponseType(typeof(tbl_SupCheckIn))]
        public IHttpActionResult Posttbl_SupCheckIn(tbl_SupCheckIn tbl_SupCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_SupCheckIn.Add(tbl_SupCheckIn);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_SupCheckInExists(tbl_SupCheckIn.SupCheckIn_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_SupCheckIn.SupCheckIn_id }, tbl_SupCheckIn);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_SupCheckIn))]
        [Route("api/SupCheckIn/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_SupCheckIn> tbl_SupCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_SupCheckIn)
            {
                if (tbl_SupCheckInExists(item.SupCheckIn_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_SupCheckIn.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Ok(tbl_SupCheckIn);
        }
        // DELETE: api/SupCheckIn/5
        [ResponseType(typeof(tbl_SupCheckIn))]
        public IHttpActionResult Deletetbl_SupCheckIn(string id)
        {
            tbl_SupCheckIn tbl_SupCheckIn = db.tbl_SupCheckIn.Find(id);
            if (tbl_SupCheckIn == null)
            {
                return NotFound();
            }

            db.tbl_SupCheckIn.Remove(tbl_SupCheckIn);
            db.SaveChanges();

            return Ok(tbl_SupCheckIn);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_SupCheckInExists(string id)
        {
            return db.tbl_SupCheckIn.Count(e => e.SupCheckIn_id == id) > 0;
        }
    }
}