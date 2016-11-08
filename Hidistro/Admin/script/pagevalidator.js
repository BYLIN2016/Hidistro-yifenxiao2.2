/*
 * To Run This Code jQuery 1.3.2(http://www.jQuery.com) Required.
 */
//base class for a validator
function Validator(controlId, allowEmpty, errorMsg, onValid) {
    this._ErrorMsg = errorMsg;
    this._ValidateTargetId = controlId;
    this._AllowEmpty = allowEmpty;
    this._DefaultMsg = $("#"+controlId+"Tip").html();
    if (null != onValid && undefined != onValid)
        this._OnValid = onValid;
    this._IsValid = false;
}

//note: tipControl panel must with these format : valid control id and "Tip"
Validator.prototype.UpdateStatus = function() {
    var controlToValidate = $("#" + this._ValidateTargetId);
    var tipControl = $("#" + this._ValidateTargetId + "Tip");

    if (!this._IsValid) 
    {
        controlToValidate.removeClass("forminput");
        controlToValidate.addClass("errorFocus");       
        tipControl.html(this._ErrorMsg);
        tipControl.removeClass();
        tipControl.addClass("msgError");
    }
    else 
    {
        // 校验通过，则清除样式和文字，并隐藏tipControl
        controlToValidate.removeClass("errorFocus");
        controlToValidate.addClass("forminput");
        
        tipControl.html(this._DefaultMsg);        
        tipControl.removeClass();
        //tipControl.addClass("msgTrue");
        if (null != this._OnValid)
            this._OnValid(controlToValidate);
    }
}

function SelectValidator(controlId, allowEmpty, errorMsg, onValid) {
    this._base = Validator;
    this._base(controlId, allowEmpty, errorMsg, onValid);
    if (allowEmpty)
    {
        this._IsValid = true;
    }
}

SelectValidator.prototype.UpdateStatus = Validator.prototype.UpdateStatus;
SelectValidator.prototype.valid = function() {
    if (this._AllowEmpty)
        this._IsValid = true;
    else {
        var isValid = true;
        var controlToValidate = $("#" + this._ValidateTargetId);
        var groupname = controlToValidate.attr("groupname");
        var ctls = $("select[groupname='" + groupname + "']");

        ctls.each(function() {
            if (this.options.length > 0) {
                if (this.value == "" || this.value == "-1")
                    isValid = false;
                else
                    isValid = isValid && true;
            }
            else {
                isValid = isValid && true;
            }
        });
        this._IsValid = isValid;
    }
}

//this class represent checking a input element inherit from Validator
function InputValidator(controlId, min, max, allowEmpty, regex, errorMsg, onValid) {
    this._base = Validator;
    this._base(controlId, allowEmpty, errorMsg, onValid);

    this.min = min;
    this.max = max;
    this.regex = regex;

    if (allowEmpty) {
        this._IsValid = true;
    }
}

InputValidator.prototype.UpdateStatus = Validator.prototype.UpdateStatus;
InputValidator.prototype.valid = function() {
    var controlToValidate = $("#" + this._ValidateTargetId);
    var val = controlToValidate.val();
    var len = val.length;

    // 2007-12-21 by jeffery
    // 如果输入框不能为空且没有输入任何内容，则直接验证失败
    //
    if (!this._AllowEmpty && (val.length == 0)) {
        this._IsValid = false;
        return;
    }

    // 结合实际，我们把一个汉字的长度也当作1
    //    for (var i = 0; i < val.length; i++) {
    //        if ( val.charCodeAt(i)>=0x4e00 && val.charCodeAt(i)<= 0x9fa5 )
    //            len+=2;
    //        else
    //        len++;
    //    }

    if (val.length == 0 && this._AllowEmpty == true) {
        this._IsValid = true;
    }
    else if ((len < this.min) || ((this.max > 0) && (len > this.max))) {
        this._IsValid = false;
    }
    //check with regexpression
    else if (this.regex != null && this.regex != undefined && typeof this.regex == "string" && this.regex != "") {
        var exp = new RegExp("^" + this.regex + "$", "i");
        // 2007-12-7 by jeffery
        // 改为直接将匹配结果赋值给isValid
        //
        this._IsValid = exp.test(val);
    }
    else {
        this._IsValid = true;
    }
}

//this class represent compare valid
function CompareValidator(controlId, desID, errorMsg) {
    this.base = Validator;
    this.base(controlId, true, errorMsg, null);

    this._compare = desID;
}

CompareValidator.prototype.UpdateStatus = Validator.prototype.UpdateStatus;
CompareValidator.prototype.valid = function() {
    var controlToValidate = $("#" + this._ValidateTargetId);
    var desCTLWithJQuery = $("#" + this._compare);

    // 2007-12-7 by jeffery
    // 直接将比较结果赋值给isValid
    //
    this._IsValid = (controlToValidate.val() == desCTLWithJQuery.val());
}

// 2007-12-05 by jeffery
// 添加整数值范围的验证类型
//
function NumberRangeValidator(controlId, minValue, maxValue, errorMsg, onValid) {
    this.base = Validator;
    this.base(controlId, true, errorMsg, onValid);
    this._minValue = minValue;
    this._maxValue = maxValue;
}

NumberRangeValidator.prototype.UpdateStatus = Validator.prototype.UpdateStatus;
NumberRangeValidator.prototype.valid = function() {
    var controlToValidate = $("#" + this._ValidateTargetId);
    if (controlToValidate.val().length == 0) {
        this._IsValid = true;
    }
    else {
        var num = parseInt(controlToValidate.val());
        this._IsValid = ((num >= this._minValue) && (num <= this._maxValue));
    }
}

// 添加金额范围的验证类型
//
function MoneyRangeValidator(controlId, minValue, maxValue, errorMsg, onValid) {
    this.base = Validator;
    this.base(controlId, true, errorMsg, onValid);

    this._minValue = minValue;
    this._maxValue = maxValue;
}

MoneyRangeValidator.prototype.UpdateStatus = Validator.prototype.UpdateStatus;
MoneyRangeValidator.prototype.valid = function() {
    var controlToValidate = $("#" + this._ValidateTargetId);
    if (controlToValidate.val().length == 0) {
        this._IsValid = true;
    }
    else {
        var num = parseFloat(controlToValidate.val());
        this._IsValid = ((num >= this._minValue) && (num <= this._maxValue));
    }
}

//this class represent ajax valid
function AjaxValidator(controlId, url, errorMsg, ajaxCallback) {
    this.base = Validator;
    this.base(controlId, true, null, errorMsg, null);

    this.url = url;
    this.isAjax = true;

    if (null != ajaxCallback)
        this.callback = ajaxCallback;
}

AjaxValidator.prototype.UpdateStatus = Validator.prototype.UpdateStatus;
AjaxValidator.prototype.valid = function() {
    var controlToValidate = $("#" + this._ValidateTargetId);
    var tipControl = $("#" + controlToValidate.attr("id") + "Tip");

    tipControl.html("loading...");
    tipControl.removeClass();
    tipControl.addClass("msgAjaxing");

    controlToValidate.get(0).ajaxvalid = this;
    $.ajax({
        type: "POST",
        url: this.url,
        data: controlToValidate.attr("name") + "=" + controlToValidate.val(),
        success: function(data) {
            var obj = eval("({value:" + data + "})");
            var t = controlToValidate.get(0).ajaxvalid;

            if (undefined != t.callback) {
                t.callback(obj, t);
            }
            else {
                if (obj.value)
                    t._IsValid = true;
                else
                    t._IsValid = false;
            }

            t.UpdateStatus();
        }
    });
}

//function represent init a element need to valid
function initValid(validator) {
    var controlId = validator._ValidateTargetId;
    var controlToValidate = $("#" + controlId);
    
    $("#"+controlId).attr("ValidateGroup","default");

    // 2008-09-12 by jeffery 判断目标控件是否为空，为空的话就不进行初始化
    if (controlToValidate == null || controlToValidate.get(0) == null)
        return;

    var srcTag = controlToValidate.get(0).tagName;
    var arrayValidator = new Array();

    arrayValidator.push(validator);
    controlToValidate.get(0).validator = arrayValidator;

    // 初始化时不显示任何信息
    //    var tipControl = $("#" + controlId + "Tip");
    //    
    //    tipControl.html(controlToValidate.attr("description"));
    //    tipControl.addClass("msgNormal");

    if (srcTag == "INPUT" || srcTag == "TEXTAREA") {
        var type = controlToValidate.attr("type");
        if (type == "text" || type == "password" || type == "file" || srcTag == "TEXTAREA") {
            var defaultVal = controlToValidate.attr("value");
            if (null != defaultVal && defaultVal != undefined && defaultVal != "") {
                validator._IsValid = true;
            }
            // 取消聚焦事件
               controlToValidate.focus(function()
               {
                         //var tipControl = $("#" + this["id"] + "Tip");
                          //tipControl.html( this.validator[0].msgOnTip );                          
                         // var controlToValidate = $("#" + this._ValidateTargetId);                              			 
                           //tipControl.removeClass();
                           //tipControl.addClass("msgOnFocus");                                                   
                        var inputFocus = $("#" + this["id"]);
                          inputFocus.removeClass("errorFocus");                           
                          inputFocus.addClass("forminput");
                          
                        var tipControl = $("#" + this["id"] + "Tip");                        
                         //var tipControl = $("#" + this._ValidateTargetId + "Tip");
                           tipControl.removeClass();
                        
              });
                                  
            
            
            controlToValidate.blur(function()
            {
                for (var i = 0; i < this.validator.length; i++) {
                    this.validator[i].valid();
                    if (this.validator[i].isAjax == null || this.validator[i].isAjax == undefined) {
                        this.validator[i].UpdateStatus();
                        if (!this.validator[i]._IsValid)
                            break;
                    }
                }
            });
        } else if (type == "checkbox" || type == "radio") {
            var ctls = $("input[name=" + controlToValidate.attr("name") + "]");
            var defaultVal = controlToValidate.attr("checkedValue");

            if (null != defaultVal && defaultVal != undefined) {
                ctls.each(function() {
                    if (this.value == defaultVal) {
                        this.checked = "checked";
                        validator._IsValid = true;
                    }
                });
            }
            ctls.bind("click", function() {
                var val;
                if (this.validator == undefined) {
                    val = ctls.get(0).validator[0];
                }
                else
                    val = this.validator[0];
                    
                val._IsValid = true;
                val.UpdateStatus();
            });
        }
    } else if (srcTag == "SELECT") {
        var groupname = controlToValidate.attr("groupname");
        var ctls = $("select[groupname='" + groupname + "']");

        // 2007-12-7 by jeffery
        // 如果初始化的时候有选中的值，则将isValid设为true
        //
        if (controlToValidate.val() != null && controlToValidate.val() != "" && controlToValidate.val() != "-1")
            validator._IsValid = true;

        ctls.each(function() {
            var defaultVal = $(this).attr('selectedValue');
            if (null != defaultVal && defaultVal != undefined) {
                $.each(this.options, function() {
                    if ($.trim(this.value) == $.trim(defaultVal) || this.text == defaultVal) {
                        this.selected = true;
                    }
                });
                validator._IsValid = true;
            }
        });

        ctls.bind("change", function() {
            var validators = ctls.get(0).validator;
            for (var i = 0; i < validators.length; i++) {
                if (validators[i].isAjax == null || validators[i].isAjax == undefined) {
                    validators[i].valid();
                    validators[i].UpdateStatus();

                    if (!validators[i]._IsValid)
                        break;
                }
                else {
                    if (this.id == controlId)
                        validators[i].valid();
                }
            }
        });
    }
}

function appendValid(validator) {
    var controlToValidate = $("#" + validator._ValidateTargetId).get(0);
    if (controlToValidate.validator == undefined)
        controlToValidate.validator = new Array();
    // 2007-12-07 by jeffery
    // 追加的验证在初始化时默认为true
    //
    validator._IsValid = true;
    controlToValidate.validator.push(validator);
}

// 2007-12-7 by jeffery
// 添加验证分组
//
function PageIsValid() {
  /* var isValid = true;
    var validateGroup = "default"; // 默认分组

    if (arguments.length > 0)
        validateGroup = arguments[0];

    var ctls = $("[ValidateGroup='" + validateGroup + "']");
    ctls.each(function() {
        if ($("#" + this["id"]).get(0).validator != undefined && $("#" + this["id"]).get(0).validator != null) {
            for (var i = 0; i < $("#" + this["id"]).get(0).validator.length; i++) {
                if ($("#" + this["id"]).get(0).validator[i]._IsValid == false) {
                    $("#" + this["id"]).get(0).validator[i].UpdateStatus();
                    isValid = false;
                }
            }
        }
    });
    return isValid;*/    
    
    var isValid = true;
    var validateGroup = "default";// 默认分组

    if(arguments.length > 0)
        validateGroup = arguments[0];        
    var ctls = $("[ValidateGroup='" + validateGroup + "']" );
    ctls.each(function(){       
       if ($( "#"+this["id"] ).get(0).validator!=undefined && $( "#"+this["id"] ).get(0).validator!=null )
       {    
           for( var i = 0 ; i < $( "#"+this["id"] ).get(0).validator.length; i++ ){
              if ( $( "#"+this["id"] ).get(0).validator[i]._IsValid==false )
              {
                 $( "#"+this["id"] ).get(0).validator[i].UpdateStatus();
                 isValid = false;
              }
           }
       }
    });
    return isValid; 
}