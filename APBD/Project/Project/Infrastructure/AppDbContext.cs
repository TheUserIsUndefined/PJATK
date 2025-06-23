using Microsoft.EntityFrameworkCore;
using Project.Core.Models;

namespace Project.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareCategory> SoftwareCategories { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<DiscountType> DiscountTypes { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Contract>()
            .Property(e => e.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<Payment>()
            .Property(e => e.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<SoftwareCategory>().HasData(
                new SoftwareCategory { CategoryId = 1, Name = "Business Management" },
                new SoftwareCategory { CategoryId = 2, Name = "Accounting" },
                new SoftwareCategory { CategoryId = 3, Name = "CRM" },
                new SoftwareCategory { CategoryId = 4, Name = "Project Management" },
                new SoftwareCategory { CategoryId = 5, Name = "Security" }
            );

            modelBuilder.Entity<Software>().HasData(
                new Software 
                { 
                    SoftwareId = 1, 
                    Name = "BusinessPro", 
                    Description = "Complete business management solution",
                    CurrentVersion = "3.2.1",
                    UpfrontCostPerYear = 1200.00m,
                    SubscriptionCost = 150.00m,
                    CategoryId = 1 // Will be set via foreign key
                },
                new Software 
                { 
                    SoftwareId = 2, 
                    Name = "AccounTech", 
                    Description = "Advanced accounting software",
                    CurrentVersion = "2.1.5",
                    UpfrontCostPerYear = null,
                    SubscriptionCost = 100.00m,
                    CategoryId = 2
                },
                new Software 
                { 
                    SoftwareId = 3, 
                    Name = "ClientConnect", 
                    Description = "Customer relationship management system",
                    CurrentVersion = "4.0.2",
                    UpfrontCostPerYear = 2000.00m,
                    SubscriptionCost = 250.00m,
                    CategoryId = 3
                },
                new Software 
                { 
                    SoftwareId = 4, 
                    Name = "TaskMaster", 
                    Description = "Project and task management platform",
                    CurrentVersion = "1.8.3",
                    UpfrontCostPerYear = 600.00m,
                    SubscriptionCost = 75.00m,
                    CategoryId = 4
                },
                new Software 
                { 
                    SoftwareId = 5, 
                    Name = "SecureGuard", 
                    Description = "Enterprise security management suite",
                    CurrentVersion = "5.1.0",
                    UpfrontCostPerYear = 3000.00m,
                    SubscriptionCost = 400.00m,
                    CategoryId = 5
                }
            );

            modelBuilder.Entity<DiscountType>().HasData(
                new DiscountType { TypeId = 1, Name = "Upfront" },
                new DiscountType { TypeId = 2, Name = "Subscription" }
            );

            modelBuilder.Entity<Discount>().HasData(
                new Discount 
                { 
                    DiscountId = 1,
                    Name = "New Year 2025",
                    Description = "Special discount for new year contracts",
                    PercentageValue = 15.00m,
                    StartDate = new DateOnly(2025, 1, 1),
                    EndDate = new DateOnly(2025, 3, 31),
                    DiscountTypeId = 1
                },
                new Discount 
                { 
                    DiscountId = 2,
                    Name = "Enterprise Package",
                    Description = "Discount for large volume purchases",
                    PercentageValue = 25.00m,
                    StartDate = new DateOnly(2025, 1, 1),
                    EndDate = new DateOnly(2025, 12, 31),
                    DiscountTypeId = 1 
                },
                new Discount 
                { 
                    DiscountId = 3,
                    Name = "Returning Customer",
                    Description = "Loyalty discount for existing clients",
                    PercentageValue = 10.00m,
                    StartDate = new DateOnly(2025, 1, 1),
                    EndDate = new DateOnly(2025, 12, 31),
                    DiscountTypeId = 1
                },
                new Discount 
                { 
                    DiscountId = 4,
                    Name = "Summer Sale",
                    Description = "Summer promotional discount",
                    PercentageValue = 20.00m,
                    StartDate = new DateOnly(2025, 6, 1),
                    EndDate = new DateOnly(2025, 8, 31),
                    DiscountTypeId = 1
                },
                new Discount 
                { 
                    DiscountId = 5,
                    Name = "Student Special",
                    Description = "Educational institution discount",
                    PercentageValue = 30.00m,
                    StartDate = new DateOnly(2025, 1, 1),
                    EndDate = new DateOnly(2025, 12, 31),
                    DiscountTypeId = 2
                }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client 
                { 
                    ClientId = 1,
                    Address = "Ul. Krakowska 15, 00-001 Warszawa",
                    Email = "jan.kowalski@email.com",
                    PhoneNumber = "123456789"
                },
                new Client 
                { 
                    ClientId = 2,
                    Address = "Ul. Marszałkowska 100, 00-026 Warszawa",
                    Email = "kontakt@techcorp.pl",
                    PhoneNumber = "987654321"
                },
                new Client 
                { 
                    ClientId = 3,
                    Address = "Ul. Nowy Świat 25, 00-029 Warszawa",
                    Email = "anna.nowak@gmail.com",
                    PhoneNumber = "555123456"
                },
                new Client 
                { 
                    ClientId = 4,
                    Address = "Ul. Piękna 50, 00-672 Warszawa",
                    Email = "info@innovate.com.pl",
                    PhoneNumber = "111222333"
                },
                new Client 
                { 
                    ClientId = 5,
                    Address = "Ul. Mokotowska 12, 00-640 Warszawa",
                    Email = "piotr.wisniewski@outlook.com",
                    PhoneNumber = "444555666"
                }
            );

            modelBuilder.Entity<Individual>().HasData(
                new Individual 
                { 
                    IndividualId = 1,
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    Pesel = "85010112345",
                    IsDeleted = false,
                    ClientId = 1
                },
                new Individual 
                { 
                    IndividualId = 2,
                    FirstName = "Anna",
                    LastName = "Nowak",
                    Pesel = "90052298765",
                    IsDeleted = false,
                    ClientId = 3
                },
                new Individual 
                { 
                    IndividualId = 3,
                    FirstName = "Piotr",
                    LastName = "Wiśniewski",
                    Pesel = "78121545678",
                    IsDeleted = false,
                    ClientId = 5
                }
            );

            modelBuilder.Entity<Company>().HasData(
                new Company 
                { 
                    CompanyId = 1,
                    Name = "TechCorp Sp. z o.o.",
                    Krs = "0000123456",
                    ClientId = 2
                },
                new Company 
                { 
                    CompanyId = 2,
                    Name = "Innovate Solutions",
                    Krs = "0000987654",
                    ClientId = 4
                }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Admin" },
                new Role { RoleId = 2, Name = "Employee" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "Admin",
                    Password = "PfQBP3YFpHxxyvwYa/AwDMxu4+EC2XFsnKiHxQnbpBo=", // Password: Password
                    Salt = "tGDsTQxWHWfTjOiLteIvAA==",
                    RefreshToken = "Ioq8YsX4f/NgOjKIcN78k8bqnzTVUWnXPbrsYKQG9yA=",
                    RefreshTokenExp = DateTime.Parse("2028-03-19 23:29:05.9465736")
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserRoleId = 1, UserId = 1, RoleId = 1 },
                new UserRole { UserRoleId = 2, UserId = 1, RoleId = 2 }
            );
    }
}