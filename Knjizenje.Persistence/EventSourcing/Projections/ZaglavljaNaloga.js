fromCategory("FinNalog")
.when({ 
    $init: function(){
        return []
    },
    'NalogOtvoren': function(state, evnt){
        state.push({
            IdNaloga: evnt.data.IdNaloga,
            DatumNaloga: evnt.data.DatumNaloga,
            IdTip: evnt.data.IdTip,
            Opis: evnt.data.Opis
        })
    },
    'IzmenjenoZaglavljeNaloga': function(state, evnt){
        let nalog = state.find(x => x.IdNaloga === evnt.data.IdNaloga);
        nalog.DatumNaloga = evnt.data.DatumNaloga;
        nalog.IdTip = evnt.data.IdTip;
        nalog.Opis = evnt.data.Opis;
    },
    'NalogObrisan': function(state, evnt){
        let nalogIdx = state.findIndex(x => x.IdNaloga === evnt.data.IdNaloga);
        state.splice(nalogIdx, 1);
    }
})