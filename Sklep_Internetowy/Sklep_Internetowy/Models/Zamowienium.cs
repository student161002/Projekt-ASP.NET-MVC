using System;
using System.Collections.Generic;

namespace Sklep_Internetowy.Models;

public partial class Zamowienium
{
    public int ZamowienieId { get; set; }

    public DateTime? DataZamowienia { get; set; }

    public string AdresWysylki { get; set; } = null!;

    public string ImieNazwiskoKlienta { get; set; } = null!;

    public decimal? WartoscCalkowita { get; set; }

    public virtual ICollection<PozycjeZamowienium> PozycjeZamowienia { get; set; } = new List<PozycjeZamowienium>();
}
