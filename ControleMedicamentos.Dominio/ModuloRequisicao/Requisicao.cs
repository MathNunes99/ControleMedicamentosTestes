using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using System;
using System.Collections.Generic;

namespace ControleMedicamentos.Dominio.ModuloRequisicao
{
    public class Requisicao : EntidadeBase<Requisicao>
    {
        public Medicamento Medicamento { get; set; }
        public Paciente Paciente { get; set; }
        public int QtdMedicamento { get; set; }
        public DateTime Data { get; set; }
        public Funcionario Funcionario { get; set; }

        public Requisicao(Medicamento medicamento, Paciente paciente, int qtd, DateTime data, Funcionario funcionario)
        {
            Medicamento = medicamento;
            Paciente = paciente;
            QtdMedicamento = qtd;
            Data = data;
            Funcionario = funcionario;
        }

        public Requisicao()
        {
        }

        public override bool Equals(object obj)
        {
            Requisicao requisicao = obj as Requisicao;

            if (requisicao == null)
                return false;

            return
                requisicao.Id.Equals(Id) &&
                requisicao.Medicamento.Id.Equals(Medicamento.Id) &&
                requisicao.Paciente.Id.Equals(Paciente.Id) &&
                requisicao.QtdMedicamento.Equals(QtdMedicamento) &&
                requisicao.Data.Equals(Data) &&
                requisicao.Funcionario.Id.Equals(Funcionario.Id);
        }
    }
}
