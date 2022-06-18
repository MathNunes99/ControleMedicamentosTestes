using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDadosTest
    {
        private Requisicao requisicao;
        private RepositorioRequisicaoEmBancoDados repositorio;

        private Medicamento medicamento;
        private RepositorioMedicamentoEmBancoDados repositorioMedicamento;

        private Paciente paciente;
        private RepositorioPacienteEmBancoDados repositorioPaciente;

        private Fornecedor fornecedor;
        private RepositorioFornecedorEmBancoDados repositorioFornecedor;

        private Funcionario funcionario;
        private RepositorioFuncionarioEmBancoDados repositorioFuncionario;

        public RepositorioRequisicaoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            fornecedor = new Fornecedor("Fornecedor1", "(49) 99999-9999", "fornecedor@email.com", "Lages", "SC");
            medicamento = new Medicamento("DorFlex", "Dor de Cabeça", "1", DateTime.Today.AddYears(1),fornecedor);
            paciente = new Paciente("Joao", "321654987");            
            funcionario = new Funcionario("Joao Funcionario", "Joao123", "joaoSenha");            
            requisicao = new Requisicao(medicamento, paciente, 20, DateTime.Today, funcionario);

            repositorio = new RepositorioRequisicaoEmBancoDados();
            repositorioPaciente = new RepositorioPacienteEmBancoDados();
            repositorioFuncionario = new RepositorioFuncionarioEmBancoDados();
            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();

        }

        [TestMethod]
        public void Deve_inserir_novo_requisicao()
        {
            //action
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioPaciente.Inserir(paciente);            
            repositorioFuncionario.Inserir(funcionario);
            repositorio.Inserir(requisicao);

            //assert
            var requisicaoEncontrado = repositorio.SelecionarPorId(requisicao.Id);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao, requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_requisicao()
        {
            //arrange
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioPaciente.Inserir(paciente);            
            repositorioFuncionario.Inserir(funcionario);
            repositorio.Inserir(requisicao);

            //action
            requisicao.Medicamento.Nome = "Advil";
            requisicao.Paciente.Nome = "Jorge";
            requisicao.Funcionario.Nome = "Matheus";

            
            repositorio.Editar(requisicao);

            //assert
            var requisicaoEncontrado = repositorio.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao, requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {
            //arrange
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioPaciente.Inserir(paciente);
            repositorioFuncionario.Inserir(funcionario);
            repositorio.Inserir(requisicao);

            //action           
            repositorio.Excluir(requisicao);

            //assert
            var requisicaoEncontrado = repositorio.SelecionarPorId(requisicao.Id);
            Assert.IsNull(requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_requisicao()
        {
            //arrange          
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioPaciente.Inserir(paciente);
            repositorioFuncionario.Inserir(funcionario);
            repositorio.Inserir(requisicao);

            //action
            var requisicaoEncontrado = repositorio.SelecionarPorId(requisicao.Id);

            //assert
            Assert.IsNotNull(requisicaoEncontrado);
            Assert.AreEqual(requisicao, requisicaoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_requisicoes()
        {
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);
            repositorioPaciente.Inserir(paciente);
            repositorioFuncionario.Inserir(funcionario);            

            //arrange
            var f01 = new Requisicao(medicamento, paciente, 20, DateTime.Today, funcionario);

            var repositorio = new RepositorioRequisicaoEmBancoDados();
            repositorio.Inserir(f01);

            //action
            var requisicoes = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(1, requisicoes.Count);

            Assert.AreEqual(f01.Id, requisicoes[0].Id);

        }
    }
}
