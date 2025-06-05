namespace DocumentManagement.Rest.Api.Enums
{
    public enum DocumentStatusEnum : byte
    {
        KreiranDokument = 1,
        URadu = 2,
        PoslatNaOdobrenje = 3,
        VracenNaIspravku = 4,
        Odbijen = 5,
        Potpisan = 6,
        Storniran = 9,
        Arhiviran = 10
    }
}