using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using APIFORMOBILEAPPWORKING.Models;

namespace APIFORMOBILEAPPWORKING.Controllers
{
    public class UserController : ApiController
    {
        private DBMOBILEAPPEntities db = new DBMOBILEAPPEntities();

        // GET: api/User
        public IQueryable<tbl_user> Gettbl_user()
        {
            return db.tbl_user;
        }

        // GET: api/User/5
        [ResponseType(typeof(tbl_user))]
        public IHttpActionResult Gettbl_user(string id)
        {
            tbl_user tbl_user = db.tbl_user.Find(id);
            if (tbl_user == null)
            {
                return NotFound();
            }

            return Ok(tbl_user);
        }
        [HttpGet]
        [ResponseType(typeof(tbl_user))]
        [Route("api/User/SyncClient/{date}/{user}")]
        public IQueryable<tbl_user> SyncClient(string date, string user)
        {
            string date1 = date.Replace("_", ":").Replace("T", " ").Replace("t", " ");
            DateTime dt = Convert.ToDateTime(date1);
            string role = db.tbl_user.SingleOrDefault(n => n.Us_id == user).Us_Role.ToString();
            string sup_group = db.tbl_user.SingleOrDefault(n => n.Us_id == user).Us_username.ToString();
            if (role == "Sup")
            {
                return db.tbl_user.Where(n => n.Us_SupGroup == sup_group);
            }
            else
            {
                return db.tbl_user.Where(n => n.Us_id == user && (n.Us_deleteflag > dt || n.Us_SyncFlag > dt));

            }
        }

        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_user(string id, tbl_user tbl_user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_user.Us_id)
            {
                return BadRequest();
            }

            db.Entry(tbl_user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_userExists(id))
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

        // POST: api/User
        [ResponseType(typeof(tbl_user))]
        public IHttpActionResult Posttbl_user(tbl_user tbl_user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_user.Add(tbl_user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_userExists(tbl_user.Us_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_user.Us_id }, tbl_user);
        }
        [HttpPost]
        [ResponseType(typeof(tbl_user))]
        [Route("api/User/Login")]
        public IHttpActionResult Login(tbl_user user)
        {
            string mkhash = GetMD5(user.Us_password);
            int checkcode = db.tbl_user.Count(n => n.Us_username == user.Us_username && n.Us_password == mkhash);
            if (checkcode > 0)
            {
                return Ok(db.tbl_user.Where(n => n.Us_username == user.Us_username && n.Us_password == mkhash));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ResponseType(typeof(tbl_user))]
        [Route("api/User/SyncServer")]
        public IHttpActionResult SyncServer(List<tbl_user> tbl_user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in tbl_user)
            {
                if (tbl_userExists(item.Us_id))
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    db.tbl_user.Add(item);
                }
            }
            db.SaveChangesAsync();

            return Ok(tbl_user);
        }
        // DELETE: api/User/5
        [ResponseType(typeof(tbl_user))]
        public IHttpActionResult Deletetbl_user(string id)
        {
            tbl_user tbl_user = db.tbl_user.Find(id);
            if (tbl_user == null)
            {
                return NotFound();
            }

            db.tbl_user.Remove(tbl_user);
            db.SaveChanges();

            return Ok(tbl_user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_userExists(string id)
        {
            return db.tbl_user.Count(e => e.Us_id == id) > 0;
        }
        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }
    }
}