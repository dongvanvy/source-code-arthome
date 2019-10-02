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
    public class DetailBaoCaoHinhAnhController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DetailBaoCaoHinhAnh
        public IQueryable<tbl_DetailBaoCaoHinhAnh> Gettbl_DetailBaoCaoHinhAnh()
        {
            return db.tbl_DetailBaoCaoHinhAnh;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DetailBaoCaoHinhAnh))]
        [Route("api/DetailBaoCaoHinhAnh/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DetailBaoCaoHinhAnh> SyncClient_DetailBaoCaoHinhAnh(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var lst = db.tbl_BaoCaoHinhAnh.Where(n => n.Us_id == user).Select(n => n.BCHA_ID).ToList();
            return db.tbl_DetailBaoCaoHinhAnh.Where(n => lst.Contains(n.BCHA_ID) && (n.DBCHA_DeleteFlag > dt || n.DBCHA_SyncFlag > dt));
        }
        // GET: api/DetailBaoCaoHinhAnh/5
        [ResponseType(typeof(tbl_DetailBaoCaoHinhAnh))]
        public IHttpActionResult Gettbl_DetailBaoCaoHinhAnh(string id)
        {
            tbl_DetailBaoCaoHinhAnh tbl_DetailBaoCaoHinhAnh = db.tbl_DetailBaoCaoHinhAnh.Find(id);
            if (tbl_DetailBaoCaoHinhAnh == null)
            {
                return NotFound();
            }

            return Ok(tbl_DetailBaoCaoHinhAnh);
        }

        // PUT: api/DetailBaoCaoHinhAnh/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_DetailBaoCaoHinhAnh(string id, tbl_DetailBaoCaoHinhAnh tbl_DetailBaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DetailBaoCaoHinhAnh.BCHA_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_DetailBaoCaoHinhAnh).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DetailBaoCaoHinhAnhExists(id))
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

        // POST: api/DetailBaoCaoHinhAnh
        [ResponseType(typeof(tbl_DetailBaoCaoHinhAnh))]
        public IHttpActionResult Posttbl_DetailBaoCaoHinhAnh(tbl_DetailBaoCaoHinhAnh tbl_DetailBaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DetailBaoCaoHinhAnh.Add(tbl_DetailBaoCaoHinhAnh);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_DetailBaoCaoHinhAnhExists(tbl_DetailBaoCaoHinhAnh.BCHA_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DetailBaoCaoHinhAnh.BCHA_ID }, tbl_DetailBaoCaoHinhAnh);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DetailBaoCaoHinhAnh))]
        [Route("api/DetailBaoCaoHinhAnh/SyncServer")]
        public IHttpActionResult SyncServer_DetailBaoCaoHinhAnh(List<tbl_DetailBaoCaoHinhAnh> tbl_DetailBaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_DetailBaoCaoHinhAnh)
            {
                if (db.tbl_DetailBaoCaoHinhAnh.Count(n => n.BCHA_ID == item.BCHA_ID && n.TCBCHA_ID == item.TCBCHA_ID) > 0)
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DetailBaoCaoHinhAnh.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Ok(tbl_DetailBaoCaoHinhAnh);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DetailBaoCaoHinhAnh))]
        [Route("api/DetailBaoCaoHinhAnh/GetList")]
        public IHttpActionResult get_DetailBaoCaoHinhAnh(List<tbl_BaoCaoHinhAnh> tbl_BaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var listid = tbl_BaoCaoHinhAnh.Select(n => n.BCHA_ID).ToList();
            var check = db.tbl_DetailBaoCaoHinhAnh.Where(n => listid.Contains(n.BCHA_ID));
            return Ok(check);
        }
        // DELETE: api/DetailBaoCaoHinhAnh/5
        [ResponseType(typeof(tbl_DetailBaoCaoHinhAnh))]
        public IHttpActionResult Deletetbl_DetailBaoCaoHinhAnh(string id)
        {
            tbl_DetailBaoCaoHinhAnh tbl_DetailBaoCaoHinhAnh = db.tbl_DetailBaoCaoHinhAnh.Find(id);
            if (tbl_DetailBaoCaoHinhAnh == null)
            {
                return NotFound();
            }

            db.tbl_DetailBaoCaoHinhAnh.Remove(tbl_DetailBaoCaoHinhAnh);
            db.SaveChanges();

            return Ok(tbl_DetailBaoCaoHinhAnh);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DetailBaoCaoHinhAnhExists(string id)
        {
            return db.tbl_DetailBaoCaoHinhAnh.Count(e => e.BCHA_ID == id) > 0;
        }
    }
}