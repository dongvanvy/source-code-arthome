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
    public class KhoDoiController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/KhoDoi
        public IQueryable<Tbl_KhoDoi> GetTbl_KhoDoi()
        {
            return db.Tbl_KhoDoi;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_KhoDoi))]
        [Route("api/KhoDoi/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_KhoDoi> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_KhoDoi.Where(n => n.KD_DeleteFlag > dt || n.KD_SyncFlag > dt && n.KD_UserQL.Contains(user));
        }
        // GET: api/KhoDoi/5
        [ResponseType(typeof(Tbl_KhoDoi))]
        public async Task<IHttpActionResult> GetTbl_KhoDoi(string id)
        {
            Tbl_KhoDoi tbl_KhoDoi = await db.Tbl_KhoDoi.FindAsync(id);
            if (tbl_KhoDoi == null)
            {
                return NotFound();
            }

            return Ok(tbl_KhoDoi);
        }

        // PUT: api/KhoDoi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_KhoDoi(string id, Tbl_KhoDoi tbl_KhoDoi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_KhoDoi.KD_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_KhoDoi).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_KhoDoiExists(id))
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

        // POST: api/KhoDoi
        [ResponseType(typeof(Tbl_KhoDoi))]
        public async Task<IHttpActionResult> PostTbl_KhoDoi(Tbl_KhoDoi tbl_KhoDoi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_KhoDoi.Add(tbl_KhoDoi);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_KhoDoiExists(tbl_KhoDoi.KD_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_KhoDoi.KD_ID }, tbl_KhoDoi);
        }

        // DELETE: api/KhoDoi/5
        [ResponseType(typeof(Tbl_KhoDoi))]
        public async Task<IHttpActionResult> DeleteTbl_KhoDoi(string id)
        {
            Tbl_KhoDoi tbl_KhoDoi = await db.Tbl_KhoDoi.FindAsync(id);
            if (tbl_KhoDoi == null)
            {
                return NotFound();
            }

            db.Tbl_KhoDoi.Remove(tbl_KhoDoi);
            await db.SaveChangesAsync();

            return Ok(tbl_KhoDoi);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_KhoDoiExists(string id)
        {
            return db.Tbl_KhoDoi.Count(e => e.KD_ID == id) > 0;
        }
    }
}