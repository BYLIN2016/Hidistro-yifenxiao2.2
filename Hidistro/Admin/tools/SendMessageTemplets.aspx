<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.SendMessageTemplets" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
	  <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
	    <h1>邮件短信设置</h1>
	    <span>在这里可以管理短信、邮件和站内信的发送，以及短信、邮件和站内信的配置和相关的模版设置。</span>
     </div>
	  <!-- 添加按钮-->
	  <!--结束-->
	  <!--数据列表区域-->
<div class="datalist">
	    <div class="search clearfix searcha">
			<ul>
            	<li><img src="../images/gif-0677.gif" width="13" height="10" /></li>
				<li>电子邮件</li>
                <li><a href="EmailSettings.aspx">配置</a></li>
                <li class="Pg_45"><img src="../images/p_icon09.gif" width="12" height="13" /></li>
                <li>手机短信</li>
                <li><a href="SMSSettings.aspx">配置</a></li>
			</ul>
	</div>
	  <UI:Grid ID="grdEmailTemplets" runat="server" ShowHeader="true" DataKeyNames="MessageType" AutoGenerateColumns="false" GridLines="None" Width="100%">
	    <HeaderStyle CssClass="table_title" />
            <Columns>
                <asp:BoundField HeaderText="消息类型" ItemStyle-Width="200px" HeaderStyle-CssClass="td_right td_left" DataField="Name" />
                <asp:TemplateField HeaderText = "电子邮件" HeaderStyle-CssClass="td_right td_left">    
                    <ItemTemplate>
                    <table cellpadding="0" cellspacing="0" style="border:none;">
                    <tr><td style="border:none; width:10px;"><asp:CheckBox runat="server" ID="chkSendEmail" Checked='<%# Eval("SendEmail")%>' /></td>
                    <td style="border:none;"><span class="submit_bianji float"><a href='<%# "EditEmailTemplet.aspx?MessageType="+Eval("MessageType")%>'>编辑</a></span></td></tr>
                    </table> 
                    </ItemTemplate>
                </asp:TemplateField>   
                <asp:TemplateField HeaderText="站内消息" HeaderStyle-CssClass="td_right td_left">                                                                                                        
                    <ItemTemplate> 
                    <table cellpadding="0" cellspacing="0" style="border:none;">
                    <tr><td style="border:none; width:10px;"><asp:CheckBox runat="server" ID="chkInnerMessage"  Checked='<%# Eval("SendInnerMessage")%>'/></td>
                    <td style="border:none;"><span class="submit_bianji"><a href='<%# "EditInnerMessageTemplet.aspx?MessageType="+Eval("MessageType")%>'>编辑</a></span></td></tr>
                    </table>                          
                    </ItemTemplate>
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="手机短信" HeaderStyle-CssClass="td_left td_right_fff">                                                                                                       
                    <ItemTemplate>  
                    <table cellpadding="0" cellspacing="0" style="border:none;">
                    <tr><td style="border:none; width:10px;"><asp:CheckBox runat="server" ID="chkCellPhoneMessage"  Checked='<%# Eval("SendSMS")%>' /></td>
                    <td style="border:none;"><span class="submit_bianji"><a href='<%# "EditCellPhoneMessageTemplet.aspx?MessageType="+Eval("MessageType")%>'>编辑</a></span></td></tr>
                    </table>                          
                    </ItemTemplate>
                </asp:TemplateField>                              
            </Columns>
        </UI:Grid>        
    <div class="Pg_15 Pg_010" style="text-align:center;"><asp:Button ID="btnSaveSendSetting" runat="server" OnClientClick="return PageIsValid();" Text="保存设置"  CssClass="submit_DAqueding"/></div>
</div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

	
</asp:Content>
