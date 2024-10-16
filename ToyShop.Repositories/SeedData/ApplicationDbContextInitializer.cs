using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ToyShop.Contract.Repositories.Entity;
using ToyShop.Repositories.Base;
using ToyShop.Repositories.Entity;

public class ApplicationDbContextInitializer
{
    private readonly ToyShopDBContext _context;

    public ApplicationDbContextInitializer(ToyShopDBContext context)
    {
        _context = context;
    }

    public void Initialize()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                if (_context.Database.CanConnect())
                {
                    _context.Database.Migrate();
                }
                else
                {
                    _context.Database.Migrate();
                }
            }

            Seed();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            _context.Dispose();
        }
    }

    private void Seed()
    {
        SeedRoles();
        SeedUsers();
        //SeedDefectCodes();
        SeedToys();
        SeedContracts();
        SeedRestoreToys();
        SeedChats();
        _context.SaveChanges();
    }

    private void SeedRoles()
    {
        if (_context.ApplicationRoles.Any()) return;

        var roles = new ApplicationRole[]
        {
        new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN", FullName = "Administrator", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow, ConcurrencyStamp = Guid.NewGuid().ToString() },
        new ApplicationRole { Name = "Manager", NormalizedName = "MANAGER", FullName = "Manager", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow, ConcurrencyStamp = Guid.NewGuid().ToString() },
        new ApplicationRole { Name = "Customer", NormalizedName = "CUSTOMER", FullName = "Customer", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow, ConcurrencyStamp = Guid.NewGuid().ToString() },
        };

        _context.ApplicationRoles.AddRange(roles);
        _context.SaveChanges();
    }

    private void SeedUsers()
    {
        if (_context.ApplicationUsers.Any()) return;

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var users = new ApplicationUser[]
        {
        new ApplicationUser { UserName = "admin", FullName = "Admin User", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = passwordHasher.HashPassword(null, "admin123@"), CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new ApplicationUser { UserName = "manager", FullName = "Manager User", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = passwordHasher.HashPassword(null, "manager123@"), CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new ApplicationUser { UserName = "customer1", FullName = "Customer One", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = passwordHasher.HashPassword(null, "customer123@"), CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new ApplicationUser { UserName = "customer2", FullName = "Customer Two", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = passwordHasher.HashPassword(null, "customer123@"), CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new ApplicationUser { UserName = "customer3", FullName = "Customer Three", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = passwordHasher.HashPassword(null, "customer123@"), CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new ApplicationUser { UserName = "customer4", FullName = "Customer Four", EmailConfirmed = true, SecurityStamp = Guid.NewGuid().ToString(), PasswordHash = passwordHasher.HashPassword(null, "customer123@"), CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        };

        _context.ApplicationUsers.AddRange(users);
        _context.SaveChanges();
    }

    //private void SeedDefectCodes()
    //{
    //    if (_context.DefectCodes.Any()) return;

    //    var defectCodes = new DefectCode[]
    //    {
    //    new DefectCode { Code = "0", Name = "No Defect", Description = "Toy is in good condition.", BackGroundColor = "#A2E7AA", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
    //    new DefectCode { Code = "1", Name = "Missing Part", Description = "A part is missing from the toy.", BackGroundColor = "#EF9A9A", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
    //    new DefectCode { Code = "2", Name = "Broken", Description = "The toy is broken and unusable.", BackGroundColor = "#E078D5", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
    //    new DefectCode { Code = "3", Name = "Discoloration", Description = "The toy has faded colors.", BackGroundColor = "#88DF4A", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
    //    new DefectCode { Code = "4", Name = "Scratches", Description = "The toy has visible scratches.", BackGroundColor = "#F0B90B", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
    //    new DefectCode { Code = "5", Name = "Dented", Description = "The toy has dents or damage.", BackGroundColor = "#E67F6D", CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
    //    };

    //    _context.DefectCodes.AddRange(defectCodes);
    //}

    private void SeedToys()
    {
        if (_context.Toys.Any()) return;

        var toys = new Toy[]
        {
        new Toy
        {
            ToyName = "Superhero Action Figure",
            ToyImg = "images/superhero_action_figure.png",
            ToyDescription = "A detailed action figure of your favorite superhero.",
            ToyPrice = 29,
            ToyRemainingQuantity = 10,
            ToyQuantitySold = 0,
            Option = "Collectible",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new Toy
        {
            ToyName = "Classic Board Game",
            ToyImg = "images/classic_board_game.png",
            ToyDescription = "A classic board game for family fun.",
            ToyPrice = 19,
            ToyRemainingQuantity = 5,
            ToyQuantitySold = 0,
            Option = "Family",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new Toy
        {
            ToyName = "Wooden Dollhouse",
            ToyImg = "images/wooden_dollhouse.png",
            ToyDescription = "A beautifully crafted wooden dollhouse.",
            ToyPrice = 49,
            ToyRemainingQuantity = 2,
            ToyQuantitySold = 0,
            Option = "Toy",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new Toy
        {
            ToyName = "Soccer Ball",
            ToyImg = "images/soccer_ball.png",
            ToyDescription = "A standard size soccer ball for outdoor play.",
            ToyPrice = 15,
            ToyRemainingQuantity = 20,
            ToyQuantitySold = 0,
            Option = "Sports",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new Toy
        {
            ToyName = "Building Blocks Set",
            ToyImg = "images/building_blocks_set.png",
            ToyDescription = "A set of colorful building blocks for creative play.",
            ToyPrice = 25,
            ToyRemainingQuantity = 15,
            ToyQuantitySold = 5,
            Option = "Educational",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new Toy
        {
            ToyName = "Remote Control Car",
            ToyImg = "images/remote_control_car.png",
            ToyDescription = "A fast remote control car for kids.",
            ToyPrice = 45,
            ToyRemainingQuantity = 8,
            ToyQuantitySold = 2,
            Option = "Electronics",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new Toy
        {
            ToyName = "Puzzle Game",
            ToyImg = "images/puzzle_game.png",
            ToyDescription = "A fun puzzle game for all ages.",
            ToyPrice = 20,
            ToyRemainingQuantity = 12,
            ToyQuantitySold = 6,
            Option = "Family",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        };

        _context.Toys.AddRange(toys);
        _context.SaveChanges();
    }

    private void SeedContracts()
    {
        if (_context.ContractEntitys.Any()) return;

        var users = _context.ApplicationUsers.ToList();
        var toys = _context.Toys.ToList();

        if (users.Count == 0 || toys.Count == 0) return;

        var contracts = new ContractEntity[]
        {
        new ContractEntity
        {
            ContractType = true, // Rental contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ToyId = toys[0].Id,
            UserId = users[0].Id,
            Status = "Active",
            TotalValue = 150,
            NumberOfRentals = 1,
            DateStart = DateOnly.FromDateTime(DateTime.Now),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            ContractType = false, // Return contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ToyId = toys[1].Id,
            UserId = users[1].Id,
            Status = "Completed",
            TotalValue = 100,
            NumberOfRentals = 0,
            DateStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            ContractType = true, // Rental contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ToyId = toys[2].Id,
            UserId = users[2].Id,
            Status = "Active",
            TotalValue = 75,
            NumberOfRentals = 2,
            DateStart = DateOnly.FromDateTime(DateTime.Now),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            ContractType = false, // Return contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ToyId = toys[3].Id,
            UserId = users[3].Id,
            Status = "Completed",
            TotalValue = 60,
            NumberOfRentals = 0,
            DateStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            ContractType = true, // Rental contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ToyId = toys[4].Id,
            UserId = users[4].Id,
            Status = "Active",
            TotalValue = 150,
            NumberOfRentals = 1,
            DateStart = DateOnly.FromDateTime(DateTime.Now),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            ContractType = false, // Return contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ToyId = toys[5].Id,
            UserId = users[5].Id,
            Status = "Completed",
            TotalValue = 50,
            NumberOfRentals = 0,
            DateStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-20)),
            DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
            RestoreToyId = null
        }
        };

        _context.ContractEntitys.AddRange(contracts);
        _context.SaveChanges();

    }

    private void SeedRestoreToys()
    {
        if (_context.RestoreToys.Any()) return;

        var contracts = _context.ContractEntitys.ToList(); // Lấy tất cả các ContractEntity

        var restoreToys = new RestoreToy[]
        {
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[0]?.Id, // Lấy ContractId từ danh sách contracts
            ToyQuality = 90.5,
            Reward = 50,
            OverdueTime = 2.5, // Giả sử là số giờ bị trễ
            TotalMoney = 150.00,
            ContractEntity = contracts[0]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[1]?.Id,
            ToyQuality = 80.0,
            Reward = 40,
            OverdueTime = 1.0,
            TotalMoney = 120.00,
            ContractEntity = contracts[1]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[2]?.Id,
            ToyQuality = 85.0,
            Reward = 45,
            OverdueTime = 3.0,
            TotalMoney = 140.00,
            ContractEntity = contracts[2]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[3]?.Id,
            ToyQuality = 75.0,
            Reward = 35,
            OverdueTime = 4.0,
            TotalMoney = 110.00,
            ContractEntity = contracts[3]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[4]?.Id,
            ToyQuality = 95.0,
            Reward = 60,
            OverdueTime = 0.5,
            TotalMoney = 160.00,
            ContractEntity = contracts[4]
        }
        };

        _context.RestoreToys.AddRange(restoreToys);
        _context.SaveChanges();
    }

    private void SeedChats()
    {
        if (_context.Chats.Any()) return;

        var users = _context.ApplicationUsers.ToList();

        if (users.Count == 0) return;

        var chats = new Chat[]
        {
        new Chat { CreatedBy = users[0].UserName, PartnerId = users[1].UserName, CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new Chat { CreatedBy = users[1].UserName, PartnerId = users[2].UserName, CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new Chat { CreatedBy = users[2].UserName, PartnerId = users[3].UserName, CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new Chat { CreatedBy = users[3].UserName, PartnerId = users[4].UserName, CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow },
        new Chat { CreatedBy = users[4].UserName, PartnerId = users[5].UserName, CreatedTime = DateTimeOffset.UtcNow, LastUpdatedTime = DateTimeOffset.UtcNow }
        };

        _context.Chats.AddRange(chats);
        _context.SaveChanges();
    }

}
