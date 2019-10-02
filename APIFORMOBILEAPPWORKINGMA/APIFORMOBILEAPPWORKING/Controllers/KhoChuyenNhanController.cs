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
    public class KhoChuyenNhanController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/KhoChuyenNhan
        public IQueryable<Tbl_KhoChuyenNhan> GetTbl_KhoChuyenNhan()
        {
            return db.Tbl_KhoChuyenNhan;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_KhoChuyenNhan))]
        [Route("api/KhoChuyenNhan/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_KhoChuyenNhan> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_KhoChuyenNhan.Where(n => n.CNK_DeleteFlag > dt || n.CNK_SyncFlag > dt);
        }
        // GET: api/KhoChuyenNhan/5
        [ResponseType(typeof(Tbl_KhoChuyenNhan))]
        public async Task<IHttpActionResult> GetTbl_KhoChuyenNhan(string id)
        {
            Tbl_KhoChuyenNhan tbl_KhoChuyenNhan = await db.Tbl_KhoChuyenNhan.FindAsync(id);
            if (tbl_KhoChuyenNhan == null)
            {
                return NotFound();
            }

            return Ok(tbl_KhoChuyenNhan);
        }

        // PUT: api/KhoChuyenNhan/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_KhoChuyenNhan(string id, Tbl_KhoChuyenNhan tbl_KhoChuyenNhan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_KhoChuyenNhan.CNK_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_KhoChuyenNhan).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_KhoChuyenNhanExists(id))
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

        // POST: api/KhoChuyenNhan
        [ResponseType(typeof(Tbl_KhoChuyenNhan))]
        public async Task<IHttpActionResult> PostTbl_KhoChuyenNhan(Tbl_KhoChuyenNhan tbl_KhoChuyenNhan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_KhoChuyenNhan.Add(tbl_KhoChuyenNhan);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_KhoChuyenNhanExists(tbl_KhoChuyenNhan.CNK_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_KhoChuyenNhan.CNK_ID }, tbl_KhoChuyenNhan);
        }

        // DELETE: api/KhoChuyenNhan/5
        [ResponseType(typeof(Tbl_KhoChuyenNhan))]
        public async Task<IHttpActionResult> DeleteTbl_KhoChuyenNhan(string id)
        {
            Tbl_KhoChuyenNhan tbl_KhoChuyenNhan = await db.Tbl_KhoChuyenNhan.FindAsync(id);
            if (tbl_KhoChuyenNhan == null)
            {
                return NotFound();
            }

            db.Tbl_KhoChuyenNhan.Remove(tbl_KhoChuyenNhan);
            await db.SaveChangesAsync();

            return Ok(tbl_KhoChuyenNhan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_KhoChuyenNhanExists(string id)
        {
            return db.Tbl_KhoChuyenNhan.Count(e => e.CNK_ID == id) > 0;
        }
    }
}