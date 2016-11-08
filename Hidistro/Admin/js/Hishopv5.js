//Javascript Document
function $_getID(id)
  {
    return document.getElementById(id);
  }

var Browser = new Object();

Browser.isMozilla = (typeof document.implementation != 'undefined') && (typeof document.implementation.createDocument != 'undefined') && (typeof HTMLDocument != 'undefined');
Browser.isIE = window.ActiveXObject ? true : false;
Browser.isFirefox = (navigator.userAgent.toLowerCase().indexOf("firefox") != - 1);
Browser.isSafari = (navigator.userAgent.toLowerCase().indexOf("safari") != - 1);
Browser.isOpera = (navigator.userAgent.toLowerCase().indexOf("opera") != - 1);

var imgPlus = new Image();
imgPlus.src = "../images/plus.gif";


/*上方菜单 */
function switchTab(tabpage,tabid){
var oItem = document.getElementById(tabpage).getElementsByTagName("li"); 
    for(var i=0;i<oItem.length; i++){
        var x = oItem[i];    
        x.className = "";
}
	document.getElementById(tabid).className = "Selected";
	var dvs=document.getElementById("cnt").getElementsByTagName("div");
	for (var i=0;i<dvs.length;i++){
	  if (dvs[i].id==('d'+tabid))
	    dvs[i].style.display='block';
	  else
  	  dvs[i].style.display='none';
	}
}
/* 左侧菜单 */
function border_left(tabpage,left_tabid)
{
    var oItem = document.getElementById(tabpage).getElementsByTagName("li"); 
    for(var i=0; i<oItem.length; i++)
    {
        var x = oItem[i];    
        x.className = "";
    }
	document.getElementById(left_tabid).className = "Selected";
	var dvs=document.getElementById("left_menu_cnt").getElementsByTagName("ul");
	for (var i=0;i<dvs.length;i++)
	{
	  if (dvs[i].id==('d'+left_tabid))
	    dvs[i].style.display='block';
	  else
  	    dvs[i].style.display='none';
	}
}
/* 左侧菜单active */
function dleft_tab_active(tabpage,activeid){
var obj=activeid
var oItem = document.getElementById(tabpage).getElementsByTagName("a"); 
    for(var i=0; i<oItem.length; i++){
        var x = oItem[i];    
        x.className = "";
}
	obj.className = "Selected";
}
function menu(tab){
if(tab.style.display=='block')tab.style.display='block';
else tab.style.display='block';
}
function su_click(obj){
	if(obj.className == 'open')
	{obj.className = 'close';}
	else{obj.className = 'open';}
	
}
function show_title(str){
	document.getElementById("dnow99").innerHTML=str;
}
function go_cmdurl(title,tabid){
	show_title(title);
	switchTab('TabPage1','Tab3');
	menu(document.getElementById('Tab3')); 
	dleft_tab_active('TabPage3',tabid);
}

/*展开和折叠*/
//

    function ProductClassTableExp(TableExpID,maxminID)
    {
        //alert(TableExpID);
         if (TableExpID.style.display == 'none')
            {
               TableExpID.style.display = 'block';
               maxminID.className = 'productDlminExp';
            }
         else
            {
               TableExpID.style.display = 'none';
               maxminID.className = 'productDlmaxExp';
            }
    }


/**
 * 折叠分类列表
 */
function rowClicked(obj)
{
    var tbl = document.getElementById("AlticleClassTable");
    var lvl = parseInt(obj.className);
    var fnd = false;

    for (i = 0; i < tbl.rows.length; i++)
    {
        var row = tbl.rows[i];

        if (tbl.rows[i] == obj)
        {
            fnd = true;
        }
        else
        {
            if (fnd == true)
            {
                var cur = parseInt(row.className);
                if (cur > lvl)
                {
                    row.style.display = (row.style.display != 'none') ? 'none' : (Browser.isIE) ? 'block' : 'table-row';
                }
                else
                {
                    fnd = false;
                    break;
                }
            }
        }
    }

    for (i = 0; i < obj.cells[0].childNodes.length; i++)
    {
        var imgObj = obj.cells[0].childNodes[i];
        if (imgObj.tagName == "IMG" && imgObj.src != '../images/icon_flag.gif')
        {
            imgObj.src = (imgObj.src == imgPlus.src) ? '../images/minus.gif' : imgPlus.src;
        }
    }
 }

        function dispalyalert(ReplaceHelpID)
		{
		    ReplaceHelpID.className = "warningmsg";
		    
		}
		function disablealert(ReplaceHelpID)
		{
		    ReplaceHelpID.className = "inputalert";
		}
		function testFuc()
		{
		    if(document.getElementById("<%=txtAddRoleName.ClientID%>").value == "")
		    {
		    document.getElementById("<%=statusEditRoles.ClientID%>").style.display = "none";
		    }
		    
		}
		
    //select card
    function listItemsScript(thisMenuListItems,thisListItems)
    {
		thisListItems = $_getID(thisListItems);
		var MenuListLength = $_getID('ListItemsAdd').getElementsByTagName('li').length;
		var menuListLI = $_getID('ListItemsAdd').getElementsByTagName('li');

		var listItemsLength = $_getID('allListItems').getElementsByTagName('div').length;
		var listItemsDiv = $_getID('allListItems').getElementsByTagName('div');
		
		for (i=0;i<MenuListLength ;i++ )
		    {	
               menuListLI[i].className = '';
			}
		for (t=0;t<listItemsLength ;t++ )
		{
			if (listItemsDiv[t].id.indexOf("ListItems") != -1)
			{
				listItemsDiv[t].className = 'hiddenThisLayer';
			}
		}
		thisMenuListItems.className = 'tabs-selected';
		thisListItems.className = 'showThisLayer';
    }

    function ProductClassTableExp(TableExpID,maxminID)
    {
        //alert(TableExpID);
        TableExpID = $_getID(TableExpID);
        maxminID = $_getID(maxminID);
         if (TableExpID.style.display == 'none')
            {
               TableExpID.style.display = 'block';
               maxminID.className = 'productDlminExp';
            }
         else
            {
               TableExpID.style.display = 'none';
               maxminID.className = 'productDlmaxExp';
            }
    }

	/*取得所有class为leftTD的对象;//
	var tdArray = $("."+"leftTD");
	var tdArrayValue = new Array();
	tdArrayValue.length = tdArray.length;
	//对数组按降序排列;
	function desc(x,y) {if (x > y) return -1; if (x < y) return 1;}  
	//对数组按升序排列;//备用//
	function asc(x,y) {if (x > y) return 1; if (x < y) return -1;}
	for (i=0;i<tdArray.length ;i++ )
	{
		tdArrayValue[i] = tdArray[i].offsetWidth;
	}
	var descTdArray = tdArrayValue.sort(desc);
	for (s=0;s<descTdArray.length ;s++ )
	{
		tdArray[s].style.width = descTdArray[0] + "px";
	}
	//alert(descTdArray); */
	    //显示隐藏添加的对话框：
    function expItemsTable(thisID)
    {
        var thisID = $_getID(thisID);
        var displayString = thisID.style.display;
        (displayString == "none")?thisID.style.display = "":thisID.style.display = "none";
    }

    var curentShipperId="";//存储当前所选择要修改的到达地区文本框ID号s
    function submitAllValue() {
        var subCheckBoxList = new Array();
        var inputTextValue = new Array();
        var inputTempValue = new Array();
        var inputIDValue = new Array();
        var inputTempIDValue = new Array();
         var suoyin=0;
        subCheckBoxList = $_getID('mainCheckBoxList').getElementsByTagName('input');
        for (var i = 0; i < subCheckBoxList.length; i++) {
            if (subCheckBoxList[i].checked) {
                
                inputTextValue[suoyin] = subCheckBoxList[i].value;
                inputIDValue[suoyin] = subCheckBoxList[i].id;
                suoyin++;
            }
        }
        for (var l = 0; l < inputTextValue.length; l++) {
            if (inputTextValue[l] != null&&inputTextValue[l]!="undefined") {
                inputTempValue = inputTempValue.concat(inputTextValue[l]);
                inputTempIDValue = inputTempIDValue.concat(inputIDValue[l]);
            }
        }
        var thisID;
        var thisIDValue
        if($("#AddHotareaPric").css("display")=="none"){
            if(curentShipperId!=""&&curentShipperId!="undefined"){
                thisID=$_getID(curentShipperId);
                $(thisID).next("input").eq(0).attr("id");
                thisIDValue=$_getID($(thisID).next("input").eq(0).attr("id"));
            }
            
        }else{
            thisID=$_getID("ctl00_contentHolder_txtRegion");
            thisIDValue=$_getID("ctl00_contentHolder_txtRegion_Id");
        }
       
        
        if (thisID == "")
         {
            return false;
        }
        else {
           thisID.value = inputTempValue;
           thisIDValue.value = inputTempIDValue;
           $("#layerArea").fadeOut();
        }
    }
    

  //反选部分;
  function checkexBox(thisObject)
  {
    curentShipperId=thisObject.id;
	var txtBoxValue = new Array();
	var areaItemsValue = new Array();
	var checkedItems = new Array();
	txtBoxValue = thisObject.value.split(",");
	$_getID("layerArea").style.display = "block";
	areaItemsValueObject = $_getID('areaItems').getElementsByTagName('input');
	checkBoxItems = $_getID('mainCheckBoxList').getElementsByTagName('input');
	checkBoxItemsText = $_getID('mainCheckBoxList').getElementsByTagName('label');
	subCheckBoxList = $_getID('mainCheckBoxList').getElementsByTagName('input');

	//$_getID("OK").className = thisObject.id;
	//$_getID("OK").idvalueBox = thisObject.id;
	//alert($_getID("OK").idvalueBox);
	//清空所有父区域;
	for (y=0;y<areaItemsValueObject.length ;y++ )
	{
		areaItemsValueObject[y].checked = false;
	}
	//清空所有子区域;
	for (y=0;y<subCheckBoxList.length ;y++)
	{
		subCheckBoxList[y].checked = false;
	}
	for (i=0;i<checkBoxItemsText.length ;i++ )
	{
		for (var t=0;t<txtBoxValue.length ;t++ )
		{
			if (checkBoxItemsText[i].innerHTML == txtBoxValue[t])
			{
				checkBoxItems[i].checked = true;
				checkedItems = checkedItems.concat(checkBoxItems[i].id);
			}
		}
	}
	//alert(checkedItems.toString()); /*checkedItems是一个object类型*/
	var verifyValue = true;
	var areaItemsString = new String();
	//alert(typeof(verifyValue));
	for (i=0;i<areaItemsValueObject.length ;i++ )
	{
		areaItemsValue[i] = new Array();//创建多维数组用于存储父区域的各项的值；
		for (t=0;t<areaItemsValueObject.length ;t++ )
		{
			areaItemsValue[i][t] = areaItemsValueObject[t].value.split(",");
		}
	}
	for (var arrLength=0;arrLength<checkedItems.length;arrLength++ )
	{
		if (arrLength >= areaItemsValue.length)
		{
			return false;
		}
		else
		{
			for (var chked=0;chked<areaItemsValue[arrLength].length;chked++)
			{
				verifyValue = compArr(areaItemsValue[arrLength][chked],checkedItems);
				if(verifyValue == true)
				{
					for (x=0;x<areaItemsValueObject.length;x++ )
					{
						if (areaItemsValueObject[x].value == areaItemsValue[arrLength][chked])
						{
							areaItemsValueObject[x].checked = true;
						}
					}
				}
			}
			break;
		}
	}
	displayAreaLayer(thisObject);
  }
	function compArr(a,b){
		if(a.length==b.length) return a.sort().join("")==b.sort().join("");
		var arr = a.slice(0);
		a = a.sort().join(",");
		b = b.sort().join(",");
		var re = new RegExp(arr.join(",|"),"g");
		return (b.length - b.replace(re,"").length == a.length)
	}
	function getTop(e){ 
    var offset=e.offsetTop; 
    if(e.offsetParent!=null) offset+=getTop(e.offsetParent); 
    return offset; 
    }
    function getLeft(e){ 
    var offset=e.offsetLeft; 
    if(e.offsetParent!=null) offset+=getLeft(e.offsetParent); 
    return offset; 
    }
	function displayAreaLayer(thisInputBoxID)
	{
		$_getID("layerArea").style.top = getTop(thisInputBoxID) + 25 + "px";
		$_getID("layerArea").style.left = getLeft(thisInputBoxID);
		$_getID("layerArea").style.display = "block";
	}
	function closeLayerArea()
	{
		$_getID("layerArea").style.display = "none";
	}
    
    function ocMy(mythis,BoxIndex)
	{
		var boxTitleList = document.getElementById('boxTitle').getElementsByTagName('li');
		var boxTitleLenght = boxTitleList.length;
		for(var i=0;i<boxTitleLenght;i++)
		{
			boxTitleList[i].className = "";
			document.getElementById('Box' + i).className = "bottomBoxHidden";
		}
		mythis.className = "pitchon"
		document.getElementById('Box' + BoxIndex).className = "bottomBox";
}

var menuTimer = null;
function showmenu(obj1, obj2, state, location) {
    var btn = obj1;
    //alert(btn.nextSibling.id);
    var obj = document.getElementById(obj2);
    var h = btn.offsetHeight;
    var w = btn.offsetWidth;
    var x = btn.offsetLeft;
    var y = btn.offsetTop;

    obj.onmouseover = function() {
        showmenu(obj1, obj2, 'show', location);
    }
    obj.onmouseout = function() {
        showmenu(obj1, obj2, 'hide', location);
    }

    while (btn = btn.offsetParent) { y += btn.offsetTop; x += btn.offsetLeft; }

    var hh = obj.offsetHeight;
    var ww = obj.offsetWidth;
    var xx = obj.offsetLeft; //style.left;
    var yy = obj.offsetTop; //style.top;
    var obj2state = state.toLowerCase();
    var obj2location = location.toLowerCase();

    var showx, showy;

    if (obj2location == "left" || obj2location == "l" || obj2location == "top" || obj2location == "t" || obj2location == "u" || obj2location == "b" || obj2location == "r" || obj2location == "up" || obj2location == "right" || obj2location == "bottom") {
        if (obj2location == "left" || obj2location == "l") { showx = x - ww; showy = y; }
        if (obj2location == "top" || obj2location == "t" || obj2location == "u") { showx = x; showy = y - hh; }
        if (obj2location == "right" || obj2location == "r") { showx = x + w; showy = y; }
        if (obj2location == "bottom" || obj2location == "b") { showx = x; showy = y + h; }
    } else {
        showx = xx; showy = yy;
    }
    obj.style.left = showx + "px";
    obj.style.top = showy + "px";
    if (state == "hide") {
        menuTimer = setTimeout("hiddenmenu('" + obj2 + "')", 10);
    } else {
        clearTimeout(menuTimer);
        obj.style.visibility = "visible";
    }
}
function hiddenmenu(psObjId) {
    document.getElementById(psObjId).style.visibility = "hidden";
}

