using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LinkedNewsChatApp.Hubs;

namespace DataLayer.Context
{
    public class LinkedNewsDbContext: DbContext
    {
        private readonly IConfiguration _config;

        public LinkedNewsDbContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<GroupMember> GroupMembers => Set<GroupMember>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<HubUser> hubUsers => Set<HubUser>();
        public DbSet<HubMessage> HubMessages => Set<HubMessage>();
        public DbSet<PrivateChat> privateChats => Set<PrivateChat>();
        public DbSet<HubGroupMessage> hubGroupMessages => Set<HubGroupMessage>();
        public DbSet<HubGroup> hubGroups => Set<HubGroup>();
        public DbSet<UserFriends> Friends => Set<UserFriends>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config.GetConnectionString("LinkedNewsDb"));
        }
    }
}
