using Blog.BL.DTOs.Posts;
using Blog.BL.DTOs.Tags;
using Blog.BL.Exception_Handling;
using Blog.BL.Managers.Posts;
using Blog.DAL.Models;
using Blog.DAL.Repos.Posts;
using Blog.DAL.Repos.PostTag;
using Blog.DAL.Repos.Tags;
using MapsterMapper;
using Moq;

namespace Blog.UnitTesting
{
    public class PostManagerTests
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

        [Test]
        public void GetById_PostNotFound_ThrowsBusinessException()
        {
            _postRepo.Setup(p => p.Get(It.IsAny<int>())).Throws<BusinessException>();

            Assert.That(
                async () => await _postManager.GetById(1),
                Throws.Exception.TypeOf<BusinessException>()
                );
        }

        [Test]
        async public Task GetById_PostFound_ReturnsReadPostDTO()
        {
            // arrange
            var postId = 1;
            var post = new Post
            {
                Id = postId,
                Title = "Sample Title",
                Body = "Sample Body",
                TotalLikes = 10,
                AuthorId = "author123",
                CreatedAt = DateTime.Now,
                PostsTags =
                [
                    new PostsTags
                    {
                        Tag = new Tag
                        {
                            Id = 1,
                            Name = "tag1",
                            CreatedAt = DateTime.Now
                        }
                    }
                ]
            };

            var readPost = new ReadPostDTO
            (
                postId,
                "Sample Title",
                "Sample Body",
                10,
                "author123",
                DateTime.Now,
                [
                    new ReadTagDTO(1, "tag1", DateTime.Now)
                ]
            );

            _postRepo.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(post);
            _mapper.Setup(m => m.Map<ReadPostDTO>(post)).Returns(readPost);

            // act
            var result = await _postManager.GetById(postId);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ReadPostDTO>());
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(postId));
                Assert.That(result.Title, Is.EqualTo(post.Title));
                Assert.That(result.Body, Is.EqualTo(post.Body));
            });

        }
    }
}