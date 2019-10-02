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
    public class SupChamDiemController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/SupChamDiem
        public IQueryable<tbl_SupChamDiem> Gettbl_SupChamDiem()
        {
            return db.tbl_SupChamDiem;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        [Route("api/SupChamDiem/SyncClient/{date}/{user}")]
        public IQueryable<tbl_SupChamDiem> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var lst = db.tbl_SupBigPlan.Where(n => n.Us_id == user).Select(n => n.SupBigPlan_id).ToList();
            return db.tbl_SupChamDiem.Where(n => lst.Contains(n.SupBigPlan_id) && (n.SupChamDiem_DeleteFlag > dt || n.SupChamDiem_SyncFlag > dt));
        }
        // GET: api/SupChamDiem/5
        [ResponseType(typeof(tbl_SupChamDiem))]
        public IHttpActionResult Gettbl_SupChamDiem(string id)
        {
            tbl_SupChamDiem tbl_SupChamDiem = db.tbl_SupChamDiem.Find(id);
            if (tbl_SupChamDiem == null)
            {
                return NotFound();
            }

            return Ok(tbl_SupChamDiem);
        }

        // PUT: api/SupChamDiem/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_SupChamDiem(string id, tbl_SupChamDiem tbl_SupChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_SupChamDiem.SupChamDiem_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_SupChamDiem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_SupChamDiemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/SupChamDiem
        [ResponseType(typeof(tbl_SupChamDiem))]
        public IHttpActionResult Posttbl_SupChamDiem(tbl_SupChamDiem tbl_SupChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_SupChamDiem.Add(tbl_SupChamDiem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_SupChamDiemExists(tbl_SupChamDiem.SupChamDiem_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_SupChamDiem.SupChamDiem_id }, tbl_SupChamDiem);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_ChuongTrinhDoiQua))]
        [Route("api/SupChamDiem/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_SupChamDiem> tbl_SupChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_SupChamDiem)
            {
                if (tbl_SupChamDiemExists(item.SupChamDiem_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_SupChamDiem.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Ok(tbl_SupChamDiem);
        }
        // DELETE: api/SupChamDiem/5
        [ResponseType(typeof(tbl_SupChamDiem))]
        public IHttpActionResult Deletetbl_SupChamDiem(string id)
        {
            tbl_SupChamDiem tbl_SupChamDiem = db.tbl_SupChamDiem.Find(id);
            if (tbl_SupChamDiem == null)
            {
                return NotFound();
            }

            db.tbl_SupChamDiem.Remove(tbl_SupChamDiem);
            db.SaveChanges();

            return Ok(tbl_SupChamDiem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_SupChamDiemExists(string id)
        {
            return db.tbl_SupChamDiem.Count(e => e.SupChamDiem_id == id) > 0;
        }
    }
}