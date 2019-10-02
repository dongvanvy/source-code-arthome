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
    public class BillChuyenNhanKhoController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/BillChuyenNhanKho
        public IQueryable<Tbl_BillChuyenNhanKho> GetTbl_BillChuyenNhanKho()
        {
            return db.Tbl_BillChuyenNhanKho;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_BillChuyenNhanKho))]
        [Route("api/BillChuyenNhanKho/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_BillChuyenNhanKho> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_BillChuyenNhanKho.Where(n => n.BCNK_DeleteFlag > dt || n.BCNK_SyncFlag > dt && n.Us_IDChuyen==user);
        }
        // GET: api/BillChuyenNhanKho/5
        [ResponseType(typeof(Tbl_BillChuyenNhanKho))]
        public async Task<IHttpActionResult> GetTbl_BillChuyenNhanKho(string id)
        {
            Tbl_BillChuyenNhanKho tbl_BillChuyenNhanKho = await db.Tbl_BillChuyenNhanKho.FindAsync(id);
            if (tbl_BillChuyenNhanKho == null)
            {
                return NotFound();
            }

            return Ok(tbl_BillChuyenNhanKho);
        }

        // PUT: api/BillChuyenNhanKho/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_BillChuyenNhanKho(string id, Tbl_BillChuyenNhanKho tbl_BillChuyenNhanKho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_BillChuyenNhanKho.BCNK_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_BillChuyenNhanKho).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_BillChuyenNhanKhoExists(id))
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

        // POST: api/BillChuyenNhanKho
        [ResponseType(typeof(Tbl_BillChuyenNhanKho))]
        public async Task<IHttpActionResult> PostTbl_BillChuyenNhanKho(Tbl_BillChuyenNhanKho tbl_BillChuyenNhanKho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_BillChuyenNhanKho.Add(tbl_BillChuyenNhanKho);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_BillChuyenNhanKhoExists(tbl_BillChuyenNhanKho.BCNK_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_BillChuyenNhanKho.BCNK_ID }, tbl_BillChuyenNhanKho);
        }

        // DELETE: api/BillChuyenNhanKho/5
        [ResponseType(typeof(Tbl_BillChuyenNhanKho))]
        public async Task<IHttpActionResult> DeleteTbl_BillChuyenNhanKho(string id)
        {
            Tbl_BillChuyenNhanKho tbl_BillChuyenNhanKho = await db.Tbl_BillChuyenNhanKho.FindAsync(id);
            if (tbl_BillChuyenNhanKho == null)
            {
                return NotFound();
            }

            db.Tbl_BillChuyenNhanKho.Remove(tbl_BillChuyenNhanKho);
            await db.SaveChangesAsync();

            return Ok(tbl_BillChuyenNhanKho);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_BillChuyenNhanKhoExists(string id)
        {
            return db.Tbl_BillChuyenNhanKho.Count(e => e.BCNK_ID == id) > 0;
        }
    }
}