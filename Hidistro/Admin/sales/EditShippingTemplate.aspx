<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EditShippingTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.EditShippingTemplate" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
 <Hi:Style ID="Style1"  runat="server" Href="/admin/css/Hishopv5.css" Media="screen" />
 <Hi:Script ID="Script1" runat="server" Src="/admin/js/Hishopv5.js"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
     function showAddAreaDiv()
     {
        //var fagtxtRegion=document.getElementById("ctl00_contentHolder_txtRegion").value="";//到达地      
     　　var fagtxtRegionPrice=document.getElementById("ctl00_contentHolder_txtRegionPrice").value="";//起步价
     　　var fagtxtAddRegionPrice=document.getElementById("ctl00_contentHolder_txtAddRegionPrice").value="";//加价 
     　　DivWindowOpen(450,360,'AddHotareaPric'); 　
     }    

function InitValidators()
{
    initValid(new InputValidator('ctl00_contentHolder_txtModeName', 1, 60, false, null, '配送方式名称不能为空，长度限制在60字符以内'))
    initValid(new InputValidator('ctl00_contentHolder_txtWeight', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '起步重量不能为空,必须为正数,限制在100千克以内'))
    appendValid(new NumberRangeValidator('ctl00_contentHolder_txtWeight', 0, 100000, ' 必须为非负数字,限制在100千克以内'));
    initValid(new InputValidator('ctl00_contentHolder_txtAddWeight', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '加价重量必须为正数,限制在100千克以内'))
    appendValid(new NumberRangeValidator('ctl00_contentHolder_txtAddWeight', 0, 100000, ' 必须为非负数字,限制在100千克以内'));
        initValid(new InputValidator('ctl00_contentHolder_txtPrice', 1, 10, false, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '默认起步价不能为空,必须为非负数字,限制在1000万以内'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtPrice', 0, 10000000, ' 必须为非负数字,限制在1000万以内'));
        initValid(new InputValidator('ctl00_contentHolder_txtAddPrice', 1, 10, true, '(0|(0+(\\.[0-9]{1,2}))|[1-9]\\d*(\\.\\d{1,2})?)', '默认加价必须为非负数字,限制在1000万以内'))
        appendValid(new NumberRangeValidator('ctl00_contentHolder_txtAddPrice', 0, 10000000, ' 必须为非负数字,限制在1000万以内'));
   
}

   function IsFlagDate()
   {  
       var fagtxtRegion=document.getElementById("ctl00_contentHolder_txtRegion").value;//到达地      
     　　var fagtxtRegionPrice=document.getElementById("ctl00_contentHolder_txtRegionPrice").value;//起步价
     　　　var fagtxtAddRegionPrice=document.getElementById("ctl00_contentHolder_txtAddRegionPrice").value;//加价    　　　
            if (fagtxtRegion.length <= 0 || fagtxtRegion.length > 60) { alert("到达地不能为空，长度限制在60字符以内"); return false; }
     　　   if(fagtxtRegionPrice.length<=0 ||fagtxtRegionPrice.length>10){alert("起步价必须为非负整数,限制在1000万以内!"); return false;}
            else{ var reg=/^[0-9]+([.]{1}[0-9]{1,2})?$/;if(!reg.test(fagtxtRegionPrice)){alert("起步价必须为非负整数,限制在1000万以内");return false;}}
            if(fagtxtAddRegionPrice.length<=0 ||fagtxtAddRegionPrice.length>10){alert("加价必须为非负整数,限制在1000万以内!");return false;}
            else{var reg=/^[0-9]+([.]{1}[0-9]{1,2})?$/;if(!reg.test(fagtxtAddRegionPrice)){alert("加价必须为非负整数,限制在1000万以内");return false;}
        }
       return true;         　    
    }  

$(document).ready(function() { InitValidators();});
function checkRansack(checkBoxList, checked) {
    if (typeof (checkBoxList) != 'undefined') {
        //定义subCheckBoxList数组，用于存放子checkbox的ID值；
        var subCheckBoxList = new Array();
        var subCheckBoxListArrayID = checkBoxList.split(",");
        for (var i = 0; i < subCheckBoxListArrayID.length; i++) {
            var checkBoxID = subCheckBoxListArrayID[i];
            //alert(checkBoxID);
            var childCheckBoxID = $_getID(checkBoxID);
            if (checked) {
                childCheckBoxID.checked = true;
            }
            else {
                childCheckBoxID.checked = false;
            }
        }
    }
};
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnright">
		  <div class="title"> 
            <em><img src="../images/05.gif" width="32" height="32" /></em>
		    <h1>编辑运费模板</h1>
		    <span>您可以为不同的地区设置不同的收费标准</span></div>
		  <div class="formitem validator4">
		    <ul>
		      <li> <span class="formitemtitle Pw_110"><em >*</em>模板名称：</span>
		        <asp:TextBox ID="txtModeName" runat="server" class="forminput"></asp:TextBox>
		        <p id="ctl00_contentHolder_txtModeNameTip">配送方式名称不能为空，长度限制在60字符以内</p>
	          </li>
		      <li><span class="formitemtitle Pw_110"><em >*</em>起步重量：</span>
		       <asp:TextBox ID="txtWeight" runat="server" class="forminput"></asp:TextBox>(克)
		       <p id="ctl00_contentHolder_txtWeightTip">起步重量不能为空,必须为正数,限制在100千克以内</p>
		      </li>
              <li><span class="formitemtitle Pw_110">加价重量：</span>
		        <asp:TextBox ID="txtAddWeight" runat="server" class="forminput"></asp:TextBox>(克)
		        <p id="ctl00_contentHolder_txtAddWeightTip">加价重量必须为正数,限制在100千克以内</p>
		      </li>
              <li><span class="formitemtitle Pw_110"><em >*</em>默认起步价：</span>
		        <asp:TextBox ID="txtPrice" runat="server" class="forminput"></asp:TextBox>(元)
		       <p id="ctl00_contentHolder_txtPriceTip">默认起步价不能为空,必须为非负数字,限制在1000万以内</p>
		      </li>
              <li><span class="formitemtitle Pw_110">默认加价：</span>
		        <asp:TextBox ID="txtAddPrice" runat="server" class="forminput"></asp:TextBox>(元)
	          <p id="ctl00_contentHolder_txtAddPriceTip">默认加价必须为非负数字,限制在1000万以内</p>
	          </li>
		      <li class="m_none"> 
              <span class="formitemtitle Pw_110 ">地区价格：</span>              
	           <span class="abbr"><a href="javascript:showAddAreaDiv();">点击添加地区价格</a></span>
              <div class="content Pa_110 clearfix">
              <UI:Grid ID="grdRegion" runat="server" ShowHeader="true" AutoGenerateColumns="false" CssClass="colorQ Pg_20" HeaderStyle-CssClass="table_title" DataKeyNames="RegionsId" GridLines="None">
                            <Columns>   
                            <asp:TemplateField HeaderText="到达地区" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtRegionvalue" runat="server" CssClass="forminput" Text='<%# Eval("Regions")%>' onclick="checkexBox(this);" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtRegionvalue_Id" runat="server" Text='<%# Eval("RegionsId")%>' Style="display: none"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="起步价" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtModeRegionPrice" runat="server" CssClass="forminput" Text='<%# Eval("RegionPrice") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="加价" ItemStyle-Width="100px" HeaderStyle-CssClass="td_right td_left">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtModeRegionAddPrice"  runat="server" Text='<%# Eval("RegionAddPrice") %>' class="forminput"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="50px">
                                <ItemTemplate>
                                   <span class="submit_shanchu clear"><asp:LinkButton ID="lkbDelete" runat="server" CommandName="Delete" Text="删除" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>                                                                         
                            </Columns>
                        </UI:Grid>
              
                </div>
	          </li>
	        </ul>
    <ul class="btn Pa_110 clear" style="padding-top:20px;">
    <asp:Button ID="btnUpdate" runat="server" OnClientClick="return PageIsValid();"  CssClass="submit_DAqueding inbnt" Text="保 存"/>
            </ul>
	      </div>
  </div>
</div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

 <div class="Pop_up" id="AddHotareaPric" style="display:none;">
  <h1>添加地区价格 </h1>
  <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
  <div class="mianform">
   <ul>
    <li><span class="formitemtitle Pw_100">到达地：<em >*</em></span>
    <asp:TextBox ID="txtRegion_Id" runat="server"  Style="display: none"></asp:TextBox>
    <input name="txtRegion" runat="server" type="text" id="txtRegion" readonly="readonly" class="forminput"  onclick="checkexBox(this);" />
     <p class="Pa_100" id="ctl00_contentHolder_txtRegionTip">长度限制在60个字符以内</p>
   </li>
    <li><span class="formitemtitle Pw_100">起步价：<em >*</em></span>
   <asp:TextBox ID="txtRegionPrice" runat="server" Columns="15" Text="0" class="forminput"></asp:TextBox>              
    <p class="Pa_100" id="ctl00_contentHolder_txtRegionPriceTip">必须为非负数字,限制在1000万以内</p>
   </li>
    <li><span class="formitemtitle Pw_100">加价：<em >*</em></span>
      <asp:TextBox ID="txtAddRegionPrice" runat="server" Columns="15" Text="0" class="forminput"></asp:TextBox>
    <p class="Pa_100" id="ctl00_contentHolder_txtAddRegionPriceTip">长度限制在20个字符以内</p>
   </li>
   <li class="clear"></li>
  </ul>
   <ul class="up Pa_100">
        <asp:Button ID="btnAdd" runat="server" Text="添加价格" CssClass="submit_DAqueding" ValidationGroup="validatRegion" OnClientClick="return IsFlagDate();"/>
  </ul>
  </div>
</div>
        <!-- 选择地区 -->
        <div id="layerArea" class="layerArea" style="position:absolute;display:none;z-index:99999">
            <div class="closeBotton">
                <a style="cursor:pointer" onclick="closeLayerArea()">×</a>
            </div>
            <Hi:RegionArea ID="regionArea" runat="server" >
            <SkinTemplate>
                <div id="areaItems" class="areaItems"> 
                <div id="contents" runat="server" style="width:350px"></div>
                </div>
                 <div class="lines"></div> 
                <div id="mainCheckBoxList" class="mainCheckBoxList">
                 <div id="contentRegion" runat="server"></div>
                </div>
            </SkinTemplate>
            </Hi:RegionArea>
           <div class="submitTj">
            <input type="button" id="Button1" class="submit_queding" value="确定" onclick="submitAllValue();" />
            <input type="button" id="Button2" class="submit_queding" value="取消" onclick="closeLayerArea();" />
           </div>
        </div>	
</asp:Content>
