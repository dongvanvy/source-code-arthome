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
    public class SkuController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/Sku
        public IQueryable<tbl_sku> Gettbl_sku()
        {
            return db.tbl_sku;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_sku))]
        [Route("api/Sku/SyncClient/{date}/{user}")]
        public IQueryable<tbl_sku> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_sku.Where(n => n.sku_deleteflag > dt || n.sku_SyncFlag > dt);
        }
        // GET: api/Sku/5
        [ResponseType(typeof(tbl_sku))]
        public IHttpActionResult Gettbl_sku(double id)
        {
            tbl_sku tbl_sku = db.tbl_sku.Find(id);
            if (tbl_sku == null)
            {
                return NotFound();
            }

            return Ok(tbl_sku);
        }

        // PUT: api/Sku/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_sku(double id, tbl_sku tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_sku.sku_barcode)
            {
                return BadRequest();
            }

            db.Entry(tbl_sku).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_skuExists(id))
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

        // POST: api/Sku
        [ResponseType(typeof(tbl_sku))]
        public IHttpActionResult Posttbl_sku(tbl_sku tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_sku.Add(tbl_sku);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_skuExists(tbl_sku.sku_barcode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_sku.sku_barcode }, tbl_sku);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_sku))]
        [Route("api/Sku/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_sku> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_skuExists(item.sku_barcode))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_sku.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Ok(tbl_sku);
        }
        // DELETE: api/Sku/5
        [ResponseType(typeof(tbl_sku))]
        public IHttpActionResult Deletetbl_sku(double id)
        {
            tbl_sku tbl_sku = db.tbl_sku.Find(id);
            if (tbl_sku == null)
            {
                return NotFound();
            }

            db.tbl_sku.Remove(tbl_sku);
            db.SaveChanges();

            return Ok(tbl_sku);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_skuExists(double id)
        {
            return db.tbl_sku.Count(e => e.sku_barcode == id) > 0;
        }
    }
}