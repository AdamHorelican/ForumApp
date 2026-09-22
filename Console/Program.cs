using Console.UI;
using FileRepositories;
using InMemoryRepositories;
using RepositaryContracts;

System.Console.WriteLine("Starting ....");


IUserRepository userRepo = new UserFileRepository();
IPostRepository postRepo = new PostFileRepository();
ICommentRepository commentRepo = new CommentFileRepository();

CliApp app = new CliApp(userRepo, postRepo, commentRepo);
await app.StartAsync();