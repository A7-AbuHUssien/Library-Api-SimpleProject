using System.Data;
using System.Reflection.Metadata;
using DTOs;
using Microsoft.Data.SqlClient;

namespace DAL;

public class ClientDAL
{
    public bool AddClient(DTO.ClientDTO client)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_AddClient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClientName", client.ClientName);
                    command.Parameters.AddWithValue("@Phone", client.Phone);
                    command.Parameters.AddWithValue("@Email", client.Email);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool DeleteClient(int ID)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_DeleteClient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClientID", ID);

                    // إضافة باراميتر لاستقبال قيمة RETURN من الـ Stored Procedure
                    SqlParameter returnParameter = new SqlParameter();
                    returnParameter.ParameterName = "@ReturnVal";
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnParameter);

                    command.ExecuteNonQuery();

                    int result = (int)returnParameter.Value;
                    return result >0;
                }
            }
        }
        catch (Exception ex)
        {
            // سجل الخطأ أو تعامل معه هنا
            return false;
        }
    }

    public List<DTO.ClientDTO> GetClients()
    {
        List<DTO.ClientDTO> clients = new List<DTO.ClientDTO>();
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SP_GetAllClients", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DTO.ClientDTO client = new DTO.ClientDTO();
                            client.ClientID = reader.GetInt32(0);
                            client.ClientName = reader["ClientName"].ToString();
                            client.Phone = reader["Phone"].ToString();
                            client.Email = reader["Email"].ToString();
                            clients.Add(client);
                        }
                    }
                }
            }
            return clients;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public DTO.ClientDTO GetClientByID(int ID)
    {
        DTO.ClientDTO client = new DTO.ClientDTO();
        using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SP_GetClientByID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ClientID", ID);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    client.ClientID = reader.GetInt32(0);
                    client.ClientName = reader["ClientName"].ToString();
                    client.Phone = reader["Phone"].ToString();
                    client.Email = reader["Email"].ToString();
                }
            }
        }
        return client;
    }
    public DTO.ClientDTO GetClientByPhoneOrEmail(string phone, string email)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
        using (SqlCommand command = new SqlCommand("SP_GetClientByPhoneOrEmail", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Phone", phone);
            command.Parameters.AddWithValue("@Email", email);

            SqlParameter returnParameter = new SqlParameter();
            returnParameter.ParameterName = "@ReturnVal";
            returnParameter.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnParameter);

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    DTO.ClientDTO client = new DTO.ClientDTO
                    {
                        ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                        ClientName = reader.GetString(reader.GetOrdinal("ClientName")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        Email = reader.GetString(reader.GetOrdinal("Email"))
                    };

                    reader.Close();

                    int result = (int)returnParameter.Value;

                    if (result == -1)
                    {
                        // العميل غير موجود
                        return null;
                    }
                    else if (result == -2)
                    {
                        throw new Exception("More than one client found with the given phone or email.");
                    }

                    return client;
                }
                else
                {
                    return null;
                }
            }
        }
    }
    public int UpdateClient(DTO.ClientDTO client)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            using (SqlCommand command = new SqlCommand("SP_UpdateClient", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ClientID", client.ClientID);
                command.Parameters.AddWithValue("@ClientName", client.ClientName);
                command.Parameters.AddWithValue("@Phone", client.Phone);
                command.Parameters.AddWithValue("@Email", client.Email);

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnParameter);

                connection.Open();
                command.ExecuteNonQuery();

                int result = (int)returnParameter.Value;

                return result;
            }
        }
        catch (Exception ex)
        {
            return -3; 
        }
    }
    
}