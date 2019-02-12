options({ 
    resultStreamName: "nalozi"
})

fromCategory("FinNalog")
.when({ 
    $any: function(state, evnt){
        linkTo('nalozi', evnt);
    }
});