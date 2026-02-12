using MeuProjeto.Domain.Common;

namespace MeuProjeto.Domain.Entities;

public class Parcela : BaseEntity
{
    public Guid ProjetoId { get; private set; }
    public int Numero { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime Vencimento { get; private set; }
    public DateTime? PagoEm { get; private set; }

    private Parcela() { }

    public static Result<Parcela> Criar(Guid projetoId, int numero, decimal valor, DateTime vencimento)
    {
        if (numero <= 0)
            return Result.Falha<Parcela>("Número da parcela deve ser maior que zero.");

        if (valor <= 0)
            return Result.Falha<Parcela>("Valor da parcela deve ser maior que zero.");

        return Result.Ok(new Parcela
        {
            ProjetoId = projetoId,
            Numero = numero,
            Valor = valor,
            Vencimento = vencimento
        });
    }

    public Result MarcarPago()
    {
        if (PagoEm.HasValue)
            return Result.Falha("Parcela já está paga.");

        PagoEm = DateTime.UtcNow;
        MarcarAtualizado();
        return Result.Ok();
    }

    public bool Atrasada => !PagoEm.HasValue && DateTime.UtcNow.Date > Vencimento.Date;
}
