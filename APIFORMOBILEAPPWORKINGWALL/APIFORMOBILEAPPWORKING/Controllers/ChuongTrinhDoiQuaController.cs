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
    public class ChuongTrinhDoiQuaController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/ChuongTrinhDoiQua
        public IQueryable<tbl_ChuongTrinhDoiQua> Gettbl_ChuongTrinhDoiQua()
        {
            return db.tbl_ChuongTrinhDoiQua;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        [Route("api/ChuongTrinhDoiQua/SyncClient/{date}/{user}")]
        public IQueryable<tbl_ChuongTrinhDoiQua> SyncClient_ChuongTrinhDoiQua(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_ChuongTrinhDoiQua.Where(n => (n.CTDQ_DeleteFlag > dt || n.CTDQ_SyncFlag > dt));
        }
        // GET: api/ChuongTrinhDoiQua/5
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        public IHttpActionResult Gettbl_ChuongTrinhDoiQua(string id)
        {
            tbl_ChuongTrinhDoiQua tbl_ChuongTrinhDoiQua = db.tbl_ChuongTrinhDoiQua.Find(id);
            if (tbl_ChuongTrinhDoiQua == null)
            {
                return NotFound();
            }

            return Ok(tbl_ChuongTrinhDoiQua);
        }

        // PUT: api/ChuongTrinhDoiQua/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_ChuongTrinhDoiQua(string id, tbl_ChuongTrinhDoiQua tbl_ChuongTrinhDoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_ChuongTrinhDoiQua.CTDQ_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_ChuongTrinhDoiQua).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_ChuongTrinhDoiQuaExists(id))
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

        // POST: api/ChuongTrinhDoiQua
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        public IHttpActionResult Posttbl_ChuongTrinhDoiQua(tbl_ChuongTrinhDoiQua tbl_ChuongTrinhDoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_ChuongTrinhDoiQua.Add(tbl_ChuongTrinhDoiQua);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_ChuongTrinhDoiQuaExists(tbl_ChuongTrinhDoiQua.CTDQ_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_ChuongTrinhDoiQua.CTDQ_ID }, tbl_ChuongTrinhDoiQua);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        [Route("api/ChuongTrinhDoiQua/SyncServer")]
        public IHttpActionResult SyncServer_ChuongTrinhDoiQua(List<tbl_ChuongTrinhDoiQua> tbl_ChuongTrinhDoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_ChuongTrinhDoiQua)
            {
                if (tbl_ChuongTrinhDoiQuaExists(item.CTDQ_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_ChuongTrinhDoiQua.Add(item);
                }
            }
            db.SaveChanges();
            return Ok(tbl_ChuongTrinhDoiQua);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_ChuongTrinhDoiQua.CTDQ_ID }, tbl_ChuongTrinhDoiQua);
        }
        // DELETE: api/ChuongTrinhDoiQua/5
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        public IHttpActionResult Deletetbl_ChuongTrinhDoiQua(string id)
        {
            tbl_ChuongTrinhDoiQua tbl_ChuongTrinhDoiQua = db.tbl_ChuongTrinhDoiQua.Find(id);
            if (tbl_ChuongTrinhDoiQua == null)
            {
                return NotFound();
            }

            db.tbl_ChuongTrinhDoiQua.Remove(tbl_ChuongTrinhDoiQua);
            db.SaveChanges();

            return Ok(tbl_ChuongTrinhDoiQua);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_ChuongTrinhDoiQuaExists(string id)
        {
            return db.tbl_ChuongTrinhDoiQua.Count(e => e.CTDQ_ID == id) > 0;
        }
    }
}