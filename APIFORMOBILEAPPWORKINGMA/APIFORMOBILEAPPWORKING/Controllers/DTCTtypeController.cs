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
    public class DTCTtypeController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/DTCTtype
        public IQueryable<tbl_DTCTtype> Gettbl_DTCTtype()
        {
            return db.tbl_DTCTtype;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTtype))]
        [Route("api/DTCTtype/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTtype> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTtype.Where(n => n.ty_SyncFlag > dt || n.ty_DeleteFlag > dt);
        }
        // GET: api/DTCTtype/5
        [ResponseType(typeof(tbl_DTCTtype))]
        public async Task<IHttpActionResult> Gettbl_DTCTtype(string id)
        {
            tbl_DTCTtype tbl_DTCTtype = await db.tbl_DTCTtype.FindAsync(id);
            if (tbl_DTCTtype == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTtype);
        }

        // PUT: api/DTCTtype/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTtype(string id, tbl_DTCTtype tbl_DTCTtype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTtype.ty_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTtype).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTtypeExists(id))
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

        // POST: api/DTCTtype
        [ResponseType(typeof(tbl_DTCTtype))]
        public async Task<IHttpActionResult> Posttbl_DTCTtype(tbl_DTCTtype tbl_DTCTtype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTtype.Add(tbl_DTCTtype);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTtypeExists(tbl_DTCTtype.ty_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTtype.ty_id }, tbl_DTCTtype);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTtype))]
        [Route("api/DTCTtype/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTtype> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTtypeExists(item.ty_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTtype.Add(item);
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
        // DELETE: api/DTCTtype/5
        [ResponseType(typeof(tbl_DTCTtype))]
        public async Task<IHttpActionResult> Deletetbl_DTCTtype(string id)
        {
            tbl_DTCTtype tbl_DTCTtype = await db.tbl_DTCTtype.FindAsync(id);
            if (tbl_DTCTtype == null)
            {
                return NotFound();
            }

            db.tbl_DTCTtype.Remove(tbl_DTCTtype);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTtype);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTtypeExists(string id)
        {
            return db.tbl_DTCTtype.Count(e => e.ty_id == id) > 0;
        }
    }
}