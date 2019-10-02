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
    public class BillCNKDetailController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/BillCNKDetail
        public IQueryable<Tbl_BillCNKDetail> GetTbl_BillCNKDetail()
        {
            return db.Tbl_BillCNKDetail;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_BillCNKDetail))]
        [Route("api/BillCNKDetail/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_BillCNKDetail> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var billid = db.Tbl_BillChuyenNhanKho.Where(n => n.Us_IDChuyen == user).Select(n => n.BCNK_ID).ToList();
            return db.Tbl_BillCNKDetail.Where(n => n.DBCNK_DeleteFlag > dt || n.DBCNK_SyncFlag > dt && billid.Contains(n.BCNK_ID));
        }
        // GET: api/BillCNKDetail/5
        [ResponseType(typeof(Tbl_BillCNKDetail))]
        public async Task<IHttpActionResult> GetTbl_BillCNKDetail(string id)
        {
            Tbl_BillCNKDetail tbl_BillCNKDetail = await db.Tbl_BillCNKDetail.FindAsync(id);
            if (tbl_BillCNKDetail == null)
            {
                return NotFound();
            }

            return Ok(tbl_BillCNKDetail);
        }

        // PUT: api/BillCNKDetail/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_BillCNKDetail(string id, Tbl_BillCNKDetail tbl_BillCNKDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_BillCNKDetail.DBCNK_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_BillCNKDetail).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_BillCNKDetailExists(id))
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

        // POST: api/BillCNKDetail
        [ResponseType(typeof(Tbl_BillCNKDetail))]
        public async Task<IHttpActionResult> PostTbl_BillCNKDetail(Tbl_BillCNKDetail tbl_BillCNKDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_BillCNKDetail.Add(tbl_BillCNKDetail);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_BillCNKDetailExists(tbl_BillCNKDetail.DBCNK_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_BillCNKDetail.DBCNK_ID }, tbl_BillCNKDetail);
        }

        // DELETE: api/BillCNKDetail/5
        [ResponseType(typeof(Tbl_BillCNKDetail))]
        public async Task<IHttpActionResult> DeleteTbl_BillCNKDetail(string id)
        {
            Tbl_BillCNKDetail tbl_BillCNKDetail = await db.Tbl_BillCNKDetail.FindAsync(id);
            if (tbl_BillCNKDetail == null)
            {
                return NotFound();
            }

            db.Tbl_BillCNKDetail.Remove(tbl_BillCNKDetail);
            await db.SaveChangesAsync();

            return Ok(tbl_BillCNKDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_BillCNKDetailExists(string id)
        {
            return db.Tbl_BillCNKDetail.Count(e => e.DBCNK_ID == id) > 0;
        }
    }
}