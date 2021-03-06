﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Enums
{
    public enum EnumGeneralSetting
    {
        Undef = 0,
        ProductNameMinLength = 1,
        ProductNameMaxLength = 2,
        ProductMinPrice = 3,
        ProductMaxPrice = 4,
        ProductPartNumberMinLength = 5,
        ProductPartNumberMaxLength = 6,
        ProductDescriptionMinLength = 7,
        ProductDescriptionMaxLength = 8,

        CoinImpPrivateKey = 9,
        CoinImpPublicKey = 10,
        CoinImpApiBaseUrl = 11,
        CoinImpApiCallTresholdSec = 12, 
        CoinImpXMRPayoutPer1MHashes = 13,

        MiningRoomNameMinLength = 14,
        MiningRoomNameMaxLength = 15,

        LoadXMRPriceIntervalMs = 16,

        CoinImpClientIdMinLength = 17,
        CoinImpClientIdMaxLength = 18,

        ScriptDonaterConnCntValue = 19,
        ScriptDonaterClientId = 20,

        JSECoinPrivateKey = 21,
        JSECoinPublicKey = 22,
        JSECoinApiUrl = 23,
        JSEAPITresholdSec = 24,

        JSECoinClientIdMinLength = 25,
        JSECoinClientIdMaxLength = 26,

    }
}