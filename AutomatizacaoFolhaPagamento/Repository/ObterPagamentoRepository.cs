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







        public List<DashboardModel> ObterPagamentoMensal()
        {
            string connectionString = @"Data Source=JESSICAOM-NB\MSSQLSERVER01;Initial Catalog=Folha_Pagamento;Integrated Security=True;Encrypt=False";


            List<DashboardModel> pagamentos = new List<DashboardModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    DATENAME(MONTH, data_pagamento) + '/' + DATENAME(YEAR, data_pagamento) AS nome_mes,
                                    SUM(total_liq) AS somatoria_total_liquido
                                FROM [dbo].[tb_histpagment]
                                GROUP BY YEAR(data_pagamento), MONTH(data_pagamento), DATENAME(MONTH, data_pagamento), DATENAME(YEAR, data_pagamento)
                                ORDER BY YEAR(data_pagamento), MONTH(data_pagamento)
                                ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DashboardModel pagamento = new DashboardModel
                            {
                                texto = reader.GetString(0),
                                TotalLiq = (float)reader.GetDouble(1)
                            };
                            pagamentos.Add(pagamento);
                        }
                    }
                }
            }
            return pagamentos;
        }





        public List<DashboardModel> ObterPagamentosDepartamento()
        {
            string connectionString = @"Data Source=JESSICAOM-NB\MSSQLSERVER01;Initial Catalog=Folha_Pagamento;Integrated Security=True;Encrypt=False";


            List<DashboardModel> pagamentos = new List<DashboardModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT f.departamento,
                                   
                                    SUM(total_liq) AS somatoria_total_liquido
                                FROM [dbo].[tb_histpagment] h
								inner join tb_funcionario f on h.id_funcionario = f.id_funcionario

                                GROUP BY f.departamento
                                ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DashboardModel pagamentoDep = new DashboardModel
                            {
                                texto = reader.GetString(0),
                                TotalLiq = (float)reader.GetDouble(1)
                            };
                            pagamentos.Add(pagamentoDep);
                        }
                    }
                }
            }
            return pagamentos;
        }
    }
}
