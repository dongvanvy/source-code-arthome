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
    public class DanhMucQuaTangController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/DanhMucQuaTang
        public IQueryable<tbl_DanhMucQuaTang> Gettbl_DanhMucQuaTang()
        {
            return db.tbl_DanhMucQuaTang;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DanhMucQuaTang))]
        [Route("api/DanhMucQuaTang/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DanhMucQuaTang> SyncClient_DanhMucQuaTang(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_DanhMucQuaTang.Where(n => n.DMQT_DeleteFlag > dt || n.DMQT_SyncFlag > dt);
        }
        // GET: api/DanhMucQuaTang/5
        [ResponseType(typeof(tbl_DanhMucQuaTang))]
        public IHttpActionResult Gettbl_DanhMucQuaTang(double id)
        {
            tbl_DanhMucQuaTang tbl_DanhMucQuaTang = db.tbl_DanhMucQuaTang.Find(id);
            if (tbl_DanhMucQuaTang == null)
            {
                return NotFound();
            }

            return Ok(tbl_DanhMucQuaTang);
        }

        // PUT: api/DanhMucQuaTang/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_DanhMucQuaTang(double id, tbl_DanhMucQuaTang tbl_DanhMucQuaTang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DanhMucQuaTang.DMQT_Barcode)
            {
                return BadRequest();
            }

            db.Entry(tbl_DanhMucQuaTang).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DanhMucQuaTangExists(id))
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

        // POST: api/DanhMucQuaTang
        [ResponseType(typeof(tbl_DanhMucQuaTang))]
        public IHttpActionResult Posttbl_DanhMucQuaTang(tbl_DanhMucQuaTang tbl_DanhMucQuaTang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DanhMucQuaTang.Add(tbl_DanhMucQuaTang);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_DanhMucQuaTangExists(tbl_DanhMucQuaTang.DMQT_Barcode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DanhMucQuaTang.DMQT_Barcode }, tbl_DanhMucQuaTang);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DanhMucQuaTang))]
        [Route("api/DanhMucQuaTang/SyncServer")]
        public IHttpActionResult SyncServer_DanhMucQuaTang(List<tbl_DanhMucQuaTang> tbl_DanhMucQuaTang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_DanhMucQuaTang)
            {
                if (tbl_DanhMucQuaTangExists(item.DMQT_Barcode))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DanhMucQuaTang.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }
            return Ok(tbl_DanhMucQuaTang);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_DanhMucQuaTang.DMQT_Barcode }, tbl_DanhMucQuaTang);
        }
        // DELETE: api/DanhMucQuaTang/5
        [ResponseType(typeof(tbl_DanhMucQuaTang))]
        public IHttpActionResult Deletetbl_DanhMucQuaTang(double id)
        {
            tbl_DanhMucQuaTang tbl_DanhMucQuaTang = db.tbl_DanhMucQuaTang.Find(id);
            if (tbl_DanhMucQuaTang == null)
            {
                return NotFound();
            }

            db.tbl_DanhMucQuaTang.Remove(tbl_DanhMucQuaTang);
            db.SaveChanges();

            return Ok(tbl_DanhMucQuaTang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DanhMucQuaTangExists(double id)
        {
            return db.tbl_DanhMucQuaTang.Count(e => e.DMQT_Barcode == id) > 0;
        }
    }
}