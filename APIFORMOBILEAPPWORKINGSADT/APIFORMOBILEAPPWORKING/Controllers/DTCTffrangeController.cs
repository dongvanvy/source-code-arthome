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
    public class DTCTffrangeController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DTCTffrange
        public IQueryable<tbl_DTCTffrange> Gettbl_DTCTffrange()
        {
            return db.tbl_DTCTffrange;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTffrange))]
        [Route("api/DTCTffrange/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTffrange> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTffrange.Where(n => n.ff_SyncFlag > dt || n.ff_DeleteFlag > dt);
        }
        // GET: api/DTCTffrange/5
        [ResponseType(typeof(tbl_DTCTffrange))]
        public async Task<IHttpActionResult> Gettbl_DTCTffrange(string id)
        {
            tbl_DTCTffrange tbl_DTCTffrange = await db.tbl_DTCTffrange.FindAsync(id);
            if (tbl_DTCTffrange == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTffrange);
        }

        // PUT: api/DTCTffrange/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTffrange(string id, tbl_DTCTffrange tbl_DTCTffrange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTffrange.ff_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTffrange).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTffrangeExists(id))
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

        // POST: api/DTCTffrange
        [ResponseType(typeof(tbl_DTCTffrange))]
        public async Task<IHttpActionResult> Posttbl_DTCTffrange(tbl_DTCTffrange tbl_DTCTffrange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTffrange.Add(tbl_DTCTffrange);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTffrangeExists(tbl_DTCTffrange.ff_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTffrange.ff_id }, tbl_DTCTffrange);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTffrange))]
        [Route("api/DTCTffrange/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTffrange> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTffrangeExists(item.ff_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTffrange.Add(item);
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
        // DELETE: api/DTCTffrange/5
        [ResponseType(typeof(tbl_DTCTffrange))]
        public async Task<IHttpActionResult> Deletetbl_DTCTffrange(string id)
        {
            tbl_DTCTffrange tbl_DTCTffrange = await db.tbl_DTCTffrange.FindAsync(id);
            if (tbl_DTCTffrange == null)
            {
                return NotFound();
            }

            db.tbl_DTCTffrange.Remove(tbl_DTCTffrange);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTffrange);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTffrangeExists(string id)
        {
            return db.tbl_DTCTffrange.Count(e => e.ff_id == id) > 0;
        }
    }
}