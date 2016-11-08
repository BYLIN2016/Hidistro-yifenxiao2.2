<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageLogs.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageLogs" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth databody">
    <div class="title">
		  <em><img src="../images/02.gif" width="32" height="32" /></em>
          <h1><strong>������־</strong></h1>
          <span>�鿴��������Ա����ʷ������¼ </span>
		</div>

    <!--�����б�����-->
    <div class="datalist">
	
    <!--����-->
    <div class="searcharea clearfix br_search">
			<ul class="a_none_left">
		    <li><span>�����ˣ�</span><span><Hi:LogsUserNameDropDownList ID="dropOperationUserNames" runat="server"/></span></li>
				
                <li><span>ѡ��ʱ��Σ�</span><span><UI:WebCalendar runat="server" CalendarType="StartDate" CssClass="forminput" ID="calenderFromDate" /></span><span class="Pg_1010">��</span><span><UI:WebCalendar runat="server"  CalendarType="EndDate"  CssClass="forminput" ID="calenderToDate" IsStartTime="false"/></span></li>
				<li><asp:Button ID="btnQueryLogs" runat="server" class="searchbutton" Text="��ѯ" /></li>
			</ul>
	  </div>
	  	 <div class="searcharea clearfix br_search">
			 <ul>
				<li>
					<span class="submit_dalata"><Hi:ImageLinkButton ID="lkbDeleteAll" runat="server" Text="�����־" IsShow="true" DeleteMsg="ȷ��Ҫɾ�����в�����־��ɾ���󽫲��ɻָ���"/></span>
			   </li>
			</ul>
		</div>
    <div class="functionHandleArea clearfix">
      <!--��ҳ����-->
      <div class="pageHandleArea">
        <ul>
          <li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
        </ul>
      </div>
      <div class="pageNumber"> 
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
        </div>
       </div>
      <!--����-->
      <div class="blank8 clearfix"></div>
      <div class="batchHandleArea">
        <ul>
          <li class="batchHandleButton"> <span class="signicon"></span> 
          <span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span>
		  <span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">��ѡ</a></span>
		  <span class="delete"><Hi:ImageLinkButton ID="lkbDeleteCheck1" runat="server" Text="ɾ��" IsShow="true"/></span>
					</li>
        </ul>
      </div>
    </div>
     <asp:DataList ID="dlstLog" runat="server" DataKeyField="LogId" Style="width: 100%;">
     <HeaderTemplate>
      <table width="0" border="0" cellspacing="0" >
        <tr class="table_title">
          <td colspan="2" class="td_right td_left">ѡ��</td>
          <td width="18%" class="td_right td_left">����</td>
          <td width="23%" class="td_left td_right_fff">����</td>
        </tr>
     </HeaderTemplate>
     <ItemTemplate>
      <tr  class="td_bg">
          <td width="3%"><label>
           <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("LogId") %>'>
          </label></td>
          <td width="56%">�û����� <asp:Literal runat="server" ID="litUserName" Text=' <%#Eval("UserName")%>' /></td>
          <td >�û�IP��ַ��<asp:Literal runat="server" ID="litIp" Text=' <%#Eval("IPAddress")%>' /></td>
          <td>���ʱ�䣺<Hi:FormatedTimeLabel runat="server" ID="time" Time='<%# Eval("AddedTime")%>' /></td>
        </tr>
         <tr>
          <td colspan="3"><span>ҳ���ַ��<asp:Literal runat="server" ID="litPageUrl" Text=' <%#Eval("PageUrl")%>' /></span>
          ��־����ϸ������<abbr class="colorG"><asp:Literal runat="server" ID="litDescription" Text=' <%#Eval("Description")%>' /></abbr></td>
          <td><span class="submit_shanchu"><Hi:ImageLinkButton  ID="lkbDelete" IsShow="true" runat="server"  CommandName="Delete"  Text="ɾ��" /></span></td>
        </tr>
     </ItemTemplate>
     <FooterTemplate>
             </table>
     </FooterTemplate>
     </asp:DataList>
      

    </div>
    <!--�����б�ײ���������-->
<div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
						<span class="bottomSignicon"></span>
						<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span>
						<span class="reverseSelect"><a href="javascript:void(0)" onclick=" ReverseSelect()">��ѡ</a></span>
					<span class="delete"><Hi:ImageLinkButton ID="lkbDeleteCheck" runat="server" Text="ɾ��" IsShow="true"/></span></li>
				</ul>
			</div>
		</div>
    <div class="bottomPageNumber clearfix">
      <div class="pageNumber"> 
      <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
        </div>
        </div>
    </div>
</div>
  <div class="bottomarea testArea">
    <!--����logo����-->
  </div>
  
  

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

</asp:Content>

