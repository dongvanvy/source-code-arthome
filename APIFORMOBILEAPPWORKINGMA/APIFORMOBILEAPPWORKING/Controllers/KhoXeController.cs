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
    public class KhoXeController : ApiController
    {
        private DBMOBILEAPPMAEntities db = new DBMOBILEAPPMAEntities();

        // GET: api/KhoXe
        public IQueryable<Tbl_KhoXe> GetTbl_KhoXe()
        {
            return db.Tbl_KhoXe;
        }
        [HttpGet]
        [ResponseType(typeof(Tbl_KhoXe))]
        [Route("api/KhoXe/SyncClient/{date}/{user}")]
        public IQueryable<Tbl_KhoXe> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            return db.Tbl_KhoXe.Where(n => n.KX_DeleteFlag > dt || n.KX_SyncFlag > dt && n.KX_UserQuanLy.Contains(user));
        }
        // GET: api/KhoXe/5
        [ResponseType(typeof(Tbl_KhoXe))]
        public async Task<IHttpActionResult> GetTbl_KhoXe(string id)
        {
            Tbl_KhoXe tbl_KhoXe = await db.Tbl_KhoXe.FindAsync(id);
            if (tbl_KhoXe == null)
            {
                return NotFound();
            }

            return Ok(tbl_KhoXe);
        }

        // PUT: api/KhoXe/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTbl_KhoXe(string id, Tbl_KhoXe tbl_KhoXe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_KhoXe.KX_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_KhoXe).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_KhoXeExists(id))
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

        // POST: api/KhoXe
        [ResponseType(typeof(Tbl_KhoXe))]
        public async Task<IHttpActionResult> PostTbl_KhoXe(Tbl_KhoXe tbl_KhoXe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tbl_KhoXe.Add(tbl_KhoXe);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (Tbl_KhoXeExists(tbl_KhoXe.KX_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_KhoXe.KX_ID }, tbl_KhoXe);
        }

        // DELETE: api/KhoXe/5
        [ResponseType(typeof(Tbl_KhoXe))]
        public async Task<IHttpActionResult> DeleteTbl_KhoXe(string id)
        {
            Tbl_KhoXe tbl_KhoXe = await db.Tbl_KhoXe.FindAsync(id);
            if (tbl_KhoXe == null)
            {
                return NotFound();
            }

            db.Tbl_KhoXe.Remove(tbl_KhoXe);
            await db.SaveChangesAsync();

            return Ok(tbl_KhoXe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Tbl_KhoXeExists(string id)
        {
            return db.Tbl_KhoXe.Count(e => e.KX_ID == id) > 0;
        }
    }
}