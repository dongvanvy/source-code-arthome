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
    public class ChitietSupChamDiemController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/ChitietSupChamDiem
        public IQueryable<tbl_ChitietSupChamDiem> Gettbl_ChitietSupChamDiem()
        {
            return db.tbl_ChitietSupChamDiem;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_ChitietSupChamDiem))]
        [Route("api/ChitietSupChamDiem/SyncClient/{date}/{user}")]
        public IQueryable<tbl_ChitietSupChamDiem> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var lst = db.tbl_SupBigPlan.Where(n => n.Us_id == user).Select(n => n.SupBigPlan_id).ToList();
            var lsts = db.tbl_SupChamDiem.Where(n => lst.Contains(n.SupBigPlan_id)).Select(n => n.SupChamDiem_id).ToList();
            return db.tbl_ChitietSupChamDiem.Where(n => lsts.Contains(n.SupChamDiem_id) && (n.ChitietSupChamDiem_SyncFlag > dt || n.ChitietSupChamDiem_DeleteFlag > dt));
        }
        // GET: api/ChitietSupChamDiem/5
        [ResponseType(typeof(tbl_ChitietSupChamDiem))]
        public async Task<IHttpActionResult> Gettbl_ChitietSupChamDiem(string id)
        {
            tbl_ChitietSupChamDiem tbl_ChitietSupChamDiem = await db.tbl_ChitietSupChamDiem.FindAsync(id);
            if (tbl_ChitietSupChamDiem == null)
            {
                return NotFound();
            }

            return Ok(tbl_ChitietSupChamDiem);
        }

        // PUT: api/ChitietSupChamDiem/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_ChitietSupChamDiem(string id, tbl_ChitietSupChamDiem tbl_ChitietSupChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_ChitietSupChamDiem.ChitietSupChamDiem_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_ChitietSupChamDiem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_ChitietSupChamDiemExists(id))
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

        // POST: api/ChitietSupChamDiem
        [ResponseType(typeof(tbl_ChitietSupChamDiem))]
        public async Task<IHttpActionResult> Posttbl_ChitietSupChamDiem(tbl_ChitietSupChamDiem tbl_ChitietSupChamDiem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_ChitietSupChamDiem.Add(tbl_ChitietSupChamDiem);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_ChitietSupChamDiemExists(tbl_ChitietSupChamDiem.ChitietSupChamDiem_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_ChitietSupChamDiem.ChitietSupChamDiem_ID }, tbl_ChitietSupChamDiem);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_ChitietSupChamDiem))]
        [Route("api/ChitietSupChamDiem/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_ChitietSupChamDiem> tbl_SupCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_SupCheckIn)
            {
                if (tbl_ChitietSupChamDiemExists(item.ChitietSupChamDiem_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_ChitietSupChamDiem.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
            }

            return Ok(tbl_SupCheckIn);
        }
        // DELETE: api/ChitietSupChamDiem/5
        [ResponseType(typeof(tbl_ChitietSupChamDiem))]
        public async Task<IHttpActionResult> Deletetbl_ChitietSupChamDiem(string id)
        {
            tbl_ChitietSupChamDiem tbl_ChitietSupChamDiem = await db.tbl_ChitietSupChamDiem.FindAsync(id);
            if (tbl_ChitietSupChamDiem == null)
            {
                return NotFound();
            }

            db.tbl_ChitietSupChamDiem.Remove(tbl_ChitietSupChamDiem);
            await db.SaveChangesAsync();

            return Ok(tbl_ChitietSupChamDiem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_ChitietSupChamDiemExists(string id)
        {
            return db.tbl_ChitietSupChamDiem.Count(e => e.ChitietSupChamDiem_ID == id) > 0;
        }
    }
}