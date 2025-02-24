﻿using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class ItemsPedidosModel
    {
        public int Id { get; set; }

        [Range(0, 999999.99, ErrorMessage = "A quantidade de compra não pode ser negativa.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O Id do Pedido é obrigatório.")]
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "O Id do Produto é obrigatório.")]
        public int IdProduto { get; set; }

        [Range(0, 999999.99, ErrorMessage = "A quantidade de compra não pode ser negativa.")]
        public decimal Quantidade { get; set; }
    }
}
