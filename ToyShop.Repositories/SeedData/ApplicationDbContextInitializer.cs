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
        SeedFeedBacks();
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
        if (_context.ContractEntitys.Any())
        {
            Console.WriteLine("Contracts already seeded.");
            return; // Check if contracts already exist
        }

        var users = _context.ApplicationUsers.ToList();
        var toys = _context.Toys.ToList();

        if (!users.Any() || !toys.Any())
        {
            Console.WriteLine("No users or toys available for seeding contracts.");
            return; // Ensure that there are users and toys to create contracts
        }

        var contracts = new List<ContractEntity>
    {
        new ContractEntity
        {
            UserId = users[0].Id,
            ToyId = toys[0].Id,
            RestoreToyId = null,
            StaffConfirmed = "Staff A",
            TotalValue = 150.00,
            NumberOfRentals = 1,
            DateCreated = DateOnly.FromDateTime(DateTime.UtcNow),
            ContractType = true,
            DateStart = DateOnly.FromDateTime(DateTime.UtcNow),
            DateEnd = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
            Status = "Active"
        },
        // Additional contracts...
    };

        _context.ContractEntitys.AddRange(contracts);
        _context.SaveChanges();
        Console.WriteLine($"{contracts.Count} contracts seeded successfully.");
    }

    private void SeedRestoreToys()
    {
        if (_context.RestoreToys.Any())
        {
            Console.WriteLine("Restore toys already seeded.");
            return; // Check if restore toys already exist
        }

        var contracts = _context.ContractEntitys.ToList();

        if (!contracts.Any())
        {
            Console.WriteLine("No contracts available for seeding restore toys.");
            return; // Ensure that there are contracts to reference
        }

        var restoreToys = new RestoreToy[]
        {
        new RestoreToy
        {
            CreatedTime = DateTimeOffset.UtcNow,
            LastUpdatedTime = DateTimeOffset.UtcNow,
            ContractId = contracts[0]?.Id,
            ToyQuality = 90.5,
            Reward = 50,
            OverdueTime = 2.5,
            TotalMoney = 150.00,
            ContractEntity = contracts[0]
        },
            // Additional restore toys...
        };

        _context.RestoreToys.AddRange(restoreToys);
        _context.SaveChanges();
        Console.WriteLine($"{restoreToys.Length} restore toys seeded successfully.");
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
}
