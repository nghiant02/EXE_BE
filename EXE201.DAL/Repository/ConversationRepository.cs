using EXE201.DAL.Interfaces;
using EXE201.DAL.Models;
using MCC.DAL.Repository.Implements;
using Microsoft.EntityFrameworkCore;

namespace EXE201.DAL.Repository;

public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
{
    public ConversationRepository(EXE201Context context) : base(context)
    {
    }

    public async Task<(IEnumerable<Conversation> conversations, Conversation conversation)> NewConversationAsync(int senderId,
        int receiverId)
    {
        try
        {
            var existingConversation = await FindConversationAsync(senderId, receiverId);

            if (existingConversation != null)
            {
                var conversations = await GetConversations(senderId);
                return (conversations, existingConversation);
            }

            var newConversation = new Conversation
            {
                User1Id = senderId,
                User2Id = receiverId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = BitConverter.GetBytes(DateTime.UtcNow.ToBinary())
            };

            await _context.Conversations.AddAsync(newConversation);
            await _context.SaveChangesAsync();

            var newConversations = await GetConversations(senderId);

            return (newConversations, newConversation);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error creating new conversation", ex);
            throw;
        }
    }

    public async Task<Conversation> FindConversationAsync(int senderId, int receiverId)
    {
        try
        {
            return await _context.Conversations
                .FirstOrDefaultAsync(c =>
                    (c.User1Id == senderId && c.User2Id == receiverId) ||
                    (c.User1Id == receiverId && c.User2Id == senderId));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error finding conversation", ex);
            throw;
        }
    }

    public async Task<IEnumerable<Conversation>> GetConversations(int userId)
    {
        try
        {
            return await _context.Conversations
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error getting conversations", ex);
            throw;
        }
    }

    public async Task<Conversation> GetConversationByIdAsync(int conversationId)
    {
        try
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error getting conversation by ID", ex);
            throw;
        }
    }
}