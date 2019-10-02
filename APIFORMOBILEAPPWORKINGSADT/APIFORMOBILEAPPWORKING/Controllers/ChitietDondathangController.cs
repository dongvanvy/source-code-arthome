using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using APIFORMOBILEAPPWORKING.Models;

namespace APIFORMOBILEAPPWORKING.Controllers
{
    public class ChitietDondathangController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/ChitietDondathang
        public IQueryable<tbl_chitietDondathang> Gettbl_chitietDondathang()
        {
            return db.tbl_chitietDondathang;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_chitietDondathang))]
        [Route("api/chitietDondathang/SyncClient/{date}/{user}")]
        public IQueryable<tbl_chitietDondathang> Gettbl_chitietDondathang(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var ddh = db.tbl_Dondathang.Where(n => n.Us_id == user).Select(n => n.DDH_ID).ToList();
            return db.tbl_chitietDondathang.Where(n => (n.CTDDH_SyncFlag > dt || n.CTDDH_DeleteFlag > dt) && ddh.Contains(n.DDH_ID));
        }
        // GET: api/ChitietDondathang/5
        [ResponseType(typeof(tbl_chitietDondathang))]
        public IHttpActionResult Gettbl_chitietDondathang(string id)
        {
            tbl_chitietDondathang tbl_chitietDondathang = db.tbl_chitietDondathang.Find(id);
            if (tbl_chitietDondathang == null)
            {
                return NotFound();
            }

            return Ok(tbl_chitietDondathang);
        }

        // PUT: api/ChitietDondathang/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_chitietDondathang(string id, tbl_chitietDondathang tbl_chitietDondathang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_chitietDondathang.DDH_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_chitietDondathang).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_chitietDondathangExists(id))
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

        // POST: api/ChitietDondathang
        [ResponseType(typeof(tbl_chitietDondathang))]
        public IHttpActionResult Posttbl_chitietDondathang(tbl_chitietDondathang tbl_chitietDondathang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_chitietDondathang.Add(tbl_chitietDondathang);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_chitietDondathangExists(tbl_chitietDondathang.DDH_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_chitietDondathang.DDH_ID }, tbl_chitietDondathang);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_chitietDondathang))]
        [Route("api/chitietDondathang/SyncServer")]
        public IHttpActionResult Posttbl_chitietDondathang(List<tbl_chitietDondathang> tbl_chitietDondathang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_chitietDondathang)
            {
                if (tbl_chitietDondathangExistsSync(item.DDH_ID, item.sku_barcode))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_chitietDondathang.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok(tbl_chitietDondathang);
        }
        // DELETE: api/ChitietDondathang/5
        [ResponseType(typeof(tbl_chitietDondathang))]
        public IHttpActionResult Deletetbl_chitietDondathang(string id)
        {
            tbl_chitietDondathang tbl_chitietDondathang = db.tbl_chitietDondathang.Find(id);
            if (tbl_chitietDondathang == null)
            {
                return NotFound();
            }

            db.tbl_chitietDondathang.Remove(tbl_chitietDondathang);
            db.SaveChanges();

            return Ok(tbl_chitietDondathang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_chitietDondathangExists(string id)
        {
            return db.tbl_chitietDondathang.Count(e => e.DDH_ID == id) > 0;
        }
        private bool tbl_chitietDondathangExistsSync(string id, double skubarcode)
        {
            return db.tbl_chitietDondathang.Count(e => e.DDH_ID == id) > 0;
        }
    }
}