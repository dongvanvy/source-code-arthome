using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using APIFORMOBILEAPPWORKING.Models;

namespace APIFORMOBILEAPPWORKING.Controllers
{
    public class KHtiepcanController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/KHtiepcan
        public IQueryable<Tbl_KHtiepcan> GetTbl_KHtiepcan()
        {
            return db.Tbl_KHtiepcan;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_KHtiepcan))]
        [Route("api/KHtiepcan/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_KHtiepcan> Gettbl_Target(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var big = db.tbl_bigplan.Where(n => n.Us_id == user && n.BigPlan_SyncFlag >= dt || n.Bigplan_deleteFlag>=dt);
            var selectbig = big.Select(n => n.Bigplan_id);
            return db.Tbl_KHtiepcan.Where(n => selectbig.Contains(n.Bigplan_id));
        }
        // GET: api/KHtiepcan/5
        [ResponseType(typeof(Tbl_KHtiepcan))]
        public async Task<IHttpActionResult> GetTbl_KHtiepcan(string id)
        {
            Tbl_KHtiepcan tbl_KHtiepcan = await db.Tbl_KHtiepcan.FindAsync(id);
            if (tbl_KHtiepcan == null)
            {
                return NotFound();
            }

            return Ok(tbl_KHtiepcan);
        }

        // PUT: api/KHtiepcan/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_KHtiepcan(string id, Tbl_KHtiepcan tbl_KHtiepcan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_KHtiepcan.KHTC_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_KHtiepcan).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_KHtiepcanExists(id))
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

        // POST: api/KHtiepcan
        [ResponseType(typeof(Tbl_KHtiepcan))]
        public async Task<IHttpActionResult> PostTbl_KHtiepcan(Tbl_KHtiepcan tbl_KHtiepcan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_KHtiepcan.Add(tbl_KHtiepcan);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_KHtiepcanExists(tbl_KHtiepcan.KHTC_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_KHtiepcan.KHTC_ID }, tbl_KHtiepcan);
        }
        [HttpPost]
        [ResponseType(typeof(Tbl_KHtiepcan))]
        [Route("api/KHtiepcan/SyncServer")]
        public IHttpActionResult Posttbl_Target(List<Tbl_KHtiepcan> tbl_Target)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_Target)
            {
                if (Tbl_KHtiepcanExists(item.KHTC_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.Tbl_KHtiepcan.Add(item);
                }
            }
            db.SaveChanges();

            return Ok(tbl_Target);
        }

        // DELETE: api/KHtiepcan/5
        [ResponseType(typeof(Tbl_KHtiepcan))]
        public async Task<IHttpActionResult> DeleteTbl_KHtiepcan(string id)
        {
            Tbl_KHtiepcan tbl_KHtiepcan = await db.Tbl_KHtiepcan.FindAsync(id);
            if (tbl_KHtiepcan == null)
            {
                return NotFound();
            }

            db.Tbl_KHtiepcan.Remove(tbl_KHtiepcan);
            await db.SaveChangesAsync();

            return Ok(tbl_KHtiepcan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_KHtiepcanExists(string id)
        {
            return db.Tbl_KHtiepcan.Count(e => e.KHTC_ID == id) > 0;
        }
    }
}