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
    public class TargetController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/Target
        public IQueryable<tbl_Target> Gettbl_Target()
        {
            return db.tbl_Target;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_Target))]
        [Route("api/Target/SyncClient/{date}/{user}")]
        public IQueryable<tbl_Target> Gettbl_Target(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_Target.Where(n => n.Us_id == user && (n.Target_SyncFlag > dt || n.Target_DeleteFlag > dt));
        }
        [HttpGet]
        [ResponseType(typeof(tbl_Target))]
        [Route("api/Target/GetReport/{user}/{datebd}/{datekt}")]
        public IQueryable<tbl_Target> GetReporttbl_Target( string user, string datebd, string datekt)
        {
            datebd = datebd.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtbd = Convert.ToDateTime(datebd);
            datekt = datekt.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtkt = Convert.ToDateTime(datekt);
            return db.tbl_Target.Where(n => n.Us_id == user && n.Target_DayStart>=dtbd && n.Target_DayEnd<=dtkt);
        }
        // GET: api/Target/5
        [ResponseType(typeof(tbl_Target))]
        public IHttpActionResult Gettbl_Target(string id)
        {
            tbl_Target tbl_Target = db.tbl_Target.Find(id);
            if (tbl_Target == null)
            {
                return NotFound();
            }

            return Ok(tbl_Target);
        }

        // PUT: api/Target/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Target(string id, tbl_Target tbl_Target)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Target.Target_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_Target).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_TargetExists(id))
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

        // POST: api/Target
        [ResponseType(typeof(tbl_Target))]
        public IHttpActionResult Posttbl_Target(tbl_Target tbl_Target)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Target.Add(tbl_Target);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_TargetExists(tbl_Target.Target_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_Target.Target_ID }, tbl_Target);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_Target))]
        [Route("api/Target/SyncServer")]
        public IHttpActionResult Posttbl_Target(List<tbl_Target> tbl_Target)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_Target)
            {
                if (tbl_TargetExists(item.Target_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_Target.Add(item);
                }
            }
            db.SaveChanges();

            return Ok(tbl_Target);
        }
        // DELETE: api/Target/5
        [ResponseType(typeof(tbl_Target))]
        public IHttpActionResult Deletetbl_Target(string id)
        {
            tbl_Target tbl_Target = db.tbl_Target.Find(id);
            if (tbl_Target == null)
            {
                return NotFound();
            }

            db.tbl_Target.Remove(tbl_Target);
            db.SaveChanges();

            return Ok(tbl_Target);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_TargetExists(string id)
        {
            return db.tbl_Target.Count(e => e.Target_ID == id) > 0;
        }
    }
}