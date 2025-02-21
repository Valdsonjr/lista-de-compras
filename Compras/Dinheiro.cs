using System;
using System.Collections.Generic;

namespace Compras;

public readonly struct Dinheiro
{
    private readonly long _valor;

    private Dinheiro(decimal valor)
    {
        _valor = Convert.ToInt64(valor * 100);
    }

    private Dinheiro(long valor)
    {
        _valor = valor;
    }

    public static implicit operator Dinheiro(decimal valor)
        => new(valor);

    public static Dinheiro operator +(Dinheiro valor1, Dinheiro valor2)
        => new(valor1._valor + valor2._valor);

    public static Dinheiro operator *(Dinheiro valor1, byte multiplicador)
        => new(valor1._valor * multiplicador);

    public override string ToString()
        => ((decimal)_valor/100).ToString("C");

    public IEnumerable<Dinheiro> DividirEntre(long divisor)
    {
        if (divisor <= 0)
            yield break;

        var quociente = Math.DivRem(_valor, divisor, out long resto);

        // infelizmente Enumerable.Range não funciona com long
        for (int i = 0; i < divisor; i++)
        {
            yield return i < resto ? new Dinheiro(quociente + 1) : new Dinheiro(quociente);
        }
    }
}