using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace YourNamespace
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
            services.AddControllersWithViews(); // Add MVC services

            // Add other services here as needed
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Use developer exception page in development environment
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Use custom error page in production environment
                app.UseHsts(); // Use HTTP Strict Transport Security (HSTS)
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

            app.UseStaticFiles(); // Serve static files like HTML, CSS, and JavaScript

            app.UseRouting(); // Enable routing middleware

            app.UseAuthorization(); // Use authentication middleware

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"); // Define default route
            });
        }
    }
}
