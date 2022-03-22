var Auto = Auto || {};



Auto.adlr_auto = (function() {

    var autoUsedOnChange = function(context){
        let formContext = context.getFormContext();
        let usedAtr = formContext.getAttribute("adlr_used");

        if(usedAtr.getValue()==true){
            formContext.getControl("adlr_ownerscount").setVisible(true);
            formContext.getControl("adlr_isdamaged").setVisible(true);
            formContext.getControl("adlr_km").setVisible(true);
        }
        else {
            formContext.getControl("adlr_ownerscount").setVisible(false);
            formContext.getControl("adlr_isdamaged").setVisible(false);
            formContext.getControl("adlr_km").setVisible(false);
            
            formContext.getAttribute("adlr_ownerscount").setValue(null);
            formContext.getAttribute("adlr_isdamaged").setValue(null);
            formContext.getAttribute("adlr_km").setValue(null);
        }
    }

    return {
    onLoad : function(context){

        let formContext = context.getFormContext();
        
        let usedAtr = formContext.getAttribute("adlr_used");
        if(usedAtr.getValue()==false){
            formContext.getControl("adlr_ownerscount").setVisible(false);
            formContext.getControl("adlr_isdamaged").setVisible(false);
            formContext.getControl("adlr_km").setVisible(false);
        }
        usedAtr.addOnChange(autoUsedOnChange);
    }
}
})();

