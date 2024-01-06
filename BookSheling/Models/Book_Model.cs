namespace BookSheling.Models
{
    public class Book_Model
    {
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string BookTitle { get; set; }
        public int BookWiseAuthorID { get; set; }
        public int TypeID { get; set; }
        public double Price { get; set; }
        public int INSBN { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
