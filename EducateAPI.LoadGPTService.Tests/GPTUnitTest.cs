using Xunit;
using Moq;
using System.Threading.Tasks;
using EducateAPI.LoadGPTService.Interfaces;

namespace EducateAPI.LoadGPTService.Tests
{
    public class GPTUnitTest
    {
        [Fact]
        public async Task GetResponseAsync_ShouldReturnResponse()
        {
            var prompt = "Are you alive ?";
            var expectedResponse = "No, I have no life, I'm a machine thoughts.";

            var mockAPIClient = new Mock<IGPTClient>();
            mockAPIClient.Setup(client => client.GetResponseAsync(prompt))
                            .ReturnsAsync(expectedResponse);

            var gptServiceClient = mockAPIClient.Object;

            //Act
            var result = await mockAPIClient.Object.GetResponseAsync(prompt);

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetResponseAsync_ShouldThrowException_WhenPromptIsNull()
        {
            var mockAPIClient = new Mock<IGPTClient>();
            mockAPIClient.Setup(client => client.GetResponseAsync(null))
                         .ThrowsAsync(new ArgumentNullException());

            var apiClient = mockAPIClient.Object;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await apiClient.GetResponseAsync(null));
        }
    }
}