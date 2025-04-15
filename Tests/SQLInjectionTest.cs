using System;
using System.Data.SqlClient;

namespace Tests
{
    class SQLInjectionTest
    {
        static void Main(string[] args)
        {
            string userInput = Console.ReadLine();
            string connectionString = "Data Source=localhost;Initial Catalog=TestDb;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // ❌ Vulnerable: SQL Injection
                string query = "SELECT * FROM Users WHERE username = '" + userInput + "'";
                SqlCommand command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                // ✅ Safe: Parameterized query
                string safeQuery = "SELECT * FROM Users WHERE username = @username";
                SqlCommand safeCommand = new SqlCommand(safeQuery, connection);
                safeCommand.Parameters.AddWithValue("@username", userInput);
                var safeReader = safeCommand.ExecuteReader();
            }
        }
    }
}
