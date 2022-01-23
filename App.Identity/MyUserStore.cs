using App.Identity.Domain;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace App.Identity
{
    public class MyUserStore : IUserStore<Users>, IUserPasswordStore<Users>
    {
        public async Task<IdentityResult> CreateAsync(Users user, CancellationToken cancellationToken)
        {
            using (var conection = GetOpenConection())
            {
                await conection.ExecuteAsync("insert into Users " +
                    "("+
                    "Id, UserName, NormalizedUserName, PasswordHash) " +
                    "values(@id,@userName,@normalizedUserName,@passwordHash)",
                 new
                 {
                     id = user.Id,
                     userName = user.UserName,
                     normalizedUserName = user.NormalizedUserName,
                     passwordHash = user.PasswordHash
                 });

            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Users user, CancellationToken cancellationToken)
        {
            using (var conection = GetOpenConection())
            {
                await conection.ExecuteAsync("Delete from Users where Id = @id", 
                 new
                {
                    id = user.Id,
                    
                });

            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }
        public static DbConnection GetOpenConection()
        {
            var conection = new SqlConnection(
                "Server = localhost; Database = Curso_Identity; Trusted_Connection = True");

            conection.Open();
            return conection;
        }

        public async Task<Users> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var conection = GetOpenConection())
            {
                return await conection.QueryFirstOrDefaultAsync<Users>("select * from Users where Id = @id",
                    new {id = userId});
            }
        }

        public async Task<Users> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var conection = GetOpenConection())
            {
                return await conection.QueryFirstOrDefaultAsync<Users>("select * from Users where NormalizedUserName = @name",
                    new { name = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(Users user, string normalizedName, CancellationToken cancellationToken)
        {   // encapsulamento get
            user.NormalizedUserName = normalizedName; 
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(Users user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Users user, CancellationToken cancellationToken)
        {
            using (var conection = GetOpenConection())
            {
                await conection.ExecuteAsync("Update Users" +
                    "set UserName = @username" +
                    "NormalizedUserName = @normalizedUserName" +
                    "PasswordHash = @passwordHash" +
                    "Where Id = @id ",
                    new
                    { 
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    passwordHash = user.PasswordHash
                    });

            }

            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(Users user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
