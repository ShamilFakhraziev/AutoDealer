var Auto = Auto || {};



Auto.adlr_creditprogram = (function() {

    return {
    onSave : function(context){

        let formContext = context.getFormContext();
        
        let startDate = formContext.getAttribute("adlr_datestart").getValue();
        let endDate = formContext.getAttribute("adlr_dateend").getValue();

        let daysLag = Math.ceil(Math.abs(endDate.getTime() - startDate.getTime()) / (1000 * 3600 * 24));
        
        if(daysLag < 365){
            alert("Дата окончания должна быть больше даты начала, не менее, чем на год");
            context.getEventArgs().preventDefault();
        }
    }
}
})();

