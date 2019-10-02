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
    public class CaLamViecDetailController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/CaLamViecDetail
        public IQueryable<tbl_CaLamViecDetail> Gettbl_CaLamViecDetail()
        {
            return db.tbl_CaLamViecDetail;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_CaLamViecDetail))]
        [Route("api/CaLamViecDetail/SyncClient/{date}/{user}")]
        public IQueryable<tbl_CaLamViecDetail> SyncClient_CaLamViecDetail(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_CaLamViecDetail.Where(n => (n.CaLamViecDetail_DeleteFlag > dt || n.CaLamViecDetail_SyncFlag > dt));
        }
        // GET: api/CaLamViecDetail/5
        [ResponseType(typeof(tbl_CaLamViecDetail))]
        public IHttpActionResult Gettbl_CaLamViecDetail(string id)
        {
            tbl_CaLamViecDetail tbl_CaLamViecDetail = db.tbl_CaLamViecDetail.Find(id);
            if (tbl_CaLamViecDetail == null)
            {
                return NotFound();
            }

            return Ok(tbl_CaLamViecDetail);
        }

        // PUT: api/CaLamViecDetail/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_CaLamViecDetail(string id, tbl_CaLamViecDetail tbl_CaLamViecDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_CaLamViecDetail.CaLamViecDetail_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_CaLamViecDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_CaLamViecDetailExists(id))
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

        // POST: api/CaLamViecDetail
        [ResponseType(typeof(tbl_CaLamViecDetail))]
        public IHttpActionResult Posttbl_CaLamViecDetail(tbl_CaLamViecDetail tbl_CaLamViecDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_CaLamViecDetail.Add(tbl_CaLamViecDetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_CaLamViecDetailExists(tbl_CaLamViecDetail.CaLamViecDetail_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_CaLamViecDetail.CaLamViecDetail_id }, tbl_CaLamViecDetail);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_CaLamViecDetail))]
        [Route("api/CaLamViecDetail/SyncServer")]
        public IHttpActionResult SyncServer_CaLamViecDetail(List<tbl_CaLamViecDetail> tbl_CaLamViecDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_CaLamViecDetail)
            {
                if (tbl_CaLamViecDetailExists(item.CaLamViecDetail_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_CaLamViecDetail.Add(item);
                }
            }
            db.SaveChanges();
            return Ok(tbl_CaLamViecDetail);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_CaLamViecDetail.CaLamViecDetail_id }, tbl_CaLamViecDetail);
        }
        // DELETE: api/CaLamViecDetail/5
        [ResponseType(typeof(tbl_CaLamViecDetail))]
        public IHttpActionResult Deletetbl_CaLamViecDetail(string id)
        {
            tbl_CaLamViecDetail tbl_CaLamViecDetail = db.tbl_CaLamViecDetail.Find(id);
            if (tbl_CaLamViecDetail == null)
            {
                return NotFound();
            }

            db.tbl_CaLamViecDetail.Remove(tbl_CaLamViecDetail);
            db.SaveChanges();

            return Ok(tbl_CaLamViecDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_CaLamViecDetailExists(string id)
        {
            return db.tbl_CaLamViecDetail.Count(e => e.CaLamViecDetail_id == id) > 0;
        }
    }
}