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
    public class ChucnangphuController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/Chucnangphu
        public IQueryable<tbl_chucnangphu> Gettbl_chucnangphu()
        {
            return db.tbl_chucnangphu;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_chucnangphu))]
        [Route("api/chucnangphu/SyncClient/{date}/{user}")]
        public IQueryable<tbl_chucnangphu> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.tbl_chucnangphu.Where(n => n.CNP_DeleteFlag > dt || n.CNP_SyncFlag > dt);
        }
        // GET: api/Chucnangphu/5
        [ResponseType(typeof(tbl_chucnangphu))]
        public async Task<IHttpActionResult> Gettbl_chucnangphu(string id)
        {
            tbl_chucnangphu tbl_chucnangphu = await db.tbl_chucnangphu.FindAsync(id);
            if (tbl_chucnangphu == null)
            {
                return NotFound();
            }

            return Ok(tbl_chucnangphu);
        }

        // PUT: api/Chucnangphu/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_chucnangphu(string id, tbl_chucnangphu tbl_chucnangphu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_chucnangphu.CNP_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_chucnangphu).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_chucnangphuExists(id))
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

        // POST: api/Chucnangphu
        [ResponseType(typeof(tbl_chucnangphu))]
        public async Task<IHttpActionResult> Posttbl_chucnangphu(tbl_chucnangphu tbl_chucnangphu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_chucnangphu.Add(tbl_chucnangphu);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_chucnangphuExists(tbl_chucnangphu.CNP_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_chucnangphu.CNP_ID }, tbl_chucnangphu);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_chucnangphu))]
        [Route("api/chucnangphu/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_chucnangphu> tbl_sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_sku)
            {
                if (tbl_chucnangphuExists(item.CNP_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_chucnangphu.Add(item);
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
        // DELETE: api/Chucnangphu/5
        [ResponseType(typeof(tbl_chucnangphu))]
        public async Task<IHttpActionResult> Deletetbl_chucnangphu(string id)
        {
            tbl_chucnangphu tbl_chucnangphu = await db.tbl_chucnangphu.FindAsync(id);
            if (tbl_chucnangphu == null)
            {
                return NotFound();
            }

            db.tbl_chucnangphu.Remove(tbl_chucnangphu);
            await db.SaveChangesAsync();

            return Ok(tbl_chucnangphu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_chucnangphuExists(string id)
        {
            return db.tbl_chucnangphu.Count(e => e.CNP_ID == id) > 0;
        }
    }
}