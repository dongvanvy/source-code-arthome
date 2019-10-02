$(document).ready(function(){
    $("#btn_submit").click(function(){
        if(validate(this.form)){
            alert("Edit Success!! please waitting a few minutes");
        }
        return false;
    });
});

function validate(form)
{
    var bolInput = true;
    var bolSelect = true;
    var bolArea =true;
    bolInput = validateInput(form);
    bolSelect = validateSelect(form);
    bolArea = validateTextArea(form);
    if(bolInput && bolSelect && bolArea){
        return true;
    }
    return false;
}

function validateInput(form){
    var bolVal = true;
    var isChecked = true;
    var currentForm = $(form);
    $.each(currentForm[0], function(key, val){
        if(val.localName === "input"){            
            if(val.value === ""){
                bolVal = false;
                $(val).removeClass("error");
                $(val).addClass("error");
                $(val).parent().find(".text-error").text("Cannot blank");
            }
            else{                
                    // validate ky tu dac biet, length
                $(val).removeClass("error");
                $(val).parent().find(".text-error").text("");                
                
            }
            if(val.type === "radio")
            {
                var divRadio = $(val).parent().find("input[type=radio]");
                $.each(divRadio, function(key, val){
                    if(!val.checked){
                        isChecked = false;
                        $(val).parent().removeClass("error");
                        $(val).parent().addClass("error");
                        $(val).parent().parent().find(".text-error").text("Cannot blank");
                    }
                    else{
                        isChecked = true;
                        $(val).parent().removeClass("error");
                        $(val).parent().parent().find(".text-error").text("");
                        return false;
                    }
                });           
            }
        }
    });
    if(bolVal && isChecked)
        return true;
    return false;
};
function validateSelect(form)
{
    var bolSelect=true;
    var currentForm=$(form);
    $.each(currentForm[0],function(key,val){
        if(val.localName === "select"){
            if(val.value === "0" || val.value === "")
            {
                bolSelect=false;
                $(val).removeClass("error");
                $(val).addClass("error");
                $(val).parent().find(".text-error").text("can't blank!!");
            }else{
                $(val).removeClass("error");               
                $(val).parent().find(".text-error").text("");
            }
            
        }
    });
    return bolSelect;
};
function validateTextArea(form)
{
    var bolArea= true;
    var currentForm = $(form);
    $.each(currentForm[0], function(key,val){
        if(val.localName === "textarea")
        {
            if(val.value === ""){
                bolArea = false;
                $(val).removeClass("error");
                $(val).addClass("error");
                $(val).parent().find(".text-error").text("Cannot blank");
            }
            else{                
                  
                $(val).removeClass("error");
                $(val).parent().find(".text-error").text("");                
                
            }
        }
    });
    return bolArea;
    
};
