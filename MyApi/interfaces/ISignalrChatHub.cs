using Message.Models;

// Interface that signs the methods used by the chat room hub
namespace SignalrDemo.Server.Interfaces {
    public interface ISignalrChatHub {

        // Send a message
        Task SendAsync(string action, string? context, MessageDto payload  );
    }
}