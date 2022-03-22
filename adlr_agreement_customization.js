var Auto = Auto || {};


var contactOnChange = function(context){

    let formContext = context.getFormContext();
    let auto = formContext.getAttribute("adlr_autoid").getValue();
    let contact = formContext.getAttribute("adlr_contact").getValue();

    if(contact != null && auto != null) {
        formContext.getControl("adlr_creditid").setVisible(true);
    }
    else {
        formContext.getControl("adlr_creditid").setVisible(false); 
    }

}

var autoOnChange = function(context){

    let formContext = context.getFormContext();
    let auto = formContext.getAttribute("adlr_autoid").getValue();
    let contact = formContext.getAttribute("adlr_contact").getValue();

    if(contact != null && auto != null) {
        formContext.getControl("adlr_creditid").setVisible(true);
    }
    else {
        formContext.getControl("adlr_creditid").setVisible(false); 
    }

    if(auto!=null){
        let autoRef = auto[0];
        Xrm.WebApi.retrieveRecord("adlr_auto", autoRef.id, "?$select=adlr_used,adlr_amount").then(
            function (result) {
                let autoUsed = result.adlr_used;
                let autoSum = result.adlr_amount;

                if(autoUsed==true){
                    formContext.getAttribute("adlr_summa").setValue(autoSum);
                }
            },
            function (error) {
                console.log(error.message);
            }
        );
        
    }
}

var creditProgramOnChange = function(context){

    let formContext = context.getFormContext();
    let creditProgram = formContext.getAttribute("adlr_creditid").getValue();

    if(creditProgram != null){
        formContext.ui.tabs.get("tab_creditagreement").setVisible(true);
        formContext.getControl("adlr_summa").setVisible(true);
        formContext.getControl("adlr_fact").setVisible(true);

        let creditProgramRef = creditProgram[0];
        Xrm.WebApi.retrieveRecord("adlr_credit", creditProgramRef.id, "?$select=adlr_creditperiod").then(
            function (result) {
                let creditPeriod = result.adlr_creditperiod / 365;
                formContext.getAttribute("adlr_creditperiod").setValue(creditPeriod);
            },
            function (error) {
                console.log(error.message);
            }
        );
    }
    else {
        formContext.ui.tabs.get("tab_creditagreement").setVisible(false);
        formContext.getControl("adlr_summa").setVisible(false);
        formContext.getControl("adlr_fact").setVisible(false);
        formContext.getAttribute("adlr_fact").setValue(null);
        formContext.getAttribute("adlr_summa").setValue(null);
    }
}

var nameAgreementChange = function(context){

    let formContext = context.getFormContext();
    let nameAgrmntValue = formContext.getAttribute("adlr_name").getValue();
    if(nameAgrmntValue != null){

        let replacedName = nameAgrmntValue.replace(/[^0-9,-]/g,"");
        formContext.getAttribute("adlr_name").setValue(replacedName);
        
    }
}

Auto.adlr_agreement_customization = (function() {

    return {
        onLoad : function(context){

            let formContext = context.getFormContext();

            let creditIdAtr = formContext.getAttribute("adlr_creditid");
            let contactAtr = formContext.getAttribute("adlr_contact");
            let autoIdAtr = formContext.getAttribute("adlr_autoid");
            
            contactAtr.addOnChange(contactOnChange);
            autoIdAtr.addOnChange(autoOnChange);
            creditIdAtr.addOnChange(creditProgramOnChange);
            formContext.getAttribute("adlr_name").addOnChange(nameAgreementChange);
    
            
            
            if(contactAtr.getValue()==null && autoIdAtr.getValue()==null){
                formContext.getControl("adlr_creditid").setVisible(false);
            }
    
            formContext.getControl("ownerid").setVisible(false);
            
            if(creditIdAtr.getValue()==null){
                formContext.ui.tabs.get("tab_creditagreement").setVisible(false);
                formContext.getControl("adlr_summa").setVisible(false);
                formContext.getControl("adlr_fact").setVisible(false);
            }
        }

    }
}
)();

