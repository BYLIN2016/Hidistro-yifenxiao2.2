//返回弹出的DIV的坐标
windowPop=function(divWidth,divHeight,msgInfo,switchResult)
{
  this.divWidth=divWidth;
  this.divHeight=divHeight;
  this.msgInfo=msgInfo;
  this.switchResult=switchResult;
  
 this.showHTml=function()
 { 
    var isCase=this.switchResult;
	var icoNum=1;
	var html;
	if(isCase==0||isCase=="error"||isCase==false){isCase=0;icoNum=2;}
	if(isCase==1||isCase==true) {isCase=1;icoNum=1;} 
	switch(isCase)
	{
				case 0:
				case 1:
				html='<div class="windowsdialog" style="display:none;" id="windowsdialog">'+
				      '<div class="dialog-header" id="handWindow"><div class="dialog-tl"></div>'+
				        '<div class="dialog-tc"><div class="dialog-tc1"><div id="dialog-title" class="dialog-title">友情提示：</div>'+
				        '</div></div>'+
				        '<div class="dialog-tr"><a class="dialog-close" title="关闭"></a></div></div>'+
				        '<div class="dialog-content"><table cellspacing="0" cellpadding="0" border="0" width="100%"><tbody><tr><td valign="top">'+
				        '<div class="ico_'+icoNum+'"></div></td><td><div class="dialog-word"><ul><li>'+this.msgInfo+'</li></ul></div></td></tr></tbody></table></div>'+
				        '<div class="dialog-bot"><div class="dialog-bl"></div><div class="dialog-bc"></div><div class="dialog-br"></div></div></div>';	
				    break;
				case 2:
				case 3:
	}
	return html;
  }
  
  //关闭窗口
 this.closewindowPop=function()
  {
	  $(".dialog-close").click(function(){
		$("#popDivLock").remove();
		$("#windowsdialog").fadeOut(200);
	   });
  }

 this.autoClosePop=function()
  {
     setTimeout(function(){
      $("#popDivLock").remove();
      $("#windowsdialog").fadeOut(200);      
     },3000)  
  }
 this.windoPopOpen=function()
 {      var html=this.showHTml();
		var clientH = $(window).height();	//浏览器高度
		var clientW = $(window).width();	//浏览器宽度
		var div_X = (clientW - this.divWidth)/2;	
		var div_Y = (clientH - this.divHeight)/2;
		div_X += window.document.documentElement.scrollLeft;	//DIV显示的实际横坐标
		div_Y += window.document.documentElement.scrollTop;	//DIV显示的实际纵坐标
		$("body").append(html);//增加DIV
		 this.closewindowPop()//添加关闭窗口事件
		//$("#"+objDiv).show();	
		//divWindow的样式
		$("#windowsdialog").fadeIn(100);
		$("#windowsdialog").css("left",(div_X + "px"));//定位DIV的横坐标
		$("#windowsdialog").css("top",(div_Y + "px"));	//定位DIV的纵坐标
		$("#windowsdialog").width(this.divWidth);
		//$("#windowsdialog").height(divHeight);
		$("#windowsdialog").css("border","solid 1px #999");
		$("#windowsdialog").css("-moz-border-radius","0px 0px 0px 0px");
}

//锁定背景屏幕
this.lockScreen=function(){
	if($("#popDivLock").length == 0){	//判断DIV是否存在
		var clientH = $(window).height();	//浏览器高度
		var clientW = $(window).width();	//浏览器宽度		
		var docH = $("body").height();	//网页高度
		var docW = $("body").width();	//网页宽度
		var bgW = clientW > docW ? clientW : docW;	//取有效宽
		var bgH = clientH > docH ? clientH : docH;	//取有效高		
		$("body").append("<div id='popDivLock'></div>")	//增加DIV
		$("#popDivLock").height(bgH+100);
		$("#popDivLock").width(bgW);
		$("#popDivLock").css("display","block");
		$("#popDivLock").css("background-color","#333333");
		$("#popDivLock").css("position","absolute");
		$("#popDivLock").css("z-index","500");
		$("#popDivLock").css("top","0px");
		$("#popDivLock").css("left","0px");
		$("#popDivLock").css("opacity","0.4");
	 }
	else{
		clientH = $(window).height();	//浏览器高度
		clientW = $(window).width();	//浏览器宽度
		$("#popDivLock").height(clientH);
		$("#popDivLock").width(clientW);
	}
 }
}


function ShowMsg(msg,success)
{
      if (success)
	    {    //参数(宽value,高value,提示信息string,信息类别提示0,1,error,true,false,3,4)
             var popWin=new windowPop(350,200,msg,1);
             popWin.windoPopOpen();        
             popWin.autoClosePop()
        }
     else
      { 
         var popWin=new windowPop(350,200,msg,0);
         popWin.lockScreen();	//锁定背景
         popWin.windoPopOpen();
       }
}


//访jquery unblock弹出窗口

function DivWindowOpen(divWidth,divHeight,objDiv){
	lockScreen();	//锁定背景
	divOpen(objDiv,divWidth,divHeight);	
    $(".img_datala img").click(function()
     {
       $(this).css("cursor","pointer");
       $("#divLock").remove();$("#"+objDiv).hide(200);
     });
}

//返回弹出的DIV的坐标
function divOpen(objDiv,divWidth,divHeight){
		var clientH = $(window).height();	//浏览器高度
		var clientW = $(window).width();	//浏览器宽度
		var div_X = (clientW - divWidth)/2;	
		var div_Y = (clientH - divHeight)/2;
		div_X += window.document.documentElement.scrollLeft;	//DIV显示的实际横坐标
		div_Y += window.document.documentElement.scrollTop;	//DIV显示的实际纵坐标
		$("#"+objDiv).show();	//增加DIV
		//divWindow的样式
		$("#"+objDiv).css("position","absolute");
		$("#"+objDiv).css("z-index","9999");
		$("#"+objDiv).css("left",(div_X + "px"));//定位DIV的横坐标
		$("#"+objDiv).css("top",(div_Y + "px"));	//定位DIV的纵坐标
		$("#"+objDiv).css("opacity","0.9");
		$("#"+objDiv).width(divWidth);
		$("#"+objDiv).height(divHeight);
		$("#"+objDiv).css("background-color","#FFFFFF");
		$("#"+objDiv).css("border","solid 7px #666666");
		$("#"+objDiv).css("-moz-border-radius","5px 5px 5px 5px");
}

//锁定背景屏幕
function lockScreen(){
	if($("#divLock").length == 0){	//判断DIV是否存在
		var clientH = $(window).height();	//浏览器高度
		var clientW = $(window).width();	//浏览器宽度		
		var docH = $("body").height();	//网页高度
		var docW = $("body").width();	//网页宽度
		//var docH =window.screen.height;	//网页高度
		//var docW =window.screen.width;	//网页宽度
		var bgW = clientW > docW ? clientW : docW;	//取有效宽
		var bgH = clientH > docH ? clientH : docH;	//取有效高	
		$("body").append("<div id='divLock'></div>")	//增加DIV
		$("#divLock").height(clientH);
		$("#divLock").width(clientW);
		$("#divLock").css("display","block");
		$("#divLock").css("background-color","#333333");
		//$("#divLock").css("position","fixed");
		$("#divLock").css("z-index","500");
		$("#divLock").css("top","0px");
		$("#divLock").css("left","0px");
		$("#divLock").css("opacity","0.4");
		if(testBrowsVersion()=="ie6.0")		
		{
		  $("#divLock").css("position","absolute");
		  $("#divLock").css("height",bgH);
		}
		else
		 {
		   $("#divLock").css("position","fixed");
		 }	
		
	 }
	else{
		clientH = $(window).height();	//浏览器高度
		clientW = $(window).width();	//浏览器宽度
		$("#divLock").height(clientH);
		$("#divLock").width(clientW);
	}
}

function CloseDiv(objDiv)
{
    $("#divLock").remove();
    $("#" + objDiv).hide(200);
}

 function testBrowsVersion()
      {
        var Sys = {};
        var ua = navigator.userAgent.toLowerCase();
        var s;
        (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
        (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
        (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
        (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
        //以下进行测试
        if (Sys.ie) return "ie"+Sys.ie;
        if (Sys.firefox) return Sys.firefox; /*document.write('Firefox: ' + Sys.firefox);*/
        if (Sys.chrome) return Sys.chrome;
        if (Sys.opera) return Sys.opera;
        if (Sys.safari) return Sys.safari;     
   }    
   
