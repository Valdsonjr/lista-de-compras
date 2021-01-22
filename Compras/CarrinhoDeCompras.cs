using System.Collections.Generic;
using System.Linq;

namespace Compras
{
    public static class CarrinhoDeCompras
    {

        public static IDictionary<string, Dinheiro> CalcularValorPorEmail
            (IEnumerable<(Item item, byte quantidade)> compras, IEnumerable<string> emails)
            => compras
                 // Calcular a soma dos valores, ou seja, multiplicar o preço de cada item por sua quantidade e somar todos os itens
                .Aggregate((Dinheiro)0, (custo, compra) => custo + compra.item.PrecoUnitario * compra.quantidade)
                 // Dividir o valor de forma igual entre a quantidade de e-mails
                .DividirEntre(emails.Count())
                 // Retornar um mapa/dicionário onde a chave será o e-mail e o valor será quanto ele deve pagar nessa conta
                .Zip(emails, (valor, email) => (valor, email))
                .ToDictionary(compra => compra.email, compra => compra.valor)
                ;
    }
}
