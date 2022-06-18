using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
        private Medicamento medicamento;
        private RepositorioMedicamentoEmBancoDados repositorioMedicamento;

        private Fornecedor fornecedor;
        private RepositorioFornecedorEmBancoDados repositorioFornecedor;

        public RepositorioMedicamentoEmBancoDadosTest()
        {
            Db.ExecutarSql("DELETE FROM TBREQUISICAO; DBCC CHECKIDENT (TBREQUISICAO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFUNCIONARIO; DBCC CHECKIDENT (TBFUNCIONARIO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBPACIENTE; DBCC CHECKIDENT (TBPACIENTE, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBMEDICAMENTO; DBCC CHECKIDENT (TBMEDICAMENTO, RESEED, 0)");
            Db.ExecutarSql("DELETE FROM TBFORNECEDOR; DBCC CHECKIDENT (TBFORNECEDOR, RESEED, 0)");

            fornecedor = new Fornecedor("Fornecedor1", "(49) 99999-9999", "fornecedor@email.com", "Lages", "SC");
            medicamento = new Medicamento("DorFlex", "Dor de Cabeça", "1", DateTime.Today.AddYears(1), fornecedor);

            repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            repositorioFornecedor = new RepositorioFornecedorEmBancoDados();
        }

        [TestMethod]
        public void Deve_inserir_novo_medicamento()
        {
            //action
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);

            //assert
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(medicamento.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_editar_informacoes_requisicao()
        {
            //arrange
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);

            //action
            medicamento.Nome = "Advil";
            medicamento.Lote = "1";
            medicamento.QuantidadeDisponivel = 10;
            medicamento.Descricao = "Dor no corpo";

            repositorioMedicamento.Editar(medicamento);

            //assert
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(fornecedor.Id);

            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_excluir_fornecedor()
        {
            //arrange
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);

            //action           
            repositorioMedicamento.Excluir(medicamento);

            //assert
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(medicamento.Id);
            Assert.IsNull(medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_apenas_um_requisicao()
        {
            //arrange          
            repositorioFornecedor.Inserir(fornecedor);
            repositorioMedicamento.Inserir(medicamento);

            //action
            var medicamentoEncontrado = repositorioMedicamento.SelecionarPorId(medicamento.Id);

            //assert
            Assert.IsNotNull(medicamentoEncontrado);
            Assert.AreEqual(medicamento, medicamentoEncontrado);
        }

        [TestMethod]
        public void Deve_selecionar_todos_um_medicamentos()
        {
            repositorioFornecedor.Inserir(fornecedor);

            //arrange
            var f01 = new Medicamento("DorFlex", "Dor de Cabeça", "1", DateTime.Today.AddYears(1), fornecedor);

            var repositorio = new RepositorioMedicamentoEmBancoDados();
            repositorio.Inserir(f01);

            //action
            var fornecedores = repositorio.SelecionarTodos();

            //assert

            Assert.AreEqual(1, fornecedores.Count);

            Assert.AreEqual(f01.Nome, fornecedores[0].Nome);

        }


    }
}
