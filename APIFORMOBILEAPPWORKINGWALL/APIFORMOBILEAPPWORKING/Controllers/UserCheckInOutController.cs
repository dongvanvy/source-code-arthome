using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using APIFORMOBILEAPPWORKING.Models;

namespace APIFORMOBILEAPPWORKING.Controllers
{
    public class UserCheckInOutController : ApiController
    {
        private DBMOBILEAPPWALLEntities db = new DBMOBILEAPPWALLEntities();

        // GET: api/UserCheckInOut
        public IQueryable<tbl_UserCheckInOut> Gettbl_UserCheckInOut()
        {
            return db.tbl_UserCheckInOut;
        }
        [HttpGet]
        [ResponseType(typeof(tbl_UserCheckInOut))]
        [Route("api/UserCheckInOut/SyncClient/{date}/{user}")]
        public IQueryable<tbl_UserCheckInOut> SyncClient(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            //var bigplan = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == user.ToLower() && n.Bigplan_date.Year==dt.Year && n.Bigplan_date.Month==dt.Month && n.Bigplan_date.Day==dt.Day).Select(n => n.Bigplan_id);
            var bigplan = db.tbl_bigplan.SqlQuery("select * from tbl_bigplan where FORMAT (Bigplan_date,'d','en-US')= FORMAT ( convert(datetime, '" + dt.ToString("yyyy/MM/dd") + "', 111), 'd', 'en-US' ) and Us_id like '" + user + "'").ToList().Select(n => n.Bigplan_id);
            //var bigplan = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == user.ToLower()).Select(n => n.Bigplan_id).ToList();
            return db.tbl_UserCheckInOut.Where(n=>bigplan.Contains(n.Bigplan_id));
        }
        [HttpGet]
        [ResponseType(typeof(tbl_UserCheckInOut))]
        [Route("api/UserCheckInOut/SyncGet/{date}/{user}")]
        public IQueryable<tbl_UserCheckInOut> Syncget(string date, string user)
        {
            date = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date);
            //var bigplan = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == user.ToLower() && n.Bigplan_date.Year==dt.Year && n.Bigplan_date.Month==dt.Month && n.Bigplan_date.Day==dt.Day).Select(n => n.Bigplan_id);
            //var bigplan = db.tbl_bigplan.SqlQuery("select * from tbl_bigplan where FORMAT (Bigplan_date,'d','en-US')= FORMAT ( convert(datetime, '"+dt.ToString("yyyy/MM/dd")+"', 111), 'd', 'en-US' ) and Us_id like '"+user+"'").ToList().Select(n=>n.Bigplan_id);
            var bigplan = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == user.ToLower()).Select(n => n.Bigplan_id).ToList();
            return db.tbl_UserCheckInOut.Where(n => bigplan.Contains(n.Bigplan_id));
        }
        [HttpGet]
        [ResponseType(typeof(tbl_UserCheckInOut))]
        [Route("api/UserCheckInOut/GetReport/{user}/{datebd}/{datekt}")]
        public IQueryable<tbl_UserCheckInOut> getClient(string user, string datebd, string datekt)
        {
            datebd = datebd.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtbd = Convert.ToDateTime(datebd);
            datekt = datekt.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dtkt = Convert.ToDateTime(datekt);
            //var bigplan = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == user.ToLower() && n.Bigplan_date.Year==dt.Year && n.Bigplan_date.Month==dt.Month && n.Bigplan_date.Day==dt.Day).Select(n => n.Bigplan_id);
            var bigplan = db.tbl_bigplan.Where(n => n.Us_id.ToLower() == user.ToLower() && n.Bigplan_date >= dtbd && n.Bigplan_date <= dtkt).Select(n => n.Bigplan_id);
            return db.tbl_UserCheckInOut.Where(n => bigplan.Contains(n.Bigplan_id));
        }
        // GET: api/UserCheckInOut/5
        [ResponseType(typeof(tbl_UserCheckInOut))]
        public IHttpActionResult Gettbl_UserCheckInOut(string id)
        {
            tbl_UserCheckInOut tbl_UserCheckInOut = db.tbl_UserCheckInOut.Find(id);
            if (tbl_UserCheckInOut == null)
            {
                return NotFound();
            }

            return Ok(tbl_UserCheckInOut);
        }

        // PUT: api/UserCheckInOut/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_UserCheckInOut(string id, tbl_UserCheckInOut tbl_UserCheckInOut)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_UserCheckInOut.UserCheckInOut_ID)
            {
                return BadRequest();
            }

            db.Entry(tbl_UserCheckInOut).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_UserCheckInOutExists(id))
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

        // POST: api/UserCheckInOut
        [ResponseType(typeof(tbl_UserCheckInOut))]
        public IHttpActionResult Posttbl_UserCheckInOut(tbl_UserCheckInOut tbl_UserCheckInOut)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_UserCheckInOut.Add(tbl_UserCheckInOut);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_UserCheckInOutExists(tbl_UserCheckInOut.UserCheckInOut_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_UserCheckInOut.UserCheckInOut_ID }, tbl_UserCheckInOut);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_UserCheckInOut))]
        [Route("api/UserCheckInOut/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_UserCheckInOut> tbl_UserCheckInOut)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_UserCheckInOut)
            {
                if (tbl_UserCheckInOutExists(item.UserCheckInOut_ID))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_UserCheckInOut.Add(item);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {

            }

            return Ok(tbl_UserCheckInOut);
        }
        [HttpPost]
        [Route("api/UserCheckInOut/Uploads/{filename}")]
        public Task<HttpResponseMessage> Upload(string filename)
        {
            filename = filename + ".jpg";
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/Uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o =>
                {
                    FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);
                    if (File.Exists(Path.Combine(root, filename)))
                    {
                        File.Delete(Path.Combine(root, filename));
                        File.Move(finfo.FullName, Path.Combine(root, filename));
                    }
                    else
                    {
                        File.Move(finfo.FullName, Path.Combine(root, filename));
                    }

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("File uploaded.")
                    };
                }
            );
            return task;
        }
        // DELETE: api/UserCheckInOut/5
        [ResponseType(typeof(tbl_UserCheckInOut))]
        public IHttpActionResult Deletetbl_UserCheckInOut(string id)
        {
            tbl_UserCheckInOut tbl_UserCheckInOut = db.tbl_UserCheckInOut.Find(id);
            if (tbl_UserCheckInOut == null)
            {
                return NotFound();
            }

            db.tbl_UserCheckInOut.Remove(tbl_UserCheckInOut);
            db.SaveChanges();

            return Ok(tbl_UserCheckInOut);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_UserCheckInOutExists(string id)
        {
            return db.tbl_UserCheckInOut.Count(e => e.UserCheckInOut_ID == id) > 0;
        }
    }
}