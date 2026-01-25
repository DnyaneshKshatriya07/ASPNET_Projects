using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AeroDroxUAV.Controllers
{
    // Apply anti-caching
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ContactController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: /Contact/Index
        public IActionResult Index()
        {
            ViewData["Title"] = "Contact Us";
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
            return View();
        }

        // POST: /Contact/Submit
        [HttpPost]
        public async Task<IActionResult> Submit(string name, string email, string phone, string company, string subject, string message, bool newsletter = false)
        {
            try
            {
                // Basic validation
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(subject))
                {
                    ViewBag.ErrorMessage = "Please fill all required fields.";
                    ViewData["Title"] = "Contact Us";
                    ViewBag.IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
                    ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
                    return View("Index");
                }

                // Prepare email
                string toEmail = "dnyaneshkshatriya7123@gmail.com";
                string subjectText = $"Website contact Form: {subject} - {name}";
                
                string body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #1a2980;'>New Contact Form Submission</h2>
                    <div style='background-color: #f8f9fa; padding: 20px; border-radius: 10px;'>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tr>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'><strong>Name:</strong></td>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'>{name}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'><strong>Email:</strong></td>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'><a href='mailto:{email}'>{email}</a></td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'><strong>Phone:</strong></td>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'>{phone ?? "Not provided"}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'><strong>Company:</strong></td>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'>{company ?? "Not provided"}</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'><strong>Subject:</strong></td>
                                <td style='padding: 8px; border-bottom: 1px solid #dee2e6;'>{subject}</td>
                            </tr>
                        </table>
                    </div>
                    <div style='margin-top: 20px;'>
                        <h3 style='color: #1a2980;'>Message:</h3>
                        <div style='background-color: #f0f8ff; padding: 15px; border-left: 4px solid #26d0ce; border-radius: 5px;'>
                            <p style='white-space: pre-line;'>{message}</p>
                        </div>
                    </div>
                    <div style='margin-top: 30px; padding-top: 20px; border-top: 1px solid #dee2e6; color: #6c757d; font-size: 12px;'>
                        <p>This email was sent from the contact form on AeroDrox UAV website.</p>
                        <p>Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                    </div>
                </body>
                </html>";

                // Send email using SMTP
                await SendEmailAsync(toEmail, subjectText, body);

                ViewBag.SuccessMessage = "Thank you for contacting us! We have received your message and will get back to you shortly.";
                
                // Clear form data (optional)
                // You can store form data in TempData if you want to repopulate on error
            }
            catch (Exception ex)
            {
                // Log the error (in production, use proper logging)
                Console.WriteLine($"Error sending email: {ex.Message}");
                ViewBag.ErrorMessage = "Sorry, there was an error sending your message. Please try again later or contact us directly.";
            }

            ViewData["Title"] = "Contact Us";
            ViewBag.IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
            ViewBag.Role = HttpContext.Session.GetString("Role") ?? "";
            return View("Index");
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Get SMTP configuration from appsettings.json or use default
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "@gmail.com";
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";
            var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername, "AeroDrox UAV Contact Form"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                // Add CC to sender for confirmation
                var senderEmail = HttpContext.Request.Form["email"];
                if (!string.IsNullOrEmpty(senderEmail))
                {
                    mailMessage.CC.Add(new MailAddress(senderEmail));
                }

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}