namespace DataAccess.Entities
{
    public class Material
    {
        public int ID { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string SubjectArea { get; set; }
        public Material(string author, string title, string subjectArea)
        {
            this.Author = author;
            this.Title = title;
            this.SubjectArea = subjectArea;
        }

        public Material()
        {

        }
    }
}