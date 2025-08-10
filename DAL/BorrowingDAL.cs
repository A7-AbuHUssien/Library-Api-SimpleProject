using DTOs;

namespace DAL;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using DTOs;
public class BorrowingDAL
{
    public bool ReturnBook(int OpID)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
        {
            SqlCommand command = new SqlCommand("SP_ReturnBook", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@OpID", SqlDbType.Int).Value = OpID;
            connection.Open();
            int Result = command.ExecuteNonQuery();
            connection.Close();
            return Result > 0 ;
        }
    }
    public bool AddBorrowing(DTO.BorrowingDTO borrowing)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            using (SqlCommand command = new SqlCommand("SP_AddBorrowing", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ClientID", borrowing.ClientID);
                command.Parameters.AddWithValue("@BookID", borrowing.BookID);
                command.Parameters.AddWithValue("@BorrowDate", borrowing.BorrowDate);

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnParameter);

                connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
   
    public List<DTO.BorrowingDetailsDTO> GetAllBorrowingsDetails()
    {
        List<DTO.BorrowingDetailsDTO> list = new List<DTO.BorrowingDetailsDTO>();

        using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
        using (SqlCommand command = new SqlCommand("SP_GetAllBorrowingsDetails", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var borrowingDetail = new DTO.BorrowingDetailsDTO
                    {
                        BorrowingID = reader.GetInt32(reader.GetOrdinal("BorrowingID")),
                        ClientName = reader.GetString(reader.GetOrdinal("ClientName")),
                        BookTitle = reader.GetString(reader.GetOrdinal("BookTitle")),
                        BorrowDate = reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                        ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate")) 
                            ? (DateTime?)null 
                            : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                        Status = reader.GetString(reader.GetOrdinal("Status"))
                    };

                    list.Add(borrowingDetail);
                }
            }
        }

        return list;
    }

    
}