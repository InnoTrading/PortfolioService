using PortfolioService.Application.DTOs;
using PortfolioService.Application.Interfaces;
using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioService.Application.Services
{
    public class AccountApplicationService(IAccountManager accountManager) : IAccountService
    {
        private readonly IAccountManager _accountManager = accountManager;


        public async Task<IsSuccessResultDto> CreateAccount(Guid userID)
        {
            var result = await _accountManager.CreateAccount(userID);
            return new IsSuccessResultDto(result);
        }
        public async Task<IsSuccessResultDto> DeleteAccount(Guid userID)
        {
            var result = await _accountManager.DeleteAccount(userID);
            return new IsSuccessResultDto(result);
        }
        public async Task<IsSuccessResultDto> Deposit(Guid userId, decimal amount)
        {
            var success = await _accountManager.Deposit(userId, amount);
            return new IsSuccessResultDto(success);
        }

        public async Task<IsSuccessResultDto> Withdraw(Guid userId, decimal amount)
        {
            var success = await _accountManager.Withdraw(userId, amount);
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

        public async Task<IEnumerable<StockAggregateDTO>> GetStocks(Guid userId)
        {
            var userStocks = await _accountManager.GetStocks(userId);

            var stockSummaries = userStocks.Select(s => 
                new StockAggregateDTO(s.Stock, s.Quantity)
            );

            return stockSummaries;
        }
    }
}
