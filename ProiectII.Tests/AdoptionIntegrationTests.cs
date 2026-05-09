using Microsoft.EntityFrameworkCore;
using ProiectII.Data;
using ProiectII.Models;
using Xunit;

//teste InMemory

namespace ProiectII.Tests
{
    public class AdoptionIntegrationTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        [Fact]
        public async Task CreateAdoption_ShouldLinkUserAndFox_InDatabase()
        {
            // Arrange
            using var context = GetDatabaseContext();
            
            var user = new ApplicationUser { Id = "user-1", UserName = "tester@fox.com", FirstName = "John", LastName = "Doe" };
            var status = new Status { Id = 1, Name = "Healthy", Description = "Good" };
            
            // Avem nevoie de o locatie pentru LastSeenLocation deoarece in model nu este nullable
            var location = new Location { Id = 1, Name = "Forest", Coordinate = new Coordinate { Latitude = 45, Longitude = 25 } };
            
            var fox = new Fox 
            { 
                Id = 1, 
                Name = "Red", 
                Status = status, 
                Description = "Fast",
                ImageUrl = "fox.jpg", // Cimpul obligatoriu care lipsea
                LastSeenLocation = location // la fel ca ImageUrl; obligatoriu
            };
            
            context.Users.Add(user);
            context.Statuses.Add(status);
            context.Locations.Add(location);
            context.Foxes.Add(fox);
            await context.SaveChangesAsync();

            // Act
            var adoption = new Adoption 
            { 
                FoxId = fox.Id, 
                UserId = user.Id, 
                AdoptionStatus = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow,
                Reason = "I love foxes"
            };
            context.Adoptions.Add(adoption);
            await context.SaveChangesAsync();

            // Assert
            var savedAdoption = await context.Adoptions
                .Include(a => a.Fox)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == adoption.Id);

            Assert.NotNull(savedAdoption);
            Assert.Equal("Red", savedAdoption.Fox.Name);
            Assert.Equal("John", savedAdoption.User.FirstName);
        }
    }
}
