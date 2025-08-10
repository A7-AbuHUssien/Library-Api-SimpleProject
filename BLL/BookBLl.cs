
using DTOs;
namespace BusinessLogicLayer;

public class BookBLL
{
    DAL.BookDAL Book = new DAL.BookDAL();
    public bool AddBook(DTO.BookDTO book)
    { 
        return Book.AddNewBook(book);
    }

    public List<DTO.BookDTO> GetAllBooks()
    {
        return Book.GetAllBooks();
    }

    public int DeleteBook(int id)
    {
        return Book.DeleteBook(id);
    }

    public int UpdateBook(DTO.BookDTO book)
    {
        return Book.UpdateBook(book);
    }

    public DTO.BookDTO? GetBook(int id)
    {
        return Book.GetBookById(id);
    }

    public List<DTO.BookDTO> GetAvailableBooks()
    {
        return Book.GetAvailableBooks();
    }
}