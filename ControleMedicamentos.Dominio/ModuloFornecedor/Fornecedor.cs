namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class Fornecedor : EntidadeBase<Fornecedor>
    {
        public Fornecedor()
        {
        }

        public Fornecedor(string nome, string telefone, string email, string cidade, string estado)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            Cidade = cidade;
            Estado = estado;
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public override bool Equals(object obj)
        {
            Fornecedor fornecedor = obj as Fornecedor;

            if (fornecedor == null)
                return false;

            return
                fornecedor.Id.Equals(Id) &&
                fornecedor.Nome.Equals(Nome) &&
                fornecedor.Telefone.Equals(Telefone);
                fornecedor.Email.Equals(Email);
                fornecedor.Cidade.Equals(Cidade);
                fornecedor.Estado.Equals(Estado);
        }
    }
}
