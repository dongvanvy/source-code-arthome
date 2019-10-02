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
    public class DTCTcrmrangeController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/DTCTcrmrange
        public IQueryable<tbl_DTCTcrmrange> Gettbl_DTCTcrmrange()
        {
            return db.tbl_DTCTcrmrange;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTcrmrange))]
        [Route("api/DTCTcrmrange/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTcrmrange> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTcrmrange.Where(n => n.crm_DeleteFlag > dt || n.crm_SyncFlag > dt);
        }
        // GET: api/DTCTcrmrange/5
        [ResponseType(typeof(tbl_DTCTcrmrange))]
        public async Task<IHttpActionResult> Gettbl_DTCTcrmrange(string id)
        {
            tbl_DTCTcrmrange tbl_DTCTcrmrange = await db.tbl_DTCTcrmrange.FindAsync(id);
            if (tbl_DTCTcrmrange == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTcrmrange);
        }

        // PUT: api/DTCTcrmrange/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTcrmrange(string id, tbl_DTCTcrmrange tbl_DTCTcrmrange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTcrmrange.crm_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTcrmrange).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTcrmrangeExists(id))
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

        // POST: api/DTCTcrmrange
        [ResponseType(typeof(tbl_DTCTcrmrange))]
        public async Task<IHttpActionResult> Posttbl_DTCTcrmrange(tbl_DTCTcrmrange tbl_DTCTcrmrange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTcrmrange.Add(tbl_DTCTcrmrange);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTcrmrangeExists(tbl_DTCTcrmrange.crm_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTcrmrange.crm_id }, tbl_DTCTcrmrange);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTcrmrange))]
        [Route("api/DTCTcrmrange/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTcrmrange> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTcrmrangeExists(item.crm_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTcrmrange.Add(item);
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
        // DELETE: api/DTCTcrmrange/5
        [ResponseType(typeof(tbl_DTCTcrmrange))]
        public async Task<IHttpActionResult> Deletetbl_DTCTcrmrange(string id)
        {
            tbl_DTCTcrmrange tbl_DTCTcrmrange = await db.tbl_DTCTcrmrange.FindAsync(id);
            if (tbl_DTCTcrmrange == null)
            {
                return NotFound();
            }

            db.tbl_DTCTcrmrange.Remove(tbl_DTCTcrmrange);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTcrmrange);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTcrmrangeExists(string id)
        {
            return db.tbl_DTCTcrmrange.Count(e => e.crm_id == id) > 0;
        }
    }
}