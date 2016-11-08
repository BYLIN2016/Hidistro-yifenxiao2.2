<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ReceivedMessages" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->
	<div class="optiongroup mainwidth">
	  <ul>
	    <li class="menucurrent"><a href="javascript:void(0);"><span>收件箱</span></a></li>
	    <li><a href="SendedMessages.aspx"><span>发件箱</span></a></li>
	    <li><a href="SendMessage.aspx" class="optioncurrentend"><span>发送站内信</span></a></li>
      </ul>      	
</div>
	<div class="dataarea mainwidth">
    <div class="toptitle"> <em><img src="../images/07.gif" width="32" height="32" /></em> <span class="title_height">你可以查看回复删除会员发送给你的站内消息.</span> </div>
    <!--搜索-->
    <div class="functionHandleArea m_none">
      <!--分页功能-->
      <div class="pageHandleArea">
        <ul>
          <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
        </ul>
      </div>
    

      <div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
				</div>
			</div>

      <!--结束-->
      <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span>
           <span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
            <span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span> 
            <span class="delete"><Hi:ImageLinkButton ID="btnDeleteSelect1" IsShow="true" runat="server" Text="删除" /></span></li>
        </ul>
      </div>
      <div class="filterClass"> <span><b>回复状态：</b></span> <span class="formselect">
        <Hi:MessageStatusDropDownList ID="statusList" class="formselect input100" runat="server"  />
      </span></div>
    </div>
    <!--数据列表区域-->
    <div class="datalist">
      <UI:Grid ID="messagesList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="MessageId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
            <Columns>                 
                <UI:CheckBoxColumn HeaderStyle-CssClass="table_title"/>
                 
                 <asp:TemplateField HeaderText="标题" HeaderStyle-CssClass="td_right td_left">
                  <itemstyle  CssClass="Name" />
                    <ItemTemplate >                        
                            <%# Eval("Title")%>
                    </ItemTemplate>
                 </asp:TemplateField>    
                
                <asp:TemplateField HeaderText="发件人" HeaderStyle-CssClass="td_right td_left">
                 <itemstyle  CssClass="Name" />
                    <ItemTemplate>
                        <a href="javascript:DialogFrame('<%# Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}",Eval("UserId"))) %>','查看会员详细信息',null,null)"><span class='<%# Eval("IsRead").ToString()=="False"? "cstrong":"" %>'><%# Eval("Sernder")%></span></a>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="时间" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <span class='<%# Eval("IsRead").ToString()=="False"? "cstrong":"" %>'><Hi:FormatedTimeLabel ID="litDateTime" ShopTime="true" runat="server" Time='<%#Eval("Date") %>' /></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="内容" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="50%">
                    <ItemTemplate>
                       <asp:Label ID="litPublishContent" runat="server" Text='<%#Eval("Content")%>' CssClass="line" />
                    </ItemTemplate>
                </asp:TemplateField>            
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="5%" HeaderStyle-CssClass="td_left td_right_fff">             
                    <ItemTemplate>                        
                            <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/comment/ReplyReceivedMessages.aspx?MessageId={0}", Eval("MessageId")))%>' class='<%# Eval("IsRead").ToString()=="False"? "cstrong":"" %>'>回复</a>
                    </ItemTemplate>
                </asp:TemplateField>                    
            </Columns>
        </UI:Grid>         
    <div class="blank12 clearfix"></div>
    </div>
     <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="bottomSignicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDeleteSelect" IsShow="true" Height="25px" runat="server" Text="删除" /></span></li>
				</ul>
			</div>
	  </div>
    <!--数据列表底部功能区域-->
     <div class="page">
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
			</div>
			</div>
		</div>
		</div>
</div>
</asp:Content>

