using Npgsql;

namespace Discount.API.Extensions
{
    public static class InitializeDB
    {
        /// <summary>
        /// Responsável por executar query no banco PostgresSQL
        /// Previamente deve ser criado o Server no Servidor Postgres com o mesmo informado na String de conexão
        /// </summary>
        /// <param name="builder"></param>
        public static void InitializePostgres(this WebApplicationBuilder builder)
        {
            try {                
                var strConn = builder?.Configuration?.GetValue<string>("DatabaseSettings:ConnectionString");
                var initializeDb = builder?.Configuration?.GetValue<bool>("DatabaseSettings:Initializedb");
   
                if (initializeDb.HasValue && initializeDb.Value)
                {
                    var m_conn = new NpgsqlConnection(strConn);
                    m_conn.CreateTables();
                }
            }
            catch (Exception ex) {
                Console.WriteLine("--- Falha ao criar tabelas no banco de dados");
                Console.WriteLine($"Message Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private static void CreateTables(this NpgsqlConnection m_conn)
        {
            var tableCoupon = @$"CREATE TABLE IF NOT EXISTS Coupon(
                            Id SERIAL PRIMARY KEY,
				            ProductName VARCHAR(24) NOT NULL,
				            Description TEXT,
				            Amount INT);";

            var createdb = new NpgsqlCommand($"{tableCoupon}", m_conn);

            m_conn.Open();
            createdb.ExecuteNonQuery();
            m_conn.Close();
        }

        private static void CreateDatabase(this NpgsqlConnection m_conn, string? nameDb)
        {
            var createdb = new NpgsqlCommand(@$"
                    CREATE DATABASE IF NOT EXISTS {nameDb}
                        OWNER = {nameDb}
                        ENCODING = 'UTF8'
                        CONNECTION LIMIT = -1"
                    , m_conn);

            m_conn.Open();
            createdb.ExecuteNonQuery();
            m_conn.Close();
        }
    }
}
