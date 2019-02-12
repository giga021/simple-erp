using Knjizenje.Domain.Events;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knjizenje.Domain.Entities.FinNalogAggregate
{
	public class FinNalog : AggregateRoot<FinNalog, FinNalogId>
	{
		public virtual TipNaloga Tip { get; protected set; }
		public virtual DateTime DatumNaloga { get; protected set; }
		public virtual string Opis { get; protected set; }
		public virtual bool Zakljucan { get; protected set; }
		public virtual bool Obrisan { get; protected set; }
		public virtual IReadOnlyCollection<FinStavka> Stavke => _stavke.AsReadOnly();
		protected virtual List<FinStavka> _stavke { get; }

		protected FinNalog()
		{
			_stavke = new List<FinStavka>();
			Obrisan = false;
		}

		public FinNalog(TipNaloga tip, DateTime datumNaloga, string opis, IEnumerable<FinStavka> stavke) : this()
		{
			if (tip == null)
				throw new ArgumentNullException(nameof(tip));
			if (stavke == null)
				throw new ArgumentNullException(nameof(stavke));
			if (!stavke.Any())
				throw new KnjizenjeException("Nalog mora imati bar jednu stavku");

			Id = new FinNalogId();
			Obavesti(new NalogOtvoren(Id, datumNaloga, tip.Id, opis));

			foreach (var stavka in stavke)
			{
				ProknjiziStavku(stavka);
			}
		}

		public virtual void ProknjiziStavku(FinStavka stavka)
		{
			if (stavka == null)
				throw new ArgumentNullException(nameof(stavka));
			if (stavka.Id == Guid.Empty)
				throw new KnjizenjeException("Id stavke nije validan");
			if (_stavke.Any(x => x.Id == stavka.Id))
				throw new KnjizenjeException("Stavka je već proknjižena");
			ProveriZakljucan();

			DateTime datumKnjizenja = DateTime.Today;
			Obavesti(new StavkaProknjizena(Id, stavka.Id, datumKnjizenja, stavka.IdKonto,
				stavka.Iznos.Duguje, stavka.Iznos.Potrazuje, stavka.Opis));
		}

		public virtual void StornirajStavku(FinStavka originalStavka)
		{
			if (originalStavka == null)
				throw new ArgumentNullException(nameof(originalStavka));
			ProveriStavkaPripadaNalogu(originalStavka);

			var stornoStavka = new FinStavka(originalStavka.IdKonto, -originalStavka.Iznos.Duguje, -originalStavka.Iznos.Potrazuje, null);
			ProknjiziStavku(stornoStavka);
		}

		public virtual void UkloniStavku(FinStavka stavka)
		{
			if (stavka == null)
				throw new ArgumentNullException(nameof(stavka));
			ProveriZakljucan();
			if (!Obrisan && _stavke.Count <= 1)
				throw new KnjizenjeException("Nalog mora imati bar jednu stavku");
			ProveriStavkaPripadaNalogu(stavka);

			Obavesti(new StavkaUklonjena(Id, stavka.Id, stavka.DatumKnjizenja, stavka.IdKonto,
				stavka.Iznos.Duguje, stavka.Iznos.Potrazuje, stavka.Opis));
		}

		public virtual void Zakljucaj()
		{
			if (!Zakljucan)
				Obavesti(new NalogZakljucan(Id));
		}

		public virtual void Otkljucaj()
		{
			if (Zakljucan)
				Obavesti(new NalogOtkljucan(Id));
		}

		public virtual void IzmeniZaglavlje(TipNaloga tip, DateTime datumNaloga, string opis)
		{
			if (tip == null)
				throw new ArgumentNullException(nameof(tip));
			ProveriZakljucan();

			if (this.Tip == tip &&
				this.DatumNaloga == datumNaloga &&
				this.Opis == opis)
				return;

			Obavesti(new IzmenjenoZaglavljeNaloga(Id, datumNaloga, tip.Id, opis));
		}

		public virtual void Obrisi()
		{
			if (!Obrisan)
			{
				ProveriZakljucan();
				Obavesti(new NalogObrisan(Id));
			}
		}

		private void ProveriZakljucan()
		{
			if (Zakljucan)
				throw new KnjizenjeException("Nalog je zaključan");
		}

		private void ProveriStavkaPripadaNalogu(FinStavka stavka)
		{
			if (!_stavke.Contains(stavka))
				throw new KnjizenjeException("Stavka ne pripada nalogu");
		}

		internal void Primeni(NalogOtvoren evnt)
		{
			Id = evnt.IdNaloga;
			DatumNaloga = evnt.DatumNaloga;
			Tip = TipNaloga.Get(evnt.IdTip);
			Opis = evnt.Opis;
		}

		internal void Primeni(StavkaProknjizena evnt)
		{
			var stavka = new FinStavka(evnt.IdStavke, evnt.IdKonto, evnt.Duguje, evnt.Potrazuje, evnt.Opis, evnt.DatumKnjizenja);
			_stavke.Add(stavka);
		}

		internal void Primeni(StavkaUklonjena evnt)
		{
			var stavka = _stavke.Where(x => x.Id == evnt.IdStavke).Single();
			_stavke.Remove(stavka);
		}

		internal void Primeni(NalogZakljucan evnt)
		{
			Zakljucan = true;
		}

		internal void Primeni(NalogOtkljucan evnt)
		{
			Zakljucan = false;
		}

		internal void Primeni(NalogObrisan evnt)
		{
			Obrisan = true;
		}

		internal void Primeni(IzmenjenoZaglavljeNaloga evnt)
		{
			DatumNaloga = evnt.DatumNaloga;
			Opis = evnt.Opis;
			Tip = TipNaloga.Get(evnt.IdTip);
		}
	}
}
