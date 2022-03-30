using ApiExamenTicketsApb.Models;
using ApiExamenTicketsApb.Repositories;
using ApiExamenTicketsApb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiExamenTicketsApb.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase {
        private RepositoryApplication repo;
        private ServiceLogicApp service;
        public EmpresaController(RepositoryApplication repo, ServiceLogicApp service) {
            this.repo = repo;
            this.service = service;
        }
        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<List<Usuario>> GetUsuarios() {
            return this.repo.GetUsuarios();
        }
        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<Usuario> FindUsuario() {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string jsonUsuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            return usuario;
        }
        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<List<Ticket>> TicketsUsuario() {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string jsonUsuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            List<Ticket> tickets = this.repo.MostrarTicketsByUsuario(usuario.IdUsuario);
            return tickets;
        }
        [HttpGet]
        [Authorize]
        [Route("[action]/{idticket}")]
        public ActionResult<Ticket> FindTicket(int idticket) {
            Ticket ticket = this.repo.FindTicket(idticket);
            return ticket;
        }
        [HttpPost]
        [Route("[action]")]
        public void CreateUsuario(Usuario usuario) {
            this.repo.CrearUsuario(usuario.Nombre, usuario.Apellidos, usuario.Email, usuario.Username, usuario.Password);
        }
        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public async Task CreateTicket(Ticket ticket) {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string jsonUsuario = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            await this.service.CreateTicket(usuario.IdUsuario, ticket.Fecha, ticket.Importe, ticket.Producto, ticket.Filename, ticket.Url);
        }
        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public async Task ProcessTicket(Ticket ticket) {
            await this.service.ProcessTicket(ticket.IdTicket, ticket.Filename);
        }
    }
}
