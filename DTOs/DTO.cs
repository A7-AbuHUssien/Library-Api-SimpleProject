namespace DTOs;

public class DTO
{
    public class BookDTO
    {
        public int BookID { get; set; } 
        public string BookTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } 
        public string CategoryName { get; set; } 
        public long PublicationYear { get; set; } 
        public bool IsFree { get; set; } 
        public int CategoryID { get; set; }
        public int AuthorID { get; set; }
        
    }
    public class AuthorDTO
    {
        public long AuthorID { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
    public class ClientDTO
    {
        public long ClientID { get; set; } 
        public string ClientName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; } 
        public DateTime RegistrationDate { get; set; } 
    }
    public class CategoryDTO
    {
        public long CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
    public class BorrowingDTO
    {
        public long BorrowingID { get; set; }
        public long ClientID { get; set; }
        public long BookID { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
    public class BorrowingDetailsDTO
    {
        public long BorrowingID { get; set; }
        public string ClientName { get; set; }
        public string BookTitle { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; }
    }

}