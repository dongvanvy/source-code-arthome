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
    public class DoithucanhtranhIMAGEDetailController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DoithucanhtranhIMAGEDetail
        public IQueryable<Tbl_DoithucanhtranhIMAGEDetail> GetTbl_DoithucanhtranhIMAGEDetail()
        {
            return db.Tbl_DoithucanhtranhIMAGEDetail;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGEDetail))]
        [Route("api/DoithucanhtranhIMAGEDetail/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_DoithucanhtranhIMAGEDetail> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var list = db.Tbl_DoithucanhtranhIMAGE.Where(n => n.US_ID == user).Select(n => n.DTCTIMAGE_ID).ToList();
            return db.Tbl_DoithucanhtranhIMAGEDetail.Where(n => (n.DTCTIMAGED_DeleteFlag > dt || n.DTCTIMAGED_SyncFlag > dt)&&list.Contains(n.DTCTIMAGE_ID));
        }
        // GET: api/DoithucanhtranhIMAGEDetail/5
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGEDetail))]
        public async Task<IHttpActionResult> GetTbl_DoithucanhtranhIMAGEDetail(string id)
        {
            Tbl_DoithucanhtranhIMAGEDetail tbl_DoithucanhtranhIMAGEDetail = await db.Tbl_DoithucanhtranhIMAGEDetail.FindAsync(id);
            if (tbl_DoithucanhtranhIMAGEDetail == null)
            {
                return NotFound();
            }

            return Ok(tbl_DoithucanhtranhIMAGEDetail);
        }

        // PUT: api/DoithucanhtranhIMAGEDetail/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_DoithucanhtranhIMAGEDetail(string id, Tbl_DoithucanhtranhIMAGEDetail tbl_DoithucanhtranhIMAGEDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DoithucanhtranhIMAGEDetail.DTCTIMAGED_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_DoithucanhtranhIMAGEDetail).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_DoithucanhtranhIMAGEDetailExists(id))
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

        // POST: api/DoithucanhtranhIMAGEDetail
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGEDetail))]
        public async Task<IHttpActionResult> PostTbl_DoithucanhtranhIMAGEDetail(Tbl_DoithucanhtranhIMAGEDetail tbl_DoithucanhtranhIMAGEDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_DoithucanhtranhIMAGEDetail.Add(tbl_DoithucanhtranhIMAGEDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_DoithucanhtranhIMAGEDetailExists(tbl_DoithucanhtranhIMAGEDetail.DTCTIMAGED_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DoithucanhtranhIMAGEDetail.DTCTIMAGED_ID }, tbl_DoithucanhtranhIMAGEDetail);
        }
        [HttpPost]
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGEDetail))]
        [Route("api/DoithucanhtranhIMAGEDetail/SyncServer")]
        public IHttpActionResult SyncServer(List<Tbl_DoithucanhtranhIMAGEDetail> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (Tbl_DoithucanhtranhIMAGEDetailExists(item.DTCTIMAGED_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.Tbl_DoithucanhtranhIMAGEDetail.Add(item);
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
        // DELETE: api/DoithucanhtranhIMAGEDetail/5
        [ResponseType(typeof(Tbl_DoithucanhtranhIMAGEDetail))]
        public async Task<IHttpActionResult> DeleteTbl_DoithucanhtranhIMAGEDetail(string id)
        {
            Tbl_DoithucanhtranhIMAGEDetail tbl_DoithucanhtranhIMAGEDetail = await db.Tbl_DoithucanhtranhIMAGEDetail.FindAsync(id);
            if (tbl_DoithucanhtranhIMAGEDetail == null)
            {
                return NotFound();
            }

            db.Tbl_DoithucanhtranhIMAGEDetail.Remove(tbl_DoithucanhtranhIMAGEDetail);
            await db.SaveChangesAsync();

            return Ok(tbl_DoithucanhtranhIMAGEDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_DoithucanhtranhIMAGEDetailExists(string id)
        {
            return db.Tbl_DoithucanhtranhIMAGEDetail.Count(e => e.DTCTIMAGED_ID == id) > 0;
        }
    }
}