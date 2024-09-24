using API.DTOs;
using API.Entites;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController(IMessagesRepository messagesRepository,
        IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto messageDto)
        {
            var username = User.Getusername();
            if (username == messageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot message itself");
            var sender = await userRepository.GetUserByUsernameAsync(username);
            var recipient = await userRepository.GetUserByUsernameAsync(messageDto.RecipientUsername);

            if (recipient == null || sender == null)
                return BadRequest("Cann't send message at this time");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = messageDto.Content
            };
            messagesRepository.AddMessage(message);
            if (await messagesRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));

            return BadRequest("faild to save message");

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForuser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.Getusername();
            var messages = await messagesRepository.GetMessageForUser(messageParams);
            Response.AddPaginationHeader(messages);
            return messages;
        }
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesThread(string username)
        {
            var currentUserName = User.Getusername();
            return Ok(await messagesRepository.GetMessageThread(currentUserName, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.Getusername();
            var message = await messagesRepository.GetMessage(id);
            if (message == null) return BadRequest("cannot delete this messages");
            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Forbid();
            if (message.SenderUsername == username) message.SenderDeleted = true;
            if (message.RecipientUsername == username) message.RecipientDeleted = true;

            if (message is { SenderDeleted: true, RecipientDeleted: true })
            {
                messagesRepository.DeleteMessage(message);
            }

            if (await messagesRepository.SaveAllAsync()) return Ok();
            return BadRequest("Error is deleting message");
        }
    }
}
