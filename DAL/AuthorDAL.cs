using System.Data.Common;
using DTOs;
using Microsoft.Data.SqlClient;

namespace DAL;

public class AuthorDAL
{
    public string connectionString = ConnectionString.connectionString;

    public bool AddNewAuthor(string AuthorName)
    {

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command =  new SqlCommand("SP_AddAuthor", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@AuthorName",AuthorName));
            if (command.ExecuteNonQuery() > 0)
            {
                return true;
            }
            connection.Close();
            
        }
        return false;
    }

    public bool DeleteAuthor(int ID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command =  new SqlCommand("SP_DeleteAuthor", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@ID",ID));
            if (command.ExecuteNonQuery() > 0)
            {
                return true;
            }
            connection.Close();
            
        }
        return false;
    }
    public List<DTO.AuthorDTO>? GetAuthors()
    {
        List<DTO.AuthorDTO> authors = new List<DTO.AuthorDTO>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SP_GetAuthors", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DTO.AuthorDTO authorDTO = new DTO.AuthorDTO();
                authorDTO.AuthorID = Convert.ToInt32(reader["AuthorID"]);
                authorDTO.AuthorName = Convert.ToString(reader["AuthorName"]);
                authors.Add(authorDTO);
            }
        }
        return authors;
    }
}