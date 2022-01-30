using System;
using System.Collections.Generic;

namespace LearActionPlans.Models
{
    public class Akce : DatabaseTable
    {
        public sealed override int Id { get; set; }
        public int BodAPId { get; set; }
        public string SkutecnaPricina { get; set; }
        public string NapravnaOpatreni { get; set; }
        public byte Typ { get; set; }
        public bool AkceUlozena { get; set; }
        //public bool ZnovuOtevrit { get; set; }
        public byte StavObjektuAkce { get; set; }

        public Akce(int id, int bodAPId, byte stavObjektu)
        {
            this.Id = id;
            this.BodAPId = bodAPId;
            this.StavObjektuAkce = stavObjektu;
        }

        //public Akce(int id, byte zamitnutiTerminu, byte zmenaTerminu)
        //{
        //    Id = id;
        //    ZamitnutiTerminu = zamitnutiTerminu;
        //    ZmenaTerminu = zmenaTerminu;
        //}

        //public Akce(int id,
        //    int bodAPId,
        //    string napravnaOpatreni,
        //    int odpovednaOsoba1Id,
        //    int? odpovednaOsoba2Id,
        //    List<UkonceniBodAP> ukonceniAkce,
        //    DateTime? kontrolaEfektivnosti,
        //    int? oddeleniId,
        //    string priloha,
        //    byte typ,
        //    byte stavObjektu,
        //    bool akceUlozena,
        //    bool znovuOtevrit)
        //{
        //    Id = id;
        //    BodAPId = bodAPId;
        //    NapravnaOpatreni = napravnaOpatreni;
        //    OdpovednaOsoba1Id = odpovednaOsoba1Id;
        //    OdpovednaOsoba2Id = odpovednaOsoba2Id;
        //    UkonceniAkce = ukonceniAkce;
        //    KontrolaEfektivnosti = kontrolaEfektivnosti;
        //    Oddeleni_Id = oddeleniId;
        //    Priloha = priloha;
        //    Typ = typ;
        //    StavObjektuAkce = stavObjektu;
        //    AkceUlozena = akceUlozena;
        //    ZnovuOtevrit = znovuOtevrit;
        //}

        //public Akce(int id,
        //    int bodAPId,
        //    string napravnaOpatreni,
        //    int odpovednaOsoba1Id,
        //    int? odpovednaOsoba2Id,
        //    List<UkonceniBodAP> ukonceniAkce,
        //    DateTime? kontrolaEfektivnosti,
        //    int? oddeleniId,
        //    string priloha,
        //    byte typ,
        //    byte stavObjektu)
        //{
        //    Id = id;
        //    BodAPId = bodAPId;
        //    NapravnaOpatreni = napravnaOpatreni;
        //    OdpovednaOsoba1Id = odpovednaOsoba1Id;
        //    OdpovednaOsoba2Id = odpovednaOsoba2Id;
        //    UkonceniAkce = ukonceniAkce;
        //    KontrolaEfektivnosti = kontrolaEfektivnosti;
        //    Oddeleni_Id = oddeleniId;
        //    Priloha = priloha;
        //    Typ = typ;
        //    StavObjektuAkce = stavObjektu;
        //}

        //public Akce(string napravnaOpatreni,
        //    int odpovednaOsoba1Id,
        //    int? odpovednaOsoba2Id,
        //    DateTime? kontrolaEfektivnosti,
        //    DateTime? kontrolaEfektivnostiPuvodniDatum,
        //    string kontrolaEfektivnostiOdstranit,
        //    int? oddeleniId,
        //    string priloha,
        //    byte typ,
        //    byte stavObjektu,
        //    bool akceUlozena,
        //    bool znovuOtevrit)
        //{
        //    NapravnaOpatreni = napravnaOpatreni;
        //    OdpovednaOsoba1Id = odpovednaOsoba1Id;
        //    OdpovednaOsoba2Id = odpovednaOsoba2Id;
        //    UkonceniAkce = new List<UkonceniBodAP>();
        //    KontrolaEfektivnosti = kontrolaEfektivnosti;
        //    KontrolaEfektivnostiPuvodniDatum = kontrolaEfektivnostiPuvodniDatum;
        //    KontrolaEfektivnostiOdstranit = kontrolaEfektivnostiOdstranit;
        //    Oddeleni_Id = oddeleniId;
        //    Priloha = priloha;
        //    Typ = typ;
        //    StavObjektuAkce = stavObjektu;
        //    AkceUlozena = akceUlozena;
        //    ZnovuOtevrit = znovuOtevrit;
        //}
    }
}
