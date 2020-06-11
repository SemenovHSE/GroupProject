namespace GroupProject.Database.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using GroupProject.Database.ModelsExtensions;
    using GroupProject.Database.ModelsGenerated;

    public partial class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext()
            : base("name=DatabaseContext")
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeHouse> EmployeeHouses { get; set; }
        public virtual DbSet<EmployeeInformationBlock> EmployeeInformationBlocks { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<InformationBlock> InformationBlocks { get; set; }
        public virtual DbSet<ManagementCompany> ManagementCompanies { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Reply> Replies { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Resident> Residents { get; set; }
        public virtual DbSet<ResidentTag> ResidentTags { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SettingType> SettingTypes { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagInformationBlock> TagInformationBlocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resident>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.Resident)
                .WillCascadeOnDelete();
        }

        public IPerson GetUser(string phoneNumber)
        {
            IPerson resident = Residents.FirstOrDefault(r => r.PhoneNumber == phoneNumber);
            IPerson employee = Employees.FirstOrDefault(e => e.PhoneNumber == phoneNumber);
            IPerson user = resident ?? employee;
            return user;
        }


        public IPerson Login(string phoneNumber, string password)
        {
            IPerson resident = Residents.FirstOrDefault(r => r.PhoneNumber == phoneNumber && r.Password == password);
            IPerson employee = Employees.FirstOrDefault(e => e.PhoneNumber == phoneNumber && e.Password == password);
            IPerson user = resident ?? employee;
            return user;
        }
    }
}
