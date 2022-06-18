using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public class RepositorioRequisicaoEmBancoDados
    {
        private string enderecoBanco =
            @"Data Source=(LOCALDB)\MSSQLLOCALDB;
              Initial Catalog=ControleMedicamentosDb;
              Integrated Security=True";

        private const string sqlInserir =
          @"INSERT INTO [TBREQUISICAO] 
                (
                    [MEDICAMENTO_ID],
                    [PACIENTE_ID],
                    [QTDMEDICAMENTO],
                    [DATA],
                    [FUNCIONARIO_ID]                    
	            )
	            VALUES
                (
                    @MEDICAMENTO_ID,
                    @PACIENTE_ID,
                    @QTDMEDICAMENTO,
                    @DATA,
                    @FUNCIONARIO_ID
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBREQUISICAO]	
		        SET
                    [MEDICAMENTO_ID] = @MEDICAMENTO_ID,
                    [PACIENTE_ID] = @PACIENTE_ID,
                    [QTDMEDICAMENTO] = @QTDMEDICAMENTO,
                    [DATA] = @DATA,
                    [FUNCIONARIO_ID] = @FUNCIONARIO_ID
		        WHERE
			        [ID] = @ID";


        private const string sqlExcluir =
           @"DELETE FROM [TBREQUISICAO]			        
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarPorId =
          @"SELECT 
                REQUISICAO.[ID],       
                REQUISICAO.[FUNCIONARIO_ID],
                REQUISICAO.[PACIENTE_ID],
                REQUISICAO.[MEDICAMENTO_ID],             
                REQUISICAO.[QTDMEDICAMENTO],      
                REQUISICAO.[DATA],   
                FUNCIONARIO.[NOME] AS NOME_FUNCIONARIO,              
                FUNCIONARIO.[LOGIN] AS LOGIN_FUNCIONARIO,                    
                FUNCIONARIO.[SENHA] AS SENHA_FUNCIONARIO, 
                PACIENTE.[NOME] AS NOME_PACIENTE,
                PACIENTE.[CARTAOSUS] AS CARTAOSUS_PACIENTE,
                MEDICAMENTO.[NOME] AS NOME_MEDICAMENTO,
                MEDICAMENTO.[DESCRICAO] AS DESCRICAO_MEDICAMENTO,
                MEDICAMENTO.[LOTE] AS LOTE_MEDICAMENTO,
                MEDICAMENTO.[VALIDADE] AS VALIDADE_MEDICAMENTO,
                MEDICAMENTO.[QUANTIDADEDISPONIVEL] AS QUANTIDADEDISPONIVEL_MEDICAMENTO
            FROM
                [TBREQUISICAO] AS REQUISICAO 
                INNER JOIN [TBFUNCIONARIO] AS FUNCIONARIO
                    ON REQUISICAO.FUNCIONARIO_ID = FUNCIONARIO.[ID]
                INNER JOIN [TBPACIENTE] AS PACIENTE 
                    ON REQUISICAO.PACIENTE_ID = PACIENTE.[ID]
                INNER JOIN [TBMEDICAMENTO] AS MEDICAMENTO
                    ON REQUISICAO.MEDICAMENTO_ID = MEDICAMENTO.[ID]
            WHERE
                REQUISICAO.[ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT 
                REQUISICAO.[ID],       
                REQUISICAO.[FUNCIONARIO_ID],
                REQUISICAO.[PACIENTE_ID],
                REQUISICAO.[MEDICAMENTO_ID],             
                REQUISICAO.[QTDMEDICAMENTO],      
                REQUISICAO.[DATA],   
                FUNCIONARIO.[NOME] AS NOME_FUNCIONARIO,              
                FUNCIONARIO.[LOGIN] AS LOGIN_FUNCIONARIO,                    
                FUNCIONARIO.[SENHA] AS SENHA_FUNCIONARIO, 
                PACIENTE.[NOME] AS NOME_PACIENTE,
                PACIENTE.[CARTAOSUS] AS CARTAOSUS_PACIENTE,
                MEDICAMENTO.[NOME] AS NOME_MEDICAMENTO,
                MEDICAMENTO.[DESCRICAO] AS DESCRICAO_MEDICAMENTO,
                MEDICAMENTO.[LOTE] AS LOTE_MEDICAMENTO,
                MEDICAMENTO.[VALIDADE] AS VALIDADE_MEDICAMENTO,
                MEDICAMENTO.[QUANTIDADEDISPONIVEL] AS QUANTIDADEDISPONIVEL_MEDICAMENTO
            FROM
                [TBREQUISICAO] AS REQUISICAO
                INNER JOIN [TBFUNCIONARIO] AS FUNCIONARIO
                    ON REQUISICAO.FUNCIONARIO_ID = FUNCIONARIO.[ID]
                INNER JOIN [TBPACIENTE] AS PACIENTE 
                    ON REQUISICAO.PACIENTE_ID = PACIENTE.[ID]
                INNER JOIN [TBMEDICAMENTO] AS MEDICAMENTO
                    ON REQUISICAO.MEDICAMENTO_ID = MEDICAMENTO.[ID]";



        public ValidationResult Inserir(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            requisicao.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Requisicao requisicao)
        {
            var validador = new ValidadorRequisicao();

            var resultadoValidacao = validador.Validate(requisicao);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosRequisicao(requisicao, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Requisicao requisicao)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", requisicao.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public Requisicao SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            Requisicao requisicao = null;
            if (leitorMedicamento.Read())
                requisicao = ConverterParaRequisicao(leitorMedicamento);

            conexaoComBanco.Close();

            return requisicao;
        }

        public List<Requisicao> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorRequisicao = comandoSelecao.ExecuteReader();

            List<Requisicao> requisicoes = new List<Requisicao>();

            while (leitorRequisicao.Read())
                requisicoes.Add(ConverterParaRequisicao(leitorRequisicao));

            conexaoComBanco.Close();

            return requisicoes;
        }

        private void ConfigurarParametrosRequisicao(Requisicao requisicao, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", requisicao.Id);
            comando.Parameters.AddWithValue("MEDICAMENTO_ID", requisicao.Medicamento.Id);
            comando.Parameters.AddWithValue("PACIENTE_ID", requisicao.Paciente.Id);
            comando.Parameters.AddWithValue("QTDMEDICAMENTO", requisicao.QtdMedicamento);
            comando.Parameters.AddWithValue("DATA", requisicao.Data);
            comando.Parameters.AddWithValue("FUNCIONARIO_ID", requisicao.Funcionario.Id);
        }

        private Requisicao ConverterParaRequisicao(SqlDataReader leitorRequisicao)
        {
            int id = Convert.ToInt32(leitorRequisicao["ID"]);
            int qtdMedicamento = Convert.ToInt32(leitorRequisicao["QTDMEDICAMENTO"]);
            DateTime data = Convert.ToDateTime(leitorRequisicao["DATA"]);

            var requisicao = new Requisicao()
            {
                Id = id,
                QtdMedicamento = qtdMedicamento,
                Data = data
            };

            int idMedicamento = Convert.ToInt32(leitorRequisicao["MEDICAMENTO_ID"]);
            string nomeMedicamento = Convert.ToString(leitorRequisicao["NOME_MEDICAMENTO"]);
            string descricaoMedicamento = Convert.ToString(leitorRequisicao["DESCRICAO_MEDICAMENTO"]);
            string loteMedicamento = Convert.ToString(leitorRequisicao["LOTE_MEDICAMENTO"]);
            DateTime validadeMedicamento = Convert.ToDateTime(leitorRequisicao["VALIDADE_MEDICAMENTO"]);

            requisicao.Medicamento = new Medicamento()
            {
                Id = idMedicamento,
                Nome = nomeMedicamento,
                Descricao = descricaoMedicamento,
                Lote = loteMedicamento,
                Validade = validadeMedicamento
            };

            int idPaciente = Convert.ToInt32(leitorRequisicao["PACIENTE_ID"]);
            string nomePaciente = Convert.ToString(leitorRequisicao["NOME_PACIENTE"]);
            string cartaoSusPaciente = Convert.ToString(leitorRequisicao["CARTAOSUS_PACIENTE"]);

            requisicao.Paciente = new Paciente()
            {
                Id = idPaciente,
                Nome = nomePaciente,
                CartaoSUS = cartaoSusPaciente
            };

            int idFuncionario = Convert.ToInt32(leitorRequisicao["FUNCIONARIO_ID"]);
            string nomeFuncionario = Convert.ToString(leitorRequisicao["NOME_FUNCIONARIO"]);
            string loginFuncionario = Convert.ToString(leitorRequisicao["LOGIN_FUNCIONARIO"]);
            string senhaFuncionario = Convert.ToString(leitorRequisicao["SENHA_FUNCIONARIO"]);


            requisicao.Funcionario = new Funcionario()
            {
                Id = idFuncionario,
                Nome = nomeFuncionario,
                Login = loginFuncionario,
                Senha = senhaFuncionario
            };

            return requisicao;
        }
    }
}
