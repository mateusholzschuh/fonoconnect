using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class DatabaseController {

    public static string DB_URL = "URI=file:" + Application.dataPath + "/database.db";

    private IDbConnection connection;
    private IDbCommand    command;
    private DataTable     dataTable;

    public DatabaseController(string dbURL) {
        connection = new SqliteConnection(dbURL);        
    }

    ~DatabaseController() {
        command = null;
        dataTable = null;
        connection.Close();
    }

    public DataTable RunQuery(string query) {
        connection.Open();
        command = connection.CreateCommand();
        dataTable = new DataTable();
        command.CommandText = query;
        dataTable.Load(command.ExecuteReader());
        connection.Close();
        return dataTable;
    }

    public int RunQueryWithoutReturn(string query) {
        connection.Open();
        command = connection.CreateCommand();
        command.CommandText = query;
        int i = command.ExecuteNonQuery();
        connection.Close();
        return i;
    }

}
