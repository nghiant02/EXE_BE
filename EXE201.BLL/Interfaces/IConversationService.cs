using EXE201.DAL.Models;

namespace EXE201.BLL.Interfaces;

public interface IConversationService
{
    Task<Conversation> GetConversationByConversationId(int conversationId);
    Task<IEnumerable<Conversation>> GetConversations(int userId);
    Task<Conversation> FindConversationAsync(int senderId, int receiverId);
    Task<(IEnumerable<Conversation> conversations, Conversation conversation)> NewConversationAsync(int senderId,
        int receiverId);
}