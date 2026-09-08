namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public string UsernameId { get; set; }
    public HashSet<int> LikedCommentIds { get; set; } = new HashSet<int>();
    public HashSet<int> DislikedCommentIds { get; set; } = new HashSet<int>();
}