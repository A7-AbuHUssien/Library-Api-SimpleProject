using DTOs;

namespace BusinessLogicLayer;

public class BorrowingBLL
{
    DAL.BorrowingDAL dal = new DAL.BorrowingDAL();
    public bool AddBorrowing(DTO.BorrowingDTO borrowing)
    {
        return dal.AddBorrowing(borrowing);
    }

    public List<DTO.BorrowingDetailsDTO> GetAllBorrowingsDetails()
    {
        return dal.GetAllBorrowingsDetails();
    }
    public bool ReturnBook(int Id)
    {
        return dal.ReturnBook(Id);
    }
}