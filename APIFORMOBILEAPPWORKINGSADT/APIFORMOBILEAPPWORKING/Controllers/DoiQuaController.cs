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
    public class DoiQuaController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/DoiQua
        public IQueryable<tbl_DoiQua> Gettbl_DoiQua()
        {
            return db.tbl_DoiQua;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DoiQua))]
        [Route("api/DoiQua/SyncClient/{date}/{user}")]
        public IQueryable<tbl_DoiQua> Gettbl_DoiQua(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            var lst = db.tbl_bill.Where(n => n.Us_id == user).Select(n => n.Bill_id).ToList();
            return db.tbl_DoiQua.Where(n => lst.Contains(n.Bill_id) && (n.DoiQua_DeleteFlag > dt || n.DoiQua_SyncFlag > dt));
        }
        [HttpGet]
        [ResponseType(typeof(tbl_DoiQua))]
        [Route("api/DoiQua/Getreport/{user}/{datebd}/{datekt}")]
        public IQueryable<tbl_DoiQua> GetReport(string user, string datebd, string datekt)
        {
            datebd = datebd.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtbd = Convert.ToDateTime(datebd);
            datekt = datekt.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtkt = Convert.ToDateTime(datekt);
            var lst = db.tbl_bill.Where(n => n.Us_id.ToLower() == user.ToLower() && n.Bill_date>=dtbd && n.Bill_date<=dtkt).Select(n => n.Bill_id).ToList();
            return db.tbl_DoiQua.Where(n => lst.Contains(n.Bill_id));
        }
        // GET: api/DoiQua/5
        [ResponseType(typeof(tbl_DoiQua))]
        public async Task<IHttpActionResult> Gettbl_DoiQua(string id)
        {
            tbl_DoiQua tbl_DoiQua = await db.tbl_DoiQua.FindAsync(id);
            if (tbl_DoiQua == null)
            {
                return NotFound();
            }

            return Ok(tbl_DoiQua);
        }

        // PUT: api/DoiQua/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttbl_DoiQua(string id, tbl_DoiQua tbl_DoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_DoiQua.Bill_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_DoiQua).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DoiQuaExists(id))
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

        // POST: api/DoiQua
        [ResponseType(typeof(tbl_DoiQua))]
        public async Task<IHttpActionResult> Posttbl_DoiQua(tbl_DoiQua tbl_DoiQua)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_DoiQua.Add(tbl_DoiQua);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (tbl_DoiQuaExists(tbl_DoiQua.Bill_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_DoiQua.Bill_id }, tbl_DoiQua);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_DoiQua))]
        [Route("api/DoiQua/SyncServer")]
        public IHttpActionResult Posttbl_chitietDondathang(List<tbl_DoiQua> tbl_chitietDondathang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var item in tbl_chitietDondathang)
            {
                if (tbl_DoiQuaExists(item.DoiQua_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_DoiQua.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok(tbl_chitietDondathang);
        }
        // DELETE: api/DoiQua/5
        [ResponseType(typeof(tbl_DoiQua))]
        public async Task<IHttpActionResult> Deletetbl_DoiQua(string id)
        {
            tbl_DoiQua tbl_DoiQua = await db.tbl_DoiQua.FindAsync(id);
            if (tbl_DoiQua == null)
            {
                return NotFound();
            }

            db.tbl_DoiQua.Remove(tbl_DoiQua);
            await db.SaveChangesAsync();

            return Ok(tbl_DoiQua);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DoiQuaExists(string id)
        {
            return db.tbl_DoiQua.Count(e => e.Bill_id == id) > 0;
        }
    }
}