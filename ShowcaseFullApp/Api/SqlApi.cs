using System;
using ZstdSharp.Unsafe;

namespace ShowcaseFullApp.Api;
using MySql;
using MySql.Data.MySqlClient;

public class SqlApi
{
    public SqlApi()
    {
        const string connectionString =
            "Server=ACU-1872.local;Database=showcase_db;User ID=root;Password=3872810Gabe$$;";

        //establish a connection
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connected to Database");

                string query = "SELECT * FROM users";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("inside read");
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Ock way");
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("user_id");
                                string name = reader.GetString("user_email");
                                Console.WriteLine($"Id: {id}, Email: {name}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }
    }


}