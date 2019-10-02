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
    public class KhoChiNhanhController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/KhoChiNhanh
        public IQueryable<Tbl_KhoChiNhanh> GetTbl_KhoChiNhanh()
        {
            return db.Tbl_KhoChiNhanh;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_KhoChiNhanh))]
        [Route("api/KhoChiNhanh/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_KhoChiNhanh> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_KhoChiNhanh.Where(n => n.KCN_DeleteFlag > dt || n.KCN_SyncFlag > dt && n.KCN_UserQL.Contains(user));
        }
        // GET: api/KhoChiNhanh/5
        [ResponseType(typeof(Tbl_KhoChiNhanh))]
        public async Task<IHttpActionResult> GetTbl_KhoChiNhanh(string id)
        {
            Tbl_KhoChiNhanh tbl_KhoChiNhanh = await db.Tbl_KhoChiNhanh.FindAsync(id);
            if (tbl_KhoChiNhanh == null)
            {
                return NotFound();
            }

            return Ok(tbl_KhoChiNhanh);
        }

        // PUT: api/KhoChiNhanh/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_KhoChiNhanh(string id, Tbl_KhoChiNhanh tbl_KhoChiNhanh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_KhoChiNhanh.KCN_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_KhoChiNhanh).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_KhoChiNhanhExists(id))
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

        // POST: api/KhoChiNhanh
        [ResponseType(typeof(Tbl_KhoChiNhanh))]
        public async Task<IHttpActionResult> PostTbl_KhoChiNhanh(Tbl_KhoChiNhanh tbl_KhoChiNhanh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_KhoChiNhanh.Add(tbl_KhoChiNhanh);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_KhoChiNhanhExists(tbl_KhoChiNhanh.KCN_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_KhoChiNhanh.KCN_ID }, tbl_KhoChiNhanh);
        }

        // DELETE: api/KhoChiNhanh/5
        [ResponseType(typeof(Tbl_KhoChiNhanh))]
        public async Task<IHttpActionResult> DeleteTbl_KhoChiNhanh(string id)
        {
            Tbl_KhoChiNhanh tbl_KhoChiNhanh = await db.Tbl_KhoChiNhanh.FindAsync(id);
            if (tbl_KhoChiNhanh == null)
            {
                return NotFound();
            }

            db.Tbl_KhoChiNhanh.Remove(tbl_KhoChiNhanh);
            await db.SaveChangesAsync();

            return Ok(tbl_KhoChiNhanh);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_KhoChiNhanhExists(string id)
        {
            return db.Tbl_KhoChiNhanh.Count(e => e.KCN_ID == id) > 0;
        }
    }
}