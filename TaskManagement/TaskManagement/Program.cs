using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TaskManagement.Models;
using TaskManagement.Repositories;
using TaskManagement.Services;

namespace TaskManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            builder.Services.AddScoped<IGenericRepository<TaskManagement.Models.Project>, GenericRepository<TaskManagement.Models.Project>>();
            builder.Services.AddScoped<IGenericRepository<TaskManagement.Models.Task>, GenericRepository<TaskManagement.Models.Task>>();
            builder.Services.AddScoped<IGenericRepository<TaskManagement.Models.User>, GenericRepository<TaskManagement.Models.User>>();
            builder.Services.AddScoped<IGenericRepository<UserTask>, GenericRepository<UserTask>>();
            builder.Services.AddScoped<IGenericRepository<Subtask>, GenericRepository<Subtask>>();

            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ISubtaskService, SubtaskService>();

            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "UserLoginCookie";
                    config.LoginPath = "/Account/Login";
                });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
