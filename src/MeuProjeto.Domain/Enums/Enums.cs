namespace MeuProjeto.Domain.Enums;

public enum StatusProjeto
{
    Orcamento = 0,
    Aprovado = 1,
    EmProducao = 2,
    ProntoEntrega = 3,
    Entregue = 4,
    Instalado = 5,
    Concluido = 6,
    Cancelado = 99
}

public enum StatusPagamento
{
    Pendente = 0,
    ParcialmentePago = 1,
    Pago = 2,
    Atrasado = 3,
    Cancelado = 99
}

public enum TipoArquivo
{
    ImagemProjeto = 0,
    ImagemProducao = 1,
    Documento = 2,
    Contrato = 3,
    NotaFiscal = 4
}

public enum PlanoAssinatura
{
    Trial = 0,
    Basico = 1,
    Profissional = 2
}
