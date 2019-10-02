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
    public class DMQT_CTDQController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DMQT_CTDQ
        public IQueryable<tbl_DMQT_CTDQ> Gettbl_DMQT_CTDQ()
        {
            return db.tbl_DMQT_CTDQ;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DMQT_CTDQ))]
        [Route("api/DMQT_CTDQ/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DMQT_CTDQ> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DMQT_CTDQ.Where(n => n.DMCT_DeleteFlag > dt || n.DMCT_SyncFlag > dt);
        }
        // GET: api/DMQT_CTDQ/5
        [ResponseType(typeof(tbl_DMQT_CTDQ))]
        public async Task<IHttpActionResult> Gettbl_DMQT_CTDQ(string id)
        {
            tbl_DMQT_CTDQ tbl_DMQT_CTDQ = await db.tbl_DMQT_CTDQ.FindAsync(id);
            if (tbl_DMQT_CTDQ == null)
            {
                return NotFound();
            }

            return Ok(tbl_DMQT_CTDQ);
        }

        // PUT: api/DMQT_CTDQ/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DMQT_CTDQ(string id, tbl_DMQT_CTDQ tbl_DMQT_CTDQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DMQT_CTDQ.DMCT_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_DMQT_CTDQ).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DMQT_CTDQExists(id))
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

        // POST: api/DMQT_CTDQ
        [ResponseType(typeof(tbl_DMQT_CTDQ))]
        public async Task<IHttpActionResult> Posttbl_DMQT_CTDQ(tbl_DMQT_CTDQ tbl_DMQT_CTDQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DMQT_CTDQ.Add(tbl_DMQT_CTDQ);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DMQT_CTDQExists(tbl_DMQT_CTDQ.DMCT_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DMQT_CTDQ.DMCT_ID }, tbl_DMQT_CTDQ);
        }

        // DELETE: api/DMQT_CTDQ/5
        [ResponseType(typeof(tbl_DMQT_CTDQ))]
        public async Task<IHttpActionResult> Deletetbl_DMQT_CTDQ(string id)
        {
            tbl_DMQT_CTDQ tbl_DMQT_CTDQ = await db.tbl_DMQT_CTDQ.FindAsync(id);
            if (tbl_DMQT_CTDQ == null)
            {
                return NotFound();
            }

            db.tbl_DMQT_CTDQ.Remove(tbl_DMQT_CTDQ);
            await db.SaveChangesAsync();

            return Ok(tbl_DMQT_CTDQ);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DMQT_CTDQExists(string id)
        {
            return db.tbl_DMQT_CTDQ.Count(e => e.DMCT_ID == id) > 0;
        }
    }
}