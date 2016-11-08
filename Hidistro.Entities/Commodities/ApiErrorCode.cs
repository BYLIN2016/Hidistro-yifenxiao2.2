namespace Hidistro.Entities.Commodities
{
    using System;

    public enum ApiErrorCode
    {
        Ban_Register = 0xcc,
        Empty_Error = 0x68,
        Exists_Error = 0x6a,
        Format_Eroor = 0x66,
        Group_Error = 0x6c,
        NoExists_Error = 0x69,
        NoPay_Error = 0x6d,
        NoShippingMode = 110,
        Paramter_Diffrent = 0x6b,
        Paramter_Error = 0x65,
        SaleState_Error = 300,
        Session_Empty = 200,
        Session_Error = 0xc9,
        Session_TimeOut = 0xca,
        ShipingOrderNumber_Error = 0x6f,
        Signature_Error = 0x67,
        Stock_Error = 0x12d,
        Success = 100,
        Unknown_Error = 0x3e7,
        Username_Exist = 0xcb
    }
}

