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
    public class KhoController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/Kho
        public IQueryable<tbl_Kho> Gettbl_Kho()
        {
            return db.tbl_Kho;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_Kho))]
        [Route("api/Kho/SyncClient/{date}/{shopid}")]
        public IQueryable<tbl_Kho> SyncClient_Kho(string date, string shopid)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_Kho.Where(n => n.Shop_IDDICH == shopid && (n.KHO_DeleteFlag > dt || n.KHO_SyncFlag > dt));
        }
        // GET: api/Kho/5
        [ResponseType(typeof(tbl_Kho))]
        public IHttpActionResult Gettbl_Kho(string id)
        {
            tbl_Kho tbl_Kho = db.tbl_Kho.Find(id);
            if (tbl_Kho == null)
            {
                return NotFound();
            }

            return Ok(tbl_Kho);
        }

        // PUT: api/Kho/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Kho(string id, tbl_Kho tbl_Kho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Kho.KHO_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_Kho).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_KhoExists(id))
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

        // POST: api/Kho
        [ResponseType(typeof(tbl_Kho))]
        public IHttpActionResult Posttbl_Kho(tbl_Kho tbl_Kho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Kho.Add(tbl_Kho);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_KhoExists(tbl_Kho.KHO_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_Kho.KHO_ID }, tbl_Kho);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_Kho))]
        [Route("api/Kho/SyncServer")]
        public IHttpActionResult SyncServer_Kho(List<tbl_Kho> tbl_Kho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_Kho)
            {
                if (tbl_KhoExists(item.KHO_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_Kho.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }
            return Ok(tbl_Kho);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_Kho.KHO_ID }, tbl_Kho);
        }
        // DELETE: api/Kho/5
        [ResponseType(typeof(tbl_Kho))]
        public IHttpActionResult Deletetbl_Kho(string id)
        {
            tbl_Kho tbl_Kho = db.tbl_Kho.Find(id);
            if (tbl_Kho == null)
            {
                return NotFound();
            }

            db.tbl_Kho.Remove(tbl_Kho);
            db.SaveChanges();

            return Ok(tbl_Kho);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_KhoExists(string id)
        {
            return db.tbl_Kho.Count(e => e.KHO_ID == id) > 0;
        }
    }
}