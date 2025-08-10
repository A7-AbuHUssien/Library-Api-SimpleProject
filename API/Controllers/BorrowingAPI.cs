using BusinessLogicLayer;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BorrowingAPI : ControllerBase
{
    BorrowingBLL  bll = new BorrowingBLL();

    [HttpGet("GetBorrowingOperations")]
    public List<DTO.BorrowingDetailsDTO> GetBorrowings()
    {
        return bll.GetAllBorrowingsDetails();
    }

    [HttpPost("CreateBorrowing")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult CreateBorrowing(DTO.BorrowingDTO borrowingDTO)
    {
        return Ok(bll.AddBorrowing(borrowingDTO));
    }

    [HttpPut("ReturnBook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateBorrowing(int OpID)
    {
        return Ok(bll.ReturnBook(OpID));
    }
}

