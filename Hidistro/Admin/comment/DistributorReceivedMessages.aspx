<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DistributorReceivedMessages.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorReceivedMessages" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
	<div class="dataarea mainwidth databody">
    <div class="title">
		<em><img src="../images/07.gif" width="32" height="32" /></em>
		<h1>收件箱</h1>
		<span>你可以查看回复删除分销商发送给你的站内消息.</span>
	</div>

    <!--数据列表区域-->
    <div class="datalist">
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
      <UI:Grid ID="messagesList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="MessageId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
            <Columns>                 
                <UI:CheckBoxColumn HeaderStyle-CssClass="table_title"/>
                 
                 <asp:TemplateField HeaderText="标题" HeaderStyle-CssClass="td_right td_left">
                  <itemstyle  CssClass="Name" />
                    <ItemTemplate >                        
                            <asp:Literal ID="litTitle" runat="server" Text='<%# Eval("Title")%>'></asp:Literal>                       
                    </ItemTemplate>
                 </asp:TemplateField>    
                
                <asp:TemplateField HeaderText="发件人" HeaderStyle-CssClass="td_right td_left">
                 <itemstyle  CssClass="Name" />
                    <ItemTemplate>
                        <span class='<%# Eval("IsRead").ToString()=="False"? "cstrong":"" %>'><%# Eval("Sernder")%></span>
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
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="5%">
                    <ItemTemplate>
                       <a href="javascript:DialogFrame('<%# Globals.GetAdminAbsolutePath(string.Format("/comment/ReplyDistributorReceivedMessages.aspx?MessageId={0}", Eval("MessageId")))%>','回复站内信',670,560)" class='<%# Eval("IsRead").ToString()=="False"? "cstrong":"" %>'>回复</a>
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
