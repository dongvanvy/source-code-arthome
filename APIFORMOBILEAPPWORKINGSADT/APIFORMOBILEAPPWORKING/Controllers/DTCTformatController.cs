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
    public class DTCTformatController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DTCTformat
        public IQueryable<tbl_DTCTformat> Gettbl_DTCTformat()
        {
            return db.tbl_DTCTformat;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTformat))]
        [Route("api/DTCTformat/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTformat> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTformat.Where(n => n.bf_SyncFlag > dt || n.bf_DeleteFlag > dt);
        }
        // GET: api/DTCTformat/5
        [ResponseType(typeof(tbl_DTCTformat))]
        public async Task<IHttpActionResult> Gettbl_DTCTformat(string id)
        {
            tbl_DTCTformat tbl_DTCTformat = await db.tbl_DTCTformat.FindAsync(id);
            if (tbl_DTCTformat == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTformat);
        }

        // PUT: api/DTCTformat/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTformat(string id, tbl_DTCTformat tbl_DTCTformat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTformat.bf_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTformat).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTformatExists(id))
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

        // POST: api/DTCTformat
        [ResponseType(typeof(tbl_DTCTformat))]
        public async Task<IHttpActionResult> Posttbl_DTCTformat(tbl_DTCTformat tbl_DTCTformat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTformat.Add(tbl_DTCTformat);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTformatExists(tbl_DTCTformat.bf_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTformat.bf_id }, tbl_DTCTformat);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTformat))]
        [Route("api/DTCTformat/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTformat> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTformatExists(item.bf_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTformat.Add(item);
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
        // DELETE: api/DTCTformat/5
        [ResponseType(typeof(tbl_DTCTformat))]
        public async Task<IHttpActionResult> Deletetbl_DTCTformat(string id)
        {
            tbl_DTCTformat tbl_DTCTformat = await db.tbl_DTCTformat.FindAsync(id);
            if (tbl_DTCTformat == null)
            {
                return NotFound();
            }

            db.tbl_DTCTformat.Remove(tbl_DTCTformat);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTformat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTformatExists(string id)
        {
            return db.tbl_DTCTformat.Count(e => e.bf_id == id) > 0;
        }
    }
}