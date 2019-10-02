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

namespace APIFORMOBILEAPPWORKING.Models
{
    public class DTCTBrandController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/DTCTBrand
        public IQueryable<tbl_DTCTBrand> Gettbl_DTCTBrand()
        {
            return db.tbl_DTCTBrand;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTBrand))]
        [Route("api/DTCTBrand/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTBrand> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTBrand.Where(n => n.Brand_DeleteFlag > dt || n.Brand_SyncFlag > dt);
        }
        // GET: api/DTCTBrand/5
        [ResponseType(typeof(tbl_DTCTBrand))]
        public async Task<IHttpActionResult> Gettbl_DTCTBrand(string id)
        {
            tbl_DTCTBrand tbl_DTCTBrand = await db.tbl_DTCTBrand.FindAsync(id);
            if (tbl_DTCTBrand == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTBrand);
        }

        // PUT: api/DTCTBrand/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTBrand(string id, tbl_DTCTBrand tbl_DTCTBrand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTBrand.Brand_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTBrand).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTBrandExists(id))
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

        // POST: api/DTCTBrand
        [ResponseType(typeof(tbl_DTCTBrand))]
        public async Task<IHttpActionResult> Posttbl_DTCTBrand(tbl_DTCTBrand tbl_DTCTBrand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTBrand.Add(tbl_DTCTBrand);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTBrandExists(tbl_DTCTBrand.Brand_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTBrand.Brand_id }, tbl_DTCTBrand);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTBrand))]
        [Route("api/DTCTBrand/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTBrand> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTBrandExists(item.Brand_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTBrand.Add(item);
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
        // DELETE: api/DTCTBrand/5
        [ResponseType(typeof(tbl_DTCTBrand))]
        public async Task<IHttpActionResult> Deletetbl_DTCTBrand(string id)
        {
            tbl_DTCTBrand tbl_DTCTBrand = await db.tbl_DTCTBrand.FindAsync(id);
            if (tbl_DTCTBrand == null)
            {
                return NotFound();
            }

            db.tbl_DTCTBrand.Remove(tbl_DTCTBrand);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTBrand);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTBrandExists(string id)
        {
            return db.tbl_DTCTBrand.Count(e => e.Brand_id == id) > 0;
        }
    }
}