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
    public class PriceController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/Price
        public IQueryable<tbl_Price> Gettbl_Price()
        {
            return db.tbl_Price;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_Price))]
        [Route("api/Price/SyncClient/{date}/{user}")]
        public IQueryable<tbl_Price> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var lst = db.tbl_bigplan.Where(n => n.Us_id == user).Select(n => n.Shop_id).ToList();
            var lsg = db.tbl_shop.Where(n => lst.Contains(n.Shop_id)).Select(n => n.Shop_GroupPrice).ToList();
            return db.tbl_Price.Where(n => (n.Price_DeleteFlag > dt || n.Price_SyncFlag > dt) && lsg.Contains(n.Shop_GroupPrice));
        }
        // GET: api/Price/5
        [ResponseType(typeof(tbl_Price))]
        public IHttpActionResult Gettbl_Price(string id)
        {
            tbl_Price tbl_Price = db.tbl_Price.Find(id);
            if (tbl_Price == null)
            {
                return NotFound();
            }

            return Ok(tbl_Price);
        }

        // PUT: api/Price/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Price(string id, tbl_Price tbl_Price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Price.Price_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_Price).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_PriceExists(id))
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

        // POST: api/Price
        [ResponseType(typeof(tbl_Price))]
        public IHttpActionResult Posttbl_Price(tbl_Price tbl_Price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Price.Add(tbl_Price);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_PriceExists(tbl_Price.Price_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_Price.Price_ID }, tbl_Price);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_Price))]
        [Route("api/Price/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_Price> tbl_Price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_Price)
            {
                if (tbl_PriceExists(item.Price_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_Price.Add(item);
                }
            }
            db.SaveChanges();

            return Ok(tbl_Price);
        }
        // DELETE: api/Price/5
        [ResponseType(typeof(tbl_Price))]
        public IHttpActionResult Deletetbl_Price(string id)
        {
            tbl_Price tbl_Price = db.tbl_Price.Find(id);
            if (tbl_Price == null)
            {
                return NotFound();
            }

            db.tbl_Price.Remove(tbl_Price);
            db.SaveChanges();

            return Ok(tbl_Price);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_PriceExists(string id)
        {
            return db.tbl_Price.Count(e => e.Price_ID == id) > 0;
        }
    }
}