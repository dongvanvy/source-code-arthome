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
    public class TieuChiChamDiemController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/TieuChiChamDiem
        public IQueryable<tbl_TieuChiChamDiem> Gettbl_TieuChiChamDiem()
        {
            return db.tbl_TieuChiChamDiem;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        [Route("api/TieuChiChamDiem/SyncClient/{date}/{user}")]
        public IQueryable<tbl_TieuChiChamDiem> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_TieuChiChamDiem.Where(n => n.TieuChiCD_DeleteFlag > dt || n.TieuChiCD_SyncFlag > dt);
        }
        // GET: api/TieuChiChamDiem/5
        [ResponseType(typeof(tbl_TieuChiChamDiem))]
        public IHttpActionResult Gettbl_TieuChiChamDiem(string id)
        {
            tbl_TieuChiChamDiem tbl_TieuChiChamDiem = db.tbl_TieuChiChamDiem.Find(id);
            if (tbl_TieuChiChamDiem == null)
            {
                return NotFound();
            }

            return Ok(tbl_TieuChiChamDiem);
        }

        // PUT: api/TieuChiChamDiem/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_TieuChiChamDiem(string id, tbl_TieuChiChamDiem tbl_TieuChiChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_TieuChiChamDiem.TieuChiCD_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_TieuChiChamDiem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_TieuChiChamDiemExists(id))
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

        // POST: api/TieuChiChamDiem
        [ResponseType(typeof(tbl_TieuChiChamDiem))]
        public IHttpActionResult Posttbl_TieuChiChamDiem(tbl_TieuChiChamDiem tbl_TieuChiChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_TieuChiChamDiem.Add(tbl_TieuChiChamDiem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_TieuChiChamDiemExists(tbl_TieuChiChamDiem.TieuChiCD_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_TieuChiChamDiem.TieuChiCD_id }, tbl_TieuChiChamDiem);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        [Route("api/TieuChiChamDiem/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_TieuChiChamDiem> tbl_TieuChiChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_TieuChiChamDiem)
            {
                if (tbl_TieuChiChamDiemExists(item.TieuChiCD_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_TieuChiChamDiem.Add(item);
                }
            }
            db.SaveChanges();

            return Ok(tbl_TieuChiChamDiem);
        }
        // DELETE: api/TieuChiChamDiem/5
        [ResponseType(typeof(tbl_TieuChiChamDiem))]
        public IHttpActionResult Deletetbl_TieuChiChamDiem(string id)
        {
            tbl_TieuChiChamDiem tbl_TieuChiChamDiem = db.tbl_TieuChiChamDiem.Find(id);
            if (tbl_TieuChiChamDiem == null)
            {
                return NotFound();
            }

            db.tbl_TieuChiChamDiem.Remove(tbl_TieuChiChamDiem);
            db.SaveChanges();

            return Ok(tbl_TieuChiChamDiem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_TieuChiChamDiemExists(string id)
        {
            return db.tbl_TieuChiChamDiem.Count(e => e.TieuChiCD_id == id) > 0;
        }
    }
}