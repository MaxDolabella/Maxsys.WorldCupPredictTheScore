using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Maxsys.WorldCupPredictTheScore.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.WorldCupPredictTheScore.Web.Data;

public static class Seeder
{
    public static async ValueTask CreateAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var delete = config.GetValue<bool>("Seeder:DeleteDatabase");
        if (delete)
            await context.Database.EnsureDeletedAsync();

        //await context.Database.MigrateAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public static async ValueTask SeedAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await context.Matches.AnyAsync())
            return;

        var ALE = new Team("Alemanha", "GER");
        var ASA = new Team("Arábia Saudita", "KSA");
        var ARG = new Team("Argentina", "ARG");
        var AUS = new Team("Austrália", "AUS");
        var BEL = new Team("Bélgica", "BEL");
        var BRA = new Team("Brasil", "BRA");
        var CAM = new Team("Camarões", "CMR");
        var CAN = new Team("Canadá", "CAN");
        var COR = new Team("Coreia do Sul", "KOR");
        var CRI = new Team("Costa Rica", "CRC");
        var CRO = new Team("Croácia", "CRO");
        var DIN = new Team("Dinamarca", "DEN");
        var EQU = new Team("Equador", "ECU");
        var ESP = new Team("Espanha", "ESP");
        var EUA = new Team("Estados Unidos", "USA");
        var FRA = new Team("França", "FRA");
        var GAN = new Team("Gana", "GHA");
        var HOL = new Team("Holanda", "NED");
        var ING = new Team("Inglaterra", "ENG");
        var IRA = new Team("Irã", "IRN");
        var JAP = new Team("Japão", "JPN");
        var MAR = new Team("Marrocos", "MAR");
        var MEX = new Team("México", "MEX");
        var GAL = new Team("País de Gales", "WAL");
        var POL = new Team("Polônia", "POL");
        var POR = new Team("Portugal", "POR");
        var QAT = new Team("Qatar", "QAT");
        var SEN = new Team("Senegal", "SEN");
        var SER = new Team("Sérvia", "SRB");
        var SUI = new Team("Suiça", "SUI");
        var TUN = new Team("Tunísia", "TUN");
        var URU = new Team("Uruguai", "URU");

        context.Teams.AddRange(new[]
        {
            ALE, ASA, ARG, AUS, BEL, BRA, CAM, CAN,
            COR, CRI, CRO, DIN, EQU, ESP, EUA, FRA,
            GAN, HOL, ING, IRA, JAP, MAR, MEX, GAL,
            POL, POR, QAT, SEN, SER, SUI, TUN, URU
        });

        context.Matches.AddRange(new[]
        {
            new Match('A', 1, new DateTime(2022, 11, 20, 16, 0, 0), QAT,EQU),
            new Match('B', 1, new DateTime(2022, 11, 21, 13, 0, 0), ING,IRA),
            new Match('A', 1, new DateTime(2022, 11, 21, 16, 0, 0), SEN,HOL),
            new Match('B', 1, new DateTime(2022, 11, 21, 19, 0, 0), EUA,GAL),
            new Match('C', 1, new DateTime(2022, 11, 22, 10, 0, 0), ARG,ASA),
            new Match('D', 1, new DateTime(2022, 11, 22, 13, 0, 0), DIN,TUN),
            new Match('C', 1, new DateTime(2022, 11, 22, 16, 0, 0), MEX,POL),
            new Match('D', 1, new DateTime(2022, 11, 22, 19, 0, 0), FRA,AUS),
            new Match('F', 1, new DateTime(2022, 11, 23, 10, 0, 0), MAR,CRO),
            new Match('E', 1, new DateTime(2022, 11, 23, 13, 0, 0), ALE,JAP),
            new Match('E', 1, new DateTime(2022, 11, 23, 16, 0, 0), ESP,CRI),
            new Match('F', 1, new DateTime(2022, 11, 23, 19, 0, 0), BEL,CAN),
            new Match('G', 1, new DateTime(2022, 11, 24, 10, 0, 0), SUI,CAM),
            new Match('H', 1, new DateTime(2022, 11, 24, 13, 0, 0), URU,COR),
            new Match('H', 1, new DateTime(2022, 11, 24, 16, 0, 0), POR,GAN),
            new Match('G', 1, new DateTime(2022, 11, 24, 19, 0, 0), BRA,SER),

            new Match('B', 2, new DateTime(2022, 11, 25, 10, 0, 0), GAL,IRA),
            new Match('A', 2, new DateTime(2022, 11, 25, 13, 0, 0), QAT,SEN),
            new Match('A', 2, new DateTime(2022, 11, 25, 16, 0, 0), HOL,EQU),
            new Match('B', 2, new DateTime(2022, 11, 25, 19, 0, 0), ING,EUA),
            new Match('D', 2, new DateTime(2022, 11, 26, 10, 0, 0), TUN,AUS),
            new Match('C', 2, new DateTime(2022, 11, 26, 13, 0, 0), POL,ASA),
            new Match('D', 2, new DateTime(2022, 11, 26, 16, 0, 0), FRA,DIN),
            new Match('C', 2, new DateTime(2022, 11, 26, 19, 0, 0), ARG,MEX),
            new Match('E', 2, new DateTime(2022, 11, 27, 10, 0, 0), JAP,CRI),
            new Match('F', 2, new DateTime(2022, 11, 27, 13, 0, 0), BEL,MAR),
            new Match('F', 2, new DateTime(2022, 11, 27, 16, 0, 0), CRO,CAN),
            new Match('E', 2, new DateTime(2022, 11, 27, 19, 0, 0), ESP,ALE),
            new Match('G', 2, new DateTime(2022, 11, 28, 10, 0, 0), CAM,SER),
            new Match('H', 2, new DateTime(2022, 11, 28, 13, 0, 0), COR,GAN),
            new Match('G', 2, new DateTime(2022, 11, 28, 16, 0, 0), BRA,SUI),
            new Match('G', 2, new DateTime(2022, 11, 28, 19, 0, 0), POR,URU),

            new Match('A', 3, new DateTime(2022, 11, 29, 15, 0, 0), HOL,QAT),
            new Match('A', 3, new DateTime(2022, 11, 29, 15, 0, 0), EQU,SEN),
            new Match('B', 3, new DateTime(2022, 11, 29, 19, 0, 0), IRA,EUA),
            new Match('B', 3, new DateTime(2022, 11, 29, 19, 0, 0), GAL,ING),
            new Match('D', 3, new DateTime(2022, 11, 30, 15, 0, 0), TUN,FRA),
            new Match('D', 3, new DateTime(2022, 11, 30, 15, 0, 0), AUS,DIN),
            new Match('C', 3, new DateTime(2022, 11, 30, 19, 0, 0), POL,ARG),
            new Match('C', 3, new DateTime(2022, 11, 30, 19, 0, 0), ASA,MEX),
            new Match('F', 3, new DateTime(2022, 12, 1, 15, 0, 0), CRO,BEL),
            new Match('F', 3, new DateTime(2022, 12, 1, 15, 0, 0), CAN,MAR),
            new Match('E', 3, new DateTime(2022, 12, 1, 19, 0, 0), CRI,ALE),
            new Match('E', 3, new DateTime(2022, 12, 1, 19, 0, 0), JAP,ESP),
            new Match('H', 3, new DateTime(2022, 12, 2, 15, 0, 0), COR,POR),
            new Match('H', 3, new DateTime(2022, 12, 2, 15, 0, 0), GAN,URU),
            new Match('G', 3, new DateTime(2022, 12, 2, 19, 0, 0), CAM,BRA),
            new Match('G', 3, new DateTime(2022, 12, 2, 19, 0, 0), SER,SUI),
        });

        await context.SaveChangesAsync();
    }

    public static async ValueTask SeedRoles(IHost host, bool addTestUsers = false)
    {
        var adminRole = new AppRole("admin");
        var userRole = new AppRole("user");
        IdentityResult result;

        using var scope = host.Services.CreateScope();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        using var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<AppUser>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        //using var emailStore = scope.ServiceProvider.GetRequiredService<IUserEmailStore<AppUser>>();

        var adminUserName = config.GetValue<string>("Seeder:AdminUser:UserName");
        var adminPassword = config.GetValue<string>("Seeder:AdminUser:Password");

        var adminUser = new AppUser()
        {
            Email = adminUserName,
            EmailConfirmed = true
        };

        if ((await roleManager.FindByNameAsync(adminRole.Name)) is null)
            result = await roleManager.CreateAsync(adminRole);

        if ((await roleManager.FindByNameAsync(userRole.Name)) is null)
            result = await roleManager.CreateAsync(userRole);

        if ((await userManager.FindByEmailAsync(adminUser.Email)) is null)
        {
            await userStore.SetUserNameAsync(adminUser, adminUser.Email, CancellationToken.None);
            //await emailStore.SetEmailAsync(adminUser, adminUser.Email, CancellationToken.None);
            result = await userManager.CreateAsync(adminUser, adminPassword);
            result = await userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }

        if (addTestUsers/*((WebApplication)host).Environment.IsDevelopment()*/)
        {
            for (int i = 0; i < 3; i++)
            {
                var user = new AppUser()
                {
                    Email = $"User@{i}",
                    EmailConfirmed = true
                };

                if ((await userManager.FindByEmailAsync(user.Email)) is null)
                {
                    await userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
                    //await emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
                    result = await userManager.CreateAsync(user, $"User@{i}");
                    result = await userManager.AddToRoleAsync(user, userRole.Name);
                }
            }
        }
    }
}