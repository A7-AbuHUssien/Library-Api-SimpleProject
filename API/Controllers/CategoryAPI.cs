using BusinessLogicLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryAPI : ControllerBase
{
    CategoryBLL   bll = new CategoryBLL();
    
    [HttpGet("GetAllCategories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAllCategories()
    {
        return Ok(bll.GetAllCategories());
    }

    [HttpPost("AddCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AddCategory(string Name)
    {
        return Ok(bll.AddCategory(Name));
    }

    [HttpPut("DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteCategory(int id)
    {
        return Ok(bll.DeleteCategory(id));
    }
}