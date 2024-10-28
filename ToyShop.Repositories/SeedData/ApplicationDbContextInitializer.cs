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
        SeedToys();
        SeedContracts();
        SeedRestoreToys();
        SeedChats();
        SeedFeedBacks();
        SeedTransactions();
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
            ToyPriceSale = 29,
            ToyPriceRent = 4,
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
            ToyPriceSale = 19,
            ToyPriceRent = 4,
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
            ToyPriceSale = 49, 
            ToyPriceRent = 4,
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
            ToyPriceSale = 15, 
            ToyPriceRent = 4,
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
            ToyPriceSale = 25,
            ToyPriceRent = 4,
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
            ToyPriceSale = 45, 
            ToyPriceRent = 4,
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
            ToyPriceSale = 20, 
            ToyPriceRent = 4,
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
            //ContractType = true, // Rental contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            UserId = users[0].Id,
            Status = "Active",
            TotalValue = 150,
            NumberOfRentals = 1,
            //DateStart = DateOnly.FromDateTime(DateTime.Now),
            //DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            //ContractType = false, // Return contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            UserId = users[1].Id,
            Status = "Completed",
            TotalValue = 100,
            NumberOfRentals = 0,
            //DateStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
            //DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            //ContractType = true, // Rental contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            UserId = users[2].Id,
            Status = "Active",
            TotalValue = 75,
            NumberOfRentals = 2,
            //DateStart = DateOnly.FromDateTime(DateTime.Now),
            //DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            //ContractType = false, // Return contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            UserId = users[3].Id,
            Status = "Completed",
            TotalValue = 60,
            NumberOfRentals = 0,
            //DateStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
            //DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            //ContractType = true, // Rental contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            UserId = users[4].Id,
            Status = "Active",
            TotalValue = 150,
            NumberOfRentals = 1,
            //DateStart = DateOnly.FromDateTime(DateTime.Now),
            //DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
            RestoreToyId = null
        },
        new ContractEntity
        {
            //ContractType = false, // Return contract
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            UserId = users[5].Id,
            Status = "Completed",
            TotalValue = 50,
            NumberOfRentals = 0,
            //DateStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-20)),
            //DateEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
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
            TotalToyQuality = 90.5,
            TotalReward = 50,
            TotalMoney = 150.00,
            ContractEntity = contracts[0]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[1]?.Id,
            TotalToyQuality = 80.0,
            TotalReward = 40,
            TotalMoney = 120.00,
            ContractEntity = contracts[1]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[2]?.Id,
            TotalToyQuality = 85.0,
            TotalReward = 45,
            TotalMoney = 140.00,
            ContractEntity = contracts[2]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[3]?.Id,
            TotalToyQuality = 75.0,
            TotalReward = 35,
            TotalMoney = 110.00,
            ContractEntity = contracts[3]
        },
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[4]?.Id,
            TotalToyQuality = 95.0,
            TotalReward = 60,
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
    private void SeedFeedBacks()
    {
        // Kiểm tra nếu đã có dữ liệu FeedBack trong DB thì không thêm nữa
        if (_context.Feedbacks.Any()) return;

        // Lấy danh sách người dùng và đồ chơi để sử dụng cho việc thêm phản hồi
        var users = _context.ApplicationUsers.ToList();
        var toys = _context.Toys.ToList();

        if (users.Count == 0 || toys.Count == 0) return;

        // Tạo một danh sách các phản hồi giả định
        var feedbacks = new FeedBack[]
        {
        new FeedBack
        {
            UserId = users[0].Id.ToString(),
            ToyId = toys[0].Id.ToString(),
            Content = "This superhero action figure is amazing!",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new FeedBack
        {
            UserId = users[1].Id.ToString(),
            ToyId = toys[1].Id.ToString(),
            Content = "We had a lot of fun playing this classic board game.",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new FeedBack
        {
            UserId = users[2].Id.ToString(),
            ToyId = toys[2].Id.ToString(),
            Content = "The wooden dollhouse is beautifully made, my kids love it.",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new FeedBack
        {
            UserId = users[3].Id.ToString(),
            ToyId = toys[3].Id.ToString(),
            Content = "Good quality soccer ball, perfect for outdoor play.",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new FeedBack
        {
            UserId = users[4].Id.ToString(),
            ToyId = toys[4].Id.ToString(),
            Content = "The building blocks set is great for my kids' creativity.",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        },
        new FeedBack
        {
            UserId = users[5].Id.ToString(),
            ToyId = toys[5].Id.ToString(),
            Content = "My son loves this remote control car, very fast!",
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow
        }
        };

        // Thêm các phản hồi vào context và lưu lại
        _context.Feedbacks.AddRange(feedbacks);
        _context.SaveChanges();
    }
    private void SeedTransactions()
    {
        if (_context.Transactions.Any()) return; // Check if transactions already exist

        var contracts = _context.ContractEntitys.ToList(); // Get all contracts

        if (!contracts.Any())
        {
            Console.WriteLine("No contracts available for seeding transactions.");
            return; // Ensure that there are contracts to reference
        }

        var transactions = new List<Transaction>
    {
        new Transaction
        {
            TranCode = 1001,
            DateCreated = DateTime.UtcNow,
            Status = "Completed",
            Method = true, // Online payment
            ContractId = contracts[0].Id.ToString(), // Reference the first contract
            ContractEntity = contracts[0] // Optional: direct navigation property
        },
        new Transaction
        {
            TranCode = 1002,
            DateCreated = DateTime.UtcNow.AddDays(-3),
            Status = "Pending",
            Method = false, // Offline payment
            ContractId = contracts[1].Id.ToString(), // Reference the second contract
            ContractEntity = contracts[1] // Optional: direct navigation property
        },
        new Transaction
        {
            TranCode = 1003,
            DateCreated = DateTime.UtcNow.AddDays(-1),
            Status = "Failed",
            Method = true, // Online payment
            ContractId = contracts[2].Id.ToString(), // Reference the third contract
            ContractEntity = contracts[2] // Optional: direct navigation property
        },
        new Transaction
        {
            TranCode = 1004,
            DateCreated = DateTime.UtcNow.AddDays(-5),
            Status = "Completed",
            Method = false, // Offline payment
            ContractId = contracts[3].Id.ToString(), // Reference the fourth contract
            ContractEntity = contracts[3] // Optional: direct navigation property
        }
    };

        _context.Transactions.AddRange(transactions);
        _context.SaveChanges();
        Console.WriteLine($"{transactions.Count} transactions seeded successfully.");
    }
}
