using DataLayer.Entities;

namespace DataLayer
{
    public interface ILinkedNewsRepository
    {
        public List<User> GetListOfUsers();
        public User UpdateUserPassword(User user);
        public void Register(User user);
        public List<Group> GetListOfGroups();
        public void AddGroup(Group group);
    }
}
