<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddExpressTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AddExpressTemplate" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
	<script src="../js/ExpressFlex.js" ></script>
<style type="text/css">.tdline{	border-right:1px #ccc solid;}</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
			 <div class="title">
			  <em><img src="../images/03.gif" width="32" height="32" /></em>
			  <h1>新增快递单</h1>
			  <span>新增快递单模板</span>
			  </div>
			  <div class="datalist">
			     <table width="100%" id="controls" height="56" border="0" cellpadding="0" cellspacing="0" style="text-align:center; border:1px #CCC solid;  background-color:#E7E7E7; font-size:12px; padding:0px;
				margin:0px 0px 5px 0px;">
				  <tr>
					<td height="24" colspan="2" class="tdline">单据名称</td>
					<td colspan="2" class="tdline">单据尺寸</td>
					<td colspan="2" class="tdline">单据背景图</td>
					<td colspan="2" class="tdline">单据打印项</td>
					<td width="73" class="tdline"><form name="form0" style=" padding:8px 0px 0px 0px">
					  <label>
					  </label>
					</form></td>
					<td width="146"><form name="form1" style=" padding:8px 0px 0px 0px">
					  <label></label>
					</form></td>
				  </tr>
				  <tr>
					<td width="108" height="30"><form name="form2">
					  <label><select name="emsname" id="emsname" size="1"><%=ems %></select></label>
					</form></td>
					<td width="42" class="tdline"><form name="form3">
					  <label><button name="btndata" onClick="getData()" type="button">保存
							</button></label>
					</form></td>
					<td width="112"><form name="form4">
					  <label>宽:<input name="swidth" type="text" id="widths" size="4" value="228" onchange="setfsize()"/>mm</label>
					</form></td>
					<td width="120" class="tdline"><form name="form5">
					  <label>*高:<input name="sheight" type="text" id="heights" size="4" value="127" onchange="setfsize()" />mm</label>
					</form></td>
					<td width="49"><form name="form6">
					  <label><button name="btnclick"
							onClick ="clickbtn();return false;">上传
							</button></label>
					</form></td>
					<td width="48" class="tdline"><form name="form7">
					  <label><button name="btnimage"
							onClick ="imagebtn();return false;">删除
							</button></label>
					</form></td>
					<td width="125"><form name="form8">
					  <label>
						<select 
								name="item" 
								onChange="addbtn(i);tt();return false;"
								size="1">
								<option value="收货人-姓名">添加打印项
								<option value="收货人-姓名">收货人-姓名
								<option value="收货人-地址">收货人-地址
								<option value="收货人-电话">收货人-电话
								<option value="收货人-邮编">收货人-邮编	
								<option value="收货人-手机">收货人-手机	
								<option value="收货人-地区1级">收货人-地区1级
								<option value="收货人-地区2级">收货人-地区2级
								<option value="收货人-地区3级">收货人-地区3级	
								
								<option value="发货人-姓名">发货人-姓名
								<option value="发货人-地区1级">发货人-地区1级
								<option value="发货人-地区2级">发货人-地区2级
								<option value="发货人-地区3级">发货人-地区3级										
								<option value="发货人-地址">发货人-地址
								<option value="发货人-电话">发货人-电话
								<option value="发货人-邮编">发货人-邮编
								<option value="发货人-手机">发货人-手机
								
								<option value="订单-订单号">订单-订单号
								<option value="订单-总金额">订单-总金额
								<option value="订单-物品总重量">订单-物品总重量
								<option value="订单-备注">订单-备注
								<option value="订单-详情">订单-详情
								<option value="网店名称">网店名称
								<option value="√">对号-√
								<option value="自定义内容">自定义内容
							</select></label>
					</form>
					</td>
					<td width="54" class="tdline"><form name="form9">
					  <label><button name="btnitem" onClick="delData()"  type="button">删除
							</button></label>
					</form></td>
					<td class="tdline"></td>
					<td></td>
				  </tr>
				</table>
				<div id="writeroot"></div>
		
				<div id="flashoutput">
					<noscript>
						<object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">
						<param name="flashvars" value="bridgeName=example"/>
						<param name="src" value="../../Storage/master/flex/AddExpressTemplate.swf"/>
						<embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/AddExpressTemplate.swf" height="600" width="100%" flashvars="bridgeName=example"/>
						</object>
					</noscript>
					<script type="text/javascript" language="javascript" charset="utf-8">
						document.write('<object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">');
						document.write('<param name="flashvars" value="bridgeName=example"/>');
						document.write('<param name="src" value="../../Storage/master/flex/AddExpressTemplate.swf"/>');
						document.write('<embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/AddExpressTemplate.swf" height="600" width="100%" flashvars="bridgeName=example"/>');
						document.write('</object>');
					</script>
				</div>
			  </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
	var i=0;  
	function tt()
	{
		i++;
		//document.getElementById("Button2").value=i;
	}
</script>
</asp:Content>
