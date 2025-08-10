
namespace DAL;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTOs;
public class BookDAL
{
    public string connectionString = ConnectionString.connectionString;
    public bool AddNewBook(DTO.BookDTO bookDto)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SP_AddBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 200)).Value = bookDto.BookTitle;
                command.Parameters.Add(new SqlParameter("@AuthorName", SqlDbType.NVarChar, 100)).Value = bookDto.AuthorName;
                command.Parameters.Add(new SqlParameter("@CategoryName", SqlDbType.NVarChar, 100)).Value = bookDto.CategoryName;
                command.Parameters.Add(new SqlParameter("@PublicationYear", SqlDbType.Int)).Value = bookDto.PublicationYear;
                
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
    public List<DTO.BookDTO> GetAllBooks()
    {
        List<DTO.BookDTO> books = new List<DTO.BookDTO>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("SP_GetAllBooks", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DTO.BookDTO dto = new DTO.BookDTO();
                dto.BookID = Convert.ToInt32(reader["BookID"]);
                dto.BookTitle = (string)reader["BookTitle"];
                dto.AuthorName = (string)reader["AuthorName"];
                dto.CategoryName = (string)reader["CategoryName"];
                dto.PublicationYear = Convert.ToInt32(reader["PublicationYear"]);
                books.Add(dto);
            }
            connection.Close();
            return books;
        }
    }
    public int DeleteBook(int bookID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SP_DeleteBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)).Value = bookID;

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnParameter);

                command.ExecuteNonQuery();

                int returnValue = (int)returnParameter.Value;
                return returnValue;
            }
            catch
            {
                return -99; 
            }
        }
    }
    public int UpdateBook(DTO.BookDTO bookDto)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SP_UpdateBook", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", bookDto.BookID);
                command.Parameters.AddWithValue("@Title", bookDto.BookTitle);
                command.Parameters.AddWithValue("@AuthorID", bookDto.AuthorID);
                command.Parameters.AddWithValue("@CategoryID", bookDto.CategoryID);
                command.Parameters.AddWithValue("@PublishYear", bookDto.PublicationYear);

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnParameter);

                command.ExecuteNonQuery();

                int result = (int)returnParameter.Value;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("C# Exception: " + ex.Message);
                return -99;
            }
        }
    }
    public DTO.BookDTO? GetBookById(int bookId)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SP_GetBookByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = bookId;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DTO.BookDTO book = new DTO.BookDTO
                        {
                            BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                            BookTitle = reader.GetString(reader.GetOrdinal("BookTitle")),
                            AuthorName = reader.GetString(reader.GetOrdinal("AuthorName")),
                            CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                            PublicationYear = reader.IsDBNull(reader.GetOrdinal("PublicationYear")) 
                                ? 0 
                                : reader.GetInt32(reader.GetOrdinal("PublicationYear"))
                        };

                        return book;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    public List<DTO.BookDTO> GetAvailableBooks()
    {
        List<DTO.BookDTO> books = new List<DTO.BookDTO>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand("SP_GetAvailableBooks", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DTO.BookDTO dto = new DTO.BookDTO();
                dto.BookID = Convert.ToInt32(reader["BookID"]);
                dto.BookTitle = (string)reader["BookTitle"];
                dto.AuthorID = (int)reader["AuthorID"];
                dto.PublicationYear = Convert.ToInt32(reader["PublicationYear"]);
                books.Add(dto);
            }
            connection.Close();
            return books;
        }
    }
}