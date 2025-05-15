using PortfolioService.Application.DTOs;
using PortfolioService.Application.Interfaces;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Application.Services
{
    public class AccountService(IAccountManager accountManager) : IAccountService
    {
        private readonly IAccountManager _accountManager = accountManager;


        public async Task<IsSuccessResultDto> CreateAccount(RegisterDto registerDto)
        {
            var result = await _accountManager.CreateAccount(registerDto.auth0Id);
            return new IsSuccessResultDto(result);
        }

        public async Task<IsSuccessResultDto> DeleteAccount(string userId)
        {
            var result = await _accountManager.DeleteAccount(userId);
            return new IsSuccessResultDto(result);
        }

        public async Task<IsSuccessResultDto> Deposit(string userId, decimal amount)
        {
            var success = await _accountManager.Deposit(userId, amount);
            return new IsSuccessResultDto(success);
        }

        public async Task<IsSuccessResultDto> Withdrawal(string userId, decimal amount)
        {
            var success = await _accountManager.Withdrawal(userId, amount);
            return new IsSuccessResultDto(success);
        }

        public async Task<BalanceDto> GetBalance(string userId)
        {
            var balance = await _accountManager.Balance(userId);
            return new BalanceDto(balance);
        }

        public async Task<ReservedBalanceDto> GetReservedBalance(string userId)
        {
            var reservedBalance = await _accountManager.ReservedBalance(userId);
            return new ReservedBalanceDto(reservedBalance);
        }
    }
}