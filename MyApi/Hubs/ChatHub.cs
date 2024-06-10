using System.Collections.Concurrent;
using System.Security.Cryptography;
using Message.Models;
using Microsoft.AspNetCore.SignalR;
using SignalrDemo.Server.Interfaces;

namespace SignalRChat.Hubs
{
    // Hub connection for the chat room
    public class ChatHub : Hub<ISignalrChatHub>
    {   
        // Dictionnary protected from concurrent access that stores userId and the connection used
        private static ConcurrentDictionary<string ,string> UserConnections = new ConcurrentDictionary<string, string>();

        // Is called every time a user connects to the chat room hub. Takes the userId from the Context provided
        // with the authentication mechanism and assign it the Context's connection
        public override Task OnConnectedAsync()
        {   
            string? userId = Context.UserIdentifier;
            if (userId != null) {
                UserConnections[userId] = Context.ConnectionId;
            }
            
            Console.WriteLine($"User connected: {userId} with Connection ID: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
        // Is called on user disconnect from the hub. Removes the connection and the user from the dictionnary
        public override Task OnDisconnectedAsync(Exception? exception)
        {   
            string? userId = Context.UserIdentifier;
            if (userId != null) {
                UserConnections.TryRemove(userId, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        // Sends a message to a specific user (1 on 1 conversation)
        public async Task SendMessageToUser(string userId, MessageDto message) {
            if (UserConnections.TryGetValue(userId, out string? connectionId)) {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
            }
        }
    }
}