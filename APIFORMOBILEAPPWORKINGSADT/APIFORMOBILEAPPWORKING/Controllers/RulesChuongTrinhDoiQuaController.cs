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
    public class RulesChuongTrinhDoiQuaController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/RulesChuongTrinhDoiQua
        public IQueryable<tbl_RulesChuongTrinhDoiQua> Gettbl_RulesChuongTrinhDoiQua()
        {
            return db.tbl_RulesChuongTrinhDoiQua;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_RulesChuongTrinhDoiQua))]
        [Route("api/RulesChuongTrinhDoiQua/SyncClient/{date}/{user}")]
        public IQueryable<tbl_RulesChuongTrinhDoiQua> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_RulesChuongTrinhDoiQua.Where(n => n.RCTDQ_SyncFlag > dt || n.RCTDQ_Delete > dt);
        }
        // GET: api/RulesChuongTrinhDoiQua/5
        [ResponseType(typeof(tbl_RulesChuongTrinhDoiQua))]
        public IHttpActionResult Gettbl_RulesChuongTrinhDoiQua(string id)
        {
            tbl_RulesChuongTrinhDoiQua tbl_RulesChuongTrinhDoiQua = db.tbl_RulesChuongTrinhDoiQua.Find(id);
            if (tbl_RulesChuongTrinhDoiQua == null)
            {
                return NotFound();
            }

            return Ok(tbl_RulesChuongTrinhDoiQua);
        }

        // PUT: api/RulesChuongTrinhDoiQua/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_RulesChuongTrinhDoiQua(string id, tbl_RulesChuongTrinhDoiQua tbl_RulesChuongTrinhDoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_RulesChuongTrinhDoiQua.RCTDQ_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_RulesChuongTrinhDoiQua).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_RulesChuongTrinhDoiQuaExists(id))
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

        // POST: api/RulesChuongTrinhDoiQua
        [ResponseType(typeof(tbl_RulesChuongTrinhDoiQua))]
        public IHttpActionResult Posttbl_RulesChuongTrinhDoiQua(tbl_RulesChuongTrinhDoiQua tbl_RulesChuongTrinhDoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_RulesChuongTrinhDoiQua.Add(tbl_RulesChuongTrinhDoiQua);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_RulesChuongTrinhDoiQuaExists(tbl_RulesChuongTrinhDoiQua.RCTDQ_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_RulesChuongTrinhDoiQua.RCTDQ_ID }, tbl_RulesChuongTrinhDoiQua);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_RulesChuongTrinhDoiQua))]
        [Route("api/RulesChuongTrinhDoiQua/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_RulesChuongTrinhDoiQua> tbl_RulesChuongTrinhDoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_RulesChuongTrinhDoiQua)
            {
                if (tbl_RulesChuongTrinhDoiQuaExists(item.RCTDQ_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_RulesChuongTrinhDoiQua.Add(item);
                }
            }
            db.SaveChanges();

            return Ok(tbl_RulesChuongTrinhDoiQua);
        }
        // DELETE: api/RulesChuongTrinhDoiQua/5
        [ResponseType(typeof(tbl_RulesChuongTrinhDoiQua))]
        public IHttpActionResult Deletetbl_RulesChuongTrinhDoiQua(string id)
        {
            tbl_RulesChuongTrinhDoiQua tbl_RulesChuongTrinhDoiQua = db.tbl_RulesChuongTrinhDoiQua.Find(id);
            if (tbl_RulesChuongTrinhDoiQua == null)
            {
                return NotFound();
            }

            db.tbl_RulesChuongTrinhDoiQua.Remove(tbl_RulesChuongTrinhDoiQua);
            db.SaveChanges();

            return Ok(tbl_RulesChuongTrinhDoiQua);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_RulesChuongTrinhDoiQuaExists(string id)
        {
            return db.tbl_RulesChuongTrinhDoiQua.Count(e => e.RCTDQ_ID == id) > 0;
        }
    }
}