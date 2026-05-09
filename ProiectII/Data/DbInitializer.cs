using Microsoft.AspNetCore.Identity;
using ProiectII.Models;
using Microsoft.EntityFrameworkCore;

namespace ProiectII.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
<<<<<<< HEAD
            // 1. Pregătim fișierele fizice (poze)
            SeedImages("foxes");
            SeedImages("users");

            // 2. Populăm baza de date modular
            await SeedRolesAndUsersAsync(userManager, roleManager);
            await SeedSystemDataAsync(context);
            await SeedFoxesAndReportsAsync(context, userManager);
        }

        // ==========================================
        // METODE PRIVATE DE SEEDING
        // ==========================================

        private static void SeedImages(string category)
        {
            var seedImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedImages", category);
            var targetPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", category);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            if (Directory.Exists(seedImagesPath))
            {
                var files = Directory.GetFiles(seedImagesPath);
                foreach (var file in files)
                {
                    var destFile = Path.Combine(targetPath, Path.GetFileName(file));
                    if (!File.Exists(destFile))
                    {
                        File.Copy(file, destFile);
                    }
                }
            }
        }

        private static async Task SeedRolesAndUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "User", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // CRITIC: Am adăugat ProfilePictureUrl pentru toți utilizatorii
            var usersToCreate = new List<(ApplicationUser User, string Password, string Role)>
            {
                (new ApplicationUser { UserName = "admin@fox.com", Email = "admin@fox.com", FirstName = "Victor", LastName = "Admin", BornDate = new DateOnly(1995, 5, 20), ProfilePictureUrl = "admin.jpg", EmailConfirmed = true }, "SecurePass123!", "Admin"),
                (new ApplicationUser { UserName = "vet@fox.com", Email = "vet@fox.com", FirstName = "Maria", LastName = "Doctor", BornDate = new DateOnly(1988, 3, 15), ProfilePictureUrl = "vet.jpg", EmailConfirmed = true }, "VetPass123!", "Employee"),
                (new ApplicationUser { UserName = "user1@fox.com", Email = "user1@fox.com", FirstName = "Ion", LastName = "Popescu", BornDate = new DateOnly(2000, 1, 1), ProfilePictureUrl = "user1.jpg", EmailConfirmed = true }, "UserPass123!", "User"),
                (new ApplicationUser { UserName = "user2@fox.com", Email = "user2@fox.com", FirstName = "Elena", LastName = "Ionescu", BornDate = new DateOnly(2002, 7, 10), ProfilePictureUrl = "user2.jpg", EmailConfirmed = true }, "UserPass123!", "User")
            };

            foreach (var u in usersToCreate)
            {
                if (await userManager.FindByEmailAsync(u.User.Email) == null)
                {
                    await userManager.CreateAsync(u.User, u.Password);
                    await userManager.AddToRoleAsync(u.User, u.Role);
                }
            }
        }

        private static async Task SeedSystemDataAsync(ApplicationDbContext context)
        {
            // Statusuri
=======
            // 1. Roluri
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // 2. Utilizatori (Admin și un User normal pentru testarea adopțiilor/rapoartelor)
            ApplicationUser adminUser = await userManager.FindByEmailAsync("admin@fox.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@fox.com",
                    Email = "admin@fox.com",
                    FirstName = "Victor",
                    LastName = "Admin",
                    BornDate = new DateOnly(1995, 5, 20),
                    EmailConfirmed = true,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(adminUser, "SecurePass123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    Console.WriteLine($"[SEED] Eroare la crearea adminului: {errors}");
                }
            }

            ApplicationUser normalUser = await userManager.FindByEmailAsync("user@fox.com");
            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    UserName = "user@fox.com",
                    Email = "user@fox.com",
                    FirstName = "Ion",
                    LastName = "Popescu",
                    BornDate = new DateOnly(2000, 1, 1),
                    EmailConfirmed = true,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(normalUser, "UserPass123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }

            // 3. Statusuri
>>>>>>> origin/master
            if (!context.Statuses.Any())
            {
                context.Statuses.AddRange(
                    new Status { Name = "Healthy", Description = "Ready for a new home", IsAdoptable = true, FoxStatus = FoxStatus.Healthy },
                    new Status { Name = "Under Treatment", Description = "In medical wing", IsAdoptable = false, FoxStatus = FoxStatus.Healthy },
<<<<<<< HEAD
                    new Status { Name = "Quarantined", Description = "Observation period", IsAdoptable = false, FoxStatus = FoxStatus.Healthy }
=======
                    new Status { Name = "Quarantined", Description = "New arrival", IsAdoptable = false, FoxStatus = FoxStatus.Healthy }
>>>>>>> origin/master
                );
                await context.SaveChangesAsync();
            }

<<<<<<< HEAD
            // Locații
=======
            // 4. Locații (Pentru Centru Țarc, Vulpile văzute și Rapoarte)
>>>>>>> origin/master
            if (!context.Locations.Any())
            {
                context.Locations.AddRange(
                    new Location { Name = "Enclosure Alpha Center", Coordinate = new Coordinate { Latitude = 46.7712m, Longitude = 23.5923m }, PrecisionRadius = 2.0 },
<<<<<<< HEAD
                    new Location { Name = "Enclosure Beta Center", Coordinate = new Coordinate { Latitude = 46.7720m, Longitude = 23.5930m }, PrecisionRadius = 2.0 },
                    new Location { Name = "North Forest Edge", Coordinate = new Coordinate { Latitude = 46.7800m, Longitude = 23.6000m }, PrecisionRadius = 10.5 },
                    new Location { Name = "Backyard Spotted", Coordinate = new Coordinate { Latitude = 46.7750m, Longitude = 23.5800m }, PrecisionRadius = 15.0 },
                    new Location { Name = "Highway Crossing", Coordinate = new Coordinate { Latitude = 46.7600m, Longitude = 23.6100m }, PrecisionRadius = 5.0 }
=======
                    new Location { Name = "North Forest Edge", Coordinate = new Coordinate { Latitude = 46.7800m, Longitude = 23.6000m }, PrecisionRadius = 10.5 },
                    new Location { Name = "Backyard Spotted", Coordinate = new Coordinate { Latitude = 46.7750m, Longitude = 23.5800m }, PrecisionRadius = 15.0 }
>>>>>>> origin/master
                );
                await context.SaveChangesAsync();
            }

<<<<<<< HEAD
            // Țarcuri
            if (!context.Enclosures.Any())
            {
                var locAlpha = context.Locations.First(l => l.Name == "Enclosure Alpha Center");
                var locBeta = context.Locations.First(l => l.Name == "Enclosure Beta Center");

                context.Enclosures.AddRange(
                    new Enclosure { Name = "Alpha Wing", Description = "Main healthy area", ColorMaskHex = "#FF5733", Opacity = 0.6, CenterLocationId = locAlpha.Id },
                    new Enclosure { Name = "Beta Quarantine", Description = "Strict quarantine area", ColorMaskHex = "#33FF57", Opacity = 0.8, CenterLocationId = locBeta.Id }
                );
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedFoxesAndReportsAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            // Vulpi
            if (!context.Foxes.Any())
            {
                var healthy = context.Statuses.First(s => s.Name == "Healthy");
                var treatment = context.Statuses.First(s => s.Name == "Under Treatment");
                var quarantined = context.Statuses.First(s => s.Name == "Quarantined");
                var alphaWing = context.Enclosures.First(e => e.Name == "Alpha Wing");
                var betaWing = context.Enclosures.First(e => e.Name == "Beta Quarantine");
                var locForest = context.Locations.First(l => l.Name == "North Forest Edge");
                var locHighway = context.Locations.First(l => l.Name == "Highway Crossing");

                context.Foxes.AddRange(
                    new Fox { Name = "Foxy", Description = "Friendly and fast.", ImageUrl = "foxy.jpg", StatusId = healthy.Id, EnclosureId = alphaWing.Id, FirstSeenLocationId = locForest.Id },
                    new Fox { Name = "Shadow", Description = "Hard to spot.", ImageUrl = "shadow.jpg", StatusId = treatment.Id, EnclosureId = alphaWing.Id },
                    new Fox { Name = "Rusty", Description = "Found near the road.", ImageUrl = "rusty.jpg", StatusId = quarantined.Id, EnclosureId = betaWing.Id, FirstSeenLocationId = locHighway.Id },
                    new Fox { Name = "Luna", Description = "Very playful.", ImageUrl = "luna.jpg", StatusId = healthy.Id, EnclosureId = alphaWing.Id }
=======
            // 5. Țarcuri și Puncte (Relație 1:N)
            if (!context.Enclosures.Any())
            {
                var center = context.Locations.First(l => l.Name == "Enclosure Alpha Center");
                var enclosure = new Enclosure
                {
                    Name = "Alpha Wing",
                    Description = "Main recovery area",
                    ColorMaskHex = "#FF5733",
                    Opacity = 0.6,
                    CenterLocationId = center.Id
                };
                context.Enclosures.Add(enclosure);
                await context.SaveChangesAsync();

                context.EnclosurePoints.AddRange(
                    new EnclosurePoint { EnclosureId = enclosure.Id, DrawOrder = 1, Coordinate = new Coordinate { Latitude = 46.7710m, Longitude = 23.5920m } },
                    new EnclosurePoint { EnclosureId = enclosure.Id, DrawOrder = 2, Coordinate = new Coordinate { Latitude = 46.7715m, Longitude = 23.5925m } }
                );
                await context.SaveChangesAsync();
            }

            // 6. Vulpi (Relații cu Status, Enclosure și Locations)
            if (!context.Foxes.Any())
            {
                var healthy = context.Statuses.First(s => s.Name == "Healthy");
                var sick = context.Statuses.First(s => s.Name == "Under Treatment");
                var enclosure = context.Enclosures.First();
                var loc1 = context.Locations.First(l => l.Name == "North Forest Edge");

                context.Foxes.AddRange(
                    new Fox
                    {
                        Name = "Foxy",
                        Description = "Friendly and fast.",
                        ImageUrl = "foxy.jpg",
                        StatusId = healthy.Id,
                        EnclosureId = enclosure.Id,
                        FirstSeenLocationId = loc1.Id
                    },
                    new Fox
                    {
                        Name = "Shadow",
                        Description = "Hard to spot.",
                        ImageUrl = "shadow.jpg",
                        StatusId = sick.Id,
                        EnclosureId = enclosure.Id
                    }
>>>>>>> origin/master
                );
                await context.SaveChangesAsync();
            }

<<<<<<< HEAD
            var user1 = await userManager.FindByEmailAsync("user1@fox.com");
            var user2 = await userManager.FindByEmailAsync("user2@fox.com");

            // Adopții și Rapoarte rămân neschimbate (folosesc user1 și user2)
            if (!context.Adoptions.Any())
            {
                var foxy = context.Foxes.First(f => f.Name == "Foxy");
                context.Adoptions.Add(new Adoption { FoxId = foxy.Id, UserId = user1!.Id, AdoptionStatus = 0, RequestDate = DateTime.UtcNow, Reason = "Large garden." });
                await context.SaveChangesAsync();
=======
            // 7. Interacțiuni: Adopții, Comentarii, Rapoarte
            if (!context.Adoptions.Any())
            {
                var fox = context.Foxes.First(f => f.Name == "Foxy");
                context.Adoptions.Add(new Adoption
                {
                    FoxId = fox.Id,
                    UserId = normalUser.Id,
                    AdoptionStatus = 0, // Pending
                    RequestDate = DateTime.UtcNow,
                    Reason = "I love foxes."
                });
            }

            if (!context.Comments.Any())
            {
                var fox = context.Foxes.First(f => f.Name == "Foxy");
                context.Comments.Add(new Comment
                {
                    Content = "Is she eating well?",
                    CreatedAt = DateTime.UtcNow,
                    UserId = adminUser.Id,
                    FoxId = fox.Id
                });
>>>>>>> origin/master
            }

            if (!context.Reports.Any())
            {
<<<<<<< HEAD
                var locBackyard = context.Locations.First(l => l.Name == "Backyard Spotted");
                context.Reports.Add(new Report { Description = "Found a fox.", ReporterId = user1!.Id, LocationId = locBackyard.Id, ReportStatus = 0 });
                await context.SaveChangesAsync();
            }
=======
                var loc = context.Locations.First(l => l.Name == "Backyard Spotted");
                context.Reports.Add(new Report
                {
                    Description = "Found a fox near the trash can.",
                    ReporterId = normalUser.Id,
                    LocationId = loc.Id,
                    ReportStatus = 0 // Pending
                });
            }

            await context.SaveChangesAsync();
>>>>>>> origin/master
        }
    }
}