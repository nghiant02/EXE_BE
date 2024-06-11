using EXE201.DAL.Models;
using MCC.DAL.Repository.Interface;

namespace EXE201.DAL.Interfaces;

public interface IConversationRepository : IGenericRepository<Conversation>
{
    Task<Conversation> GetConversationByIdAsync(int conversationId);
    Task<IEnumerable<Conversation>> GetConversations(int userId);
    Task<Conversation> FindConversationAsync(int senderId, int receiverId);

    Task<(IEnumerable<Conversation> conversations, Conversation conversation)> NewConversationAsync(int senderId,
        int receiverId);
}