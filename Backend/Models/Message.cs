namespace Unomart.Models
{
    public class Message
    {
        public object message { get; set; }
        public object code{ get; set; }
        public DateTime Timestamp { get; set; }
        public Message(object? message, string code)
        {
            if(message != null)
            {
                this.message = message;
            }
            else
            {
                this.message = 0;
            }
            if(code == null)
            {
                throw new Exception("Need an error code");
            }
            this.code = code;
            Timestamp= DateTime.Now;
        }
    }
}
