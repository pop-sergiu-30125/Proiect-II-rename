using Xunit;
using ProiectII.Models;

// teste Mock

namespace ProiectII.Tests
{
    public class AdoptionTests
    {
        [Fact]
        public void ApproveAdoption_ShouldSetStatusToApproved_AndCallFoxAdopt()
        {
            // Arrange
            var fox = new Fox { Id = 1, Name = "Foxy", StatusId = 1 };
            var adoption = new Adoption { Id = 1, Fox = fox, AdoptionStatus = AdoptionStatus.Pending };
            uint adoptedStatusId = 2;

            // Act
            adoption.ApproveAdoption(adoptedStatusId);

            // Assert
            Assert.Equal(AdoptionStatus.Approved, adoption.AdoptionStatus);
            Assert.Equal(adoptedStatusId, fox.StatusId);
        }

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenRequestDateIsOld()
        {
            // Arrange
            var oldDate = DateTime.UtcNow.AddDays(-20);
            var adoption = new Adoption 
            { 
                AdoptionStatus = AdoptionStatus.Pending, 
                RequestDate = oldDate 
            };

            // Act
            var result = adoption.IsExpired();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenRequestDateIsRecent()
        {
            // Arrange
            var recentDate = DateTime.UtcNow.AddDays(-5);
            var adoption = new Adoption 
            { 
                AdoptionStatus = AdoptionStatus.Pending, 
                RequestDate = recentDate 
            };

            // Act
            var result = adoption.IsExpired();

            // Assert
            Assert.False(result);
        }
    }
}
