using Entities;
using RepositaryContracts;
using InMemoryRepositories;


IUserRepository userRepo = new UserInMemoryRepository();
IPostRepository postRepo = new PostInMemoryRepository();
ICommentRepository commentRepo = new CommentInMemoryRepository();

foreach (User user in userRepo.GetAll())
{
    Console.WriteLine($"[{user.Id}] {user.Username} (password: {user.Password})");
}

Console.WriteLine();
foreach (Post post in postRepo.GetAll())
{
    Console.WriteLine($"[{post.Id}] {post.Title} by user {post.UserId} " +
                      $"— {post.LikedPostIds.Count} likes, " +
                      $"{post.DislikedPostIds.Count} dislikes");
    Console.WriteLine($"      Body: {post.Body}");
}

Console.WriteLine();
foreach (Comment comment in commentRepo.GetAll())
{
    Console.WriteLine($"[{comment.Id}] on post {comment.PostId} by user {comment.UserId} " +
                      $"— {comment.LikedCommentIds.Count} likes, " +
                      $"{comment.DislikedCommentIds.Count} dislikes");
    Console.WriteLine($"      \"{comment.Body}\"");
}