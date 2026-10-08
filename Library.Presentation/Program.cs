using Library.Application.Interfaces;
using Library.Application.Profiles;
using Library.Application.Services;
using Library.Domain.Interfaces;
using Library.Infrastructure.Repositories;
using Library.Persistence;
using Library.Persistence.Interfaces;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

// تنظیم NLog
var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // تنظیمات Logging برای استفاده از NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Dependency Injection
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
    builder.Services.AddScoped<IAuthorService, AuthorService>();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<BookProfile>();
        cfg.AddProfile<AuthorProfile>();
        cfg.AddProfile<CategoryProfile>();
        cfg.AddProfile<MemberProfile>();
        cfg.AddProfile<BorrowingProfile>();
    });

    builder.Services.AddScoped<IMemberRepository, MemberRepository>();
    builder.Services.AddScoped<IMemberService, MemberService>();
    builder.Services.AddScoped<IBookRepository, BookRepository>();
    builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();
    builder.Services.AddScoped<IBorrowingService, BorrowingService>();
    builder.Services.AddScoped<IBookService, BookService>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
