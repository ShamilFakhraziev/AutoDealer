var Auto = Auto || {};



Auto.adlr_agreement_onSave = (function() {

    return {
    onSave : function(context){

        let formContext = context.getFormContext();
        let creditProgram = formContext.getAttribute("adlr_creditid").getValue();
        
        let creditProgramRef = creditProgram[0];
        Xrm.WebApi.retrieveRecord("adlr_credit", creditProgramRef.id, "?$select=adlr_dateend").then(
            function (result) {
                let creditDateEnd = result.adlr_dateend.substring(0,10).split('-');
                let agreementDate = formContext.getAttribute("adlr_date").getValue();

                if(creditDateEnd[0] < agreementDate.getFullYear() ){
                    alert("Истек срок действия кредитной программы относительно даты договора!");
                    context.getEventArgs().preventDefault();
                }
                else if(creditDateEnd[0] == agreementDate.getFullYear()) {
                    
                    if(creditDateEnd[1] < agreementDate.getMonth() + 1){
                        alert("Истек срок действия кредитной программы относительно даты договора!");
                        context.getEventArgs().preventDefault();
                    }
                    else if(creditDateEnd[1] == agreementDate.getMonth() ) {
                        if(creditDateEnd[2] < agreementDate.getDay() ){
                            alert("Истек срок действия кредитной программы относительно даты договора!");
                            context.getEventArgs().preventDefault();
                        }
                    
                    }
                }
            },
            function (error) {
                console.log(error.message);
            }
        );
        
    }
}
})();

