namespace XPOS340.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //add browser session

            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(
                opt =>
                {
                    opt.IdleTimeout =TimeSpan.FromSeconds(30);
                }
                );

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

            // Activate Browser Session
            app.UseSession();

            /*app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");*/

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Index}/{id?}");

            app.Run();
        }
    }
}