﻿using WebEcommerce.Models;

namespace WebEcommerce.Repository.Contract
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> ListarProdutos();

        Produto ObterProduto(int cod);

        Produto AddCarrinho(int Cod);
    }
}
