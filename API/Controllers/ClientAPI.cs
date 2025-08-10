using BusinessLogicLayer;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientAPI : ControllerBase
{
    ClientBLL  bll = new ClientBLL();

    [HttpGet("GetClients")]
    public IActionResult GetClients()
    {
        return Ok(bll.GetAllClients());
    }

    [HttpGet("GetClient/{id}")]
    public IActionResult GetClient(int id)
    {
        return Ok(bll.GetClientByID(id));
    }

    [HttpPost("CreateClient")]
    public IActionResult CreateClient(DTO.ClientDTO client)
    {
        return Ok(bll.AddClient(client));
    }

    [HttpPut("UpdateClient")]
    public IActionResult UpdateClient(DTO.ClientDTO client)
    {
        return Ok(bll.UpdateClient(client));
    }

    [HttpDelete("DeleteClient/{id}")]
    public IActionResult DeleteClient(int id)
    {
        return Ok(bll.DeleteClient(id));
    }

    [HttpGet("GetClientByEmailOrPhone/{Email}/{Phone}")]
    public IActionResult GetClientByID(string Email = "", string Phone = "")
    {
        return Ok(bll.GetClientByPhoneOrEmail(Phone, Email));
    }
}