using Compras;
using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Testes;

public class TestesUnitarios
{
    private readonly ITestOutputHelper output;

    public TestesUnitarios(ITestOutputHelper output)
    {
        this.output = output;
    }

    /// <summary>
    /// Teste simples para verificar que os valores estão corretos
    /// </summary>
    [Fact]
    public void TesteDivisaoNaoPerdeDinheiro()
    {
        Dinheiro dinheiro = 1.00M;
        var divisao = dinheiro.DividirEntre(3);

        Assert.Collection(divisao
            , item => Assert.Equal(0.34M, item)
            , item => Assert.Equal(0.33M, item)
            , item => Assert.Equal(0.33M, item));
    }

    /// <summary>
    /// teste da propriedade:
    ///
    /// a soma da lista da divisão (quocientes * divisor + resto) sempre será igual ao dividendo
    /// </summary>
    /// <param name="valor"></param>
    /// <param name="divisor"></param>
    [Property(MaxTest = 10000)]
    public Property TesteDivisaoNaoPerdeDinheiro2(decimal valor, uint divisor)
    {
        Dinheiro dinheiro = valor;
        var divisao = dinheiro.DividirEntre(divisor);
        Func<bool> property = () => dinheiro.Equals(divisao.Aggregate((Dinheiro)0, (acc, novo) => acc + novo));
        return property.When(valor > 0 && divisor > 0);
    }


    /// <summary>
    /// Qualquer valor dividido por 0 é indefinido,
    /// aqui eu retorno uma lista vazia
    /// </summary>
    /// <param name="valor">dinheiro</param>
    [Property(MaxTest = 10000)]
    public Property TesteDivisaoPorZeroRetornaListaVazia(decimal valor)
    {
        Dinheiro dinheiro = valor;
        var divisao = dinheiro.DividirEntre(0);
        return (!divisao.Any()).ToProperty();
    }

    [Fact]
    public void TesteTecnico()
    {
        var compras = new List<(Item item, byte quantidade)>
        {
            //(new Item { Nome = "item1", PrecoUnitario = 1.99M }, 10),
            //(new Item { Nome = "item2", PrecoUnitario = 0.02M }, 5),
            (new Item { Nome = "item3", PrecoUnitario = 0.01M }, 3)
        };

        var emails = new List<string>
        {
            "email1", "email2", "email3", "email4"
        };

        var resultado = CarrinhoDeCompras.CalcularValorPorEmail(compras, emails);

        // a função não imprime nada se a lista de e-mails estiver vazia
        resultado.ToList().ForEach(kp => output.WriteLine($"{kp.Key} = {kp.Value}"));
            
        // mas também não joga nenhuma exceção
        Assert.True(true);
    }
}