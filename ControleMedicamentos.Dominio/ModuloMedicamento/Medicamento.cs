using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    public class Medicamento : EntidadeBase<Medicamento>
    {        
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public int QuantidadeDisponivel { get; set; }

        public List<Requisicao> Requisicoes { get; set; }

        public Fornecedor Fornecedor{ get; set; }

        public int QuantidadeRequisicoes { get { return Requisicoes.Count; } }

        public Medicamento(string nome, string descricao, string lote, DateTime validade, Fornecedor fornecedor)
        {
            Nome = nome;
            Descricao = descricao;
            Lote = lote;
            Validade = validade;
            Requisicoes = new List<Requisicao>();
            Fornecedor = fornecedor;
        }

        public Medicamento()
        {
        }
        public override bool Equals(object obj)
        {
            Medicamento medicamento = obj as Medicamento;

            if (medicamento == null)
                return false;

            return
                medicamento.Id.Equals(Id) &&
                medicamento.Nome.Equals(Nome) &&
                medicamento.Descricao.Equals(Descricao) &&
                medicamento.Fornecedor.Id.Equals(Fornecedor.Id) &&
                medicamento.Validade.Equals(Validade);

        }
    }
}
