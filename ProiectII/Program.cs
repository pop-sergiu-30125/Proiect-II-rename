<<<<<<< HEAD
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.HttpOverrides;
=======
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
>>>>>>> origin/master
using ProiectII.Data;
using ProiectII.Interfaces;
using ProiectII.Mappings;
using ProiectII.Models;
using ProiectII.Repositories;
using ProiectII.Services.CoreDomain;
<<<<<<< HEAD
using ProiectII.Services.SecurityIdentity;
using ProiectII.Services.UtilityServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

// ==========================================
// 1. BAZA DE DATE & IDENTITY
// ==========================================
=======
using ProiectII.Services.UtilityServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ProiectII.Services.SecurityIdentity;

var builder = WebApplication.CreateBuilder(args);

// 1. Conexiunea la MySQL
>>>>>>> origin/master
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

<<<<<<< HEAD
=======
// 2. OBLIGATORIU: Serviciile de Identity
>>>>>>> origin/master
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

<<<<<<< HEAD
// ==========================================
// 2. DEPENDENCY INJECTION
// ==========================================
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFoxRepository, FoxRepository>();
builder.Services.AddScoped<IAdoptionRepository, AdoptionRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IEnclosureRepository, EnclosureRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IAdoptionService, AdoptionService>();
builder.Services.AddScoped<IFoxService, FoxService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// ==========================================
// 3. AUTENTIFICARE JWT
// ==========================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

=======
// 3. Configurare JWT
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
>>>>>>> origin/master
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
<<<<<<< HEAD
            var token = context.Request.Cookies["jwt_access_token"];
            Console.WriteLine($"\n[DEBUG] Cookie JWT: '{token}'");
            if (!string.IsNullOrWhiteSpace(token) && token.Contains('.') && token != "undefined")
            {
                context.Token = token.Replace("Bearer ", "").Trim('"').Trim();
                Console.WriteLine("[DEBUG] Token acceptat.");
            }
            else
            {
                Console.WriteLine("[DEBUG] Token respins.");
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[EROARE JWT]: {context.Exception.Message}");
=======
            context.Token = context.Request.Cookies["jwt_access_token"];
>>>>>>> origin/master
            return Task.CompletedTask;
        }
    };
});

<<<<<<< HEAD
// ==========================================
// 4. CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7033")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ==========================================
// 5. SWAGGER & CONTROLLERS
// ==========================================
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Proiect II API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introdu token-ul JWT."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
=======
builder.Services.AddScoped<IFoxRepository, FoxRepository>();
builder.Services.AddScoped<IAdoptionRepository, AdoptionRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IEnclosureRepository, EnclosureRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAutoMapper(typeof(MappingProfile));

// 4. Servicii pentru API și Swagger
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Proiect II API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
>>>>>>> origin/master
    {
        {
            new OpenApiSecurityScheme
            {
<<<<<<< HEAD
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
=======
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
>>>>>>> origin/master
        }
    });
});

<<<<<<< HEAD
var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

// ==========================================
// 6. MIGRARE AUTOMATA LA STARTUP (fara seed)
// ==========================================
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    var context = services.GetRequiredService<ApplicationDbContext>();

//    int retries = 10;
//    while (retries > 0)
//    {
//        try
//        {
//            logger.LogInformation($"[DB] Incercare migrare... (Ramase: {retries})");
//            await context.Database.MigrateAsync();
//            logger.LogInformation("[DB] Migrare reusita! API pornit.");

//            using (var innerScope = app.Services.CreateScope())
//            {
//                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//                foreach (var role in new[] { "Admin", "User", "Employee" })
//                    if (!await roleManager.RoleExistsAsync(role))
//                        await roleManager.CreateAsync(new IdentityRole(role));

//                if (await userManager.FindByEmailAsync("admin@fox.com") == null)
//                {
//                    var admin = new ApplicationUser
//                    {
//                        UserName = "admin@fox.com",
//                        Email = "admin@fox.com",
//                        FirstName = "Victor",
//                        LastName = "Admin",
//                        BornDate = new DateOnly(1995, 5, 20),
//                        EmailConfirmed = true,
//                        IsActive = true,
//                        LastLogin = DateTime.UtcNow
//                    };

//                    await userManager.CreateAsync(admin, "SecurePass123!");
//                    await userManager.AddToRoleAsync(admin, "Admin");
//                }

//                if (await userManager.FindByEmailAsync("user@fox.com") == null)
//                {
//                    var user = new ApplicationUser
//                    {
//                        UserName = "user@fox.com",
//                        Email = "user@fox.com",
//                        FirstName = "Ion",
//                        LastName = "Popescu",
//                        BornDate = new DateOnly(2000, 1, 1),
//                        EmailConfirmed = true,
//                        IsActive = true,
//                        LastLogin = DateTime.UtcNow
//                    };

//                    await userManager.CreateAsync(user, "UserPass123!");
//                    await userManager.AddToRoleAsync(user, "User");
//                }
//            }


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    int retries = 10;
    while (retries > 0)
    {
        try
        {
            logger.LogInformation("[DB] Încercare migrare... (Rămase: {Retries})", retries);

            // 1. Aplică structura tabelelor
            await context.Database.MigrateAsync();
            logger.LogInformation("[DB] Migrare reușită! Tabelele există.");

            // 2. Extrage managerii necesari pentru Identity
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // 3. Apelează clasa ta externă (asigură-te că namespace-ul ProiectII.Data este importat sus cu 'using')
            await DbInitializer.SeedData(context, userManager, roleManager);
            logger.LogInformation("[DB] Seeding-ul datelor a fost finalizat cu succes.");

            break; // Ieșim din buclă dacă totul a decurs fără erori
        }
        catch (Exception ex)
        {
            retries--;
            logger.LogWarning("[DB] Migrare/Seeding eșuat: {Message}", ex.Message);

            if (retries == 0)
            {
                logger.LogCritical("[DB] Eșec total la migrare și inițializare!");
                throw;
            }
            await Task.Delay(3000); // Pauză înainte de următoarea încercare
        }
    }
}








//            break;
//        }
//        catch (Exception ex)
//        {
//            retries--;
//            logger.LogWarning($"[DB] Migrare esuata: {ex.Message}");
//            if (retries == 0)
//            {
//                logger.LogCritical("[DB] Esec total la migrare!");
//                throw;
//            }
//            await Task.Delay(3000);
//        }
//    }
//}

// ==========================================
// 7. PIPELINE
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
=======
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

// Înregistrare servicii noi
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// 4. INTEGRARE SEEDING + VERIFICARE MAPPING
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // AUTOMAPPER
        var mapper = services.GetRequiredService<AutoMapper.IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        // --------------------------------------

        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DbInitializer.SeedData(context, userManager, roleManager);
        Console.WriteLine("[SEED] Datele au fost sincronizate cu succes.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        // Dacă eroarea e de la AutoMapper, aici vei vedea detaliile
        logger.LogError(ex, "Eroare critică la pornire (Mapping sau Seeding).");
        throw; // Forțăm oprirea dacă maparea e greșită
    }
}

// 5. Pipeline-ul HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Accesează /swagger în browser ca să vezi API-ul
>>>>>>> origin/master
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

<<<<<<< HEAD
app.UseStaticFiles();
app.UseRouting();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
=======
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// OBLIGATORIU: Ordinea contează!
app.UseAuthentication(); // Cine ești?
app.UseAuthorization();  // Ce ai voie să faci?

>>>>>>> origin/master
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

<<<<<<< HEAD
app.Urls.Add("http://*:8080");

app.Run();
=======
app.Run();
>>>>>>> origin/master
