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
    public class ShopController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/Shop
        public IQueryable<tbl_shop> Gettbl_shop()
        {
            return db.tbl_shop;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_shop))]
        [Route("api/Shop/SyncClient/{date}/{user}")]
        public IQueryable<tbl_shop> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var us = db.tbl_user.SingleOrDefault(n => n.Us_id == user);
            if (us.Us_Role == "Sup")
            {
                return db.tbl_shop.Where(n => n.Shop_Sup == us.Us_username);
            }
            else
            {
                var lst = db.tbl_bigplan.Where(n => n.Us_id == user).Select(n => n.Shop_id).ToList();
                return db.tbl_shop.Where(n => lst.Contains(n.Shop_id) && (n.Shop_deleteflag > dt || n.Shop_SyncFlag > dt));
            }
        }
        // GET: api/Shop/5
        [ResponseType(typeof(tbl_shop))]
        public IHttpActionResult Gettbl_shop(string id)
        {
            tbl_shop tbl_shop = db.tbl_shop.Find(id);
            if (tbl_shop == null)
            {
                return NotFound();
            }

            return Ok(tbl_shop);
        }

        // PUT: api/Shop/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_shop(string id, tbl_shop tbl_shop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_shop.Shop_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_shop).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_shopExists(id))
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

        // POST: api/Shop
        [ResponseType(typeof(tbl_shop))]
        public IHttpActionResult Posttbl_shop(tbl_shop tbl_shop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_shop.Add(tbl_shop);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_shopExists(tbl_shop.Shop_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_shop.Shop_id }, tbl_shop);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_shop))]
        [Route("api/Shop/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_shop> tbl_shop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_shop)
            {
                if (tbl_shopExists(item.Shop_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_shop.Add(item);
                }
            }
            db.SaveChanges();

            return Ok(tbl_shop);
        }
        // DELETE: api/Shop/5
        [ResponseType(typeof(tbl_shop))]
        public IHttpActionResult Deletetbl_shop(string id)
        {
            tbl_shop tbl_shop = db.tbl_shop.Find(id);
            if (tbl_shop == null)
            {
                return NotFound();
            }

            db.tbl_shop.Remove(tbl_shop);
            db.SaveChanges();

            return Ok(tbl_shop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_shopExists(string id)
        {
            return db.tbl_shop.Count(e => e.Shop_id == id) > 0;
        }
    }
}