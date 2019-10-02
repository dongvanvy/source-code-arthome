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
    public class DTCTltmrangeController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/DTCTltmrange
        public IQueryable<tbl_DTCTltmrange> Gettbl_DTCTltmrange()
        {
            return db.tbl_DTCTltmrange;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTltmrange))]
        [Route("api/DTCTltmrange/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTltmrange> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTltmrange.Where(n => n.ltm_SyncFlag > dt || n.ltm_DeleteFlag > dt);
        }
        // GET: api/DTCTltmrange/5
        [ResponseType(typeof(tbl_DTCTltmrange))]
        public async Task<IHttpActionResult> Gettbl_DTCTltmrange(string id)
        {
            tbl_DTCTltmrange tbl_DTCTltmrange = await db.tbl_DTCTltmrange.FindAsync(id);
            if (tbl_DTCTltmrange == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTltmrange);
        }

        // PUT: api/DTCTltmrange/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTltmrange(string id, tbl_DTCTltmrange tbl_DTCTltmrange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTltmrange.ltm_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTltmrange).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTltmrangeExists(id))
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

        // POST: api/DTCTltmrange
        [ResponseType(typeof(tbl_DTCTltmrange))]
        public async Task<IHttpActionResult> Posttbl_DTCTltmrange(tbl_DTCTltmrange tbl_DTCTltmrange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTltmrange.Add(tbl_DTCTltmrange);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTltmrangeExists(tbl_DTCTltmrange.ltm_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTltmrange.ltm_id }, tbl_DTCTltmrange);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTltmrange))]
        [Route("api/DTCTltmrange/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTltmrange> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTltmrangeExists(item.ltm_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTltmrange.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Ok(tbl_sku);
        }
        // DELETE: api/DTCTltmrange/5
        [ResponseType(typeof(tbl_DTCTltmrange))]
        public async Task<IHttpActionResult> Deletetbl_DTCTltmrange(string id)
        {
            tbl_DTCTltmrange tbl_DTCTltmrange = await db.tbl_DTCTltmrange.FindAsync(id);
            if (tbl_DTCTltmrange == null)
            {
                return NotFound();
            }

            db.tbl_DTCTltmrange.Remove(tbl_DTCTltmrange);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTltmrange);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTltmrangeExists(string id)
        {
            return db.tbl_DTCTltmrange.Count(e => e.ltm_id == id) > 0;
        }
    }
}