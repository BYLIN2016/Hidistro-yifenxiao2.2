<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="BalanceDrawRequest.aspx.cs" Inherits="Hidistro.UI.Web.Admin.BalanceDrawRequest" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/05.gif" width="32" height="32" /></em>
  <h1>会员提现申请</h1>
  <span>查看会员提现申请记录</span>
</div>

		<!--数据列表区域-->
		<div class="datalist">
        		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
				<li>
                <span>选择时间段：</span>
                <span><UI:WebCalendar CalendarType="StartDate" ID="calendarStart" runat="server" class="forminput"/></span>
                <span class="Pg_1010">至</span>
                <span><UI:WebCalendar ID="calendarEnd" runat="server" CalendarType="EndDate" class="forminput"/></span>
                <span style=" margin-left:11px;">用户名：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"/></span>
              </li>
				<li>
				    <asp:Button ID="btnQueryBalanceDrawRequest" runat="server" class="searchbutton" Text="查询" />
				</li>
			</ul>
	</div>
		
<!--结束-->
		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		  <div class="pageNumber"> 
		    <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
          </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">&nbsp;</li>
	        </ul>
	      </div>
</div>
		    <UI:Grid ID="grdBalanceDrawRequest" DataKeyNames="UserId" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="true" AllowSorting="true" Width="100%"  HeaderStyle-CssClass="table_title" SortOrder="DESC">
                <Columns>
                    <asp:TemplateField HeaderText="用户名" HeaderStyle-CssClass="td_right td_left">
                        <itemtemplate>			  
                            <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/EditMember.aspx?userId={0}", Eval("UserId")))%>'><asp:Label ID="Label1" Text='<%# Eval("UserName")%>' runat="server"></asp:Label></a>                     
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
                     <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                            <span class="submit_quanxuan"><Hi:ImageLinkButton CssClass="SmallCommonTextButton" ID="btnAgreeRequest" IsShow="true" runat="server" 
                                CommandName="UnLineReCharge"  DeleteMsg="如果您已经通过在线转账或线下打款将客户的提现金额转到了客户的收款账号上\n\n则此操作会结束客户提现申请的处理并将提现申请的状态更改为成功" Text="确认" /></span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton CssClass="SmallCommonTextButton" ID="btnRefuseRequest" IsShow="true" runat="server"
                                CommandName="RefuseRequest"  DeleteMsg="如果您在转账的过程遇到什么问题或者您不允许客户的此次提现\n\n则此操作会结束客户提现申请的处理并将提现申请的状态更改为成失败" Text="拒绝" /></span>               
                         </ItemTemplate>
                     </asp:TemplateField>  
                     
                       <asp:TemplateField HeaderText="备注" HeaderStyle-CssClass="td_left td_right_fff" >
                                    <itemtemplate>
                                        <img src="../images/xi.gif" onclick="showRemark(this,'<%# Eval("Remark")%>')"/>
                                    </itemtemplate>
                                </asp:TemplateField>   
                    </Columns> 
              </UI:Grid>
		  
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>
	
     <div class="Pop_up" id="BandMessage" style="display:none; z-index:1000; position:absolute; background-color:White;">
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

<div class="Pop_up" id="remark" style="display:none; z-index:1000;">
<h1>备注信息</h1>
<div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
<table width="100%" border="0" cellspacing="0" class="colorQ">
  <tr>
    <td colspan="2"><span id="spanRemark" runat="server" /></td>
  </tr>
  </table>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script language="javascript" type="text/javascript">
function CloseBankMessage(){$("#BandMessage").fadeOut(800);}
function CloseRemark(){$("#remark").fadeOut(800);}
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

    function showRemark(objThis,remark) 
    {
        if (remark == "") {
            $("#ctl00_contentHolder_spanRemark").html("无备注信息");
        }
        else {
            $("#ctl00_contentHolder_spanRemark").html(remark);
        }
        var BandMessage = document.getElementById("remark")
        
        var WinElementPos = getWinElementPos(objThis) //公用方法来源 globals.js
		 var MouseX =WinElementPos.x; 
		 var MouseY =WinElementPos.y;		 

        var pltsoffsetX = 0; // 弹出窗口位于鼠标左侧或者右侧的距离；
        var pltsoffsetY =-120; // 弹出窗口位于鼠标下方的距离；
//        BandMessage.style.position="absolute";
//        BandMessage.style.left = MouseX + pltsoffsetX+"px";
//        BandMessage.style.top = MouseY + pltsoffsetY+"px";
//        BandMessage.style.display = "block";
        DivWindowOpen(480, 250, 'remark');
    }  
</script>
</asp:Content>