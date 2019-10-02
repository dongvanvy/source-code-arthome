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
    public class DonNhapHangController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DonNhapHang
        public IQueryable<Tbl_DonNhapHang> GetTbl_DonNhapHang()
        {
            return db.Tbl_DonNhapHang;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_DonNhapHang))]
        [Route("api/DonNhapHang/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_DonNhapHang> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_DonNhapHang.Where(n => (n.DNH_DeleteFlag > dt || n.DNH_SyncFlag > dt)&&n.US_ID==user);
        }
        // GET: api/DonNhapHang/5
        [ResponseType(typeof(Tbl_DonNhapHang))]
        public async Task<IHttpActionResult> GetTbl_DonNhapHang(string id)
        {
            Tbl_DonNhapHang tbl_DonNhapHang = await db.Tbl_DonNhapHang.FindAsync(id);
            if (tbl_DonNhapHang == null)
            {
                return NotFound();
            }

            return Ok(tbl_DonNhapHang);
        }

        // PUT: api/DonNhapHang/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_DonNhapHang(string id, Tbl_DonNhapHang tbl_DonNhapHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DonNhapHang.DNH_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_DonNhapHang).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_DonNhapHangExists(id))
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

        // POST: api/DonNhapHang
        [ResponseType(typeof(Tbl_DonNhapHang))]
        public async Task<IHttpActionResult> PostTbl_DonNhapHang(Tbl_DonNhapHang tbl_DonNhapHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_DonNhapHang.Add(tbl_DonNhapHang);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_DonNhapHangExists(tbl_DonNhapHang.DNH_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DonNhapHang.DNH_ID }, tbl_DonNhapHang);
        }
        [HttpPost]
        [ResponseType(typeof(Tbl_DonNhapHang))]
        [Route("api/DonNhapHang/SyncServer")]
        public IHttpActionResult SyncServer(List<Tbl_DonNhapHang> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (Tbl_DonNhapHangExists(item.DNH_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.Tbl_DonNhapHang.Add(item);
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
        // DELETE: api/DonNhapHang/5
        [ResponseType(typeof(Tbl_DonNhapHang))]
        public async Task<IHttpActionResult> DeleteTbl_DonNhapHang(string id)
        {
            Tbl_DonNhapHang tbl_DonNhapHang = await db.Tbl_DonNhapHang.FindAsync(id);
            if (tbl_DonNhapHang == null)
            {
                return NotFound();
            }

            db.Tbl_DonNhapHang.Remove(tbl_DonNhapHang);
            await db.SaveChangesAsync();

            return Ok(tbl_DonNhapHang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_DonNhapHangExists(string id)
        {
            return db.Tbl_DonNhapHang.Count(e => e.DNH_ID == id) > 0;
        }
    }
}