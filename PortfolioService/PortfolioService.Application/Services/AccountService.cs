using PortfolioService.Application.DTOs;
using PortfolioService.Application.Interfaces;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Application.Services
{
    public class AccountService(IAccountManager accountManager) : IAccountService
    {
        private readonly IAccountManager _accountManager = accountManager;


        public async Task<IsSuccessResultDto> CreateAccount(Guid userId)
        {
            var result = await _accountManager.CreateAccount(userId);
            return new IsSuccessResultDto(result);
        }
        public async Task<IsSuccessResultDto> DeleteAccount(Guid userId)
        {
            var result = await _accountManager.DeleteAccount(userId);
            return new IsSuccessResultDto(result);
        }
        public async Task<IsSuccessResultDto> Deposit(Guid userId, decimal amount)
        {
            var success = await _accountManager.Deposit(userId, amount);
            return new IsSuccessResultDto(success);
        }

        public async Task<IsSuccessResultDto> Withdrawal(Guid userId, decimal amount)
        {
            var success = await _accountManager.Withdrawal(userId, amount);
            return new IsSuccessResultDto(success);
        }

        public async Task<BalanceDto> GetBalance(Guid userId)
        {
            var balance = await _accountManager.Balance(userId);
            return new BalanceDto(balance);
        }

        public async Task<ReservedBalanceDto> GetReservedBalance(Guid userId)
        {
            var reservedBalance = await _accountManager.ReservedBalance(userId);
            return new ReservedBalanceDto(reservedBalance);
        }

        public async Task<UserInfoDto> GetUserInfo(Guid userId)
        {
            var userEntity = await _accountManager.GetUserInfo(userId);

            return new UserInfoDto(userEntity.ID, userEntity.UserName, userEntity.UserEmail);
        }
    }
}
