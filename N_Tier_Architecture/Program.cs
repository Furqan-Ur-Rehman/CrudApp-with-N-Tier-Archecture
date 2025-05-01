using Microsoft.EntityFrameworkCore;
using N_Tier_Architecture_DataAccess.Data;
using N_Tier_Architecture_DataAccess.Repository;
using N_Tier_Architecture_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("JOTConfig"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddScoped<IUnitofWork, UnitofWork>();

builder.Services.AddRazorPages();

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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

//Create a Scope for getting all services and call a function for apply Role Based Auth.
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    await CreateAdminAndUser(service);
}
app.Run();
//Create a function for Role Based Authentication and Authorization
static async Task CreateAdminAndUser(IServiceProvider service)
{
    //Create rolemanager and usermanager variable start here
    var rolemanager = service.GetRequiredService<RoleManager<IdentityRole>>();
    var usermanger = service.GetRequiredService<UserManager<IdentityUser>>();
    //Create rolemanager and usermanager variable End here


    string[] rolenames = ["Admin", "User", "Member"];

    IdentityResult result;

    foreach (var rolename in rolenames)
    {
        var existrole = await rolemanager.RoleExistsAsync(rolename);
        if (!existrole)
        {
            //Below line Store Roles in AspNetRoles Table
            result = await rolemanager.CreateAsync(new IdentityRole(rolename));
        }
    }

    //Add Admin email and password store in AspNetUser Table Start Here
    var adminuser = new IdentityUser()
    {
        UserName = "admin786@gmail.com",
        Email = "admin786@gmail.com",
        EmailConfirmed = true
    };

    var adminpassword = "Admin786#";
    var user = await usermanger.FindByEmailAsync("admin786@gmail.com");

    if (user == null)
    {
        var createadmin = await usermanger.CreateAsync(adminuser, adminpassword);
    //Add Admin email and password store in AspNetUser Table End Here
        if (createadmin.Succeeded)
        {
            //Below line Store Roles in AspNetUserRole Table
            await usermanger.AddToRoleAsync(adminuser, "Admin");
        }
    }
}