using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evecorpfy.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string SenhaHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public byte[] FotoPerfil { get; set; }
    }
}
