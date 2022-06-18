using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
        private string enderecoBanco =
            @"Data Source=(LOCALDB)\MSSQLLOCALDB;
              Initial Catalog=ControleMedicamentosDb;
              Integrated Security=True";

        private const string sqlInserir =
          @"INSERT INTO [TBMEDICAMENTO] 
                (
                    [NOME],
                    [DESCRICAO],
                    [LOTE],
                    [VALIDADE],
                    [QUANTIDADEDISPONIVEL],                    
                    [FORNECEDOR_ID],
                    [QUANTIDADEREQUISICAO]
	            )
	            VALUES
                (
                    @NOME,
                    @DESCRICAO,
                    @LOTE,
                    @VALIDADE,
                    @QUANTIDADEDISPONIVEL,                    
                    @FORNECEDOR_ID,
                    @QUANTIDADEREQUISICAO
                );SELECT SCOPE_IDENTITY();";

        private const string sqlEditar =
           @"UPDATE [TBMEDICAMENTO]	
		        SET
			        [NOME] = @NOME,
			        [DESCRICAO] = @DESCRICAO,
                    [LOTE] = @LOTE,
                    [VALIDADE] = @VALIDADE,
                    [QUANTIDADEDISPONIVEL] = @QUANTIDADEDISPONIVEL,                    
                    [FORNECEDOR_ID] = @FORNECEDOR_ID,
                    [QUANTIDADEREQUISICAO] = @QUANTIDADEREQUISICAO
		        WHERE
			        [ID] = @ID";


        private const string sqlExcluir =
           @"DELETE FROM [TBMEDICAMENTO]			        
		        WHERE
			        [ID] = @ID";

        private const string sqlSelecionarPorId =
          @"SELECT 
                MED.[ID],       
                MED.[NOME],
                MED.[DESCRICAO],
                MED.[LOTE],             
                MED.[VALIDADE],                    
                MED.[QUANTIDADEDISPONIVEL],                                
                MED.[FORNECEDOR_ID],
                FORNE.[NOME] AS FORNECEDOR_NOME,              
                FORNE.[TELEFONE],                    
                FORNE.[EMAIL], 
                FORNE.[CIDADE],
                FORNE.[ESTADO]
            FROM
                [TBMEDICAMENTO] AS MED LEFT JOIN 
                [TBFORNECEDOR] AS FORNE
            ON
                FORNE.ID = MED.FORNECEDOR_ID
            WHERE 
                MED.[ID] = @ID";

        private const string sqlSelecionarTodos =
          @"SELECT 
                MED.[ID],       
                MED.[NOME],
                MED.[DESCRICAO],
                MED.[LOTE],             
                MED.[VALIDADE],                    
                MED.[QUANTIDADEDISPONIVEL],                                
                MED.[FORNECEDOR_ID],
                FORNE.[NOME] AS FORNECEDOR_NOME,              
                FORNE.[TELEFONE],                    
                FORNE.[EMAIL], 
                FORNE.[CIDADE],
                FORNE.[ESTADO]
            FROM
                [TBMEDICAMENTO] AS MED LEFT JOIN 
                [TBFORNECEDOR] AS FORNE
            ON
                FORNE.ID = MED.FORNECEDOR_ID";


        public ValidationResult Inserir(Medicamento medicamento)
        {
            var validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(medicamento);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosMedicamento(medicamento, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            medicamento.Id = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Editar(Medicamento medicamento)
        {
            var validador = new ValidadorMedicamento();

            var resultadoValidacao = validador.Validate(medicamento);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosMedicamento(medicamento, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public void Excluir(Medicamento medicamento)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("ID", medicamento.Id);

            conexaoComBanco.Open();
            comandoExclusao.ExecuteNonQuery();
            conexaoComBanco.Close();
        }

        public Medicamento SelecionarPorId(int id)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("ID", id);

            conexaoComBanco.Open();
            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            Medicamento medicamento = null;
            if (leitorMedicamento.Read())
                medicamento = ConverterParaMedicamento(leitorMedicamento);

            conexaoComBanco.Close();

            return medicamento;
        }

        public List<Medicamento> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);
            conexaoComBanco.Open();

            SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

            List<Medicamento> medicamentos = new List<Medicamento>();

            while (leitorMedicamento.Read())
                medicamentos.Add(ConverterParaMedicamento(leitorMedicamento));

            conexaoComBanco.Close();

            return medicamentos;
        }

        private void ConfigurarParametrosMedicamento(Medicamento medicamento, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("ID", medicamento.Id);
            comando.Parameters.AddWithValue("NOME", medicamento.Nome);
            comando.Parameters.AddWithValue("DESCRICAO", medicamento.Descricao);
            comando.Parameters.AddWithValue("LOTE", medicamento.Lote);
            comando.Parameters.AddWithValue("VALIDADE", medicamento.Validade);
            comando.Parameters.AddWithValue("QUANTIDADEDISPONIVEL", medicamento.QuantidadeDisponivel);            
            comando.Parameters.AddWithValue("FORNECEDOR_ID", medicamento.Fornecedor.Id);
            comando.Parameters.AddWithValue("QUANTIDADEREQUISICAO", medicamento.Requisicoes.Count);

        }

        private Medicamento ConverterParaMedicamento(SqlDataReader leitorMedicamento)
        {


            int id = Convert.ToInt32(leitorMedicamento["ID"]);
            string nome = Convert.ToString(leitorMedicamento["NOME"]);
            string descricao = Convert.ToString(leitorMedicamento["DESCRICAO"]);
            string lote = Convert.ToString(leitorMedicamento["LOTE"]);
            DateTime validade = Convert.ToDateTime(leitorMedicamento["VALIDADE"]);

            var medicamento = new Medicamento()
            {
                Id = id,
                Nome = nome,
                Descricao = descricao,
                Lote = lote,
                Validade = validade
            };

            int idFornecedor = Convert.ToInt32(leitorMedicamento["FORNECEDOR_ID"]);
            string nomeFornecedor = Convert.ToString(leitorMedicamento["FORNECEDOR_NOME"]);
            string telefone = Convert.ToString(leitorMedicamento["TELEFONE"]);
            string email = Convert.ToString(leitorMedicamento["EMAIL"]);
            string cidade = Convert.ToString(leitorMedicamento["CIDADE"]);
            string estado = Convert.ToString(leitorMedicamento["ESTADO"]);

            medicamento.Fornecedor = new Fornecedor()
            {
                Id = idFornecedor,
                Nome = nomeFornecedor,
                Telefone = telefone,
                Email = email,
                Cidade = cidade,
                Estado = estado
            };

            return medicamento;
        }
    }
}
