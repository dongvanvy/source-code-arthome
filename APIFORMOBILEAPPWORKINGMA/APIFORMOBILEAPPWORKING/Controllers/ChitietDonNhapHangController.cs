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
    public class ChitietDonNhapHangController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/ChitietDonNhapHang
        public IQueryable<Tbl_ChitietDonNhapHang> GetTbl_ChitietDonNhapHang()
        {
            return db.Tbl_ChitietDonNhapHang;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_ChitietDonNhapHang))]
        [Route("api/ChitietDonNhapHang/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_ChitietDonNhapHang> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var listdonnh = db.Tbl_DonNhapHang.Where(n => n.US_ID == user).Select(n=>n.DNH_ID).ToList();
            return db.Tbl_ChitietDonNhapHang.Where(n => (n.CTDNH_DeleteFlag > dt || n.CTDNH_SyncFlag > dt)&& listdonnh.Contains(n.DNH_ID));
        }
        // GET: api/ChitietDonNhapHang/5
        [ResponseType(typeof(Tbl_ChitietDonNhapHang))]
        public async Task<IHttpActionResult> GetTbl_ChitietDonNhapHang(string id)
        {
            Tbl_ChitietDonNhapHang tbl_ChitietDonNhapHang = await db.Tbl_ChitietDonNhapHang.FindAsync(id);
            if (tbl_ChitietDonNhapHang == null)
            {
                return NotFound();
            }

            return Ok(tbl_ChitietDonNhapHang);
        }

        // PUT: api/ChitietDonNhapHang/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_ChitietDonNhapHang(string id, Tbl_ChitietDonNhapHang tbl_ChitietDonNhapHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_ChitietDonNhapHang.CTDNH_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_ChitietDonNhapHang).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_ChitietDonNhapHangExists(id))
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

        // POST: api/ChitietDonNhapHang
        [ResponseType(typeof(Tbl_ChitietDonNhapHang))]
        public async Task<IHttpActionResult> PostTbl_ChitietDonNhapHang(Tbl_ChitietDonNhapHang tbl_ChitietDonNhapHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_ChitietDonNhapHang.Add(tbl_ChitietDonNhapHang);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_ChitietDonNhapHangExists(tbl_ChitietDonNhapHang.CTDNH_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_ChitietDonNhapHang.CTDNH_ID }, tbl_ChitietDonNhapHang);
        }
        [HttpPost]
        [ResponseType(typeof(Tbl_ChitietDonNhapHang))]
        [Route("api/ChitietDonNhapHang/SyncServer")]
        public IHttpActionResult SyncServer(List<Tbl_ChitietDonNhapHang> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (Tbl_ChitietDonNhapHangExists(item.CTDNH_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.Tbl_ChitietDonNhapHang.Add(item);
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
        // DELETE: api/ChitietDonNhapHang/5
        [ResponseType(typeof(Tbl_ChitietDonNhapHang))]
        public async Task<IHttpActionResult> DeleteTbl_ChitietDonNhapHang(string id)
        {
            Tbl_ChitietDonNhapHang tbl_ChitietDonNhapHang = await db.Tbl_ChitietDonNhapHang.FindAsync(id);
            if (tbl_ChitietDonNhapHang == null)
            {
                return NotFound();
            }

            db.Tbl_ChitietDonNhapHang.Remove(tbl_ChitietDonNhapHang);
            await db.SaveChangesAsync();

            return Ok(tbl_ChitietDonNhapHang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_ChitietDonNhapHangExists(string id)
        {
            return db.Tbl_ChitietDonNhapHang.Count(e => e.CTDNH_ID == id) > 0;
        }
    }
}