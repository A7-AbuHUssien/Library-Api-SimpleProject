using BusinessLogicLayer;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorAPI : ControllerBase
{
    AuthorBLL  bll = new AuthorBLL();
    
    [HttpGet("GetAllAuthors")]
    public IActionResult GetAllAuthors()
    {
        
        return Ok(bll.GetAuthors());
    }

    [HttpPost("CreateAuthor")]
    public IActionResult CreateAuthor(string Name)
    {
        return Ok(bll.AddNewAuthor(Name));
    }

    [HttpDelete("DeleteAuthor/{id}")]
    public IActionResult DeleteAuthor(int id)
    {
        return Ok(bll.DeleteAuthor(id));
    }
}