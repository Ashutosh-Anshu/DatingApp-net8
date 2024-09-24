using API.DTOs;
using API.Entites;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessagesRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessageForUser(MessageParams message);
        Task<IEnumerable<MessageDto>> GetMessageThread(string cureentUser, string recipientUsername);
        Task<bool> SaveAllAsync();

    }
}
