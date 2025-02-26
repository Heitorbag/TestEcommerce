﻿using System.ComponentModel.DataAnnotations;

namespace Lojinha.Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
    }
}
