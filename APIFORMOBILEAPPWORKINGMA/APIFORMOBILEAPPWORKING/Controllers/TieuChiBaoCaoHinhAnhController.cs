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
    public class TieuChiBaoCaoHinhAnhController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/TieuChiBaoCaoHinhAnh
        public IQueryable<tbl_TieuChiBaoCaoHinhAnh> Gettbl_TieuChiBaoCaoHinhAnh()
        {
            return db.tbl_TieuChiBaoCaoHinhAnh;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_TieuChiBaoCaoHinhAnh))]
        [Route("api/TieuChiBaoCaoHinhAnh/SyncClient/{date}/{user}")]
        public IQueryable<tbl_TieuChiBaoCaoHinhAnh> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_TieuChiBaoCaoHinhAnh.Where(n => n.TCBCHA_SyncFlag > dt || n.TCBCHA_DeleteFlag > dt);
        }
        // GET: api/TieuChiBaoCaoHinhAnh/5
        [ResponseType(typeof(tbl_TieuChiBaoCaoHinhAnh))]
        public IHttpActionResult Gettbl_TieuChiBaoCaoHinhAnh(string id)
        {
            tbl_TieuChiBaoCaoHinhAnh tbl_TieuChiBaoCaoHinhAnh = db.tbl_TieuChiBaoCaoHinhAnh.Find(id);
            if (tbl_TieuChiBaoCaoHinhAnh == null)
            {
                return NotFound();
            }

            return Ok(tbl_TieuChiBaoCaoHinhAnh);
        }

        // PUT: api/TieuChiBaoCaoHinhAnh/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_TieuChiBaoCaoHinhAnh(string id, tbl_TieuChiBaoCaoHinhAnh tbl_TieuChiBaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_TieuChiBaoCaoHinhAnh.TCBCHA_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_TieuChiBaoCaoHinhAnh).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_TieuChiBaoCaoHinhAnhExists(id))
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

        // POST: api/TieuChiBaoCaoHinhAnh
        [ResponseType(typeof(tbl_TieuChiBaoCaoHinhAnh))]
        public IHttpActionResult Posttbl_TieuChiBaoCaoHinhAnh(tbl_TieuChiBaoCaoHinhAnh tbl_TieuChiBaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_TieuChiBaoCaoHinhAnh.Add(tbl_TieuChiBaoCaoHinhAnh);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_TieuChiBaoCaoHinhAnhExists(tbl_TieuChiBaoCaoHinhAnh.TCBCHA_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_TieuChiBaoCaoHinhAnh.TCBCHA_ID }, tbl_TieuChiBaoCaoHinhAnh);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_TieuChiBaoCaoHinhAnh))]
        [Route("api/TieuChiBaoCaoHinhAnh/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_TieuChiBaoCaoHinhAnh> tbl_TieuChiBaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_TieuChiBaoCaoHinhAnh)
            {
                if (tbl_TieuChiBaoCaoHinhAnhExists(item.TCBCHA_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_TieuChiBaoCaoHinhAnh.Add(item);
                }
            }
            db.SaveChanges();

            return Ok(tbl_TieuChiBaoCaoHinhAnh);
        }
        // DELETE: api/TieuChiBaoCaoHinhAnh/5
        [ResponseType(typeof(tbl_TieuChiBaoCaoHinhAnh))]
        public IHttpActionResult Deletetbl_TieuChiBaoCaoHinhAnh(string id)
        {
            tbl_TieuChiBaoCaoHinhAnh tbl_TieuChiBaoCaoHinhAnh = db.tbl_TieuChiBaoCaoHinhAnh.Find(id);
            if (tbl_TieuChiBaoCaoHinhAnh == null)
            {
                return NotFound();
            }

            db.tbl_TieuChiBaoCaoHinhAnh.Remove(tbl_TieuChiBaoCaoHinhAnh);
            db.SaveChanges();

            return Ok(tbl_TieuChiBaoCaoHinhAnh);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_TieuChiBaoCaoHinhAnhExists(string id)
        {
            return db.tbl_TieuChiBaoCaoHinhAnh.Count(e => e.TCBCHA_ID == id) > 0;
        }
    }
}