using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthReposotory : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthReposotory(DataContext context)
        {
            this._context = context;

        }
        public async Task<User> Login(string userName, string pass)
        {
            var user  = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                return null;
            }
            if (!verifyPassHash(pass, user.PassHash,user.PassSalt)) 
                return null;
            //return (user == null) ? null : user;
            return user;
        }

        private bool verifyPassHash(string pass, byte[] passHash, byte[] passSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passSalt))
            {
                var compHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
               for (int i = 0; i < compHash.Length; i++)
               {
                   if(compHash[i] != passHash[i]) return false;
               }
               return true;
            }
        }

        public async Task<User> Register(User user, string pass)
        {
            byte[] passHash;
            byte[] passSalt;
            createPassHash(pass, out passHash, out passSalt);
            user.PassHash = passHash;
            user.PassSalt = passSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        private void createPassHash(string pass, out byte[] passHash, out byte[] passSalt)
        {
             // when we embrace in using we guarantee that object has gona be disposed, Hmac has idisposable interface 
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                passSalt = hmac.Key;
            }
        }

        public async Task<bool> userExist(string userName)
        {
           if(await _context.Users.AnyAsync(x => x.UserName == userName ))
                return true;

            return false;
        }
    }
}