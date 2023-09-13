using AutomacaoFolhaPagamento.Models;
using Microsoft.Data.SqlClient;

namespace AutomacaoFolhaPagamento.Repository
{
    public class ObterPagamentoRepository
    {

        public IEnumerable<HistPagamentoModel> ObterPagamentosFiltrados(int mes)
        {
            var todosPagamentos = ObterPagamentos();

            if (mes != 13)  // Se não for "Todos"
            {
                return todosPagamentos.Where(p => p.DataPagamento.Month == mes);
            }

            return todosPagamentos;
        }
    





        public List<HistPagamentoModel> ObterPagamentos()
        {
            string connectionString = @"Data Source=JESSICAOM-NB\MSSQLSERVER01;Initial Catalog=Folha_Pagamento;Integrated Security=True;Encrypt=False";


            List<HistPagamentoModel> pagamentos = new List<HistPagamentoModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT [id_hist], [id_funcionario], [data_pagamento], [salario_base], [hora_ex], [beneficios], [total_liq] 
                         FROM [Folha_Pagamento].[dbo].[tb_histpagment]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HistPagamentoModel pagamento = new HistPagamentoModel
                            {
                                IdHist = reader.GetInt32(0),
                                IdFuncionario = reader.GetInt32(1),
                                DataPagamento = reader.GetDateTime(2),
                                SalarioBase = reader.GetDecimal(3),
                                HoraEx = reader.GetTimeSpan(4),
                                Beneficios = (float)reader.GetDouble(5),
                                TotalLiq = (float)reader.GetDouble(6)
                            };
                            pagamentos.Add(pagamento);
                        }
                    }
                }
            }
            return pagamentos;
        }
    }
}
