using ApiExamenTicketsApb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiExamenTicketsApb.Services {
    public class ServiceLogicApp {
        private MediaTypeWithQualityHeaderValue Header;
        public ServiceLogicApp() {
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        public async Task CreateTicket(int idusuario, DateTime fecha, string importe, string producto, string filename, string url) {
            string urlFlowCreateTicket = "https://prod-162.westeurope.logic.azure.com:443/workflows/7803a788e57f4a78bab7bee600b38d49/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=jjtD6gsSln8uS-yd2cu7WhbQ1pLyNDKOR9ozSZ_xAcA";
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Ticket ticket = new Ticket {
                    IdTicket = 0,
                    IdUsuario = idusuario,
                    Fecha = fecha,
                    Importe = importe,
                    Producto = producto,
                    Filename = filename,
                    Url = url
                };
                string json = JsonConvert.SerializeObject(ticket);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlFlowCreateTicket, content);
            }
        }
        public async Task ProcessTicket(int idticket, string blobname) {
            string urlLogicApp = "https://prod-157.westeurope.logic.azure.com:443/workflows/0f33fed05e404c2d8e9e346927157883/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=9F5ORWAgIxMrL9mGZR1sO6_gznjvxabbLyCFdzOnv9Q";
            using (HttpClient client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                var objeto = new {
                    FileName = blobname, IdTicket = idticket
                };
                string json = JsonConvert.SerializeObject(objeto);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlLogicApp, content);
            }
        }
    }
}
