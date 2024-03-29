﻿using HashidsNet;
using NLog;
using PaymentGateway.Api.Business.Interfaces;
using PaymentGateway.Api.Infrastructure.DAL.Repositories;
using PaymentGateway.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentGateway.Modules.Merchant
{
    public class MerchantAuthValidationModule : IModuleBase
    {
        public string MerchantUserId { get; set; }
        public string HashId { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public IResponseBase Process()
        {
            var response = new Response();

            try
            {
                if (Guid.TryParse(MerchantUserId, out Guid result))
                {
                    var merchant = new MerchantRepository().GetById(Guid.Parse(MerchantUserId));
                    if (merchant != null && HasPasswordMatched(merchant))
                    {
                        response.IsSuccessful = true;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[MerchantAuthValidationModule][Process][FAIL] : {ex.Message}");
            }

            return response;
        }

        private bool HasPasswordMatched(Core.Models.Merchant merchant)
        {
            var hashIDs = new Hashids(merchant.AuthSalt, 10);
            var hash = hashIDs.Encode(merchant.AuthIdentifier);

            if (hash.Equals(HashId))
            {
                return true;
            }

            return false;
        }
    }
}