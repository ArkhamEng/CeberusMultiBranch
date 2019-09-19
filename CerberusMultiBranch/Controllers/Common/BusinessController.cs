using CerberusMultiBranch.Models;
using CerberusMultiBranch.Models.ApiModel;
using CerberusMultiBranch.Support;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using static CerberusMultiBranch.Support.ConfigVariable;

namespace CerberusMultiBranch.Controllers.Common
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BusinessController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("api/Contact/SendMail")]
        [ResponseType(typeof(JResponse))]
        public async Task<IHttpActionResult> ContactRequest([FromBody]ContactMessageModel message)
        {
            try
            {
                var variables = await db.Variables.ToListAsync();

                var cred = variables.First(v => v.Name.Equals(Maling.NoReplyCredentials)).Value.Split(Cons.SplitChar);
                var smtpSer = variables.First(v => v.Name.Equals(Maling.SmtpHost)).Value.Split(Cons.SplitChar);

                var sender = variables.First(v => v.Name.Equals(Maling.SmtpHost)).Value;
                var to = variables.First(v => v.Name.Equals(Maling.MailToWebSite)).Value.Split(Cons.SplitChar).ToList();

                var mail = new MailSender(cred[Cons.Zero], cred[Cons.One], smtpSer[Cons.Zero], Convert.ToInt32(smtpSer[Cons.One]));

                mail.CC = new List<string> { mail.FromAddress };

                var body = string.Format("<h2 class='text-centered'>Mensaje envíado desde la página web</h2>"+
                                        "<h3>Remitente {0} {1}</h3/><p>{2}</p> Dirección de respuesta:<a href='mailto'>{3}</a>", 
                                        message.FirstName,message.LastName, message.Message, message.Email);
                mail.SendMail(to, body, message.Subject);

                var response = new JResponse
                {
                    Body = "Tu mensaje ha sido envíado con éxito",
                    Code = Cons.Responses.Codes.Success,
                    Result = Cons.Responses.Success,
                    Header = "Mensaje enviado",
                    JProperty = message,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new JResponse
                {
                    Body = "El envío del mensaje fallo debido a un error interno, descripción: "+ ex.Message,
                    Code = Cons.Responses.Codes.ServerError,
                    Result = Cons.Responses.Warning,
                    Header = "Error al envíar el correo",
                    JProperty = message,
                };
                return this.Ok(response);
            }
        }

    }
}