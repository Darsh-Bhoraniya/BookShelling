namespace BookSheling.Models
{
    public class User_Models
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
    /*public class Api_Response
    {
        internal List<User_Models> data;

        public List<User_Models> Books { get; set; }
        public Boolean status { get; set; }
        public string Message { get; set; }

    }*/
}
