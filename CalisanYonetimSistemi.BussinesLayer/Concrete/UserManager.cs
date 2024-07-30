using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.EntityLayer.Concrete;
using CalisanYonetimSistemi.EntityLayer.Concrete.VMs.YoneticiVms.UserVm;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CalisanYonetimSistemi.BussinesLayer.Concrete
{
    public class UserManager : Manager<User, int>, IUserManager
    {

        //IUserManagerde is kurallari olusturursan ilgili manager metodunu override edip configure edebilirsin.
        public async Task<User> ChackUserLogin(User entity)
        {
            User user = await FirstOrDefault(p => p.Email == entity.Email && p.Password == entity.Password);
            if (user == null)
            {
                throw new Exception("Kullanıcı adı veya şifre hatalı");
            }
            else
            {
                return user;
            }

        }
        public async Task<bool> ChackUserRegister(User entity)
        {
            User user = await FirstOrDefault(p => p.Email == entity.Email);
            if (user == null)
            {
                return true;
            }
            else
            {
                throw new Exception("Kullanıcı zaten mevcut. !");
            }

        }

        public async Task<bool> ChackConfirmPassword(string password, string confirmpassword)
        {
            if (password == confirmpassword)
            {
                return true;
            }
            else
            {
                throw new Exception("Girilen parolalar eşleşmiyor. !");
            }

        }


        public async Task<User> GetByEmailUser(string email)
        {
            User user = await FirstOrDefault(p => p.Email == email);
            if (user == null)
            {
                throw new Exception("Kayıt işlemi sırasında beklenmedik bir hata meydana geldi, lütfen tekrar kayıt olunuz.");
            }
            return user;
        }


        public async Task<bool> ConfirmEmailAsync(string uid, string code)
        {
            User user = await GetByPK(byte.Parse(uid));

            if (user == null || user.ConfirmEmailGuid != code)
            {
                throw new Exception("Email confirm sırasında bir hata meydana geldi, lütfen tekrar deneyin");
            }

            user.isConfirmEmail = true;
            await update(user);

            return true;
        }

        public async Task<string> CreateEmailConfirmGuidCode(User user)
        {
            Guid guid = Guid.NewGuid();
            string code = guid.ToString();
            user.ConfirmEmailGuid = code;
            await update(user);
            return code;
        }

        public async Task<bool> ChackUserEmailConfirm(User user)
        {
            User myuser = await GetByPK(user.Id);
            if (myuser.isConfirmEmail)
            {
                return true;
            }
            else
            {
                throw new Exception("Lütfen email adresinizi, mail adresinize gönderdiğim linkten onaylayınız.");
            }

        }

        public async Task<string> GetByEmailToken(string email)
        {
            User user = await FirstOrDefault(p => p.Email == email);
            if (user == null)
            {
                return null;
            }
            return user.AccessToken;
        }

        public async Task<int> GetByEmailUserForCart(string email)
        {
            User user = await FirstOrDefault(p => p.Email == email);
            if (user == null)
            {
                throw new Exception("Beklenmedik bir durum meydana geldi. Hesabinizdan cikis yapiliyor lütfen tekrar giris yapiniz.");
            }
            return user.Id;
        }

        public async Task<bool> ChackSaveChangeNullorEmpty(string name, string surname, string password)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(password))
            {
                throw new Exception("Lütfen en az bir değişiklik giriniz");
            }
            return true;
        }
        public async Task<bool> ChackUserUpdateNullorEmpty(params string[] strings)
        {
            if (strings.All(string.IsNullOrEmpty))
            {
                throw new Exception("Lütfen en az bir değişiklik giriniz");
            }
            return true;

        }


        public async Task<bool> ChackUserTokenExprationTimeValid(string tokenExpration)
        {
            if (DateTime.TryParseExact(tokenExpration, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return true;
            }
            throw new Exception("Lütfen geçerli bir tarih ve saat formatında giriş yapınız (YYYY-MM-DD HH:MM:SS).");

        }

        public async Task<int> UsersCount()
        {
            return _repo.dbContext.Users.Count();
        }

        public async Task<bool> CheckUserUyelikOnay(User user)
        {
            User myuser = await GetByPK(user.Id);
            if (myuser.UyelikOnay)
            {
                return true;
            }
            else
            {
                throw new Exception("Üyeliğiniz yöneticiniz tarafından onaylanmadı !");
            }
        }

        public async Task NullByPass(User user, UserUpdateVm userUpdateVm)
        {
            if (string.IsNullOrEmpty(userUpdateVm.isConfirmEmail))
            {
                user.isConfirmEmail = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.isConfirmEmail).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.IsDelete))
            {
                user.IsDelete = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.IsDelete).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.UyelikOnay))
            {
                user.UyelikOnay = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.UyelikOnay).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.ToplamAnalitiklikPuan))
            {
                user.ToplamAnalitiklikPuan = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.ToplamAnalitiklikPuan).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.ToplamTakimCalismasiPuan))
            {
                user.ToplamTakimCalismasiPuan = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.ToplamTakimCalismasiPuan).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.ToplamUretkenlikPuan))
            {
                user.ToplamUretkenlikPuan = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.ToplamUretkenlikPuan).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.TatilIzinBakiye))
            {
                user.TatilIzinBakiye = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.TatilIzinBakiye).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.OzelIzinBakiye))
            {
                user.OzelIzinBakiye = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.OzelIzinBakiye).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.HastalikIzinBakiye))
            {
                user.HastalikIzinBakiye = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.HastalikIzinBakiye).FirstOrDefaultAsync();
            }
            if (string.IsNullOrEmpty(userUpdateVm.BaslamaTarihi))
            {
                user.BaslamaTarihi = await _repo.dbContext.Users.Where(p => p.Id == userUpdateVm.Id).Select(p => p.BaslamaTarihi).FirstOrDefaultAsync();
            }
        }
    }
}
