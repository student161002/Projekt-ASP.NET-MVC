using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Sklep_Internetowy.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Sklep_Internetowy.Models.SklepDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("SklepConnection")));

builder.Services.AddDefaultIdentity<Microsoft.AspNetCore.Identity.IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddRoles<Microsoft.AspNetCore.Identity.IdentityRole>()
	.AddEntityFrameworkStores<Sklep_Internetowy.Models.SklepDbContext>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
	var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser>>();

	if (!await roleManager.RoleExistsAsync("Admin"))
		await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole("Admin"));

	//login: admin@antykwariat.pl
	//has³o: Admin123!
	var adminEmail = "admin@antykwariat.pl";
	var adminUser = await userManager.FindByEmailAsync(adminEmail);
	if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
	{
		await userManager.AddToRoleAsync(adminUser, "Admin");
	}
}

// Configure the HTTP request pipeline.x
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();
app.Run();
