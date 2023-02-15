using System.ComponentModel.DataAnnotations;

namespace Unomart.Models
{
    public class User
    {
        public string? UID { get; set; }
        public string? Email { get; set; }
        public string ?Hash { get; set; }
        public string? Username { get; set; }
        public string? CurrencyCode{ get; set; }

    }
}
