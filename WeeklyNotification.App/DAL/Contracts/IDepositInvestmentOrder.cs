﻿using System;
using WeeklyNotification.App.DAL.Entities;

namespace WeeklyNotification.App.DAL.Contracts
{
    public interface IDepositInvestmentOrder
    {
        int Id { get; set; }
        decimal Amount { get; set; }
        bool IsDeposit { get; set; }
        string Status { get; set; }
        DateTime CreatedUtc { get; set; }
        int CustomerId { get; set; }
        decimal InterestRate { get; set; }
        Customer Customer { get; set; }
        int DepositProductId { get; set; }
        DepositProduct DepositProduct { get; set; }
    }
}