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
    public class DTCTCtyController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/DTCTCty
        public IQueryable<tbl_DTCTCty> Gettbl_DTCTCty()
        {
            return db.tbl_DTCTCty;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTCty))]
        [Route("api/DTCTCty/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTCty> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTCty.Where(n => n.cty_DeleteFlag > dt || n.cty_SyncFlag > dt);
        }
        // GET: api/DTCTCty/5
        [ResponseType(typeof(tbl_DTCTCty))]
        public async Task<IHttpActionResult> Gettbl_DTCTCty(string id)
        {
            tbl_DTCTCty tbl_DTCTCty = await db.tbl_DTCTCty.FindAsync(id);
            if (tbl_DTCTCty == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTCty);
        }

        // PUT: api/DTCTCty/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTCty(string id, tbl_DTCTCty tbl_DTCTCty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTCty.cty_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTCty).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTCtyExists(id))
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

        // POST: api/DTCTCty
        [ResponseType(typeof(tbl_DTCTCty))]
        public async Task<IHttpActionResult> Posttbl_DTCTCty(tbl_DTCTCty tbl_DTCTCty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTCty.Add(tbl_DTCTCty);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTCtyExists(tbl_DTCTCty.cty_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTCty.cty_id }, tbl_DTCTCty);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTCty))]
        [Route("api/DTCTCty/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTCty> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTCtyExists(item.cty_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTCty.Add(item);
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
        // DELETE: api/DTCTCty/5
        [ResponseType(typeof(tbl_DTCTCty))]
        public async Task<IHttpActionResult> Deletetbl_DTCTCty(string id)
        {
            tbl_DTCTCty tbl_DTCTCty = await db.tbl_DTCTCty.FindAsync(id);
            if (tbl_DTCTCty == null)
            {
                return NotFound();
            }

            db.tbl_DTCTCty.Remove(tbl_DTCTCty);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTCty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTCtyExists(string id)
        {
            return db.tbl_DTCTCty.Count(e => e.cty_id == id) > 0;
        }
    }
}