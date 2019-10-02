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
    public class BillController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/Bill
        public IQueryable<tbl_bill> Gettbl_bill()
        {
            return db.tbl_bill;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_bill))]
        [Route("api/Bill/SyncClient/{date}/{user}")]
        public IQueryable<tbl_bill> SyncClienttbl_bill(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_bill.Where(n => (n.Bill_deleteFlag > dt || n.Bill_SyncFlag > dt) && n.Us_id == user);
        }
        // GET: api/Bill/5
        [ResponseType(typeof(tbl_bill))]
        public IHttpActionResult Gettbl_bill(string id)
        {
            tbl_bill tbl_bill = db.tbl_bill.Find(id);
            if (tbl_bill == null)
            {
                return NotFound();
            }

            return Ok(tbl_bill);
        }

        // PUT: api/Bill/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_bill(string id, tbl_bill tbl_bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_bill.Bill_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_bill).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_billExists(id))
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

        // POST: api/Bill
        [ResponseType(typeof(tbl_bill))]
        public IHttpActionResult Posttbl_bill(tbl_bill tbl_bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_bill.Add(tbl_bill);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_billExists(tbl_bill.Bill_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_bill.Bill_id }, tbl_bill);
        }
        [HttpPost]
        [Route("api/Bill/SyncServer")]
        [ResponseType(typeof(tbl_bill))]
        public IHttpActionResult SyncServertbl_bill(List<tbl_bill> tbl_bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_bill)
            {
                if (tbl_billExists(item.Bill_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_bill.Add(item);
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }
            return Ok(tbl_bill);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_bill.Bill_id }, tbl_bill);
        }
        // DELETE: api/Bill/5
        [ResponseType(typeof(tbl_bill))]
        public IHttpActionResult Deletetbl_bill(string id)
        {
            tbl_bill tbl_bill = db.tbl_bill.Find(id);
            if (tbl_bill == null)
            {
                return NotFound();
            }

            db.tbl_bill.Remove(tbl_bill);
            db.SaveChanges();

            return Ok(tbl_bill);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_billExists(string id)
        {
            return db.tbl_bill.Count(e => e.Bill_id == id) > 0;
        }
    }
}