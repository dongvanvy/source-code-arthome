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
    public class DTCTVARIANTController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/DTCTVARIANT
        public IQueryable<tbl_DTCTVARIANT> Gettbl_DTCTVARIANT()
        {
            return db.tbl_DTCTVARIANT;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTVARIANT))]
        [Route("api/DTCTVARIANT/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTVARIANT> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTVARIANT.Where(n => n.va_SyncFlag > dt || n.va_DeleteFlag > dt);
        }
        // GET: api/DTCTVARIANT/5
        [ResponseType(typeof(tbl_DTCTVARIANT))]
        public async Task<IHttpActionResult> Gettbl_DTCTVARIANT(string id)
        {
            tbl_DTCTVARIANT tbl_DTCTVARIANT = await db.tbl_DTCTVARIANT.FindAsync(id);
            if (tbl_DTCTVARIANT == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTVARIANT);
        }

        // PUT: api/DTCTVARIANT/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTVARIANT(string id, tbl_DTCTVARIANT tbl_DTCTVARIANT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTVARIANT.va_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTVARIANT).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTVARIANTExists(id))
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

        // POST: api/DTCTVARIANT
        [ResponseType(typeof(tbl_DTCTVARIANT))]
        public async Task<IHttpActionResult> Posttbl_DTCTVARIANT(tbl_DTCTVARIANT tbl_DTCTVARIANT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTVARIANT.Add(tbl_DTCTVARIANT);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTVARIANTExists(tbl_DTCTVARIANT.va_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTVARIANT.va_id }, tbl_DTCTVARIANT);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTVARIANT))]
        [Route("api/DTCTVARIANT/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTVARIANT> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTVARIANTExists(item.va_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTVARIANT.Add(item);
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
        // DELETE: api/DTCTVARIANT/5
        [ResponseType(typeof(tbl_DTCTVARIANT))]
        public async Task<IHttpActionResult> Deletetbl_DTCTVARIANT(string id)
        {
            tbl_DTCTVARIANT tbl_DTCTVARIANT = await db.tbl_DTCTVARIANT.FindAsync(id);
            if (tbl_DTCTVARIANT == null)
            {
                return NotFound();
            }

            db.tbl_DTCTVARIANT.Remove(tbl_DTCTVARIANT);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTVARIANT);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTVARIANTExists(string id)
        {
            return db.tbl_DTCTVARIANT.Count(e => e.va_id == id) > 0;
        }
    }
}