using API.DTOs;
using API.Entites;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessagesRepository(DataContext _context, IMapper mapper) : IMessagesRepository
    {
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                    .OrderByDescending(x => x.MessageSent)
                    .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.RecipientUsername == messageParams.Username && x.RecipientDeleted == false),
                "Outbox" => query.Where(x => x.Sender.UserName == messageParams.Username && x.SenderDeleted == false),
                _ => query.Where(x => x.Recipient.UserName == messageParams.Username && x.DateRead == null && x.RecipientDeleted == false)
            };

            var message = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);
            return await PagedList<MessageDto>
                    .CreateAsync(message, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserNme, string recipientUsername)
        {
            var messages = await _context.Messages
                    .Include(x => x.Sender).ThenInclude(x => x.Photos)
                    .Include(x => x.Recipient).ThenInclude(x => x.Photos)
                    .Where(
                    x => x.RecipientUsername == currentUserNme && x.RecipientDeleted == false && x.SenderUsername == recipientUsername ||
                    x.SenderUsername == currentUserNme && x.SenderDeleted == false && x.RecipientUsername == recipientUsername
                    ).OrderByDescending(x => x.MessageSent).ToListAsync();

            var unReadMessage = messages.Where(x => x.DateRead == null
                && x.RecipientUsername == currentUserNme).ToList();

            if (unReadMessage.Count > 0)
            {
                unReadMessage.ForEach(x => x.DateRead = DateTime.UtcNow);
                await _context.SaveChangesAsync();
            }
            return mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
