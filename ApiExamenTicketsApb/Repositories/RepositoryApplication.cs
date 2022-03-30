using ApiExamenTicketsApb.Data;
using ApiExamenTicketsApb.Models;
using ApiExamenTicketsApb.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiExamenTicketsApb.Repositories {
    public class RepositoryApplication {
        private ApplicationContext context;
        private ServiceLogicApp service;
        public RepositoryApplication(ApplicationContext context) {
            this.context = context;
        }

        public List<Usuario> GetUsuarios() {
            return this.context.Usuarios.ToList();
        }
        public Usuario FindUsuario(int idusuario) {
            var consulta = from datos in this.context.Usuarios where datos.IdUsuario == idusuario select datos;
            if (consulta.Count() != 0) {
                return consulta.FirstOrDefault();
            }
            else {
                return null;
            }
        }
        public Usuario ExisteUsuario(string username, string password) {
            var consulta = from datos in this.context.Usuarios where datos.Username == username && datos.Password == password select datos;
            if (consulta.Count() != 0) {
                return consulta.FirstOrDefault();
            } else {
                return null;
            }
        }
        public void CrearUsuario(string nombre, string apellidos, string email, string username, string password) {
            int maxid = this.GetMaxIdUsuario();
            Usuario nuevoUsuario = new Usuario{
                IdUsuario = maxid+1,
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                Username = username,
                Password = password
            };
            this.context.Usuarios.Add(nuevoUsuario);
            this.context.SaveChanges();
        }
        public List<Ticket> MostrarTicketsByUsuario(int idusuario) {
            var consulta = from datos in this.context.Tickets where datos.IdUsuario == idusuario select datos;
            List<Ticket> ticketsByUsuario = consulta.ToList();
            return ticketsByUsuario;
        }
        public Ticket FindTicket(int idticket) {
            var consulta = from datos in this.context.Tickets where datos.IdTicket == idticket select datos;
            if (consulta.Count() != 0) {
                return consulta.FirstOrDefault();
            }
            else {
                return null;
            }
        }
        private int GetMaxIdUsuario() {
            var consulta = from datos in this.context.Usuarios select datos;
            if (consulta.Count() != 0) {
                int maxid = consulta.Max(x => x.IdUsuario);
                return maxid;
            } else {
                return 0;
            }
        }
        private int GetMaxIdTicket() {
            var consulta = from datos in this.context.Tickets select datos;
            if (consulta.Count() != 0) {
                int maxid = consulta.Max(x => x.IdTicket);
                return maxid;
            }
            else {
                return 0;
            }
        }
    }
}
