namespace MeuProjeto.Domain.Common;

public class Result
{
    public bool Sucesso { get; }
    public string? Erro { get; }
    public bool Falhou => !Sucesso;

    protected Result(bool sucesso, string? erro)
    {
        Sucesso = sucesso;
        Erro = erro;
    }

    public static Result Ok() => new(true, null);
    public static Result Falha(string erro) => new(false, erro);
    public static Result<T> Ok<T>(T valor) => new(valor, true, null);
    public static Result<T> Falha<T>(string erro) => new(default, false, erro);
}

public class Result<T> : Result
{
    public T? Valor { get; }

    protected internal Result(T? valor, bool sucesso, string? erro) : base(sucesso, erro)
    {
        Valor = valor;
    }
}
