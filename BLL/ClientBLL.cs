using DTOs;

namespace BusinessLogicLayer;

public class ClientBLL
{
    private DAL.ClientDAL dal = new DAL.ClientDAL();

    public bool AddClient(DTO.ClientDTO client)
    {
        return dal.AddClient(client);
    }

    public bool DeleteClient(int ID)
    {
        return dal.DeleteClient(ID);
    }

    public List<DTO.ClientDTO> GetAllClients()
    {
        return dal.GetClients();
    }

    public DTO.ClientDTO GetClientByID(int ID)
    {
        return dal.GetClientByID(ID);
    }

    public DTO.ClientDTO GetClientByPhoneOrEmail(string phone, string email)
    {
        return dal.GetClientByPhoneOrEmail(phone, email);
    }

    public int UpdateClient(DTO.ClientDTO client)
    {
        return dal.UpdateClient(client);
    }
}