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
    public class DTCTonpostoffpostController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DTCTonpostoffpost
        public IQueryable<tbl_DTCTonpostoffpost> Gettbl_DTCTonpostoffpost()
        {
            return db.tbl_DTCTonpostoffpost;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DTCTonpostoffpost))]
        [Route("api/DTCTonpostoffpost/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DTCTonpostoffpost> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DTCTonpostoffpost.Where(n => n.ofpost_SyncFlag > dt || n.ofpost_DeleteFlag > dt);
        }
        // GET: api/DTCTonpostoffpost/5
        [ResponseType(typeof(tbl_DTCTonpostoffpost))]
        public async Task<IHttpActionResult> Gettbl_DTCTonpostoffpost(string id)
        {
            tbl_DTCTonpostoffpost tbl_DTCTonpostoffpost = await db.tbl_DTCTonpostoffpost.FindAsync(id);
            if (tbl_DTCTonpostoffpost == null)
            {
                return NotFound();
            }

            return Ok(tbl_DTCTonpostoffpost);
        }

        // PUT: api/DTCTonpostoffpost/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DTCTonpostoffpost(string id, tbl_DTCTonpostoffpost tbl_DTCTonpostoffpost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DTCTonpostoffpost.ofpost_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DTCTonpostoffpost).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DTCTonpostoffpostExists(id))
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

        // POST: api/DTCTonpostoffpost
        [ResponseType(typeof(tbl_DTCTonpostoffpost))]
        public async Task<IHttpActionResult> Posttbl_DTCTonpostoffpost(tbl_DTCTonpostoffpost tbl_DTCTonpostoffpost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DTCTonpostoffpost.Add(tbl_DTCTonpostoffpost);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DTCTonpostoffpostExists(tbl_DTCTonpostoffpost.ofpost_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DTCTonpostoffpost.ofpost_id }, tbl_DTCTonpostoffpost);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DTCTonpostoffpost))]
        [Route("api/DTCTonpostoffpost/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_DTCTonpostoffpost> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_DTCTonpostoffpostExists(item.ofpost_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DTCTonpostoffpost.Add(item);
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
        // DELETE: api/DTCTonpostoffpost/5
        [ResponseType(typeof(tbl_DTCTonpostoffpost))]
        public async Task<IHttpActionResult> Deletetbl_DTCTonpostoffpost(string id)
        {
            tbl_DTCTonpostoffpost tbl_DTCTonpostoffpost = await db.tbl_DTCTonpostoffpost.FindAsync(id);
            if (tbl_DTCTonpostoffpost == null)
            {
                return NotFound();
            }

            db.tbl_DTCTonpostoffpost.Remove(tbl_DTCTonpostoffpost);
            await db.SaveChangesAsync();

            return Ok(tbl_DTCTonpostoffpost);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DTCTonpostoffpostExists(string id)
        {
            return db.tbl_DTCTonpostoffpost.Count(e => e.ofpost_id == id) > 0;
        }
    }
}