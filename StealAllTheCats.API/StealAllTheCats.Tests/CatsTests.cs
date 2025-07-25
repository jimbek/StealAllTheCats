using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using StealAllTheCats.API;
using StealAllTheCats.API.Models;
using StealAllTheCats.API.Models.Data;
using StealAllTheCats.API.Models.DTOs;

namespace StealAllTheCats.Tests
{
    public class CatsTests
    {
        [Fact]
        public async Task GetCat_Returns_BadRequest()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();
            var catRepository = new Mock<ICatRepository>();

            var token = new CancellationToken();
            string catId = string.Empty;

            unitOfWork
                .Setup(x => x.CatRepository)
                .Returns(catRepository.Object);

            unitOfWork
                .Setup(x => x.CatRepository.GetCatEntityAsync(token, It.Is<string>(y => y == catId)))
                .ReturnsAsync((CatEntity?)null);

            // Act
            var result = await CatEndpoints.GetCat(unitOfWork.Object, token, catId);

            //Assert
            Assert.IsType<Results<Ok<Image>, BadRequest, NotFound>>(result);

            var badRequestResult = (BadRequest)result.Result;

            Assert.NotNull(badRequestResult);
        }

        [Fact]
        public async Task GetCat_Returns_NotFound()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();
            var catRepository = new Mock<ICatRepository>();

            var token = new CancellationToken();
            string catId = Guid.NewGuid().ToString();

            unitOfWork
                .Setup(x => x.CatRepository)
                .Returns(catRepository.Object);

            unitOfWork
                .Setup(x => x.CatRepository.GetCatEntityAsync(token, It.Is<string>(y => y == catId)))
                .ReturnsAsync((CatEntity?)null);

            // Act
            var result = await CatEndpoints.GetCat(unitOfWork.Object, token, catId);

            //Assert
            Assert.IsType<Results<Ok<Image>, BadRequest, NotFound>>(result);

            var notFoundResult = (NotFound)result.Result;

            Assert.NotNull(notFoundResult);
        }

        [Fact]
        public async Task GetCat_Returns_Ok()
        {
            // Arrange
            var unitOfWork = new Mock<IUnitOfWork>();
            var catRepository = new Mock<ICatRepository>();

            var token = new CancellationToken();

            int id = 1;
            string catId = Guid.NewGuid().ToString();
            int width = 10;
            int height = 10;
            string image = "dgfdg";
            DateTime created = DateTime.UtcNow;

            unitOfWork
                .Setup(x => x.CatRepository)
                .Returns(catRepository.Object);

            catRepository
                .Setup(x => x.GetCatEntityAsync(token, It.Is<string>(y => y == catId)))
                .ReturnsAsync(new CatEntity
                {
                    Id = id,
                    CatId = catId,
                    Width = width,
                    Height = height,
                    Image = image,
                    Created = created
                });

            // Act
            var result = await CatEndpoints.GetCat(unitOfWork.Object, token, catId);

            //Assert
            Assert.IsType<Results<Ok<Image>, BadRequest, NotFound>>(result);

            var okResult = (Ok<Image>)result.Result;

            Assert.NotNull(okResult);
        }
    }
}