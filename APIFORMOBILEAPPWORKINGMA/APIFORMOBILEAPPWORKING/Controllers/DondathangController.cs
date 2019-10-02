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
    public class DondathangController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/Dondathang
        public IQueryable<tbl_Dondathang> Gettbl_Dondathang()
        {
            return db.tbl_Dondathang;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_Dondathang))]
        [Route("api/Dondathang/SyncClient/{date}/{user}")]
        public IQueryable<tbl_Dondathang> Gettbl_Dondathang(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_Dondathang.Where(n => (n.DDH_Syncflag > dt || n.DDH_DeleteFlag > dt) && n.Us_id == user);
        }
        // GET: api/Dondathang/5
        [ResponseType(typeof(tbl_Dondathang))]
        public IHttpActionResult Gettbl_Dondathang(string id)
        {
            tbl_Dondathang tbl_Dondathang = db.tbl_Dondathang.Find(id);
            if (tbl_Dondathang == null)
            {
                return NotFound();
            }

            return Ok(tbl_Dondathang);
        }

        // PUT: api/Dondathang/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Dondathang(string id, tbl_Dondathang tbl_Dondathang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Dondathang.DDH_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_Dondathang).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DondathangExists(id))
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

        // POST: api/Dondathang
        [ResponseType(typeof(tbl_Dondathang))]
        public IHttpActionResult Posttbl_Dondathang(tbl_Dondathang tbl_Dondathang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Dondathang.Add(tbl_Dondathang);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_DondathangExists(tbl_Dondathang.DDH_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_Dondathang.DDH_ID }, tbl_Dondathang);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_Dondathang))]
        [Route("api/Dondathang/SyncServer")]
        public IHttpActionResult Posttbl_Dondathang(List<tbl_Dondathang> tbl_Dondathang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_Dondathang)
            {
                if (tbl_DondathangExists(item.DDH_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_Dondathang.Add(item);
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

            return Ok(tbl_Dondathang);
        }
        // DELETE: api/Dondathang/5
        [ResponseType(typeof(tbl_Dondathang))]
        public IHttpActionResult Deletetbl_Dondathang(string id)
        {
            tbl_Dondathang tbl_Dondathang = db.tbl_Dondathang.Find(id);
            if (tbl_Dondathang == null)
            {
                return NotFound();
            }

            db.tbl_Dondathang.Remove(tbl_Dondathang);
            db.SaveChanges();

            return Ok(tbl_Dondathang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DondathangExists(string id)
        {
            return db.tbl_Dondathang.Count(e => e.DDH_ID == id) > 0;
        }
    }
}