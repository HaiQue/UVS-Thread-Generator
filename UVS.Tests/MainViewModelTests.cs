using Xunit;
using UVS.ViewModels;
using UVS.Services; 

namespace UVS.Tests
{
    public class MainViewModelTests
    {
        [Theory]
        [InlineData(1, false)]  // Too low (Minimum is 2)
        [InlineData(2, true)]   // Valid
        [InlineData(15, true)]  // Valid
        [InlineData(16, false)] // Too high (Maximum is 15)
        public void ThreadCount_Validation_ShouldReturnCorrectCanExecute(int count, bool expected)
        {
            // Arrange
            var vm = new MainViewModel();

            // Act
            vm.ThreadCount = count;
            var canStart = vm.StartCommand.CanExecute(null);

            // Assert
            Assert.Equal(expected, canStart);
        }

        [Fact]
        public void GenerateRandomString_ShouldBeSpecificLength()
        {
            // Arrange
            var service = new DataGeneratorService();
            int expectedLength = 7;

            // Act
            var result = service.GenerateRandomString(expectedLength);

            // Assert
            // This verifies Requirement 2: 5-10 character strings
            Assert.Equal(expectedLength, result.Length);
        }
    }
}