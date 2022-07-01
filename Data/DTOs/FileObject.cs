namespace PassMoveAPI.Data.DTOs
{
    public class FileObject
    {
        public string url { get; set; }
        public long bytes { get; set; }
        public string original_filename { get; set; }
        public DateTime createdAt { get; set; }
        public string format { get; set; }
        public string secure_url { get; set; }
    }
}
