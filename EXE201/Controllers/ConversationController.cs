using EXE201.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet("GetConversationByUserId")]
    public async Task<IActionResult> GetConversation(int userId)
    {
        var result = await _conversationService.GetConversations(userId);
        return Ok(result);
    }
}