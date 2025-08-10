using BusinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTOs;
namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksAPI : ControllerBase
{
    BookBLL bll = new BookBLL();
    [HttpGet( "GetAllBooks")]
    public ActionResult GetMyName()
    {
       
        return Ok(bll.GetAllBooks());
    }
    [HttpGet("GetBookByID {id}")]
    public ActionResult<DTO.BookDTO> GetBookById(int id)
    {
        var book = bll.GetBook(id);
        if (book == null) return NotFound();
        return Ok(book);
    }
    [HttpPost("AddBook")]
    public ActionResult AddBook(DTO.BookDTO book)
    {
        bll.AddBook(book);
        return CreatedAtAction(nameof(GetBookById), new { id = book.BookID }, book);
    }

    // PUT: api/LibAPI/5
    [HttpPut("UpdateBook {id}")]
    public ActionResult UpdateBook(int id, DTO.BookDTO book)
    {
        if (id != book.BookID) return BadRequest();

        var updated = bll.UpdateBook(book);
        if (updated <=0) return NotFound();

        return NoContent();
    }

    // DELETE: api/LibAPI/5
    [HttpDelete("Delete {id}")]
    public ActionResult DeleteBook(int id)
    {
        var deleted = bll.DeleteBook(id);
        if (deleted <= 0) return NotFound();

        return NoContent();
    }
}