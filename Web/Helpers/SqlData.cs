using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Helpers
{
    //Classe de conexão com o SQL Server //Bertuzzi
    public class SqlData
    {
        private string _ConnectionString = string.Empty;
        private static IConfigurationRoot _Configuration { get; set; }


        public SqlData()
        {
            //Obtem as Configs do Arquivo appsettings
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            //Obtem Serializa o json
            _Configuration = builder.Build();

            //Obtem a propriedade configurada
            _ConnectionString = _Configuration["ConnectionString"].ToString();
        }
        /// <summary>Retorna resultado de um Select SQL.
        /// </summary>
        /// <para name="SQL">Script a Ser Executado</para>
        /// <returns>Retorna o Conteudo da Consulta</returns>
        public DataTable ExecutaSelect(string sql)
        {
            try
            {
                //Auto Dispose para a Conexão. Isto Libera a Conexão para Outro POOL.
                using (SqlConnection Conn = new SqlConnection(_ConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, Conn))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            da.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>Executa um Comando SQL.
        /// </summary>
        /// <para name="SQL">Script a Ser Executado</para>
        /// <returns>Retorna Quantidade de Linhas Afetadas</returns>
        public int ExecutaQuery(string sql, int Timeout = 300)
        {
            try
            {
                //Auto Dispose para a Conexão. Isto Libera a Conexão para Outro POOL.
                using (SqlConnection Conn = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand Cmd = new SqlCommand(sql, Conn))
                    {
                        Cmd.Connection.Open();
                        Cmd.CommandType = CommandType.Text;
                        Cmd.CommandText = sql;
                        Cmd.CommandTimeout = Timeout;
                        return Cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>Executa um Comando SQL.
        /// </summary>
        /// <para name="SQL">Script a Ser Executado</para>
        /// <para name="parametros">Script a Ser Executado</para>
        /// <returns>Retorna Quantidade de Linhas Afetadas</returns>
        public int ExecutaQuery(string sql, List<SqlParameter> parametros)
        {
            try
            {
                using (SqlConnection Conn = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand Cmd = new SqlCommand(sql, Conn))
                    {
                        Cmd.Parameters.Clear();

                        foreach (SqlParameter parametro in parametros)
                        {
                            Cmd.Parameters.Add(parametro);
                        }

                        Cmd.CommandText = sql.ToString();
                        Cmd.Connection = Conn;
                        Cmd.Connection.Open();
                        Cmd.ExecuteNonQuery();

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>Executa um Comando SQL.
        /// </summary>
        /// <para name="SQL">Script a Ser Executado</para>
        /// <para name="Timeout">Timeout</para>
        /// <returns>Retorna ID Após o insert</returns>
        public string ExecuteQueryIdentity(string sql, int timeout = 300)
        {
            try
            {

                using (SqlConnection Conn = new SqlConnection(_ConnectionString))
                {
                    using (SqlCommand Cmd = new SqlCommand(sql, Conn))
                    {
                        Cmd.Connection.Open();
                        Cmd.CommandType = CommandType.Text;
                        Cmd.CommandTimeout = timeout;
                        Cmd.ExecuteNonQuery();
                        Cmd.CommandText = "select @@identity";

                        return Cmd.ExecuteScalar().ToString();
                    }
                }

            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
            }
            finally
            {

            }

        }
    }
}
