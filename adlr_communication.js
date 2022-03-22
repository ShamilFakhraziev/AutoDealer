var Auto = Auto || {};



Auto.adlr_communication = (function() {

    var communicationTypeOnChange = function(context){
        let formContext = context.getFormContext();

        let typeAtr = formContext.getAttribute("adlr_type");
        let emailCtrl = formContext.getControl("adlr_email");
        let phoneCtrl = formContext.getControl("adlr_phone");
        
        if(typeAtr.getValue() == 222950001){
            phoneCtrl.setVisible(true);
            emailCtrl.setVisible(false);
        }
        else if(typeAtr.getValue() == 222950002){
            emailCtrl.setVisible(true);
            phoneCtrl.setVisible(false);
        }
        else {
            emailCtrl.setVisible(false); 
            phoneCtrl.setVisible(false); 
        }
    }
    return {
    onLoad : function(context){

        let formContext = context.getFormContext();
            
        let emailCtrl = formContext.getControl("adlr_email");
        let phoneCtrl = formContext.getControl("adlr_phone");

        if(emailCtrl.getValue() == null){
            emailCtrl.setVisible(false);
        }

        if(phoneCtrl.getValue() == null){
            phoneCtrl.setVisible(false);
        }

        formContext.getAttribute("adlr_type").addOnChange(communicationTypeOnChange);
       
    }
}
})();

