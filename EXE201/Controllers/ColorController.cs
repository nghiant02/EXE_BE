using EXE201.DAL.DTOs.ColorDTOs;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ColorController : ControllerBase
{
    private readonly IColorRepository _colorRepository;

    public ColorController(IColorRepository colorRepository)
    {
        _colorRepository = colorRepository;
    }

    [HttpGet("GetAllColors")]
    public async Task<IActionResult> GetAllColors()
    {
        var colors = await _colorRepository.GetAllColors();
        return Ok(colors);
    }

    [HttpPost("CreateColor")]
    public async Task<IActionResult> CreateColor([FromBody] CreateColorDTO request)
    {
        var response = await _colorRepository.CreateColor(request.ColorName);
        if (response.Status == "Error")
        {
            return BadRequest(new { Message = response.Message });
        }
        return Ok(response.DataObject);
    }

    [HttpDelete("DeleteColor")]
    public async Task<IActionResult> DeleteColor(int colorId)
    {
        var response = await _colorRepository.DeleteColor(colorId);
        if (response.Status == "Error")
        {
            return NotFound(new { Message = response.Message });
        }
        return Ok(new { Message = response.Message });
    }

    [HttpPut("UpdateColor")]
    public async Task<IActionResult> UpdateColor([FromBody] UpdateColorDTO request)
    {
        var response = await _colorRepository.UpdateColor(request.ColorId, request.NewColorName);
        if (response.Status == "Error")
        {
            return BadRequest(new { Message = response.Message });
        }
        return Ok(response.DataObject);
    }
}
