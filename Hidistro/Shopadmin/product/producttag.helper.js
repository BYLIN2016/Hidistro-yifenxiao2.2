function CheckTagId(checkboxobj){
   var tagIds=$("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g,"");
   var arrtag=null;
   if(tagIds!=""){
        if(tagIds.indexOf(',')<0){
            arrtag=new Array(1);
            arrtag[0]=tagIds;
        }else{
            arrtag=tagIds.split(',');
        }
   }else{
        tagIds=checkboxobj.value;
   }
   
   if(arrtag!=null){
        var x=-1;
        for(var t=0;t<arrtag.length;t++){
            if(checkboxobj.value==arrtag[t]){
                x=t;
                break;
            }
        }
        if(x>=0){
            tagIds="";
            for(var k=0;k<arrtag.length;k++){
                if(k!=x){
                    tagIds+=arrtag[k]+",";
                }
            }
            if(tagIds!=""){
                tagIds=tagIds.substr(0,tagIds.length-1);
            }
            //tagIds+=","+checkboxobj.value;
        }else{
            tagIds+=","+checkboxobj.value;
        }
   }
   $("#ctl00_contentHolder_txtProductTag").val(tagIds);
}


function AddTags(aobj){
    if($("#a_addtag").text()=="添加"){
         $("#div_addtag").show('slow');
          $("#a_addtag").text('取消');
          $("#a_addtag").attr('class','del');
    }else{
         $("#div_addtag").hide('hide');
          $("#a_addtag").text('添加');
          $("#a_addtag").attr('class','add');
    }
   
}

function AddAjaxTags(){
   var tagvalu=$("#txtaddtag").val().replace(/\s/g,"");
    if(tagvalu==""){
        alert('请输入标签名称！');
        return false;
    }
    $("#div_addtag").hide('hide');
    $("#a_addtag").text('添加');
    $("#a_addtag").attr('class','add');
    $.ajax({
            url: "ProductTags.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {TagValue:tagvalu,Mode:"Add",isAjax: "true"},
            async: false,
            success: function(data)
            {
                if (data.Status == "true")
                {
                    var newtagId=data.msg;
                    if(newtagId!=""&&parseInt(newtagId)>0){
                           $("#div_tags").append($("　<input type=\"checkbox\" value=\""+newtagId+"\" onclick=\"CheckTagId(this)\" checked=\"checked\" />"));
                           $("#div_tags").append(tagvalu);
                           var oldtagId=$("#ctl00_contentHolder_txtProductTag").val().replace(/\s/g,"");
                           if(oldtagId!=""){
                                oldtagId+=","+newtagId;
                           }else{
                                oldtagId=newtagId;
                           }
                           $("#ctl00_contentHolder_txtProductTag").val(oldtagId);
                           ShowMsg("添加标签名成功", true);
                    }       
                }
                else {
                    ShowMsg(data.msg, false);
                }
            }
        });
}