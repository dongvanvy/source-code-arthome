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
    public class KHOQUAMAController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/KHOQUAMA
        public IQueryable<Tbl_KHOQUA> GetTbl_KHOQUA()
        {
            return db.Tbl_KHOQUA;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_KHOQUA))]
        [Route("api/KHOQUAMA/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_KHOQUA> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_KHOQUA.Where(n => n.KV_DeleteFlag > dt || n.KV_SyncFlag > dt && n.KV_UserQL.Contains(user));
        }
        // GET: api/KHOQUAMA/5
        [ResponseType(typeof(Tbl_KHOQUA))]
        public async Task<IHttpActionResult> GetTbl_KHOQUA(string id)
        {
            Tbl_KHOQUA tbl_KHOQUA = await db.Tbl_KHOQUA.FindAsync(id);
            if (tbl_KHOQUA == null)
            {
                return NotFound();
            }

            return Ok(tbl_KHOQUA);
        }

        // PUT: api/KHOQUAMA/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_KHOQUA(string id, Tbl_KHOQUA tbl_KHOQUA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_KHOQUA.KV_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_KHOQUA).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_KHOQUAExists(id))
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

        // POST: api/KHOQUAMA
        [ResponseType(typeof(Tbl_KHOQUA))]
        public async Task<IHttpActionResult> PostTbl_KHOQUA(Tbl_KHOQUA tbl_KHOQUA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_KHOQUA.Add(tbl_KHOQUA);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_KHOQUAExists(tbl_KHOQUA.KV_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_KHOQUA.KV_ID }, tbl_KHOQUA);
        }

        // DELETE: api/KHOQUAMA/5
        [ResponseType(typeof(Tbl_KHOQUA))]
        public async Task<IHttpActionResult> DeleteTbl_KHOQUA(string id)
        {
            Tbl_KHOQUA tbl_KHOQUA = await db.Tbl_KHOQUA.FindAsync(id);
            if (tbl_KHOQUA == null)
            {
                return NotFound();
            }

            db.Tbl_KHOQUA.Remove(tbl_KHOQUA);
            await db.SaveChangesAsync();

            return Ok(tbl_KHOQUA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_KHOQUAExists(string id)
        {
            return db.Tbl_KHOQUA.Count(e => e.KV_ID == id) > 0;
        }
    }
}