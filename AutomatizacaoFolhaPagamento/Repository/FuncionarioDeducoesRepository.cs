using AutomacaoFolhaPagamento.Models;
using Microsoft.Data.SqlClient;

namespace AutomacaoFolhaPagamento.Repository
{
    public class FuncionarioDeducoesRepository
    {

        public List<FuncionarioDeducoes> ObterFuncionarioDeducoes()
        {
            string connectionString = @"Data Source=JESSICAOM-NB\MSSQLSERVER01;Initial Catalog=Folha_Pagamento;Integrated Security=True;Encrypt=False";

            List<FuncionarioDeducoes> funcionarioDeducoesList = new List<FuncionarioDeducoes>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"select f.nome_funcionario, f.cpf, f.cargo, f.departamento, a.inss, 
                                a.FGTS, a.IR, a.VR, a.VT, a.Valor_liquido, a.salario, a.descontos 
                                from tb_deducoes a
                                inner join tb_funcionario f on f.id_funcionario = a.id_funcionario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FuncionarioDeducoes data = new FuncionarioDeducoes
                            {
                                nomeFuncionario = reader.GetString(0),
                                cpf = reader.GetString(1),
                                cargo = reader.GetString(2),
                                departamento = reader.GetString(3),
                                inss = reader.GetDecimal(4),
                                fgts = reader.GetDecimal(5),
                                ir = reader.GetDecimal(6),
                                vr = reader.GetDecimal(7),
                                vt = reader.GetDecimal(8),
                                valorLiquido = reader.GetDecimal(9),
                                salario = reader.GetDecimal(10),
                                descontos = reader.GetDecimal(11)
                            };
                            funcionarioDeducoesList.Add(data);
                        }
                    }
                }
            }

            return funcionarioDeducoesList;
        }

      
        public List<Funcionario> ObterFuncionariosBasicos()
        {
            string connectionString = @"Data Source=JESSICAOM-NB\MSSQLSERVER01;Initial Catalog=Folha_Pagamento;Integrated Security=True;Encrypt=False";

            List<Funcionario> funcionarios = new List<Funcionario>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT nome_funcionario, cpf FROM tb_funcionario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Funcionario funcionario = new Funcionario
                            {
                                nomeFuncionario = reader.GetString(0),
                                cpf = reader.GetString(1)
                            };
                            funcionarios.Add(funcionario);
                        }
                    }
                }
            }

            return funcionarios;
        }

    }
}
