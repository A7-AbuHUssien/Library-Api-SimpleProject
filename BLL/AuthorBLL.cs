using DAL;
using DTOs;

namespace BusinessLogicLayer;

public class AuthorBLL
{
    AuthorDAL  authorDAL = new AuthorDAL();
    public bool AddNewAuthor(string AuthorName)
    {
        return authorDAL.AddNewAuthor(AuthorName);
    }

    public bool DeleteAuthor(int AuthorID)
    {
        return authorDAL.DeleteAuthor(AuthorID);
    }
    public List<DTO.AuthorDTO> GetAuthors()
    {
        return authorDAL.GetAuthors();
    }
}