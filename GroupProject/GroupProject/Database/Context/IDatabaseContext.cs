using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GroupProject.Database.ModelsGenerated;
using GroupProject.Database.ModelsExtensions;

namespace GroupProject.Database.Context
{
    public interface IDatabaseContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<EmployeeHouse> EmployeeHouses { get; set; }
        DbSet<EmployeeInformationBlock> EmployeeInformationBlocks { get; set; }
        DbSet<House> Houses { get; set; }
        DbSet<InformationBlock> InformationBlocks { get; set; }
        DbSet<ManagementCompany> ManagementCompanies { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Reply> Replies { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<Resident> Residents { get; set; }
        DbSet<ResidentTag> ResidentTags { get; set; }
        DbSet<Setting> Settings { get; set; }
        DbSet<SettingType> SettingTypes { get; set; }
        DbSet<Status> Statuses { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<TagInformationBlock> TagInformationBlocks { get; set; }
        int SaveChanges();

        IPerson GetUser(string phoneNumber);

        IPerson Login(string phoneNumber, string password);
    }
}
