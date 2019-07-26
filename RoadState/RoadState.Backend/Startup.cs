using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoadState.Backend.Automapper;
using RoadState.DataAccessLayer;

namespace RoadState.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(x => x.MultipartBodyLengthLimit = 1_074_790_400);
            services.AddDbContext<RoadStateContext>(options => options.UseSqlServer
            (Configuration.GetConnectionString("DefaultConnection")));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddScoped<IBugReportCreator, BugReportStorage>();
            services.AddScoped<IBugReportFinder, BugReportStorage>();
            services.AddScoped<IBugReportRater, BugReportStorage>();

            services.AddScoped<ICommentCreator, CommentStorage>();
            services.AddScoped<ICommentLiker, CommentStorage>();

            services.AddScoped<IPhotoCreator, PhotoStorage>();
            services.AddScoped<IPhotoFinder, PhotoStorage>();

            services.AddScoped<IUserCreator, UserStorage>();
            services.AddScoped<IUserFinder, UserStorage>();
            services.AddScoped<IUserUpdator, UserStorage>();

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseStaticFiles();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
