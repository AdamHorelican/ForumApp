using Console.UI;
using InMemoryRepositories;
using RepositaryContracts;

System.Console.WriteLine("Starting ....");


IUserRepository userRepo = new UserInMemoryRepository();
IPostRepository postRepo = new PostInMemoryRepository();
ICommentRepository commentRepo = new CommentInMemoryRepository();

CliApp app = new CliApp(userRepo, postRepo, commentRepo);
await app.StartAsync();