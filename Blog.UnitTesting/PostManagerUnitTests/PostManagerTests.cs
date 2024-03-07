using Blog.BL.Managers.Posts;
using Blog.DAL.Repos.Posts;
using Blog.DAL.Repos.PostTag;
using Blog.DAL.Repos.Tags;
using MapsterMapper;
using Moq;

namespace Blog.UnitTesting.PostManagerUnitTests;

[TestFixture]
public partial class PostManagerTests
{
    private Mock<IPostRepo> _postRepo;
    private Mock<ITagRepo> _tagRepo;
    private Mock<IPostTagRepo> _postTagRepo;
    private Mock<IMapper> _mapper;

    private PostManager _postManager;
    [SetUp]
    public void Setup()
    {
        _postRepo = new Mock<IPostRepo>();
        _tagRepo = new Mock<ITagRepo>();
        _postTagRepo = new Mock<IPostTagRepo>();
        _mapper = new Mock<IMapper>();

        _postManager = new PostManager(
            _postRepo.Object,
            _tagRepo.Object,
            _postTagRepo.Object,
            _mapper.Object
            );
    }
}