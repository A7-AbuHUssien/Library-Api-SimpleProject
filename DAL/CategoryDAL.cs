using System.Data;
using DTOs;
using Microsoft.Data.SqlClient;

namespace DAL;

public class CategoryDAL
{
    private string ConnectionString = DAL.ConnectionString.connectionString;

    public int AddCategory(string name)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_AddCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@CategoryName", SqlDbType.VarChar, 100).Value = name;
                    SqlParameter returnValue = new SqlParameter();
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnValue);
                    command.ExecuteNonQuery();

                    int result = (int)returnValue.Value;
                    return result; 
                }
            }
        }
        catch (Exception ex)
        {
            return -99; 
        }
    }

    public int DeleteCategory(int id)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_DeleteCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = id;
                    SqlParameter returnValue = new SqlParameter();
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnValue);
                    command.ExecuteNonQuery();

                    int result = (int)returnValue.Value;
                    return result; 
                }
            }
        }
        catch (Exception ex)
        {
            return -99; 
        }
    }

    public List<DTO.CategoryDTO> GetAllCategories()
    {
        List<DTO.CategoryDTO> categories = new List<DTO.CategoryDTO>();
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SP_GetCategories", connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DTO.CategoryDTO category = new DTO.CategoryDTO();
                category.CategoryID = (int)reader["CategoryID"];
                category.CategoryName = Convert.ToString(reader["CategoryName"]);
                
                categories.Add(category);
            }
        }
        return categories;
    }
}