namespace DocumentManagement.Rest.Api.Enums
{
    public enum CaseStatusEnum : byte
    {
        Aktivan = 1,
        Zavrsen = 2,
        NaCekanju = 3,
        Storniran = 9,
        KreiranPredmet = 11,
        URadu = 12,
        Prosledjen = 13,
        VracenNaDoradu = 14,
        Odbijen = 15,
        Odobren = 16,
        StavljenNaCekanje = 17,
        DataSaglasnost = 21,
        OdbijenaSaglasnost = 22,
        TrazenjeSaglasnosti = 23,
        KreiranPredmetPoslatUgovor = 25,
        Otkljucan = 26
    }
}
