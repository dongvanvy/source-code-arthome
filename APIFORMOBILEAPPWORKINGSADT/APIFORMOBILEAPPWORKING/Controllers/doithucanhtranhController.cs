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
    public class doithucanhtranhController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/doithucanhtranh
        public IQueryable<tbl_doithucanhtranh> Gettbl_doithucanhtranh()
        {
            return db.tbl_doithucanhtranh;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_doithucanhtranh))]
        [Route("api/doithucanhtranh/SyncClient/{date}/{user}")]
        public IQueryable<tbl_doithucanhtranh> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_doithucanhtranh.Where(n => (n.dt_DeleteFlag > dt || n.dt_SyncFlag > dt) && n.US_ID==user);
        }
        // GET: api/doithucanhtranh/5
        [ResponseType(typeof(tbl_doithucanhtranh))]
        public async Task<IHttpActionResult> Gettbl_doithucanhtranh(string id)
        {
            tbl_doithucanhtranh tbl_doithucanhtranh = await db.tbl_doithucanhtranh.FindAsync(id);
            if (tbl_doithucanhtranh == null)
            {
                return NotFound();
            }

            return Ok(tbl_doithucanhtranh);
        }

        // PUT: api/doithucanhtranh/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_doithucanhtranh(string id, tbl_doithucanhtranh tbl_doithucanhtranh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_doithucanhtranh.dt_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_doithucanhtranh).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_doithucanhtranhExists(id))
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

        // POST: api/doithucanhtranh
        [ResponseType(typeof(tbl_doithucanhtranh))]
        public async Task<IHttpActionResult> Posttbl_doithucanhtranh(tbl_doithucanhtranh tbl_doithucanhtranh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_doithucanhtranh.Add(tbl_doithucanhtranh);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_doithucanhtranhExists(tbl_doithucanhtranh.dt_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_doithucanhtranh.dt_id }, tbl_doithucanhtranh);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_doithucanhtranh))]
        [Route("api/doithucanhtranh/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_doithucanhtranh> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_doithucanhtranhExists(item.dt_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_doithucanhtranh.Add(item);
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
        // DELETE: api/doithucanhtranh/5
        [ResponseType(typeof(tbl_doithucanhtranh))]
        public async Task<IHttpActionResult> Deletetbl_doithucanhtranh(string id)
        {
            tbl_doithucanhtranh tbl_doithucanhtranh = await db.tbl_doithucanhtranh.FindAsync(id);
            if (tbl_doithucanhtranh == null)
            {
                return NotFound();
            }

            db.tbl_doithucanhtranh.Remove(tbl_doithucanhtranh);
            await db.SaveChangesAsync();

            return Ok(tbl_doithucanhtranh);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_doithucanhtranhExists(string id)
        {
            return db.tbl_doithucanhtranh.Count(e => e.dt_id == id) > 0;
        }
    }
}