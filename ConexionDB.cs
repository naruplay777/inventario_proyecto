using MySql.Data.MySqlClient;
using System;

public class ConexionDB
{
    private static string connectionString = "Server=localhost;Database=heladeria;Uid=root;Pwd=andrewserver;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}