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
    public class ImageDonNhapHangController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/ImageDonNhapHang
        public IQueryable<Tbl_ImageDonNhapHang> GetTbl_ImageDonNhapHang()
        {
            return db.Tbl_ImageDonNhapHang;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_ImageDonNhapHang))]
        [Route("api/ImageDonNhapHang/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_ImageDonNhapHang> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var list = db.Tbl_DonNhapHang.Where(n => n.US_ID == user).Select(n => n.DNH_ID).ToList();
            return db.Tbl_ImageDonNhapHang.Where(n => (n.IDNH_DeleteFlag > dt || n.IDNH_SyncFlag > dt)&& list.Contains(n.DNH_ID));
        }
        // GET: api/ImageDonNhapHang/5
        [ResponseType(typeof(Tbl_ImageDonNhapHang))]
        public async Task<IHttpActionResult> GetTbl_ImageDonNhapHang(string id)
        {
            Tbl_ImageDonNhapHang tbl_ImageDonNhapHang = await db.Tbl_ImageDonNhapHang.FindAsync(id);
            if (tbl_ImageDonNhapHang == null)
            {
                return NotFound();
            }

            return Ok(tbl_ImageDonNhapHang);
        }

        // PUT: api/ImageDonNhapHang/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_ImageDonNhapHang(string id, Tbl_ImageDonNhapHang tbl_ImageDonNhapHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_ImageDonNhapHang.IDNH_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_ImageDonNhapHang).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_ImageDonNhapHangExists(id))
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

        // POST: api/ImageDonNhapHang
        [ResponseType(typeof(Tbl_ImageDonNhapHang))]
        public async Task<IHttpActionResult> PostTbl_ImageDonNhapHang(Tbl_ImageDonNhapHang tbl_ImageDonNhapHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_ImageDonNhapHang.Add(tbl_ImageDonNhapHang);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_ImageDonNhapHangExists(tbl_ImageDonNhapHang.IDNH_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_ImageDonNhapHang.IDNH_ID }, tbl_ImageDonNhapHang);
        }
        [HttpPost]
        [ResponseType(typeof(Tbl_ImageDonNhapHang))]
        [Route("api/ImageDonNhapHang/SyncServer")]
        public IHttpActionResult SyncServer(List<Tbl_ImageDonNhapHang> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (Tbl_ImageDonNhapHangExists(item.IDNH_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.Tbl_ImageDonNhapHang.Add(item);
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
        // DELETE: api/ImageDonNhapHang/5
        [ResponseType(typeof(Tbl_ImageDonNhapHang))]
        public async Task<IHttpActionResult> DeleteTbl_ImageDonNhapHang(string id)
        {
            Tbl_ImageDonNhapHang tbl_ImageDonNhapHang = await db.Tbl_ImageDonNhapHang.FindAsync(id);
            if (tbl_ImageDonNhapHang == null)
            {
                return NotFound();
            }

            db.Tbl_ImageDonNhapHang.Remove(tbl_ImageDonNhapHang);
            await db.SaveChangesAsync();

            return Ok(tbl_ImageDonNhapHang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_ImageDonNhapHangExists(string id)
        {
            return db.Tbl_ImageDonNhapHang.Count(e => e.IDNH_ID == id) > 0;
        }
    }
}