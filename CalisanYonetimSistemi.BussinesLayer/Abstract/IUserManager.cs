using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.UserVm;

namespace CalisanYonetimSistemi.BussinesLayer.Abstract
{
    public interface IUserManager : IManager<User, int>
    {
        //Gerekli is kurallari var ise eklenecek.
        public Task<User> ChackUserLogin(User entity);
        public Task<bool> ChackUserRegister(User entity);

        public Task<bool> ChackConfirmPassword(string password, string confirmpassword);

        public Task<bool> ConfirmEmailAsync(string uid, string code);

        public Task<User> GetByEmailUser(string email);

        public Task<string> CreateEmailConfirmGuidCode(User user);

        public Task<bool> ChackUserEmailConfirm(User user);

        public Task<string> GetByEmailToken(string email);

        public Task<int> GetByEmailUserForCart(string email);

        public Task<bool> ChackSaveChangeNullorEmpty(string name, string surname, string password);

        public Task<int> UsersCount();

        public Task<bool> ChackUserUpdateNullorEmpty(params string[] strings);
        public Task<bool> ChackUserTokenExprationTimeValid(string tokenExpration);

        public Task<bool> CheckUserUyelikOnay(User user);

        public Task NullByPass(User user, UserUpdateVm userUpdateVm);

    }
}
