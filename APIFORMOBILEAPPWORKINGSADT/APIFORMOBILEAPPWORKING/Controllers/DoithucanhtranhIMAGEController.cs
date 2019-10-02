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
    public class DoithucanhtranhIMAGEController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DoithucanhtranhIMAGE
        public IQueryable<Tbl_DoithucanhtranhIMAGE> GetTbl_DoithucanhtranhIMAGE()
        {
            return db.Tbl_DoithucanhtranhIMAGE;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGE))]
        [Route("api/DoithucanhtranhIMAGE/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_DoithucanhtranhIMAGE> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_DoithucanhtranhIMAGE.Where(n =>( n.DTCTIMAGE_DeleteFlag > dt || n.DTCTIMAGE_SyncFlag > dt)&&n.US_ID==user);
        }
        // GET: api/DoithucanhtranhIMAGE/5
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGE))]
        public async Task<IHttpActionResult> GetTbl_DoithucanhtranhIMAGE(string id)
        {
            Tbl_DoithucanhtranhIMAGE tbl_DoithucanhtranhIMAGE = await db.Tbl_DoithucanhtranhIMAGE.FindAsync(id);
            if (tbl_DoithucanhtranhIMAGE == null)
            {
                return NotFound();
            }

            return Ok(tbl_DoithucanhtranhIMAGE);
        }

        // PUT: api/DoithucanhtranhIMAGE/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_DoithucanhtranhIMAGE(string id, Tbl_DoithucanhtranhIMAGE tbl_DoithucanhtranhIMAGE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DoithucanhtranhIMAGE.DTCTIMAGE_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_DoithucanhtranhIMAGE).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_DoithucanhtranhIMAGEExists(id))
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

        // POST: api/DoithucanhtranhIMAGE
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGE))]
        public async Task<IHttpActionResult> PostTbl_DoithucanhtranhIMAGE(Tbl_DoithucanhtranhIMAGE tbl_DoithucanhtranhIMAGE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_DoithucanhtranhIMAGE.Add(tbl_DoithucanhtranhIMAGE);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_DoithucanhtranhIMAGEExists(tbl_DoithucanhtranhIMAGE.DTCTIMAGE_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DoithucanhtranhIMAGE.DTCTIMAGE_ID }, tbl_DoithucanhtranhIMAGE);
        }
        [HttpPost]
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGE))]
        [Route("api/DoithucanhtranhIMAGE/SyncServer")]
        public IHttpActionResult SyncServer(List<Tbl_DoithucanhtranhIMAGE> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (Tbl_DoithucanhtranhIMAGEExists(item.DTCTIMAGE_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.Tbl_DoithucanhtranhIMAGE.Add(item);
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
        // DELETE: api/DoithucanhtranhIMAGE/5
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGE))]
        public async Task<IHttpActionResult> DeleteTbl_DoithucanhtranhIMAGE(string id)
        {
            Tbl_DoithucanhtranhIMAGE tbl_DoithucanhtranhIMAGE = await db.Tbl_DoithucanhtranhIMAGE.FindAsync(id);
            if (tbl_DoithucanhtranhIMAGE == null)
            {
                return NotFound();
            }

            db.Tbl_DoithucanhtranhIMAGE.Remove(tbl_DoithucanhtranhIMAGE);
            await db.SaveChangesAsync();

            return Ok(tbl_DoithucanhtranhIMAGE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_DoithucanhtranhIMAGEExists(string id)
        {
            return db.Tbl_DoithucanhtranhIMAGE.Count(e => e.DTCTIMAGE_ID == id) > 0;
        }
    }
}