<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.DistributorBalanceDrawRequest" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
	  <div class="title">
          <em><img src="../images/05.gif" width="32" height="32" /></em>
          <h1>分销商提现申请</h1>
          <span>查看分销商的提现申请记录</span>
      </div>
		<!--数据列表区域-->
	  <div class="datalist">
	  		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>选择时间段：</span>
                    <span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" CssClass="forminput" /></span>
                    <span class="Pg_1010"> 至 </span>
               	  <span><UI:WebCalendar CalendarType="EndDate" ID="calendarEnd" runat="server" CssClass="forminput" /></span>
                
              </li>
                <li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" ></asp:TextBox></span></li>
                
			  <li><asp:Button ID="btnQueryBalanceDrawRequest" runat="server" class="searchbutton" Text="查询" /></li>
			</ul>
		</div>
		<!--结束-->


         <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize ID="hrefPageSize" runat="server" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
			<!--结束-->
		</div>
	   <UI:Grid ID="grdBalanceDrawRequest" DataKeyNames="UserId" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                            <Columns>                              
                                <asp:TemplateField HeaderText="分销商名称" HeaderStyle-CssClass="td_right td_left">
                                    <itemtemplate>			  
                                     <asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label></a>                     
                                    </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="申请时间" HeaderStyle-CssClass="td_right td_left" >
                                    <itemtemplate>
                                        <Hi:FormatedTimeLabel ID="lblTime" Time='<%# Eval("RequestTime")%>' runat="server" />
                                    </itemtemplate>
                                </asp:TemplateField>
                                <Hi:MoneyColumnForAdmin HeaderText="提现金额" DataField="Amount" HeaderStyle-CssClass="td_right td_left" />
                                
                                <asp:TemplateField HeaderText="银行账户信息" HeaderStyle-CssClass="td_right td_left" >
                                    <itemtemplate>
                                        <img src="../images/Visa.gif" onclick="showBankMessage(this,'<%# Eval("BankName")%>','<%# Eval("AccountName")%>','<%# Eval("MerchantCode")%>')"/>
                                    </itemtemplate>
                                </asp:TemplateField> 
                                
                                 <asp:TemplateField HeaderText="操作" ItemStyle-Width="22%" HeaderStyle-CssClass="td_left td_right_fff">
                                     <ItemTemplate>
                                        <span class="submit_quanxuan"><Hi:ImageLinkButton  ID="btnAgreeRequest" IsShow="true" runat="server" 
                                            CommandName="UnLineReCharge"  DeleteMsg="如果您已经通过在线转账或线下打款将客户的提现金额转到了客户的收款账号上\n\n则此操作会结束客户提现申请的处理" Text="接受" /></span>
                                        <span class="submit_shanchu"><Hi:ImageLinkButton  ID="btnRefuseRequest" IsShow="true" runat="server"  
                                            CommandName="RefuseRequest"  DeleteMsg="如果您在转账的过程遇到什么问题或者您不允许客户的此次提现\n\n则此操作会结束客户提现申请的处理" Text="拒绝" /></span>		               
                                     </ItemTemplate>
                                 </asp:TemplateField>    
                                </Columns> 
              </UI:Grid>
              
              
  
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
	  <div class="bottomPageNumber clearfix">
	  <div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                </div>
			</div>
		</div>


</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
	
	
<div class="Pop_up" id="BandMessage" style="display:none;z-index:1000;">
<h1>银行账号信息</h1>
<div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
<table width="100%" border="0" cellspacing="0">
  <tr>
    <td width="39%" align="right">开户行名称：</td>
    <td width="61%" class="colorQ"><span runat="server" id="spanBankName" /></td>
    </tr>
  <tr>
    <td align="right">开户人姓名：</td>
    <td class="colorQ"><span runat="server" id="spanaccountName" /></td>
    </tr>
  <tr>
    <td align="right">银行账号：</td>
    <td class="colorQ"><span runat="server" id="spanmerchantCode" /></td>
    </tr>
</table>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script language="javascript" type="text/javascript">
function CloseBankMessage(){$("#BandMessage").fadeOut(800);}
function showBankMessage(objThis,bankName, accountName, merchantCode)
 {      
       $("#ctl00_contentHolder_spanBankName").html(bankName);
        $("#ctl00_contentHolder_spanaccountName").html(accountName);
        $("#ctl00_contentHolder_spanmerchantCode").html(merchantCode);
		 var BandMessage = document.getElementById("BandMessage");
		 var WinElementPos = getWinElementPos(objThis) //公用方法来源 globals.js
		 var MouseX =WinElementPos.x; 
		 var MouseY =WinElementPos.y;		 

        var pltsoffsetX = 0; // 弹出窗口位于鼠标左侧或者右侧的距离；
        var pltsoffsetY =-120; // 弹出窗口位于鼠标下方的距离；
//        BandMessage.style.position="absolute";
//        BandMessage.style.left = MouseX + pltsoffsetX+"px";
//        BandMessage.style.top = MouseY + pltsoffsetY+"px";
        //        BandMessage.style.display = "block";
        DivWindowOpen(480, 250, 'BandMessage');   
}
</script>
</asp:Content>
