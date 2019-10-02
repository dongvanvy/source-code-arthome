using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using APIFORMOBILEAPPWORKING.Models;

namespace APIFORMOBILEAPPWORKING.Controllers
{
    public class HsSmsController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/HsSms
        public IQueryable<Tbl_HsSms> GetTbl_HsSms()
        {
            return db.Tbl_HsSms;
        }

        // GET: api/HsSms/5
        [ResponseType(typeof(Tbl_HsSms))]
        public async Task<IHttpActionResult> GetTbl_HsSms(int id)
        {
            Tbl_HsSms tbl_HsSms = await db.Tbl_HsSms.FindAsync(id);
            if (tbl_HsSms == null)
            {
                return NotFound();
            }

            return Ok(tbl_HsSms);
        }

        // PUT: api/HsSms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_HsSms(string id, Tbl_HsSms tbl_HsSms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_HsSms.HSsms_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_HsSms).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_HsSmsExists(id))
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

        // POST: api/HsSms
        [ResponseType(typeof(Tbl_HsSms))]
        public async Task<IHttpActionResult> PostTbl_HsSms(Tbl_HsSms tbl_HsSms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_HsSms.Add(tbl_HsSms);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tbl_HsSms.HSsms_ID }, tbl_HsSms);
        }
        [HttpPost]
        [Route("api/HsSms/SendSMS/")]
        [ResponseType(typeof(Tbl_HsSms))]
        public IHttpActionResult SyncServertbl_bill(Tbl_HsSms tbl_bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Tbl_HsSmsExists(tbl_bill.HSsms_ID))
            {
                db.Entry(tbl_bill).State = EntityState.Modified;
            }
            else
            {
                db.Tbl_HsSms.Add(tbl_bill);
            }
            SpeedSMSAPI api = new SpeedSMSAPI("E2Pu4Xvw8lceXNAgAQTANhUJcmRGNe4Z");
            String[] phones = new String[] { tbl_bill.HSsms_Phone};
            String str = "Ma OTP cua ban la "+tbl_bill.HSsms_OTP;
            String response = api.sendSMS(phones, str, 2, "");
            try
            {
                db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
            }
            if (response.Contains("success")==true)
            {
                return Ok(tbl_bill);
            }
            else
            {
                return BadRequest();
            }
           
            //return CreatedAtRoute("DefaultApi", new { id = tbl_bill.Bill_id }, tbl_bill);
        }
        // DELETE: api/HsSms/5
        [ResponseType(typeof(Tbl_HsSms))]
        public async Task<IHttpActionResult> DeleteTbl_HsSms(int id)
        {
            Tbl_HsSms tbl_HsSms = await db.Tbl_HsSms.FindAsync(id);
            if (tbl_HsSms == null)
            {
                return NotFound();
            }

            db.Tbl_HsSms.Remove(tbl_HsSms);
            await db.SaveChangesAsync();

            return Ok(tbl_HsSms);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_HsSmsExists(string id)
        {
            return db.Tbl_HsSms.Count(e => e.HSsms_ID == id) > 0;
        }
    }
}