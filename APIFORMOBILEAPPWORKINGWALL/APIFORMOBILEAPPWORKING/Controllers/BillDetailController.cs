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
    public class BillDetailController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/BillDetail
        public IQueryable<tbl_billDetail> Gettbl_billDetail()
        {
            return db.tbl_billDetail;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_billDetail))]
        [Route("api/BillDetail/SyncClient/{date}/{user}")]
        public IQueryable<tbl_billDetail> SyncClienttbl_billDetail(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var lbill = db.tbl_bill.Where(n => n.Us_id == user).Select(n => n.Bill_id).ToList();
            return db.tbl_billDetail.Where(n => lbill.Contains(n.Bill_id) && (n.BillDetail_SyncFlag > dt || n.BillDetail_deleteFlag > dt));
        }
        [HttpGet]
        [ResponseType(typeof(tbl_billDetail))]
        [Route("api/BillDetail/GetReport/{user}/{datebd}/{datekt}")]
        public IQueryable<tbl_billDetail> GetTimetbl_billDetail(string user, string datebd, string datekt)
        {
            datebd = datebd.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtbd = Convert.ToDateTime(datebd);
            datekt = datekt.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtkt = Convert.ToDateTime(datekt);
            var check = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == user.ToLower() && n.Bigplan_date >= dtbd && n.Bigplan_date <= dtkt).ToList();
            if (check.Count>0)
            {
                var checkbig = check.Select(n => n.Bigplan_id).ToList();
                return db.tbl_billDetail.Where(n => checkbig.Contains(n.Bigplan_id));
            }
            else
            {
                return db.tbl_billDetail.Where(n => n.Bigplan_id=="9999999999");
            }
            
        }
        // GET: api/BillDetail/5
        [ResponseType(typeof(tbl_billDetail))]
        public IHttpActionResult Gettbl_billDetail(string id)
        {
            tbl_billDetail tbl_billDetail = db.tbl_billDetail.Find(id);
            if (tbl_billDetail == null)
            {
                return NotFound();
            }

            return Ok(tbl_billDetail);
        }

        // PUT: api/BillDetail/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_billDetail(string id, tbl_billDetail tbl_billDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_billDetail.Bill_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_billDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_billDetailExists(id,tbl_billDetail.sku_barcode))
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

        // POST: api/BillDetail
        [ResponseType(typeof(tbl_billDetail))]
        public IHttpActionResult Posttbl_billDetail(tbl_billDetail tbl_billDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_billDetail.Add(tbl_billDetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_billDetailExists(tbl_billDetail.Bill_id,tbl_billDetail.sku_barcode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_billDetail.Bill_id }, tbl_billDetail);
        }
        [HttpPost]
        [Route("api/BillDetail/SyncServer")]
        [ResponseType(typeof(tbl_billDetail))]
        public IHttpActionResult SyncServertbl_billDetail(List<tbl_billDetail> tbl_billDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_billDetail)
            {
                if (tbl_billDetailExists(item.Bill_id,item.sku_barcode))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_billDetail.Add(item);
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }
            return Ok(tbl_billDetail);
            //return CreatedAtRoute("DefaultApi", new { id = tbl_billDetail.Bill_id }, tbl_billDetail);
        }
        // DELETE: api/BillDetail/5
        [ResponseType(typeof(tbl_billDetail))]
        public IHttpActionResult Deletetbl_billDetail(string id)
        {
            tbl_billDetail tbl_billDetail = db.tbl_billDetail.Find(id);
            if (tbl_billDetail == null)
            {
                return NotFound();
            }

            db.tbl_billDetail.Remove(tbl_billDetail);
            db.SaveChanges();

            return Ok(tbl_billDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_billDetailExists(string id,double barcode)
        {
            return db.tbl_billDetail.Count(e => e.Bill_id == id && e.sku_barcode==barcode) > 0;
        }
    }
}