<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyAfficheList.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyAfficheList" %>
<%@ Import Namespace="Hidistro.Membership.Context"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>公告管理 </h1>
     <span>发布店铺的相关公告信息</span></div>
    <!-- 添加按钮-->
    <div class="btn">
     <a href="AddMyAffiche.aspx"  class="submit_jia">添加新公告</a>
    </div>
    <div class="clear"></div>
    <!--结束-->
    <!--数据列表区域-->
    <div class="datalist">

    <div class="Pg_2020">
	  <div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)">全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteSelect" IsShow="true" runat="server" >删除</Hi:ImageLinkButton></span>
                    </li>
	           </ul>
	  </div>
     </div> 
     
     <UI:Grid ID="grdAfficheList" runat="server" ShowHeader="true" SortOrderBy="AddedDate" SortOrder="Desc" AutoGenerateColumns="false" DataKeyNames="AfficheId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                 <UI:CheckBoxColumn ReadOnly="true" HeaderStyle-CssClass="td_right td_left"/>
                 <asp:TemplateField HeaderText="公告标题" HeaderStyle-CssClass="td_right td_left">
                       <ItemTemplate>
                       <Hi:DistributorDetailsHyperLink ID="lkh" DetailsId='<%# Eval("AfficheId") %>' Title='<%#  Eval("Title")%>' DetailsPageUrl="/AffichesDetails.aspx?AfficheId=" runat="server" />
                        </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="发布时间" HeaderStyle-CssClass="td_right td_left">
                       <ItemTemplate>
                           <Hi:FormatedTimeLabel id="lblAddedDate" Time='<%#Eval("AddedDate") %>' runat="server"></Hi:FormatedTimeLabel>
                       </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                     <ItemTemplate>
                          <span class="submit_bianji"><a href='<%# "EditMyAffiche.aspx?afficheId=" + Eval("AfficheId")%> '>编辑</a></span>
                          <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbtnDelete" CommandName="Delete" IsShow="true" runat="server" Text="删除" /></span>
                     </ItemTemplate>
                 </asp:TemplateField>
            </Columns>
        </UI:Grid>
         <div class="blank12 clearfix"></div>
        <div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="bottomSignicon"></span>
					<span class="allSelect"><a onclick="CheckClickAll()" href="javascript:void(0)" >全选</a></span>
					<span class="reverseSelect"><a onclick="CheckReverse()" href="javascript:void(0)" >反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="lkbtnDeleteSelect1" IsShow="true" runat="server" Text="删除" /></span>
                    </li>
               </ul>
				

    </div>

      <div class="blank12 clearfix"></div>
      
    <!--数据列表底部功能区域-->
  </div>
  <div class="bottomarea testArea">
    <!--顶部logo区域-->
  </div>
  </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators()
    {
    initValid(new InputValidator('txtAfficheTitle', 1, 200, false, null, '公告标题不能为空，长度限制在100个字符以内'))
    }
    $(document).ready(function(){ InitValidators(); });
</script>
</asp:Content>