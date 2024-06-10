// Defines the model for message sending
namespace Message.Models {
    public class MessageDto {
        public required bool IsSelf { get; set;}
        public required string Username { get; set;}
        public required string Timestamp { get; set;}
        public required string Content { get; set;}
    }
}