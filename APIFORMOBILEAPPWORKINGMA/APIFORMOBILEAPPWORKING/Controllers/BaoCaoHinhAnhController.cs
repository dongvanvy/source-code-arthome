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
    public class BaoCaoHinhAnhController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/BaoCaoHinhAnh
        public IQueryable<tbl_BaoCaoHinhAnh> Gettbl_BaoCaoHinhAnh()
        {
            return db.tbl_BaoCaoHinhAnh;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_BaoCaoHinhAnh))]
        [Route("api/BaoCaoHinhAnh/SyncClient/{date}/{user}")]
        public IQueryable<tbl_BaoCaoHinhAnh> SyncClienttbl_BaoCaoHinhAnh(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_BaoCaoHinhAnh.Where(n => (n.BCHA_SyncFlag > dt || n.BCHA_DeleteFlag > dt) && n.Us_id == user);
        }
        // GET: api/BaoCaoHinhAnh/5
        [ResponseType(typeof(tbl_BaoCaoHinhAnh))]
        public IHttpActionResult Gettbl_BaoCaoHinhAnh(string id)
        {
            tbl_BaoCaoHinhAnh tbl_BaoCaoHinhAnh = db.tbl_BaoCaoHinhAnh.Find(id);
            if (tbl_BaoCaoHinhAnh == null)
            {
                return NotFound();
            }

            return Ok(tbl_BaoCaoHinhAnh);
        }

        // PUT: api/BaoCaoHinhAnh/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_BaoCaoHinhAnh(string id, tbl_BaoCaoHinhAnh tbl_BaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_BaoCaoHinhAnh.BCHA_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_BaoCaoHinhAnh).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_BaoCaoHinhAnhExists(id))
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

        // POST: api/BaoCaoHinhAnh
        [ResponseType(typeof(tbl_BaoCaoHinhAnh))]
        public IHttpActionResult Posttbl_BaoCaoHinhAnh(tbl_BaoCaoHinhAnh tbl_BaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_BaoCaoHinhAnh.Add(tbl_BaoCaoHinhAnh);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_BaoCaoHinhAnhExists(tbl_BaoCaoHinhAnh.BCHA_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_BaoCaoHinhAnh.BCHA_ID }, tbl_BaoCaoHinhAnh);
        }
        [HttpPost]
        [Route("api/BaoCaoHinhAnh/SyncServer")]
        [ResponseType(typeof(tbl_BaoCaoHinhAnh))]
        public IHttpActionResult SyncServertbl_BaoCaoHinhAnh(List<tbl_BaoCaoHinhAnh> tbl_BaoCaoHinhAnh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_BaoCaoHinhAnh)
            {
                if (tbl_BaoCaoHinhAnhExists(item.BCHA_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_BaoCaoHinhAnh.Add(item);
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }
            return Ok(tbl_BaoCaoHinhAnh);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_BaoCaoHinhAnh.BCHA_ID }, tbl_BaoCaoHinhAnh);
        }
        // DELETE: api/BaoCaoHinhAnh/5
        [ResponseType(typeof(tbl_BaoCaoHinhAnh))]
        public IHttpActionResult Deletetbl_BaoCaoHinhAnh(string id)
        {
            tbl_BaoCaoHinhAnh tbl_BaoCaoHinhAnh = db.tbl_BaoCaoHinhAnh.Find(id);
            if (tbl_BaoCaoHinhAnh == null)
            {
                return NotFound();
            }

            db.tbl_BaoCaoHinhAnh.Remove(tbl_BaoCaoHinhAnh);
            db.SaveChanges();

            return Ok(tbl_BaoCaoHinhAnh);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_BaoCaoHinhAnhExists(string id)
        {
            return db.tbl_BaoCaoHinhAnh.Count(e => e.BCHA_ID == id) > 0;
        }
    }
}