using EXE201.DAL.Models;

namespace EXE201.BLL.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetMessages();
    
}