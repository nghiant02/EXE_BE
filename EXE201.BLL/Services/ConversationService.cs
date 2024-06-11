using EXE201.BLL.Interfaces;
using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;

namespace EXE201.BLL.Services;

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepository;

    public ConversationService(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }


    public async Task<Conversation> GetConversationByConversationId(int conversationId)
    {
        return await _conversationRepository.GetConversationByIdAsync(conversationId);
    }

    public async Task<IEnumerable<Conversation>> GetConversations(int userId)
    {
        return await _conversationRepository.GetConversations(userId);
    }

    public async Task<Conversation> FindConversationAsync(int senderId, int receiverId)
    {
        return await _conversationRepository.FindConversationAsync(senderId, receiverId);
    }

    public async Task<(IEnumerable<Conversation> conversations, Conversation conversation)> NewConversationAsync(int senderId, int receiverId)
    {
        return await _conversationRepository.NewConversationAsync(senderId, receiverId);
    }
}