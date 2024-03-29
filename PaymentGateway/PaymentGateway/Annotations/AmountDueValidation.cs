﻿using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaymentGateway.Annotations
{
    public class AmountDueValidation : ValidationAttribute
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                if (IsAmountValid(value))
                {
                    return ValidationResult.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[AmountDueValidation][IsValid] FAILED: {ex.Message}");
            }

            return new ValidationResult(ErrorMessage);
        }

        private bool IsAmountValid(object value)
        {
            var amount = Convert.ToDecimal(value);

            return amount > 0;
        }
    }
}