namespace RestWithASPNET.Data.Converter.Contract
{
    public interface IParser<O, D> // dois tipos genericos: Origem e Destino
    {
        D Parse (O origin);
        List<D> Parse (List<O> origin);
    }
}