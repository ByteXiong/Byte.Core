namespace Byte.Core.Common.Models
{
    public class JWTPayload
    {
        public Guid Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        //public UserEnum? Type { get; set; }


        public Guid DeptId { get; set; }
        public  UserType Type { get; set; }
        public string Role { get; set; }
        public DateTime Expire { get; set; }
    }
}
