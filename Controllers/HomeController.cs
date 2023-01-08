using System;
using AccountDoc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Hosting.Server;

namespace AccountDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public int value;
        int counter = 0;
        public string NAME; 
  
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Verification()
        {
            return View();
        }

        public IActionResult Review()
        {
            return View();
        }
        public IActionResult DocumentView()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult AddSucces()
        {
            string sqlConnectString = "Data Source=TARAS;Initial Catalog=AccountData;Integrated Security=True"; ;
            string sqlSelect = "SELECT TOP 1 * FROM FormData ORDER BY ID DESC;";
            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, sqlConnectString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                ViewData["Name"] = row["name"];
                ViewData["Doctype"] = row["doctype"];
                ViewData["Email"] = row["email"];
                ViewData["Organiz"] = row["organization"];
                ViewData["Day"] = row["day"];
                ViewData["Month"] = row["month"];
                ViewData["Year"] = row["year"];
            }

            string connectionString = "Data Source=TARAS;Initial Catalog=AccountData;Integrated Security=True";
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string SqlExpression = $"DELETE FROM FormData;";
                SqlCommand command = new SqlCommand(SqlExpression, connect);
            }

 
            return View(); ;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public void Password(string email)
        {
            MailAddress from = new MailAddress("taras181716@gmail.com", "Taras");
            MailAddress to = new MailAddress(email);
            MailMessage msg = new MailMessage(from, to);

            msg.Subject = "verefication";


            msg.Body = "<h1>Document</h1>" +  "<h2>" + value.ToString() + "</h2>";
            msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("taras181716@gmail.com", "**********");
            smtp.EnableSsl = true;
            smtp.Send(msg);
        }
        [HttpPost]
        public ActionResult Add(AccountDataModel doc)
        {
            string connectionString = "Data Source=TARAS;Initial Catalog=AccountData;Integrated Security=True";
            counter++;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string SqlExpression = $"INSERT INTO FormData (ID, name, doctype, email, organization, day, month, year) VALUES({counter},'{doc.Name}', '{doc.Doctype}', '{doc.Email}', '{doc.Organiz}', '{doc.Day}', '{doc.Month}', '{doc.Year}');";
                Console.WriteLine(SqlExpression);
                SqlCommand command = new SqlCommand(SqlExpression, connect);
                command.ExecuteNonQuery();

            }

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                
                string SqlExpression = $"INSERT INTO Data (ID, name, doctype, email, organization, day, month, year) VALUES({counter},'{doc.Name}', '{doc.Doctype}', '{doc.Email}', '{doc.Organiz}', '{doc.Day}', '{doc.Month}', '{doc.Year}');";
                Console.WriteLine(SqlExpression);
                SqlCommand command = new SqlCommand(SqlExpression, connect);
                command.ExecuteNonQuery();

            }
            Random rnd = new Random();
            value = rnd.Next(1000, 9999);
            
            Password(doc.Email);
            
            return RedirectToAction("Verification");
        }
  
        [HttpPost]
        public ActionResult Verification(AccountDataModel doc)
        {

            string script = "alert(\"Hello!\");";
            
            int code = doc.Code;
            if (code != value)
                return RedirectToAction("AddSucces");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Review(AccountDataModel doc)
        {
            
            return RedirectToAction("DocumentView");
        }

        //public ActionResult Verification(AccountDataModel doc)
        //{
        //    if(V)
        //    return RedirectToAction("Verification");
        //}

    }
}
