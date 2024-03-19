using Abc.AuthorLibrary;
using Abc.BusinessService;
using Abc.UnitOfWorkLibrary;
using ABC.BooksLibrary;
using ABC.BusinessBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAssemblyApp.Server;
using WebAssemblyApp.Server.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<DbContext, AbcContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sql"),
                                                    x => x.MigrationsAssembly("ABC.BusinessBase")),
                                                    contextLifetime: ServiceLifetime.Singleton);

builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
builder.Services.AddSingleton<IBooksRepository, BookRepository>();
builder.Services.AddSingleton<ITokenRepository, TokenRepository>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddSingleton<IUserInfoService, UserInfoService>();
builder.Services.AddTransient<IJwtAuthentication, JWTAuthentication>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:issuer"],
        ValidAudience = builder.Configuration["JwtConfig:audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:key"]))
    };
    
    JwtBearerEvents events = new JwtBearerEvents();

    events.OnMessageReceived =async (context) => {
        var cToken = context.Request.Headers.Authorization.FirstOrDefault();

        if(!string.IsNullOrEmpty(cToken)){
            var bearer = cToken.Split(' ');
            
            if(bearer.Length==1)
            return;

            var token = bearer[1];
            var tokenService = builder.Services.BuildServiceProvider().GetService<ITokenService>();

            if(tokenService!=null){
                var tokenData = await tokenService.GetActualToken(token);
                if(tokenData!=null){
                    context.Request.Headers.Authorization = "Bearer "+tokenData.ActualToken;
                }
            }
        }
    };
    
    // uncomment the line if you want to validate token in this event or use middleware
    //options.Events = events;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();    
    app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = new CustomExceptionHandler().Invoke });    
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<TokenMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
