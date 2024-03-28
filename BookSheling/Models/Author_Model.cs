namespace BookSheling.Models
{
    public class Author_Model
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string ContactNo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
    public class AuthorComboBox
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
    }
}