namespace API.DTOs
{
    // this DTO is for receiving the message from the client side.
    // An automapper profile isn't needed for this DTO because it's only two properties
    public class CreateMessageDto
    {
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
    }
}